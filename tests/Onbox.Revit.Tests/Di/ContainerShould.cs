using NUnit.Framework;
using Onbox.Di.VDev;
using System;

namespace Onbox.Revit.Tests.Di
{
    [Category("Dependency Injection")]
    public class ContainerShould
    {
        private Container CreateContainer()
        {
            return new Container();
        }

        [TestCase]
        public void HaveOnlyDefaultConstructor()
        {
            var type = typeof(Container);
            var constructors = type.GetConstructors();
            Assert.AreEqual(constructors.Length, 1);
            var constructor = constructors[0];
            Assert.AreEqual(constructor.GetParameters().Length, 0);
        }

        [TestCase]
        public void RegisterSingletonInstanceImplementations()
        {
            var dummyInstance0 = new DummyService();
            var sut = CreateContainer();
            sut.AddSingleton(dummyInstance0);

            var dummyInstance1 = sut.Resolve<DummyService>();
            var dummyInstance2 = sut.Resolve<DummyService>();

            Assert.AreSame(dummyInstance0, dummyInstance1);
            Assert.AreSame(dummyInstance0, dummyInstance2);
            Assert.AreSame(dummyInstance1, dummyInstance2);
        }

        [TestCase]
        public void RegisterSingletonTypeImplementations()
        {
            var sut = CreateContainer();
            sut.AddSingleton<DummyService>();

            var dummyInstance1 = sut.Resolve<DummyService>();
            var dummyInstance2 = sut.Resolve<DummyService>();

            Assert.AreSame(dummyInstance1, dummyInstance2);
        }

        [TestCase]
        public void RegisterSingletonInstanceAbstractions()
        {
            var dummyInstance0 = new DummyService();
            var sut = CreateContainer();
            sut.AddSingleton<IDummyService>(dummyInstance0);

            var dummyInstance1 = sut.Resolve<IDummyService>();
            var dummyInstance2 = sut.Resolve<IDummyService>();

            Assert.AreSame(dummyInstance0, dummyInstance1);
            Assert.AreSame(dummyInstance0, dummyInstance2);
            Assert.AreSame(dummyInstance1, dummyInstance2);
        }

        [TestCase]
        public void CreateScopes()
        {
            var sut = CreateContainer();
            using (var scope = sut.CreateScope())
            {
                Assert.AreNotSame(sut, scope);
            }
        }

        [TestCase]
        public void RegisterScopedImplementations()
        {
            var sut = CreateContainer();
            sut.AddScoped<DummyService>();

            var dummyInstance1 = sut.Resolve<DummyService>();
            var dummyInstance2 = sut.Resolve<DummyService>();

            Assert.AreSame(dummyInstance1, dummyInstance2);

            using (var scope = sut.CreateScope())
            {
                var dummyInstance3 = scope.Resolve<DummyService>();
                Assert.AreNotSame(dummyInstance1, dummyInstance3);
            }
        }

        [TestCase]
        public void RegisterScopedAbstractions()
        {
            var sut = CreateContainer();
            sut.AddScoped<IDummyService, DummyService>();

            var dummyInstance1 = sut.Resolve<IDummyService>();
            var dummyInstance2 = sut.Resolve<IDummyService>();

            Assert.AreSame(dummyInstance1, dummyInstance2);

            using (var scope = sut.CreateScope())
            {
                var dummyInstance3 = scope.Resolve<DummyService>();
                Assert.AreNotSame(dummyInstance1, dummyInstance3);
            }
        }

        [TestCase]
        public void RegisterTransientImplementations()
        {
            var sut = CreateContainer();
            sut.AddTransient<DummyService>();

            var dummyInstance1 = sut.Resolve<DummyService>();
            var dummyInstance2 = sut.Resolve<DummyService>();

            Assert.AreNotSame(dummyInstance1, dummyInstance2);
        }

        [TestCase]
        public void RegisterTransientAbstractions()
        {
            var sut = CreateContainer();
            sut.AddTransient<IDummyService, DummyService>();

            var dummyInstance1 = sut.Resolve<IDummyService>();
            var dummyInstance2 = sut.Resolve<IDummyService>();

            Assert.AreNotSame(dummyInstance1, dummyInstance2);
        }

        [TestCase]
        public void ResolveTransientsEvenWhenNotRegistered()
        {
            var sut = CreateContainer();
            var dummyInstance = sut.Resolve<DummyService>();
            Assert.NotNull(dummyInstance);
        }

        [TestCase]
        public void NotRegisterAbstractionsWithNoImplementationsAsSingletons()
        {
            var sut = CreateContainer();
            Assert.Throws(
                typeof(InvalidOperationException),
                () => sut.AddSingleton<IDummyService>()
            );
        }

        [TestCase]
        public void NotRegisterAbstractionsWithNoImplementationsAsScoped()
        {
            var sut = CreateContainer();
            Assert.Throws(
                typeof(InvalidOperationException),
                () => sut.AddScoped<IDummyService>()
            );
        }

        [TestCase]
        public void NotRegisterAbstractionsWithNoImplementationsAsTransients()
        {
            var sut = CreateContainer();
            Assert.Throws(
                typeof(InvalidOperationException),
                () => sut.AddTransient<IDummyService>()
            );
        }

        [TestCase]
        public void NotResolveCircularServices()
        {
            var sut = CreateContainer();
            Assert.Throws(
                typeof(InvalidOperationException),
                () => sut.Resolve<CircularService1>()
            );
        }

        [TestCase]
        public void NotResolveRelatedCircularServices()
        {
            var sut = CreateContainer();
            Assert.Throws(
                typeof(InvalidOperationException),
                () => sut.Resolve<CircularService2>()
           );
        }

        [TestCase]
        public void CleanItself()
        {
            var sut = CreateContainer();
            sut.AddSingleton<DummyService>();

            var dummyService1 = sut.Resolve<DummyService>();

            sut.Clear();

            var dummyService2 = sut.Resolve<DummyService>();

            Assert.AreNotSame(dummyService1, dummyService2);
        }
    }
}
