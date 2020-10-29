using NUnit.Framework;
using NUnit.Framework.Internal;
using Onbox.Di.VDev;
using System;
using System.Collections.Generic;

namespace Onbox.Revit.Tests.Di
{
    [Category("Dependency Injection")]
    public class ContainerShould
    {
        private Container CreateContainer()
        {
            return new Container();
        }

        [Test]
        public void HaveOnlyDefaultConstructor()
        {
            var type = typeof(Container);
            var constructors = type.GetConstructors();
            Assert.AreEqual(constructors.Length, 1);
            var constructor = constructors[0];
            Assert.AreEqual(constructor.GetParameters().Length, 0);
        }

        [Test]
        public void ResolveATypeDirectly()
        {
            using (var sut = CreateContainer())
            {
                var dummyInstance = sut.Resolve(typeof(DummyService));
                Assert.That(dummyInstance, Is.Not.Null);
            }
        }

        [Test]
        public void RegisterSingletonInstanceImplementations()
        {
            using (var sut = CreateContainer())
            {
                var dummyInstance0 = new DummyService();
                sut.AddSingleton(dummyInstance0);

                var dummyInstance1 = sut.Resolve<DummyService>();
                var dummyInstance2 = sut.Resolve<DummyService>();

                Assert.AreSame(dummyInstance0, dummyInstance1);
                Assert.AreSame(dummyInstance0, dummyInstance2);
                Assert.AreSame(dummyInstance1, dummyInstance2);
            }
        }

        [Test]
        public void RegisterSingletonTypeImplementations()
        {
            using (var sut = CreateContainer())
            {
                sut.AddSingleton<DummyService>();

                var dummyInstance1 = sut.Resolve<DummyService>();
                var dummyInstance2 = sut.Resolve<DummyService>();

                Assert.AreSame(dummyInstance1, dummyInstance2);
            }
        }

        [Test]
        public void RegisterSingletonInstanceAbstractions()
        {
            using (var sut = CreateContainer())
            {
                var dummyInstance0 = new DummyService();
                sut.AddSingleton<IDummyService>(dummyInstance0);

                var dummyInstance1 = sut.Resolve<IDummyService>();
                var dummyInstance2 = sut.Resolve<IDummyService>();

                Assert.AreSame(dummyInstance0, dummyInstance1);
                Assert.AreSame(dummyInstance0, dummyInstance2);
                Assert.AreSame(dummyInstance1, dummyInstance2);
            }
        }

        [Test]
        public void RegisterSingletonInstanceAbstractionWithConcreteType()
        {
            using (var sut = CreateContainer())
            {
                sut.AddSingleton<IDummyService, DummyService>();

                var dummyInstance1 = sut.Resolve<IDummyService>();
                var dummyInstance2 = sut.Resolve<IDummyService>();

                Assert.AreSame(dummyInstance1, dummyInstance2);
            }
        }

        [Test]
        public void RegisterSingletonInstanceAbstractionWithConcreteTypeInstance()
        {
            using (var sut = CreateContainer())
            {
                var dummyInstance0 = new DummyService();
                sut.AddSingleton<IDummyService, DummyService>(dummyInstance0);

                var dummyInstance1 = sut.Resolve<IDummyService>();
                var dummyInstance2 = sut.Resolve<IDummyService>();

                Assert.AreSame(dummyInstance0, dummyInstance1);
                Assert.AreSame(dummyInstance0, dummyInstance2);
                Assert.AreSame(dummyInstance1, dummyInstance2);
            }
        }

        [Test]
        public void CreateScopes()
        {
            using (var sut = CreateContainer())
            {
                using (var scope = sut.CreateScope())
                {
                    Assert.AreNotSame(sut, scope);
                }
            }
        }

        [Test]
        public void ReportIsInScope()
        {
            using (var sut = CreateContainer())
            {
                using (var scope = sut.CreateScope())
                {
                    Assert.That(scope.IsScope(), Is.True);
                }
            }
        }

        [Test]
        public void ReportIsNotInScope()
        {
            using (var sut = CreateContainer())
            {
                Assert.That(sut.IsScope(), Is.False);

                using (var scope = sut.CreateScope())
                {
                    Assert.That(sut.IsScope(), Is.False);
                }
            }
        }

        [Test]
        public void ResolveSameSingletonTypesEvenOutsideOfScope()
        {
            using (var sut = CreateContainer())
            {
                sut.AddSingleton<DummyService>();

                DummyService dummyService0;
                using (var scope = sut.CreateScope())
                {
                    dummyService0 = scope.Resolve<DummyService>();
                }

                var dummyService1 = sut.Resolve<DummyService>();
                Assert.That(dummyService0, Is.EqualTo(dummyService1));
            }
        }

