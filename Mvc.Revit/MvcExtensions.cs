using Onbox.Core.V7.Reporting;
using Onbox.Di.V7;
using Onbox.Mvc.V7;

namespace Onbox.Mvc.Revit.V7
{
    public static class MvcExtensions
    {
        public static IContainer AddRevitMvc(this IContainer container)
        {
            container.AddTransient<IProgressIndicator, RevitProgressIndicatorView>();
            container.AddSingleton<INavigator, Navigator>();

            return container;
        }
    }
}
