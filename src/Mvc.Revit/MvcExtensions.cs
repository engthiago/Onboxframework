using Onbox.Abstractions.VDev;
using Onbox.Mvc.VDev;
using Onbox.Mvc.VDev.Messaging;

namespace Onbox.Mvc.Revit.VDev
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