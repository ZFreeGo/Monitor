using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.Log
{
    /// <summary>
    /// 日志每条信息
    /// </summary>
    public class SingleLogMessage
    {
        /// <summary>
        /// 记录时间
        /// </summary>
        private DateTime time;

        /// <summary>
        /// 获取记录时间
        /// </summary>
        public DateTime Time
        {
            get
            {
                return time;
            }
        }

        /// <summary>
        /// 用户名
        /// </summary>
        private string userName;

        /// <summary>
        /// 获取用户名
        /// </summary>
        public string UserName
        {
            get
            {
                return userName;
            }
        }

        /// <summary>
        /// 操作内容
        /// </summary>
        private string content;

        /// <summary>
        /// 获取操作内容
        /// </summary>
        public string Content
        {
            get
            {
                return content;
            }
        }
        /// <summary>
        /// 操作结果
        /// </summary>
        private string result;

        /// <summary>
        /// 获取操作结果
        /// </summary>
        private string Result
        {
            get
            {
                return result;
            }
        }


        /// <summary>
        /// 日志消息类型
        /// </summary>
        private LogType type;

        /// <summary>
        /// 获取消息类型
        /// </summary>
        public LogType Type
        {
            get
            {
                return type;
            }
        }

        /// <summary>
        /// 记录消息初始化
        /// </summary>
        /// <param name="recordTime">记录时间</param>
        /// <param name="inUserName">记录用户</param>
        /// <param name="inContent">记录内容</param>
        /// <param name="inResult">记录结果</param>
        public SingleLogMessage(DateTime recordTime, string inUserName,
            string inContent, string inResult)
        {
            time = recordTime;
            userName = inUserName;
            content = inContent;
            result = inResult;
            type = LogType.Null;
        }
        /// <summary>
        /// 记录消息初始化
        /// </summary>
        /// <param name="recordTime">记录时间</param>
        /// <param name="inUserName">记录用户</param>
        /// <param name="inContent">记录内容</param>
        /// <param name="inResult">记录结果</param>
        /// <param name="inType">消息类型</param>
        public SingleLogMessage(DateTime recordTime, string inUserName,
            string inContent, string inResult, LogType inType)
        {
            time = recordTime;
            userName = inUserName;
            content = inContent;
            result = inResult;
            type = inType;
        }

        /// <summary>
        /// 记录消息初始化
        /// </summary>
        /// <param name="inUserName">记录用户</param>
        /// <param name="inContent">记录内容</param>
        /// <param name="inResult">记录结果</param>       
        public SingleLogMessage(string inUserName, string inContent, string inResult)
            :this(DateTime.Now, inUserName,  inContent, inResult)
        {

        }
        /// <summary>
        /// 记录消息初始化
        /// </summary>
        /// <param name="inUserName">记录用户</param>
        /// <param name="inContent">记录内容</param>
        public SingleLogMessage(string inUserName, string inContent)
            : this(DateTime.Now, inUserName, inContent, "")
        {

        }

        /// <summary>
        /// 记录消息初始化
        /// </summary>
        /// <param name="inUserName">记录用户</param>
        /// <param name="inContent">记录内容</param>
        /// <param name="inType">消息类型</param>
        public SingleLogMessage(string inUserName, string inContent, LogType inType)
            : this(DateTime.Now, inUserName, inContent, "", inType)
        {

        }
        /// <summary>
        /// 记录消息初始化
        /// </summary>
        /// <param name="inUserName">记录用户</param>
        /// <param name="inContent">记录内容</param>
        /// <param name="inResult">记录结果</param>
        /// <param name="inType">消息类型</param>
        public SingleLogMessage(string inUserName, string inContent, string inResult, LogType inType)
            : this(DateTime.Now, inUserName, inContent, inResult, inType)
        {

        }
        /// <summary>
        /// 获取单行信息
        /// </summary>
        /// <returns>生成单行信息</returns>
        public override string ToString()
        {
            StringBuilder strBuilder = new StringBuilder(128);

            strBuilder.Append(time.ToLongTimeString());
            strBuilder.Append("[-]");
            strBuilder.Append(userName);
            strBuilder.Append("[-]");
            strBuilder.Append(content);
            strBuilder.Append("[-]");
            strBuilder.Append(result);
            strBuilder.Append("[-]");
            strBuilder.Append(type.ToString());

            return strBuilder.ToString();
        }
     
    }
}

