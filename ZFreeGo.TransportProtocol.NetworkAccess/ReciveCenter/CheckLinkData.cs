using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using ZFreeGo.TransmissionProtocols.BasicElement;
using ZFreeGo.TransmissionProtocols.FileSever;
using ZFreeGo.TransmissionProtocols.Frame104;
using ZFreeGo.TransmissionProtocols.TransmissionControl104;


namespace ZFreeGo.TransmissionProtocols.ReciveCenter
{
    /// <summary>
    /// 101链路层数据提取
    /// </summary>
    public class CheckLinkData
    {      


        public event EventHandler<ShortFrameEventArgs> ShortEvent;
       
        /// <summary>
        /// 主站召唤命令
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> MasterInterrogationArrived;
        /// <summary>
        /// 主站复位进程命令
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> MasterResetArrived;
        /// <summary>
        /// 主站初始化结束 
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> MasterInitializeArrived;
        /// <summary>
        /// 主站时钟同步到达
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> MasterTimeArrived;
        /// <summary>
        /// 电能脉冲
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> ElectricEnergyArrived;

        /// <summary>
        ///遥信功能
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> TelesignalisationMessageArrived;
        /// <summary>
        ///遥测功能
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> TelemeteringMessageArrived;

        /// <summary>
        ///遥控功能
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> TelecontrolCommandArrived;
        /// <summary>
        /// 校准信息功能
        /// </summary>
        //public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> CalibrationMessageArrived;
        /// <summary>
        /// 保护定值设置
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> ProtectSetMessageArrived;

        /// <summary>
        /// 文件服务
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, APDU>> FileServerArrived;

        /// <summary>
        ///ID未识别
        /// </summary>
        public event EventHandler<TransmitEventArgs<TypeIdentification, byte[] >> UnknowMessageArrived;



       

        /// <summary>
        /// 接收队缓冲 一级缓冲
        /// </summary>
        public Queue<byte> ReciveQuene;
        /// <summary>
        /// 缓冲队列 二级缓冲
        /// </summary>
        private Queue<byte> ReciveQueneBuffer;

        /// <summary>
        /// 暂存一帧
        /// </summary>
        private Queue<byte> FrameQueneBuffer;

        /// <summary>
        /// 现在检测CheckCode步骤
        /// </summary>
        private CheckCode chekNow;
        /// <summary>
        /// 字节数组
        /// </summary>
        byte[] dataArray;

        /// <summary>
        /// 接收线程
        /// </summary>
        private Thread readThread;

        /// <summary>
        /// ASDU长度，用于中间处理过程
        /// </summary>
        private byte aspduCheckLen = 0;

        /// <summary>
        /// 初次校验数据，进行基本归类
        /// </summary>
        /// <param name="dataArray">原始字节数据</param>
        /// <returns>检测代码结果</returns>
        public CheckCode CheckMessageBasicClassify(byte[] dataArray)
        {
            //不满足最小长度
            if (dataArray.Length < 6)
            {
                return CheckCode.MinLength;
            } 
            //启动字符
            if (dataArray[0] != 0x68)
            {
                return CheckCode.StartCharacter;
            }
            //数据长度不满足完整帧要求
            if (dataArray.Length < dataArray[1] + 2)
            {
                return CheckCode.IntactLength;
            }
            //仅仅只有4个字节，为S或者U格式
           
            var format =  GetMessageFormat(dataArray[2], dataArray[4]);
            if (dataArray[1] == 4)
            {
                if (format == MessageFormat.NumberedSupervisoryFunctions)
                    return CheckCode.TypeS;
                if (format == MessageFormat.UnnumberedControlFunction)
                    return CheckCode.TypeU;

                return CheckCode.TypeLenghtError;
            }
            else
            {
                 if (format == MessageFormat.InformationTransmitFormat)
                 {
                     return CheckCode.TypeI;
                 }
                 return CheckCode.TypeLenghtError;
            }


        }

