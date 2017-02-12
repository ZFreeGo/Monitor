using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZFreeGo.TransmissionProtocol.NetworkAccess104.FileSever
{
    /// <summary>
    /// 文件传输服务---一个文件使用一个服务
    /// </summary>
    public class FileTransmissionServer
    {
         /// <summary>
        /// 接收缓冲 一级缓冲
        /// </summary>
        public Queue<FilePacket> RecivePacketQuene;

        /// <summary>
        /// 接收缓冲 二级缓冲队列
        /// </summary>
        protected Queue<FilePacket> mReciveQuene;


       



        /// <summary>
        /// 接收线程
        /// </summary>
        protected Thread mReadThread;


        /// <summary>
        /// 请求信号量
        /// </summary>
        protected ManualResetEvent mRequestData;

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
        /// 文件传输服务初始化
        /// </summary>
        public FileTransmissionServer()
        {
            RecivePacketQuene = new Queue<FilePacket>();
            mReciveQuene = new Queue<FilePacket>();
        }




        /// <summary>
        /// 信息入队
        /// </summary>
        /// <param name="packet">文件数据包</param>
        public void Enqueue(FilePacket packet)
        {
            lock (RecivePacketQuene)
            {
                RecivePacketQuene.Enqueue(packet);                
            }

        }
        /// <summary>
        /// TcpRead进程,从Tcp连接中读取顺序
        /// </summary>
        protected void FilePackeReciveThread()
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
                    Console.WriteLine("FilePackeReciveThread" + ex.Message);
                    Thread.Sleep(100);
                }
                catch (ThreadAbortException ex)
                {
                    Console.WriteLine("FilePackeReciveThread" + ex.Message);
                    Thread.ResetAbort();

                }

            }
            catch (Exception ex)
            {
                while (true)
                {
                    Console.WriteLine("FilePackeReciveThread" + ex.Message);
                    Thread.Sleep(100);
                }

            }
        }




    }
}
