namespace Onbox.Mvc.Abstractions.VDev
{
    /// <summary>
    /// A base contract for a view
    /// </summary>
    public interface IMvcLifecycleView : IMvcLifecycleComponent
    {
        void OnAfterInit();
    }
}