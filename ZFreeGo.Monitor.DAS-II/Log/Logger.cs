using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace ZFreeGo.Monitor.AutoStudio.Log
{
    /// <summary>
    /// 日志
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// 日志消息
        /// </summary>
        private List<SingleLogMessage> logMessage;

        /// <summary>
        /// 日志保存文件夹路径
        /// </summary>
        private string logSaveDirectoryPath;

        private string logCurrentSavePath;
        /// <summary>
        /// 日志初始化
        /// </summary>
        public Logger(string path)
        {
            logMessage = new List<SingleLogMessage>();
            AddMessage("", "启动程序，创建日志");
            logSaveDirectoryPath = path;

            var name = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".log";
            logCurrentSavePath = Path.Combine(logSaveDirectoryPath, name);
        }

        /// <summary>
        /// 增加一条日志消息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="content">内容</param>
        public void AddMessage(string userName, string content)
        {
            logMessage.Add(new SingleLogMessage(userName, content));
            checkLenAndSave(100);
        }

        /// <summary>
        /// 增加一条日志消息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <param name="content">内容</param>
        public void AddMessage(string userName, string content, LogType inType)
        {
            logMessage.Add(new SingleLogMessage(userName, content, inType));
            checkLenAndSave(100);
        }
        /// <summary>
        /// 增加一条日志消息
        /// </summary>
        /// <param name="message">消息</param>
        public void AddMessage(SingleLogMessage message)
        {
            logMessage.Add(message);
            checkLenAndSave(100);
        }

        /// <summary>
        /// 检查条数若超过则存盘,并清空当前日志
        /// </summary>
        /// <param name="len">条数</param>
        public void checkLenAndSave(int len)
        {
            if (logMessage.Count > len)
            {
                SaveLog();
                logMessage.Clear();
            }
        }
        /// <summary>
        /// 存储日志文件
        /// </summary>
        /// <param name="flag">是否清空列表，true-清空，false-保留</param>
        public void SaveLog(bool flag = true)
        {
            //先按照普通txt存储
            using (StreamWriter writer = new StreamWriter(logCurrentSavePath, true))
            {
                foreach(var m in logMessage)
                {
                    writer.WriteLine(m);
                }
            }
            if (flag)
            {
                logMessage.Clear();
            }
        }
        
    }
}
