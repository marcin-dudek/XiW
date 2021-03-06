﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using XiW.Core;
﻿using MahApps.Metro.Controls;

namespace XiW
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly XiConnection _xiConnection;

        public MainWindow()
        {
            InitializeComponent();

            _xiConnection = new XiConnection(@"C:\src\XiW\xi-core\xi-core.exe");
            _xiConnection.Send(Method.New, new { });
        }
    }
}
