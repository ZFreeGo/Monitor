using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ZFreeGo.TransportProtocol.Xmodem
{
    /// <summary>
    /// 超时函数
    /// </summary>
    public class FunctionTimeout
    {
        /// <summary> 
        /// 信号量 
        /// </summary> 
        private ManualResetEvent manu = new ManualResetEvent(false);
        /// <summary> 
        /// 是否接受到信号 
        /// </summary> 

        private bool isGetSignal;
        /// <summary> 
        /// 设置超时时间 
        /// </summary> 
        private int timeout;
        /// <summary> 
        /// 要调用的方法的一个委托 
        /// </summary> 
        private  Action FunctionNeedRun;

        /// <summary>
        /// 及时回调
        /// </summary>
        private Action callBackOntime;

        /// <summary>
        /// 超时回调
        /// </summary>
        private Action callBackOvertime;


        private Action WhatTodo;

        private IAsyncResult mIAsyncResult;

        /// <summary>
        /// 手动终止
        /// </summary>
        public bool ManualAbort;

        /// <summary> 
        /// 构造函数，传入超时的时间以及运行的方法 
        /// </summary> 
        /// <param name="_action">动作方法</param> 
        /// <param name="_timeout">超时间</param> 
        public FunctionTimeout(Action _action, Action _actionOnTime, Action _actionOverTime, int _timeout)
        {
            FunctionNeedRun = _action;
            timeout = _timeout;
            callBackOntime = _actionOnTime;
            callBackOvertime = _actionOverTime;
            ManualAbort = false;
        }

        /// <summary> 
        /// 回调函数 
        /// </summary> 
        /// <param name="ar"></param> 
        private void MyAsyncCallback(IAsyncResult ar)
        {
            //isGetSignal为false,表示异步方法其实已经超出设置的时间，此时不再需要执行回调方法。 
            if (isGetSignal == false)
            {
                callBackOvertime();
                Console.WriteLine("放弃执行回调函数");
                Thread.CurrentThread.Abort();
            }
            else
            {
                callBackOntime();
                Console.WriteLine("调用回调函数");
            }
        }
        
        /// <summary>
        /// 停止信号量等待
        /// </summary>
        public void StopDelegate()
        {
            manu.Set();
            ManualAbort = true;
        }


        /// <summary> 
        /// 调用函数 
        /// </summary> 
        public void DoAction()
        {
             WhatTodo = CombineActionAndManuset;
            //通过BeginInvoke方法，在线程池上异步的执行方法。 
             mIAsyncResult = WhatTodo.BeginInvoke(MyAsyncCallback, null);
            //设置阻塞,如果上述的BeginInvoke方法在timeout之前运行完毕，则manu会收到信号。此时isGetSignal为true。 
            //如果timeout时间内，还未收到信号，即异步方法还未运行完毕，则isGetSignal为false。 
            isGetSignal = manu.WaitOne(timeout);

            if (isGetSignal == true)
            {
                Console.WriteLine("函数运行完毕，收到设置信号,异步执行未超时");
            }
            else
            {
                Console.WriteLine("没有收到设置信号,异步执行超时");
            }
        }

        /// <summary> 
        /// 把要传进来的方法，和 manu.Set()的方法合并到一个方法体。 
        /// action方法运行完毕后，设置信号量，以取消阻塞。 
        /// </summary> 
        private void CombineActionAndManuset()
        {
            FunctionNeedRun();
            manu.Set();
        }
    } 
}
