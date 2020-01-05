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

        public TestWindow(IMessageService messageService)
        {
            this.InitializeComponent();
            this.messageService = messageService;
        }

        public async override void OnInit()
        {
        }

        public override async Task OnInitAsync()
        {
            await this.PerformAsync(async () =>
            {
                await Task.Delay(2000);
                this.messageService.Show("Resolved inside of window");
            });
        }
    }

    public interface ITestWindow : IViewMvc
    {
    }
}
