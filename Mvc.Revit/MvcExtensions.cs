using Onbox.Core.V7.Messaging;
using Onbox.Core.V7.Reporting;
using Onbox.Di.V7;
using Onbox.Mvc.V7;
using Onbox.Mvc.V7.Messaging;

namespace Onbox.Mvc.Revit.V7
{
    /// <summary>
    /// Adds Mvc Services to the Container
    /// </summary>
    public static class MvcExtensions
    {
        /// <summary>
        /// Adds <see cref="IProgressIndicator"/> as <see cref="RevitProgressIndicatorView"/>, <see cref="IMessageService"/> as <see cref="MessageBoxService"/>, and <see cref="INavigator"/> as <see cref="Navigator"/>
        /// </summary>
        public static IContainer AddRevitMvc(this IContainer container)
        {
            container.AddTransient<IProgressIndicator, RevitProgressIndicatorView>();
            container.AddTransient<IMessageService, MessageBoxService>();

            container.AddSingleton<INavigator, Navigator>();

            return container;
        }
    }
}
