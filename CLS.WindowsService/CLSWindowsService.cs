using CLS.WindowsService.Classes;
using System;
using System.ServiceProcess;
using System.Threading;

namespace CLS.WindowsService
{
    public partial class CLSWindowsService : ServiceBase
    {
        public CLSWindowsService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            var databaseWatcher = new Thread(Processor.ProcessTriggers);
            databaseWatcher.Start();
        }

        protected override void OnStop()
        {
        }
    }
}