        [Test]
        public void ResolveSameSingleInstanceEvenOutsideOfScope()
        {
            using (var sut = CreateContainer())
            {
                var dummyService0 = new DummyService();
                sut.AddSingleton(dummyService0);

                DummyService dummyService1;
                using (var scope = sut.CreateScope())
                {
                    dummyService1 = scope.Resolve<DummyService>();
                    Assert.That(dummyService0, Is.EqualTo(dummyService1));
                }

                // Just to confirm after disposing the scope
                Assert.That(dummyService0, Is.EqualTo(dummyService1));

                var dummyService2 = sut.Resolve<DummyService>();
                Assert.That(dummyService0, Is.EqualTo(dummyService2));
                Assert.That(dummyService1, Is.EqualTo(dummyService2));
            }
        }


        [Test]
        public void RegisterScopedImplementations()
        {
            using (var sut = CreateContainer())
            {
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
        }

        [Test]
        public void RegisterScopedAbstractions()
        {
            using (var sut = CreateContainer())
            {
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
        }

        [Test]
        public void RegisterTransientImplementations()
        {
            using (var sut = CreateContainer())
            {
                sut.AddTransient<DummyService>();

                var dummyInstance1 = sut.Resolve<DummyService>();
                var dummyInstance2 = sut.Resolve<DummyService>();

                Assert.AreNotSame(dummyInstance1, dummyInstance2);
            }
        }

        [Test]
        public void RegisterTransientAbstractions()
        {
            using (var sut = CreateContainer())
            {
                sut.AddTransient<IDummyService, DummyService>();

                var dummyInstance1 = sut.Resolve<IDummyService>();
                var dummyInstance2 = sut.Resolve<IDummyService>();

                Assert.AreNotSame(dummyInstance1, dummyInstance2);
            }
        }

        [Test]
        public void ResolveTransientsEvenWhenNotRegistered()
        {
            using (var sut = CreateContainer())
            {
                var dummyInstance = sut.Resolve<DummyService>();
                Assert.NotNull(dummyInstance);
            }
        }

        [Test]
        public void ResolveDifferentTransientSpeciallyOnDifferentScopes()
        {
            using (var sut = CreateContainer())
            {
                sut.AddTransient<IDummyService, DummyService>();

                var dummyInstance1 = sut.Resolve<IDummyService>();
                using (var scope = sut.CreateScope())
                {
                    var dummyInstance2 = scope.Resolve<IDummyService>();
                    Assert.AreNotSame(dummyInstance1, dummyInstance2);
                }
            }
        }

        [Test]
        public void ResolveDependentTypes()
        {
            using (var sut = CreateContainer())
            {
                var dependentType = sut.Resolve<DependentService>();
                Assert.NotNull(dependentType);
            }
        }

        [Test]
        public void NotRegisterAbstractionsWithNoImplementationsAsSingletons()
        {
            using (var sut = CreateContainer())
            {
                Assert.Throws(
                    typeof(InvalidOperationException),
                    () => sut.AddSingleton<IDummyService>()
                    );
            }
        }

        [Test]
        public void NotRegisterAbstractionsWithNoImplementationsAsScoped()
        {
            using (var sut = CreateContainer())
            {
                Assert.Throws(
                    typeof(InvalidOperationException),
                    () => sut.AddScoped<IDummyService>()
                    );
            }
        }

        [Test]
        public void NotRegisterAbstractionsWithNoImplementationsAsTransients()
        {
            using (var sut = CreateContainer())
            {
                Assert.Throws(
                     typeof(InvalidOperationException),
                     () => sut.AddTransient<IDummyService>()
                    );
            }
        }

        [Test]
        public void NotDirectlyResolveAbstractTypes()
        {
            using (var sut = CreateContainer())
            {
                Assert.Throws(
                    typeof(KeyNotFoundException),
                    () => sut.Resolve<AbstractDummyService>()
                    );
            }
        }

        [Test]
        public void NotRegisterAbstractTypes()
        {
            using (var sut = CreateContainer())
            {
                Assert.Throws(
                    typeof(InvalidOperationException),
                    () => sut.AddSingleton<AbstractDummyService>()
                    );
            }
        }

        [Test]
        public void NotResolveCircularTypes()
        {
            using (var sut = CreateContainer())
            {
                Assert.Throws(
                    typeof(InvalidOperationException),
                    () => sut.Resolve<CircularService1>()
                    );
            }
        }

        [Test]
        public void NotResolveRelatedCircularTypes()
        {
            using (var sut = CreateContainer())
            {
                Assert.Throws(
                    typeof(InvalidOperationException),
                    () => sut.Resolve<CircularService2>()
                    );
            }
        }

        [Test]
        public void NotResolveHiddenContructorTypes()
        {
            using (var sut = CreateContainer())
            {
                Assert.Throws(
                    typeof(InvalidOperationException),
                    () => sut.Resolve<DummyHiddenConstructor>()
                    );
            }
        }

        [Test]
        public void CleanItself()
        {
            using (var sut = CreateContainer())
            {
                sut.AddSingleton<DummyService>();
                var dummyService1 = sut.Resolve<DummyService>();

                sut.Clear();

                var dummyService2 = sut.Resolve<DummyService>();
                Assert.AreNotSame(dummyService1, dummyService2);
            }
        }
    }
}
