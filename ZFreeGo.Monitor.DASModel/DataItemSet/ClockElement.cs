using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace ZFreeGo.Monitor.AutoStudio.ElementParam
{
    /// <summary>
    /// 时钟元素
    /// </summary>
    public class ClockElement : ObservableObject
    {

        private int year;
        public int Year
        {
            get { return year; }
            set
            {
               year = value;
                RaisePropertyChanged("Year");
            }
        }
        private int month;
        public int Month
        {
            get { return month; }
            set
            {
                month = value;
                RaisePropertyChanged("Month");
            }
        }

        private int dayOfMonth;
        public int DayOfMonth
        {
            get { return dayOfMonth; }
            set
            {
                dayOfMonth = value;
                RaisePropertyChanged("DayOfMonth");
            }
        }
        private int hour;
        public int Hour
        {
            get { return hour; }
            set
            {
                hour = value;
                RaisePropertyChanged("Hour");
            }
        }
        private int minute;
        public int Minute
        {
            get { return minute; }
            set
            {
                minute = value;
                RaisePropertyChanged("Minute");
            }
        }
        private int second;
        public int Second
        {
            get { return second; }
            set
            {
                second = value;
                RaisePropertyChanged("Second");
            }
        }

           private int dayOfWeek;
        public int DayOfWeek
        {
            get { return dayOfWeek; }
            set
            {
                dayOfWeek = value;
                RaisePropertyChanged("DayOfWeek");
            }
        }
    



        /// <summary>
        /// 获取指示时间
        /// </summary>
        public DateTime TimeClock
        {
            get
            {
                return new DateTime(year, month, dayOfMonth, hour, minute, second);
            }
        }

        /// <summary>
        /// 用时间进行初始化
        /// </summary>
        /// <param name="time">DateTime时间格式</param>
        public ClockElement(DateTime time)
        {
            Year = time.Year;
            Month = time.Month;
            DayOfMonth = time.Day;
            Hour = time.Hour;
            Minute = time.Minute;
            Second = time.Second;
            dayOfWeek = ((int)time.DayOfWeek == 0 ? 7 : (int)time.DayOfWeek);
        }
        /// <summary>
        /// 时钟初始化
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="hour">时</param>
        /// <param name="minute">分</param>
        /// <param name="second">秒</param>
        /// <param name="dayofweek">星期</param>
        public ClockElement(int year, int month, int day, int hour, int minute, int second, int dayofweek)
        {
            Year = year;
            Month = month;
            DayOfMonth = day;
            Hour = hour;
            Minute = minute;
            Second = second;
            DayOfWeek = dayofweek;
        }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <param name="time"></param>
        public void Update(DateTime time)
        {
            Year = time.Year;
            Month = time.Month;
            DayOfMonth = time.Day;
            Hour = time.Hour;
            Minute = time.Minute;
            Second = time.Second;
            DayOfWeek = ((int)time.DayOfWeek == 0 ? 7 : (int)time.DayOfWeek);
        }
        

        
    }
}
