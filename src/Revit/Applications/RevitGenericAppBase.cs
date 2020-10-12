using Autodesk.Revit.UI;
using Onbox.Abstractions.VDev;
using Onbox.Di.VDev;
using Onbox.Revit.VDev.UI;

namespace Onbox.Revit.VDev.Applications
{
    /// <summary>
    /// The implementation of a generic IContainer type, if you like to use <see cref="Container"/>, you can use <see cref="RevitApp"/> implementation instead
    /// <para>IMPORTANT: Any children of this class should implement <see cref="ContainerProviderAttribute"/> as well</para>
    /// </summary>
    /// <typeparam name="TContainer">The contract for container implementations</typeparam>
    public abstract class RevitAppBase<TContainer> : RevitContainerProviderBase, IRevitExternalApp where TContainer : class, IContainer, new()
    {
        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit starts.
        /// </summary>
        public abstract Result OnStartup(IContainer container, UIControlledApplication application);

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit starts.
        /// </summary>
        public Result OnStartup(UIControlledApplication application)
        {
            var containerGuid = ContainerProviderReflector.GetContainerGuid(this);

            var container = new TContainer();
            this.InjectContainerToItself(container);

            this.HookUpContainer(container, containerGuid);
            this.HookupRevitContext(application, container);

            this.AddRevitUI(container, application);

            try
            {
                // Calls the client's Startup
                var result = OnStartup(container, application);
                if (result != Result.Succeeded)
                {
                    return result;
                }

                // Calls the client's CreateRibbon
                var imageManager = new ImageManager();
                var ribbonManager = new RibbonManager(application, imageManager);
                OnCreateRibbon(ribbonManager);

                return result;
            }
            catch
            {
                // If an exception the client's code, throw the exception to the stack
                throw;
            }
        }

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit shuts down.
        /// </summary>
        public abstract Result OnShutdown(IContainerResolver container, UIControlledApplication application);

        /// <summary>
        /// Implement this method to execute some tasks when Autodesk Revit shuts down.
        /// </summary>
        public Result OnShutdown(UIControlledApplication application)
        {
            var containerGuid = ContainerProviderReflector.GetContainerGuid(this);
            var container = containers[containerGuid];

            try
            {
                // Unhooks the events
                UnhookRevitContext(application, containerGuid);
                // Calls the client's Shotdown
                return OnShutdown(container, application);
            }
            catch
            {
                // If anything goes wrong with the client's code, throw the exception to the stack
                throw;
            }
            finally
            {
                // Unhooks and cleans the container even if an exception is thrown
                UnhookContainer(containerGuid, container);
            }
        }

        /// <summary>
        /// Lifecycle hook to create Ribbon UI when Revit starts.
        /// </summary>
        public virtual void OnCreateRibbon(IRibbonManager ribbonManager)
        {
        }

    }
}