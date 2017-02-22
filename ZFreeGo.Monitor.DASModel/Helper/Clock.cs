using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ZFreeGo.Monitor.AutoStudio.ElementParam;

namespace ZFreeGo.Monitor.DASModel.Helper
{
    public class Clock
    {
         /// <summary>
        /// 时间定时器
        /// </summary>
        private Timer timeClock;
            /// <summary>
        /// 时钟时间
        /// </summary>
        public ClockElement Time;

        /// <summary>
        /// 更新标志
        /// </summary>
        public bool UpdateFlag;

        /// <summary>
        /// 时钟
        /// </summary>
        public Clock()
        {
            timeClock = new Timer(1000);
            timeClock.Elapsed += timeClock_Elapsed;
            timeClock.Start();
            Time = new ClockElement(DateTime.Now);
            UpdateFlag = true;
        }
    

        /// <summary>
        /// 时钟定时器，按秒运作，检测网络是否正常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeClock_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (UpdateFlag == true)
            {
                Time.Update(DateTime.Now);
            }
        }
    }
}