        /// <summary>
        /// 初次校验数据，进行基本归类,并提取完整的一帧数据
        /// </summary>
        /// <param name="nowCode">当前代码</param>
        /// <param name="quenue">原始队列</param>
        /// <param name="dataIn">一帧字节数组</param>
        /// <returns>检测代码结果</returns>
        public CheckCode CheckMessageBasicClassify(CheckCode nowCode, Queue<byte> quenue, ref byte[] dataIn)
        {
            try
            {
                while (true)
                {
                    switch (nowCode)
                    {
                        case CheckCode.MinLength:
                            {
                                //重置ASDU长度计数
                                aspduCheckLen = 0;
                                //最小长度是否满足要求
                                if (quenue.Count >= 6)
                                {
                                    //不满足起始字符
                                    if (quenue.Dequeue() == 0x68)
                                    {
                                        nowCode = CheckCode.IntactLength;
                                        continue;
                                    }
                                    else
                                    {
                                        nowCode = CheckCode.MinLength;
                                        continue;
                                    }
                                }
                                else
                                {
                                    chekNow = CheckCode.MinLength;
                                    return CheckCode.MinLength;
                                }

                            }
                        case CheckCode.IntactLength: //此处应该添加超时检测
                            {
                                //首次进入
                                if (aspduCheckLen == 0)
                                {
                                    aspduCheckLen = quenue.Dequeue();
                                }

                                //数据长度不满足完整帧要求
                                if (quenue.Count < aspduCheckLen)
                                {
                                    return CheckCode.IntactLength;
                                }
                                else
                                {
                                    //重置寻找位置
                                    chekNow = CheckCode.MinLength;

                                    byte controlDomain1 = quenue.Dequeue();
                                    byte controlDomain2 = quenue.Dequeue();
                                    byte controlDomain3 = quenue.Dequeue();
                                    byte controlDomain4 = quenue.Dequeue();
                                    var format = GetMessageFormat(controlDomain1, controlDomain3);
                                    if (aspduCheckLen == 4)
                                    {
                                        dataIn = new byte[6] { 0x68, aspduCheckLen, 
                                        controlDomain1, controlDomain2, controlDomain3, controlDomain4 };
                                        //仅仅只有4个字节，为S或者U格式
                                        if (format == MessageFormat.NumberedSupervisoryFunctions)
                                            return CheckCode.TypeS;
                                        if (format == MessageFormat.UnnumberedControlFunction)
                                            return CheckCode.TypeU;

                                        return CheckCode.TypeLenghtError;
                                    }
                                    else
                                    {
                                        if (format == MessageFormat.InformationTransmitFormat)
                                        {
                                            dataIn = new byte[2 + aspduCheckLen];
                                            dataIn[0] = 0x68;
                                            dataIn[1] = aspduCheckLen;
                                            dataIn[2] = controlDomain1;
                                            dataIn[3] = controlDomain2;
                                            dataIn[4] = controlDomain3;
                                            dataIn[5] = controlDomain4;
                                            for (int i = 6; i < 2 + aspduCheckLen; i++)
                                            {
                                                dataIn[i] = quenue.Dequeue();
                                            }


                                            return CheckCode.TypeI;
                                        }
                                        return CheckCode.TypeLenghtError;
                                    }
                                }
                            }
                        default:
                            {
                                chekNow = CheckCode.MinLength;
                                return nowCode;

                            }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            
        
        /// <summary>
        /// 格局控制域1,3判断帧格式
        /// </summary>
        /// <param name="controldomain1">控制域1</param>
        /// <param name="controldomain3">控制3</param>
        /// <returns>帧格式</returns>
        public MessageFormat GetMessageFormat(byte controldomain1, byte controldomain3)
        {
            
            //I格式
            if ( ((controldomain1 & 0x01) == 0) && ((controldomain3 & 0x01) == 0) )
            {
               return MessageFormat.InformationTransmitFormat;

            }
            //S格式
            if (((controldomain1 & 0x03) == 0x01) && ((controldomain3 & 0x01) == 0))
            {
                return MessageFormat.NumberedSupervisoryFunctions;
            }
            //U格式
            if (((controldomain1 & 0x03) == 0x03) && ((controldomain3 & 0x01) == 0))
            {
                return MessageFormat.UnnumberedControlFunction;
            }

            return MessageFormat.Null;         
        }



        /// <summary>
        /// 获取来自APCI-U的数据,未编号的控制功能，只包含APCI  
        /// </summary>
        /// <param name="dataArray">接收数据</param>

        public CheckCode GetTypeU(byte[] dataArray)
        {
            try
            {
                if (Enum.IsDefined(typeof(TransmissionControlFunction), dataArray[2]))
                {
                    var tcf = (TransmissionControlFunction)dataArray[2];
                    var apci = new APCITypeU(tcf);
                    //发送事件消息
                    if (TransmitControlCommandArrived != null)
                    {
                        TransmitControlCommandArrived(this,
                              new TransmitEventArgs<APCITypeU, byte[]>(apci, dataArray));
                    }
                   
                    return CheckCode.TypeU;

                }

                return CheckCode.TypeUError;
            }
            catch(Exception ex)
            {
                throw ex;
            }

           
        }
        /// <summary>
        /// 获取来自APCI-S的数据，编号的监视功能，只包含APCI  
        /// </summary>
        /// <param name="dataArray">原始字节数组</param>

        public CheckCode GetTypeS(byte[] dataArray)
        {
            //接收序列号
            UInt16 tn = (UInt16)((dataArray[4] >> 1) + (dataArray[5] << 7));
            var apci = new APCITypeS(tn);
            if (SupervisoryCommandArrived != null)
            {
                SupervisoryCommandArrived(this, new TransmitEventArgs<APCITypeS, byte[]>(apci, dataArray));
            }
            return CheckCode.TypeS;
        }




        /// <summary>
        ///  获取来自APCI-I的数据,未编号的信息传输 
        /// </summary>
        /// <param name="dataArray"></param>
        /// <param name="manager"></param>
        public CheckCode GetTypeI(byte[] dataArray)
        {
            try
            {
                if (Enum.IsDefined(typeof(TypeIdentification), dataArray[6]) == false)
                {
                    return CheckCode.IDUnknow;
                }
                var id = (TypeIdentification)dataArray[6];
                //类型标识符 TI
                switch (id)
                {
                    //主站系统命令
                    case TypeIdentification.C_IC_NA_1:  //总召唤/组召唤
                    case TypeIdentification.C_RP_NA_1:   //复位进程命令
                    case TypeIdentification.M_EI_NA_1:  //初始化结束                  
                    case TypeIdentification.C_CS_NA_1: //时钟同步
                        {
                            GetMasterComand(id, dataArray);
                            return CheckCode.MasterComand;
                        }

                    //遥信信息
                    case TypeIdentification.M_SP_NA_1://单点信息
                    case TypeIdentification.M_DP_NA_1://双点信息
                    case TypeIdentification.M_SP_TB_1://带CP56Time2a时标的单点信息
                    case TypeIdentification.M_DP_TB_1://带CP56Time2a时标的双点信息
                    case TypeIdentification.M_FT_NA_1://故障值信息
                        {
                            GetTelesignalisationMessage(id, dataArray);
                            return CheckCode.TelesignalisationCommand;

                        }
                    //遥测信息 与累积量
                    case TypeIdentification.M_ME_NA_1://测量值，归一化值 
                    case TypeIdentification.M_ME_NC_1://测量值，短浮点数                 
                    case TypeIdentification.M_ME_TD_1://带CP56Time2a时标的测量值，归一化值                   
                    case TypeIdentification.M_ME_TF_1://带CP56Time2a时标的测量值，短浮点数
                    case TypeIdentification.M_IT_NB_1://累计量，短浮点数
                    case TypeIdentification.M_IT_TC_1://带CP56Time2a时标的累计量，短浮点数
                        {
                            GetTelemeteringMessage(id, dataArray);
                            return CheckCode.TelemeteringCommand;
                        }

                    //遥控信息
                    case TypeIdentification.C_SC_NA_1: //单点命令
                    case TypeIdentification.C_DC_NA_1: //双点
                        {
                            //单命令 DCO 1
                            GetTelecotrolCommand(id, dataArray);
                            return CheckCode.TelecontrolCommand;
                        }
                        //电能脉冲
                    case TypeIdentification.C_CI_NA_1: //电能脉冲召唤
                    //case TypeIdentification.M_IT_NA_1: //累积量
                        {
                            GetElectricEnergy(id, dataArray);
                            return CheckCode.ElectricEnergy;
                            
                        }                       

                        //保护定值
                    case TypeIdentification.C_SE_NC_1: //定值命令，短浮点数
                        {
                            GetProtectsetPointMessage(id, dataArray);

                            return CheckCode.ProtectsetPoint;
                            
                        }
                    case TypeIdentification.F_FR_NA_1_NR: //文件传输
                        {
                            GetFileServertMessage(id, dataArray);
                            return CheckCode.FileServer;
                        }
                    default:
                        {
                            //ID已定义但未使用
                            if (UnknowMessageArrived != null)
                            {
                                UnknowMessageArrived(this, new TransmitEventArgs<TypeIdentification, byte[]>(id, dataArray));
                            }

                            return CheckCode.IDUnused;
                        }
                }
            }catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取保护定值设置信息
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <param name="dataArray">原始字节数组</param>
        private void GetProtectsetPointMessage(TypeIdentification id, byte[] dataArray)
        {
            try
            {
                var message = new APDU(dataArray);
                switch (id)
                {
                    case TypeIdentification.C_SE_NC_1: // 保护定值
                    
                        {
                            ProtectSetMessageArrived(this,
                    new TransmitEventArgs<TypeIdentification, APDU>(id, message));
                            break;
                        }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取文件服务信息
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <param name="dataArray">原始字节数组</param>
        private void GetFileServertMessage(TypeIdentification id, byte[] dataArray)
        {
            try
            {
                var message = new APDU(dataArray);
                switch (id)
                {
                    case TypeIdentification.F_FR_NA_1_NR:
                        {
                            
                            FileServerArrived(this,
                    new TransmitEventArgs<TypeIdentification, APDU>(id, message));
                            break;
                        }
                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

        /// <summary>
        /// 获取电能脉冲
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <param name="dataArray">原始字节数组</param>
        private void GetElectricEnergy(TypeIdentification id, byte[] dataArray)
        {
            try
            {
                var message = new APDU(dataArray);
                ElectricEnergyArrived(this,
                    new TransmitEventArgs<TypeIdentification, APDU>(id, message));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       

        /// <summary>
        /// 获取主站系统命令信息
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <param name="dataArray">原始字节数组</param>
        public void GetMasterComand(TypeIdentification id, byte[] dataArray)
        {
            try
            {
                var cmd = new APDU(dataArray);
                switch (id)
                {
                    //主站系统命令
                    case TypeIdentification.C_IC_NA_1:  //总召唤/组召唤                                          
                        {

                            MasterInterrogationArrived(this, new MasterCommmadEventArgs(cmd));

                            break;
                        }
                    case TypeIdentification.C_RP_NA_1:   //复位进程命令
                        {
                            MasterResetArrived(this, new MasterCommmadEventArgs(cmd));
                            break;
                        }
                    case TypeIdentification.M_EI_NA_1:  //初始化结束    
                        {
                            MasterInitializeArrived(this, new MasterCommmadEventArgs(cmd));
                            break;
                        }
                    case TypeIdentification.C_CS_NA_1: //时钟同步
                        {
                            MasterTimeArrived(this, new MasterCommmadEventArgs(cmd));
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 获取遥信信息
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <param name="dataArray">原始字节数组</param>
        public void GetTelesignalisationMessage(TypeIdentification id, byte[] dataArray)
        {
            try
            {
                var message= new APDU(dataArray);
                switch (id)
                {
                    //遥信信息
                    case TypeIdentification.M_SP_NA_1://单点信息
                    case TypeIdentification.M_DP_NA_1://双点信息
                    case TypeIdentification.M_SP_TB_1://带CP56Time2a时标的单点信息
                    case TypeIdentification.M_DP_TB_1://带CP56Time2a时标的双点信息
                        {
                            TelesignalisationMessageArrived(this,
                                new TransmitEventArgs<TypeIdentification, APDU>(id, message));
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 获取遥测信息
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <param name="dataArray">原始字节数组</param>
        public void GetTelemeteringMessage(TypeIdentification id, byte[] dataArray)
        {
            try
            {
                var message = new APDU(dataArray);
                TelemeteringMessageArrived(this,
                    new TransmitEventArgs<TypeIdentification, APDU>(id, message));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取遥控
        /// </summary>
        /// <param name="id">类型ID</param>
        /// <param name="dataArray">原始字节数组</param>
        public void GetTelecotrolCommand(TypeIdentification id, byte[] dataArray)
        {
            try
            {
                var message = new APDU(dataArray);
                TelecontrolCommandArrived(this,
                    new TransmitEventArgs<TypeIdentification, APDU>(id, message));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// CheckCode 检测代码后期处理，用于报告错误等
        /// </summary>
        /// <param name="checkcode">检测代号</param>
        /// <param name="dataArray">字节数组</param>
        public void CheckCodeMessageDeal(CheckCode checkcode, byte[] dataArray)
        {

        }

        /// <summary>
        /// 用于实现数据解码
        /// </summary>      
        public void MainCheckStep()
        {
            try
            {
                //先进行完整帧提取处理
                var checkStep = CheckMessageBasicClassify(chekNow, ReciveQueneBuffer, ref dataArray);
                //根据提取结果分发到相应的处理内容中
                switch (checkStep)
                {
                    case CheckCode.MinLength:
                    case CheckCode.StartCharacter:
                    case CheckCode.IntactLength:
                        {
                            CheckCodeMessageDeal(checkStep, dataArray);
                            break;
                        }
                    case CheckCode.TypeLenghtError:
                        {
                            CheckCodeMessageDeal(checkStep, dataArray);
                            break;
                        }
                    case CheckCode.TypeS:
                        {
                            var m = GetTypeS(dataArray);
                            CheckCodeMessageDeal(m, dataArray);
                            break;
                        }
                    case CheckCode.TypeU:
                        {
                            var m = GetTypeU(dataArray);
                            CheckCodeMessageDeal(m, dataArray);
                            break;
                        }
                    case CheckCode.TypeI:
                        {
                            var m = GetTypeI(dataArray);
                            CheckCodeMessageDeal(m, dataArray);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("MainCheckStep Step");
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }




        /// <summary>
        /// 提取信息初始化，从数组中得到数据,并相应的进行提取,
        /// </summary>
        public CheckGetMessage()
        {
            
             ReciveQuene = new Queue<byte>();
             ReciveQueneBuffer = new Queue<byte>();
             FrameQueneBuffer = new Queue<byte>();
            //初始化为最小长度
             chekNow = CheckCode.MinLength;
             readThread = new Thread(TcpReadThread);
             readThread.Priority = ThreadPriority.AboveNormal;
             readThread.Name = "数据解码";
             readThread.Start();
        }
        /// <summary>
        /// 结束接收线程，用于最后注销时
        /// </summary>
        public void Close()
        {
            if (readThread != null)
            {
                readThread.Join(500);
                readThread.Abort();
                readThread = null;
            }
        }

        /// <summary>
        /// TcpRead进程,从Tcp连接中读取顺序
        /// </summary>
        private void TcpReadThread()
        {
            try
            {
                try
                {
                    while (true)
                    {
                        //二级缓存数据为空，再从一级缓存转存数据
                        lock (ReciveQuene)
                        {
                            while (ReciveQuene.Count > 0)  //转存到二级缓冲
                            {
                                ReciveQueneBuffer.Enqueue(ReciveQuene.Dequeue());
                            }
                        }                                                
                        if (ReciveQueneBuffer.Count > 0)
                        {
                            MainCheckStep();
                        }
                        Thread.Sleep(10);
                    }           
                }
                catch (ThreadAbortException )
                {
                    Thread.ResetAbort();                    
                }
            }
            catch (Exception ex)
            {
                while (true)
                {
                    Console.WriteLine("CheckGetMesssage" + ex.Message);
                    Thread.Sleep(100);
                }
                
            }
        }


    }
}
