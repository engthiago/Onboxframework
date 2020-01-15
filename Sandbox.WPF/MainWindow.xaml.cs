using Onbox.Core.V1;
using Onbox.Core.V1.Http;
using Onbox.Di.V1;
using Onbox.Mvc.V1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Onbox.Sandbox.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : ViewMvcBase
    {
        public BitmapSource Image { get; set; }
        public string SomethingToSearch { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        public override void OnInit()
        {
            SomethingToSearch = "You can type anything here...";
        }

        public async override void OnAfterInit()
        {
            await this.PerformAsync(GetImageAsync);
        }

        public async Task GetImageAsync()
        {
            var container = Container.Default();

            container.AddOnboxCore();

            var httpService = container.Resolve<IHttpService>();
            var uri = "https://codeproject.freetls.fastly.net/App_Themes/CodeProject/Img/logo250x135.gif";
            using (var stream = await httpService.GetStreamAsync(uri))
            {
                Image = Mvc.V1.Utils.ImageUtils.ConvertToBitmapSource(stream);
            }

        }

    }
}
