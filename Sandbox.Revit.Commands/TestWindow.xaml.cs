using Onbox.Core.V1.Logging;
using Onbox.Core.V1.Messaging;
using Onbox.Mvc.V1;
using System;
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
using System.Windows.Shapes;

namespace Onbox.Sandbox.Revit.Commands
{
    /// <summary>
    /// Interaction logic for TestWindow.xaml
    /// </summary>
    public partial class TestWindow : ViewMvcBase, ITestWindow
    {
        private readonly IMessageService messageService;
        private readonly ILoggingService loggingService;

        public TestWindow(IMessageService messageService, ILoggingService loggingService)
        {
            this.InitializeComponent();
            this.messageService = messageService;
            this.loggingService = loggingService;
        }

        public async override void OnInit()
        {
            CanRetryOnError = true;
            await this.PerformAsync(async () =>
            {
                await Task.Delay(2000);
                throw new Exception("Server error!");
            }, 
            error =>
            {
                Error = error.Message;
                Message = "Dont worry";
            });
        }

        private void OnRetry(object sender, RoutedEventArgs e)
        {
            OnInit();
        }

        public override bool CanCloseDialog()
        {
            return this.messageService.Question("Are you sure?");
        }

    }

    public interface ITestWindow : IViewMvc
    {
    }
}
