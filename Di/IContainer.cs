namespace Onbox.Di.V1
{
    public interface IContainer
    {
        void AddSingleton<TContract, TImplementation>(TImplementation instance);
        void AddSingleton<TImplementation>(TImplementation instance);
        void AddTransient<TContract, TImplementation>();
        void AddTransient<TImplementation>();
        void Reset();
        T Resolve<T>();
    }
}