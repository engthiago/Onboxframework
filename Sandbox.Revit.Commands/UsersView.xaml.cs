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
    /// Interaction logic for UsersView.xaml
    /// </summary>
    public partial class UsersView : ViewMvcBase
    {
        private readonly IMessageService messageService;
        private readonly ILoggingService loggingService;

        public string UserName { get; set; }

        public UsersView(IMessageService messageService, ILoggingService loggingService)
        {
            this.InitializeComponent();
            this.messageService = messageService;
            this.loggingService = loggingService;
        }

        public async override void OnInit()
        {
            await this.PerformAsync(async () =>
            {
                await loggingService.Log("Retrieving user...");
                await Task.Delay(1000);
                this.UserName = "Eduardo";
                await loggingService.Log($"Retrieved user: {this.UserName}...");
            });
        }
    }
}
