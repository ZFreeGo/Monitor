using GalaSoft.MvvmLight;

namespace ZFreeGo.Monitor.DASModel.DataItemSet
{
   
    public class StateMessage : ViewModelBase
    {
        private string netMessage;

        /// <summary>
        /// 网络信息-表现网络状态
        /// </summary>
        public string NetMessage
        {
            get
            {
                return netMessage;
            }
            set
            {
                netMessage = value;
                RaisePropertyChanged("NetMessage");

            }
        }



        private string netRawData;

        /// <summary>
        /// 通讯原始接收信息 16进制显示
        /// </summary>
        public string NetRawData
        {
            get
            {
                return netRawData;
            }
            set
            {
                netRawData = value;
                RaisePropertyChanged("NetRawData");

            }
        }

        private string protoclMessage;

        /// <summary>
        /// 规约解析信息
        /// </summary>
        public string ProtoclMessage
        {
            get
            {
                return protoclMessage;
            }
            set
            {
                protoclMessage = value;
                RaisePropertyChanged("ProtoclMessage");

            }
        }
        private string exceptionTrace;

        /// <summary>
        /// 异常跟踪信息
        /// </summary>
        public string ExceptionTrace
        {
            get
            {
                return exceptionTrace;
            }
            set
            {
                exceptionTrace = value;
                RaisePropertyChanged("ExceptionTrace");

            }
        }


        /// <summary>
        /// 追加更新NetRawData
        /// </summary>
        /// <param name="str">信息</param>
        /// <param name="flag">true-发送，false-接收</param>
        public void AddNetRawData(string str, bool flag)
        {
            if(flag)
            {
                NetRawData += "发送:\n" + str + "\n\n";
            }
            else
            {
                NetRawData += "接收:\n" + str + "\n\n";
            }

        }
        /// <summary>
        /// 追加更新ProtoclMessage
        /// </summary>
        /// <param name="str">信息</param>
        /// <param name="flag">true-发送，false-接收</param>
        public void AddProtoclMessage(string str, bool flag)
        {
            if (flag)
            {
                ProtoclMessage += "发送:\n" + str + "\n\n";
            }
            else
            {
                ProtoclMessage += "接收:\n" + str + "\n\n";
            }

        }
        public void AddProtoclMessage(string str)
        {            
            ProtoclMessage += "发送:\n" + str + "\n\n";          
            

        }
        public void AddNetMessage(string str)
        {
            NetMessage +=  str + "\n\n";


        }
        public void AddExcptionMessage(string str)
        {
            ExceptionTrace += str + "\n\n";
        }
        public void AddExcptionMessage(string comment, System.Exception ex)
        {
            ExceptionTrace += comment + "\n";
            ExceptionTrace += ex.Message + "\n";
            ExceptionTrace += ex.StackTrace + "\n";
        }

        /// <summary>
        /// Initializes a new instance of the StateMessage class.
        /// </summary>
        public StateMessage()
        {
            netMessage = "";
            netRawData = "";
            protoclMessage = "";
            exceptionTrace = "";
        }

    }
}