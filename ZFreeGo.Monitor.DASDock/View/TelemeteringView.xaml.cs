﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZFreeGo.Monitor.DASDock.ViewModel;

namespace ZFreeGo.Monitor.DASDock.View
{
    /// <summary>
    /// DataGridPageView.xaml 的交互逻辑
    /// </summary>
    public partial class TelemeteringView : PageFunction<String>
    {
        public TelemeteringView()
        {
            this.DataContext = new TelemeteringViewModel();
            InitializeComponent();
        }
    }
}