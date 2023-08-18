using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace WinCommander
{
    public partial class MainWindow : Window
    {
        private readonly ShortcutCatcher ShortcutCatcher;
        private readonly WindowManager WindowManager;

        public MainWindow()
        {
            InitializeComponent();
            Closing += OnWindowClosing;

            ShortcutCatcher = new ShortcutCatcher(); ShortcutCatcher.StartCatching();
            ShortcutCatcher.OnRecordRequest += ShortcutCatcher_OnRecordRequest;
            ShortcutCatcher.OnSimulateRequest += ShortcutCatcher_OnSimulateRequest;


            WindowManager = new WindowManager();

        }

        private void ShortcutCatcher_OnRecordRequest(object sender, IntPtr e)
        {
            richtextbox1.AppendText("Recording: " + WinApiHandler.GetWindowTitle(e));
            _ = WindowManager.Record(e);
        }
        private void ShortcutCatcher_OnSimulateRequest(object sender, IntPtr e)
        {
            richtextbox1.AppendText("Simulating: " + WinApiHandler.GetWindowTitle(e));
            WindowManager.Simulate(e);
        }


        public void OnWindowClosing(object sender, CancelEventArgs e)
        {
            ShortcutCatcher.StopCatching();
            WindowManager.ReleaseAll();
        }
    }
}
