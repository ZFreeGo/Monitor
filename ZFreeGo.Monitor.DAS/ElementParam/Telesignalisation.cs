using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.ElementParam
{

    /// <summary>
    /// 这个类是为了遥信参数类
    /// </summary>
    public class Telesignalisation: INotifyPropertyChanged
    {
        /// <summary>
        /// 遥信对象公共地址
        /// </summary>
        public const UInt32 BasicAddress = 0x0001;

        private int internalID;
        public int InternalID
        {
            get { return internalID; }
            set
            {
                internalID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("InternalID"));
            }
        }

        private string telesignalisationName;
        public string TelesignalisationName
        {
            get { return telesignalisationName; }
            set
            {
                telesignalisationName = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TelesignalisationName"));
            }
        }


        private int telesignalisationID;
        public int TelesignalisationID
        {
            get { return telesignalisationID; }
            set
            {
                telesignalisationID = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TelesignalisationID"));
            }
        }

        private string telesignalisationResult;
        public string TelesignalisationResult
        {
            get { return telesignalisationResult; }
            set
            {
                telesignalisationResult = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TelesignalisationResult"));
            }
        }
        private string telesignalisationState;
        public string TelesignalisationState
        {
            get { return telesignalisationState; }
            set
            {
                telesignalisationState = value;
                OnPropertyChanged(new PropertyChangedEventArgs("TelesignalisationStatet"));
            }
        }
        private string isNot;
        /// <summary>
        /// 是否取反
        /// </summary>
        public string IsNot
        {
            get { return isNot; }
            set
            {
                isNot = value;
                OnPropertyChanged(new PropertyChangedEventArgs("IsNot"));
            }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Date"));
            }
        }

         private string comment;
        public string Comment
        {
            get { return comment; }
            set
            {
                comment = value;
                OnPropertyChanged(new PropertyChangedEventArgs("Comment"));
            }
        }
        private bool isChanged;
        /// <summary>
        /// 属性值是否被改变，true-初始化后被改变，false-没有
        /// </summary>
        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value;}
        }

       
        /// <summary>
        /// 遥信参数初始化
        /// </summary>
        /// <param name="internalID">内部ID</param>
        /// <param name="telesignalisationName">遥信名称</param>
        /// <param name="telesignalisationID">遥信ID</param>
        /// <param name="isNot">是否取反</param>
        /// <param name="date">日期</param>
        /// <param name="comment">注释</param>
        public Telesignalisation(int internalID, string telesignalisationName, int telesignalisationID, string isNot, string telesignalisationResult,
            string date,string comment)
        {
            InternalID = internalID;
            TelesignalisationName = telesignalisationName;
            TelesignalisationID = telesignalisationID;
            IsNot = isNot;
            Date = date;
            Comment = comment;
            TelesignalisationResult = telesignalisationResult;
            TelesignalisationState = "";
            

            TelesignalisationID = (int)( Telesignalisation.BasicAddress + internalID - 1);

            isChanged = false;
        }



        public override string ToString()
        {
            return telesignalisationName + " (" + telesignalisationID + ")";
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            isChanged = true;
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }
    }
}
