using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.TransmissionProtocols.Frame104
{
    public class ApplicationProtocalControlnformation
    {
        /// <summary>
        /// 启动字符，默认值为0x68
        /// </summary>
        private byte startCharacter = 0x68;
        /// <summary>
        /// 启动字符
        /// </summary>
        public byte StartCharacter
        {
          //  set { starCharacter = value; }
            get { return startCharacter; }
        }

        private byte apduLength;
        /// <summary>
        /// APDU(Application Protocol Data Unit)长度
        /// </summary>
        public byte APDULength
        {
            set { apduLength = value; }
            get { return apduLength; }
        }

        private byte controlDomain1;
        /// <summary>
        /// 控制域8位位组1 0bit-7bit 第1字节(共4字节)
        /// </summary>
        public byte ControlDomain1
        {
            set { controlDomain1 = value; }
            get { return controlDomain1; }
        }

        private byte controlDomain2;
        /// <summary>
        /// 控制域8位位组2 8bit-15bit 第2字节(共4字节)
        /// </summary>
        public byte ControlDomain2
        {
            set { controlDomain2 = value; }
            get { return controlDomain2; }
        }

        private byte controlDomain3;
        /// <summary>
        /// 控制域8位位组3 17bit-23bit 第3字节(共4字节)
        /// </summary>
        public byte ControlDomain3
        {
            set { controlDomain3 = value; }
            get { return controlDomain3; }
        }

        private byte controlDomain4;
        /// <summary>
        /// 控制域8位位组4  24bit-31bit 第4字节(共4字节)
        /// </summary>
        public byte ControlDomain4
        {
            set { controlDomain4 = value; }
            get { return controlDomain4; }
        }

 



       

        /// <summary>
        /// 原始数组
        /// </summary>
        public byte[] APCIArray;

        
       
        /// <summary>
        /// 获取APCI数据
        /// </summary>
        /// <returns>6字节数组</returns>
        public byte[] GetAPCIDataArray()
        {
            APCIArray =  new byte[6] { startCharacter, apduLength, controlDomain1, controlDomain2, controlDomain3, controlDomain4};
            return APCIArray;
        }

        /// <summary>
        /// 获取APCI数据长度
        /// </summary>
        /// <returns>数据长度</returns>
        public byte Length
        {
            get { return 6; }

        }

        /// <summary>
        /// 初始化APCI数据结构 
        /// </summary>
        /// <param name="starCharacter">启动字符</param>
        /// <param name="apduLength">APDU长度</param>
        /// <param name="controlDomain1">控制位域1</param>
        /// <param name="controlDomain2">控制位域2</param>
        /// <param name="controlDomain3">控制位域3</param>
        /// <param name="controlDomain4">控制位域4</param>
        public ApplicationProtocalControlnformation(byte starCharacter, byte apduLength,
            byte controlDomain1, byte controlDomain2, byte controlDomain3, byte controlDomain4)
        {
            this.startCharacter = starCharacter;
            APDULength = apduLength;
            ControlDomain1 = controlDomain1;
            ControlDomain2 = controlDomain2;
            ControlDomain3 = controlDomain3;
            ControlDomain4 = controlDomain4;
        }

        /// <summary>
        /// APCI初始化，使用数组强制初始化
        /// </summary>
        public ApplicationProtocalControlnformation(byte[] dataArray)
        {
            if (dataArray.Length >= 6)
            {
                APCIArray = dataArray;
                startCharacter = dataArray[0];
                apduLength = dataArray[1];
                controlDomain1 = dataArray[2];
                controlDomain2 = dataArray[3];
                controlDomain3 = dataArray[4];
                controlDomain4 = dataArray[5];

            }
            else
            {
                throw new Exception(" ApplicationProtocalControlnformation(byte[] dataArray)长度不小于6");
            }

        }
        /// <summary>
        /// 默认初始化
        /// </summary>
        public ApplicationProtocalControlnformation()
        {
            startCharacter = 0x68;
            apduLength = 4;
            controlDomain1 = 0x01;
            controlDomain2 = 0;
            controlDomain3 = 0;
            controlDomain4 = 0;
        }

    }
}
