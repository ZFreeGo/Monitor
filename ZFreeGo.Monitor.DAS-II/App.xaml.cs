using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using ZFreeGo.Monitor.AutoStudio.StartupUI;

namespace ZFreeGo.Monitor.AutoStudio
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
           // SplashScreen s = new SplashScreen("pictures/sojo-c.jpg");
           // s.Show(false);
           // s.Close(new TimeSpan(0, 0, 5));

           // var start = new StartupInterface();
            //start.


            base.OnStartup(e);
        }
    }
}
