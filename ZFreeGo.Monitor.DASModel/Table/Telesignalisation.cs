using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;

namespace ZFreeGo.Monitor.DASModel.Table
{

    /// <summary>
    /// 这个类是为了遥信参数类
    /// </summary>
    public class Telesignalisation : ObservableObject
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
                RaisePropertyChanged("InternalID");
                RaisePropertyChanged("TelesignalisationID");
            }
        }

        private string telesignalisationName;
        public string TelesignalisationName
        {
            get { return telesignalisationName; }
            set
            {
                telesignalisationName = value;
                RaisePropertyChanged("TelesignalisationName");
            }
        }


        private int telesignalisationID;
        public int TelesignalisationID
        {
            get
            {
                return (int)(BasicAddress + internalID); 
            }
            set
            {
                telesignalisationID = value;
                
            }
        }

        private int telesignalisationResult;
        public int TelesignalisationResult
        {
            get { return telesignalisationResult; }
            set
            {
                telesignalisationResult = value;
                RaisePropertyChanged("TelesignalisationResult");
                RaisePropertyChanged("TelesignalisationState");
            }
        }
        private string telesignalisationState;
        public string TelesignalisationState
        {
            get
            {
                int result = telesignalisationResult;

                if (IsSingle) //但双点信息判断
                {
                    if (IsNot != "是")
                    {
                        result = 1 - result; //取反1+0 等于1
                    }
                    if (result == 0)
                    {
                        return StateA;
                    }
                    else
                    {
                        return StateB;
                    }
                }
                else
                {
                    if (IsNot != "是")
                    {
                        result = 3 - result; //取反1+2等于3
                    }
                    if (result == 1)
                    {
                        return StateA;
                    }
                    else
                    {
                        return StateB;
                    }
                }
            }
            set
            {

                // telesignalisationState = value;
                // RaisePropertyChanged("TelesignalisationStatet"));
            }
        }
        private string isNot;
        /// <summary>
        /// 是否取反 "是" "否"
        /// </summary>
        public string IsNot
        {
            get { return isNot; }
            set
            {
                if ((value == "是") || (value == "否"))
                {
                    isNot = value;
                    RaisePropertyChanged("IsNot");
                    RaisePropertyChanged("Comment");
                    RaisePropertyChanged("TelesignalisationState");
                }

            }
        }

        private string date;
        public string Date
        {
            get { return date; }
            set
            {
                date = value;
                RaisePropertyChanged("Date");
            }
        }

        private string comment;
        public string Comment
        {
            get
            {
                string str = "";
                string sA = StateA;
                string sB = StateB;
                int nA = 0; //单点信息--默认开
                int nB = 1;

                if (IsNot == "是") //是否取反判断
                {
                    sA = StateB;
                    sB = StateA;
                }
                if (!IsSingle) //但双点信息判断
                {
                    nA = 1;
                    nB = 2;
                }

                str = string.Format("{0:00}-{1}; {2:00}-{3};", nA, sA, nB, sB);

                return str;
            }
            set
            {
                comment = value;
            }

        }
        private bool isChanged;
        /// <summary>
        /// 属性值是否被改变，true-初始化后被改变，false-没有
        /// </summary>
        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }

        /// <summary>
        /// 状态A--对应双点01/ 单点0
        /// </summary>
        private string stateA;
        public string StateA
        {
            get { return stateA; }
            set
            {
                stateA = value;
                RaisePropertyChanged("StateA");
                RaisePropertyChanged("Comment");
            }
        }

        /// <summary>
        /// 状态A--对应双点10/ 单点1
        /// </summary>
        private string stateB;
        public string StateB
        {
            get { return stateB; }
            set
            {
                stateB = value;
                RaisePropertyChanged("StateB");
                RaisePropertyChanged("Comment");
            }
        }

        /// <summary>
        /// 单点或双点信息
        /// </summary>
        private bool isSingle;

        /// <summary>
        ///设置或获取单点或双点 true--单点，false--双点
        /// </summary>
        public bool IsSingle
        {
            get
            {
                return isSingle;
            }
            set
            {
                isSingle = true;
                RaisePropertyChanged("Comment");
            }
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
        /// <param name="inStateA">状态A</param>
        /// <param name="inStateB">状态B</param>
        public Telesignalisation(int internalID, string telesignalisationName, int telesignalisationID, string isNot, int telesignalisationResult,
            string date, string comment, string inStateA, string inStateB)
            : this(internalID, telesignalisationName, telesignalisationID, isNot, telesignalisationResult,
            date, comment)
        {
            StateA = inStateA;
            StateB = inStateB;
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
        public Telesignalisation(int internalID, string telesignalisationName, int telesignalisationID, string isNot, int telesignalisationResult,
            string date, string comment)
        {
            InternalID = internalID;
            TelesignalisationName = telesignalisationName;
            TelesignalisationID = telesignalisationID;
            IsNot = isNot;
            Date = date;
            // Comment = comment;
            TelesignalisationResult = telesignalisationResult;
            TelesignalisationState = "";
            TelesignalisationID = (int)(Telesignalisation.BasicAddress + internalID - 1);
            isChanged = false;

            IsSingle = true;//默认为单点信息
        }



        public override string ToString()
        {
            return telesignalisationName + "(" + telesignalisationID + ")";
        }

       


        
    }
}
