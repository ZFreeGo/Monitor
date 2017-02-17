using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZFreeGo.TransportProtocol.NetworkAccess.BasicElement;
using ZFreeGo.TransportProtocol.NetworkAccess.Frame104;
using ZFreeGo.TransportProtocol.NetworkAccess.TransmissionControl104;

namespace ZFreeGo.TransportProtocol.NetworkAccess.ReciveCenter
{
    public class ApplicationFrameManager
    {
        /// <summary>
        /// 主站控制帧合集
        /// </summary>
        public List<MasterCommand> MasterCommandList;


        /// <summary>
        /// 遥控控制帧合集
        /// </summary>
        public List<APDU> TelecontrolCommandList;

        /// <summary>
        /// 遥信命令帧合集
        /// </summary>
        public List<APDU> TelesignalisationCommandList;

        /// <summary>
        /// 启动，停止，测试命令帧合集
        /// </summary>
        public List<APCITypeU> RunControlCommmandList;
        /// <summary>
        /// 帧集合
        /// </summary>
        public List<Object> FrameList;

        /// <summary>
        /// 发送序列号，每次发送时加一
        /// </summary>
        public ushort TransmitSequenceNumber
        {
            private set;
            get;
        }

        /// <summary>
        /// 每次对接收的I格式报文计数加一
        /// </summary>
        public ushort RealReceiveSequenceNumber
        {
            private set;
            get;
        }
        /// <summary>
        /// 接收到对方发送的接收序列号，实际上是对自己的发送序列号的确认
        /// </summary>
        public ushort ReceiveSequenceNumber
        {
            private set;
            get;
        }

        /// <summary>
        /// 指示DTE已经正确收到所有小于或等于这个编号的I格式的APDU
        /// </summary>
        public ushort AckNum
        {             
            private set;
            get;
        }

        /// <summary>
        /// 当未确认 I 格式发送 APDU 个数
        /// </summary>
        public ushort NoTransimitAckNum
        {
            private set;
            get;
        }
        /// <summary>
        /// 接收方最大接收到不确认 I 格式的报文数量
        /// </summary>
        public ushort NoReciveAckNum
        {
            set;
            get;
        }
        /// <summary>
        /// 最大的未确认发送数量
        /// </summary>
        private const  ushort maxNoTransimitAckNum = 12;
        /// <summary>
        /// 最大接收不确认的I报文数量
        /// </summary>
        private const ushort maxNoReciveAckNum = 8;
        /// <summary>
        /// ASDU公共地址
        /// </summary>
        public ushort ASDUADdress;

        /// <summary>
        /// 等待时间单位ms
        /// </summary>
        public int WaitTime;


        /// <summary>
        /// t0 连接建立的超时,单位ms
        /// </summary>
        public const int TcpEstablishWaitTime = 30000;
        /// <summary>
        /// t1 发送或测试 APDU 的超时,单位ms
        /// </summary>
        public const int SendAPDUWaitTime = 15000;
        /// <summary>
        /// t2 无数据报文 t2小于 t1时确认的超时,单位ms
        /// </summary>
        public const int NoMessageACKWaitTime = 10000;
       /// <summary>
        ///t3 长期空闲 t3大于t1状态下发送测试帧的超时,单位ms
       /// </summary>
        public const int LesiueTransmitWaitime = 20000;
        /// <summary>
        ///t4 应用报文确认超时,单位ms
        /// </summary>
        public const int AppMessageAckWaitime = 80000;


        /// <summary>
        /// 默认IP端口号
        /// </summary>
        public int DefaultPort
        {
            get;
            private set;
        }

        /// <summary>
        ///传输协议 初始化控制
        /// </summary>
        public ApplicationFrameManager()
        {
            MasterCommandList = new List<MasterCommand>();
            TelecontrolCommandList = new List<APDU>();
            TelesignalisationCommandList = new List<APDU>();
            RunControlCommmandList = new List<APCITypeU>();
            RunControlCommmandList = new List<APCITypeU>();
            TransmitSequenceNumber = 0;
            ReceiveSequenceNumber = 0;
            RealReceiveSequenceNumber = 0;
            ASDUADdress = 1;
            FrameList = new List<object>();
            WaitTime = AppMessageAckWaitime;
            AckNum = 0;
            DefaultPort = 2404;
        }

        /// <summary>
        /// 将帧添加到列表中
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool AddFrame(object obj)
        {
            if (obj is MasterCommand)
            {
                MasterCommandList.Add(obj as MasterCommand);
                FrameList.Add(obj);
            }
            if (obj is APDU)
            {
                var m = obj as APDU;
                if (m.ASDU.TypeId == (byte)TypeIdentification.C_SC_NA_1)
                {
                    TelecontrolCommandList.Add(m);
                }
                else
                {
                    TelesignalisationCommandList.Add(m);
                }
               
                FrameList.Add(obj);
            }
           
            if (obj is APCITypeU)
            {
                RunControlCommmandList.Add(obj as APCITypeU);
                FrameList.Add(obj);
            }
            return false;

        }

        /// <summary>
        /// 单次更新发送序列号
        /// </summary>
        public void UpdateTransmitSequenceNumber()
        {
            TransmitSequenceNumber++;
            NoTransimitAckNum = (ushort)(TransmitSequenceNumber - RealReceiveSequenceNumber);
            //发送相当于确认接收
            NoReciveAckNum = 0; ;
        }
        /// <summary>
        /// 设置更新接收序列号
        /// </summary>
        /// <param name="trisNum">对方的发送序列号</param>
        /// <param name="reciveNum">对方的接收序列号</param>
        public void UpdateReceiveSequenceNumber(UInt16 trisNum, UInt16 reciveNun)
        {
            ReceiveSequenceNumber = reciveNun;
            RealReceiveSequenceNumber++;
            
          //  NoReciveAckNum = RealReceiveSequenceNumber
            AckNum = reciveNun;
            
            if (RealReceiveSequenceNumber != (++trisNum))
            {
                throw new Exception("序列号不一致,应该："+
                    RealReceiveSequenceNumber.ToString() + " 接收:" + trisNum.ToString());
            }
            //只要正常接收，就说明对之前的进行正常确认
            NoReciveAckNum ++;
        }
        /// <summary>
        /// 确认更新接收序列号
        /// </summary>
        /// <param name="reciveNum">来自于S帧的，对方的接收序列号</param>
        public void AckReceiveSequenceNumber(UInt16 reciveNun)
        {
            //对方的接收序号与自己的发送序号相同则确认发送序号
            
            AckNum = reciveNun;
            NoTransimitAckNum = (ushort)(TransmitSequenceNumber - RealReceiveSequenceNumber);
        }
        
    }
}
