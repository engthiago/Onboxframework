using Onbox.Mvc.Abstractions.VDev;
using Onbox.Mvc.Revit.Abstractions.VDev;
using Onbox.Mvc.Revit.VDev;
using Onbox.Revit.Abstractions.VDev;
using System.Reflection;

namespace RevitApplication.Views
{
    /// <summary>
    /// A contract a view designed to have Revit as parent window
    /// </summary>
    public interface IHelloWpfView : IRevitMvcViewBase, IMvcViewModal
    {
    }

    /// <summary>
    /// A view designed to have Revit as parent window
    /// </summary>
    public partial class HelloWpfView : RevitMvcViewBase, IHelloWpfView
    {
        public string AppName { get; set; }

        // You can inject any service that you have added to the container in constructors
        public HelloWpfView(IRevitAppData revitAppData) : base(revitAppData)
        {
            InitializeComponent();
            AppName = Assembly.GetExecutingAssembly().GetName().Name;
        }
    }
}