namespace Onbox.Mvc.Abstractions.V7
{
    /// <summary>
    /// A base contract for a view
    /// </summary>
    public interface IMvcLifecycleView : IMvcLifecycleComponent
    {
        void OnAfterInit();
    }
}