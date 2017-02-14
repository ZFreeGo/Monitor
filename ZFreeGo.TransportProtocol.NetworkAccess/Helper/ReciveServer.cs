using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZFreeGo.TransportProtocol.NetworkAccess.Helper
{
    /// <summary>
    /// 接收服务用于接收文件
    /// </summary>
    public class ReciveServer<T>
    {
         /// <summary>
        /// 接收缓冲 一级缓冲
        /// </summary>
        public Queue<T> RecivePacketQuene;

        /// <summary>
        /// 接收缓冲 二级缓冲队列
        /// </summary>
        protected Queue<T> mReciveQuene;

        /// <summary>
        /// 接收线程
        /// </summary>
        protected Thread mReadThread;

        /// <summary>
        /// 判别线程
        /// </summary>
        protected Thread mServerThread;



        /// <summary>
        /// 存在数据信号量
        /// </summary>
        protected ManualResetEvent mExistData;

        /// <summary>
        /// 获取服务状态
        /// </summary>
        protected bool serverState = false;
        /// <summary>
        /// 获取服务状态
        /// </summary>
        public bool ServerState
        {
            get
            {
                return serverState;
            }
            private set
            {
                serverState = value;
            }
        }
        /// <summary>
        /// 服务初始化
        /// </summary>
        /// <param name="overTime">重复次数</param>
        /// <param name="maRepeat">最大重复次数</param>
        public ReciveServer(int overTime, int maxRepeat)
        {
            InitData();
        }
        /// <summary>
        /// 服务初始化,默认超时时间5000ms，重复次数3次
        /// </summary>
        public  ReciveServer() : this(5000, 3)
        {

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private virtual void InitData()
        {
            mExistData = new ManualResetEvent(false);
            serverState = false;           
           
            RecivePacketQuene = new Queue<T>();
            mReciveQuene = new Queue<T>();
        }

        /// <summary>
        /// 信息入队
        /// </summary>
        /// <param name="packet">文件数据包</param>
       public  virtual void Enqueue(T packet)
        {
            lock (RecivePacketQuene)
            {
                RecivePacketQuene.Enqueue(packet);                
            }

        }

       /// <summary>
       /// 停止服务
       /// </summary>
       public virtual void StopServer()
       {
           mExistData.Close();
           if (mReadThread != null)
           {
               mReadThread.Join(500);
               mReadThread.Abort();

           }
           if (mServerThread != null)
           {
               mServerThread.Join(500);
               mServerThread.Abort();
           }
           serverState = false;
       }

        /// <summary>
        /// TcpRead进程,从Tcp连接中读取顺序
        /// </summary>
        public  virtual void ReciveThread()
        {
            try
            {
                try
                {
                    while (true)
                    {
                        Thread.Sleep(100);
                        //等待请求数据信号量请求数据                            


                        //二级缓存数据为空，再从一级缓存转存数据
                        lock (RecivePacketQuene)
                        {
                            while (RecivePacketQuene.Count > 0)  //转存到二级缓冲
                            {
                                mReciveQuene.Enqueue(RecivePacketQuene.Dequeue());
                            }
                            if (mReciveQuene.Count > 0)
                            {
                                mExistData.Set(); //发送有数据请求
                            }
                            else
                            {
                                mExistData.Reset();
                            }
                        }
                    }                    
                }
                catch (ObjectDisposedException ex)
                {
                    Console.WriteLine("ReciveThread" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    Console.WriteLine("ReciveThread" + ex.Message);
                    Thread.ResetAbort();
                }
            }
            catch (Exception ex)
            {
                while (true)
                {
                    Console.WriteLine("ReciveThread" + ex.Message);
                    Thread.Sleep(100);
                }
            }
        }
        /// <summary>
        /// 召唤文件目录服务进程
        /// </summary>
        public  virtual void ServeThread()
        {
            try
            {
                try
                {             
                    do
                    {                       
                        if (mExistData.WaitOne())     //等待有数据信号
                        {
                            //检测数据不符合应答规范则返回
                            if (!CheckData())
                            {                               
                                continue;
                            }                            
                        }
                     } while (true);
                }
                catch (ObjectDisposedException ex)
                {
                    serverState = false;
                    Console.WriteLine("ReciveThread:" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    serverState = false;
                    Console.WriteLine("ReciveThread" + ex.Message);
                    Thread.ResetAbort();
                }
            }
            catch (Exception ex)
            {
                serverState = false;
                Console.WriteLine("ReciveThread:" + ex.Message);
            }

        }

       

        /// <summary>
        /// 检测接收数据
        /// </summary>
        /// <returns>true--检测通过，false--检测失败</returns>
        public virtual bool CheckData()
        {
            throw new NotImplementedException();
        }
    }
}
