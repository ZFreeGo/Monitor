using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZFreeGo.TransmissionProtocols.Helper
{
    /// <summary>
    /// 
    /// </summary>
    public class ReciveSendServer<T>
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
        /// 请求信号量
        /// </summary>
        protected ManualResetEvent mRequestData;

        /// <summary>
        /// 存在数据信号量
        /// </summary>
        protected ManualResetEvent mExistData;


        /// <summary>
        /// 超时时间 ms
        /// </summary>
        protected  int mOverTime;

        /// <summary>
        /// 重试次数
        /// </summary>
        protected  int mRepeatCount;

        /// <summary>
        /// 最大重试次数
        /// </summary>
        protected  int  mRepeatMaxCount;

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
        public ReciveSendServer(int overTime, int maxRepeat)
        {
            InitData();
            mOverTime = overTime;
            mRepeatMaxCount = maxRepeat;
        }
        /// <summary>
        /// 服务初始化,默认超时时间5000ms，重复次数3次
        /// </summary>
        public  ReciveSendServer() : this(5000, 3)
        {

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        public virtual void InitData()
        {
            mRepeatCount = 0;
            mRequestData = new ManualResetEvent(false);
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
           mRequestData.Close();
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

           mReciveQuene = null;
           RecivePacketQuene = null;
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
                        if (mRequestData.WaitOne())
                        {
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
                }
                catch (ObjectDisposedException ex)
                {
                    StopServer();
                    Console.WriteLine("ReciveThread" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    StopServer();
                    Console.WriteLine("ReciveThread" + ex.Message);
                    Thread.ResetAbort();
                }
            }
            catch (Exception ex)
            {
                StopServer();
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
        public  virtual void ServerThread()
        {
            try
            {
                try
                {
                    if(!TransmitData())
                    {
                        StopServer();
                        return;
                    }
                    var statTime = DateTime.Now;
                    var responseTime = DateTime.Now;
                    do
                    {
                        mRequestData.Set();//发送请求数据信号

                        if (mExistData.WaitOne(mOverTime))     //等待有数据信号
                        {
                            mRequestData.Reset();//中断获取数据

                            //超时检测--处理有接收数据但不符合要求的情况
                            responseTime = DateTime.Now;
                            var diffTime = responseTime - statTime;
                            if (diffTime.TotalMilliseconds > mOverTime)
                            {
                                if (AckOverTime())
                                {
                                    continue;
                                }
                                else
                                {
                                    break;
                                }
                                

                            }
                            //检测数据不符合应答规范则返回
                            if (!CheckData())
                            {
                                continue;
                            }
                            if (!AckOnTime())
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (!AckOverTime())
                            {
                                break;
                            }
                        }
                        if (!TransmitData())
                        {
                            StopServer();
                            break;
                        }
                        statTime = DateTime.Now; //设置开始时间
                    } while (true);
                }
                catch (ObjectDisposedException ex)
                {
                    
                    StopServer();
                    Console.WriteLine("ReciveThread:" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    StopServer();
                    Console.WriteLine("ReciveThread" + ex.Message);
                    Thread.ResetAbort();
                }
            }
            catch (Exception ex)
            {
                StopServer();
                Console.WriteLine("ReciveThread:" + ex.Message);
            }
            StopServer();
        }

        /// <summary>
        /// 准时应答后相应的事件
        /// </summary>
        /// <returns>true--正常执行，false--结束执行，终止当前线程</returns>
        public virtual bool  AckOnTime()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 超时后响应的事件
        /// </summary>
        /// <returns>true--正常执行，false--结束执行，终止当前线程</returns>
        public virtual bool AckOverTime()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 检测接收数据
        /// </summary>
        /// <returns>true--检测通过，false--检测失败</returns>
        public virtual bool CheckData()
        {
            throw new NotImplementedException();
        }

        
        /// <summary>
        /// 传输数据
        /// </summary>
        /// <returns>true--正常执行，false--执行异常，终止当前线程</returns>
        public virtual bool TransmitData()
        {
            throw new NotImplementedException();
        }
    }
}
