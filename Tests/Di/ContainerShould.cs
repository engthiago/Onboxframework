using NUnit.Framework;
using Onbox.Di.V5;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Di
{
    public class TestClass : ITestClass
    {
        public string Name { get; set; }
    }

    public abstract class TestAbstract : ITestClass
    {
    }

    public interface ITestClass
    {
    }

    public class CircularSelfDependency
    {
        public CircularSelfDependency(CircularSelfDependency circularSelfDependency)
        {
        }
    }

    public class CircularDependencyMain
    {
        public CircularDependencyMain(CircularDependencyNested classNested)
        {

        }
    }

    public class CircularDependencyNested
    {
        public CircularDependencyNested(CircularDependencyMain classMain)
        {

        }
    }

    public class NoConstructorClass
    {
        private NoConstructorClass()
        {
        }
    }

    public class SimpleConstructorClass
    {
    }

    public class ConstructorWithDependencies
    {
        public ConstructorWithDependencies(SimpleConstructorClass simpleConstructorClass)
        {
        }
    }

    public class SiteService
    {
        public SiteService(CoordinateService coordinateService, DetailsService detailsService)
        {

        }
    }

    public class CoordinateService
    {
        public CoordinateService(AngleService angleService, GlobeService globeService)
        {

        }
    }

    public class AngleService
    {
    }

    public class GlobeService
    {
    }

    public class DetailsService
    {
        public DetailsService(CoordinateService coordinateService)
        {
        }
    }



    [TestFixture]
    public class ContainerShould
    {
        Container sut;

        public ContainerShould()
        {
        }

        [OneTimeSetUp]
        public void WarmUp()
        {
            for (int i = 0; i < 100; i++)
            {
                using (var container = Container.Default())
                {
                }
            }
        }

        [SetUp]
        public void ReturnContainer()
        {
            sut = Container.Default();
        }

        [Test]
        public void ReturnDefaultContainer()
        {
            Assert.That(sut, Is.AssignableTo<IContainer>());
        }

        [Test]
        public void NotAddInterfacesAsSingleton()
        {
            Assert.That(() => sut.AddSingleton<ITestClass>(), Throws.Exception);
        }

        [Test]
        public void NotAddAbstractAsSingleton()
        {
            Assert.That(() => sut.AddSingleton<TestAbstract>(), Throws.Exception);
        }

        [Test]
        public void NotResolveAbstractIfNotAddedImplementation()
        {
            Assert.That(() => sut.Resolve<TestAbstract>(), Throws.Exception);
        }

        [Test]
        public void NotResolveCircularSelfDependencies()
        {
            Assert.That(() => sut.Resolve<CircularSelfDependency>(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void NotResolveCircularDependencies()
        {
            Assert.That(() => sut.Resolve<CircularDependencyMain>(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void ResolveScopedDependencies()
        {
            Assert.DoesNotThrow(() => sut.Resolve<SiteService>());
        }

        [Test]
        public void NotResolveNoConstructorClasses()
        {
            Assert.That(() => sut.Resolve<NoConstructorClass>(), Throws.InstanceOf<InvalidOperationException>());
        }

        [Test]
        public void NotResolveAnInterfaceWhichWasNotAdded()
        {
            Assert.That(() => sut.Resolve<ITestClass>(), Throws.InstanceOf<KeyNotFoundException>());
        }

        [Test]
        public void ResolveSimpleClasses()
        {
            var simpleClass = sut.Resolve<SimpleConstructorClass>();
            Assert.That(simpleClass, Is.Not.Null
                                            .And
                                            .InstanceOf<SimpleConstructorClass>());
        }

        [Test]
        public void ResolveConstructorWithDependencyClasses()
        {
            var simpleClass = sut.Resolve<ConstructorWithDependencies>();
            Assert.That(simpleClass, Is.Not.Null
                                            .And
                                            .InstanceOf<ConstructorWithDependencies>());
        }

        [Test]
        public void AddSingletonAsInstance()
        {
            var test1 = new TestClass();
            sut.AddSingleton(test1);

            var test2 = sut.Resolve<TestClass>();
            Assert.That(test1, Is.SameAs(test2));
        }

        [Test]
        public void AddSingletonAsType()
        {
            sut.AddSingleton<TestClass>();
            
            var test1 = sut.Resolve<TestClass>();
            var test2 = sut.Resolve<TestClass>();

            Assert.That(test1, Is.SameAs(test2));
        }

        [Test]
        public void AddTransientAsType()
        {
            sut.AddTransient<TestClass>();

            var test1 = sut.Resolve<TestClass>();
            test1.Name = "Thiago";

            var test2 = sut.Resolve<TestClass>();

            Assert.That(test1, Is.Not.SameAs(test2));
            Assert.That(test1.Name, Is.Not.EqualTo(test2.Name));
        }

        [Test]
        public void AddSingletonAsInterfaceAndType()
        {
            sut.AddSingleton<ITestClass, TestClass>();

            var test = sut.Resolve<ITestClass>();
            Assert.That(test, Is.Not.Null);
        }

        [Test]
        public void AddSingletonAsInterfaceAndTypeAndInstance()
        {
            var test1 = new TestClass();
            sut.AddSingleton<ITestClass, TestClass>(test1);

            var test2 = sut.Resolve<ITestClass>();
            Assert.That(test2, Is.Not.Null);
            Assert.That(test1, Is.SameAs(test2));
        }

        [Test]
        public void AddTransientAsInterfaceAndType()
        {
            sut.AddTransient<ITestClass, TestClass>();

            var test1 = sut.Resolve<ITestClass>();
            var test2 = sut.Resolve<ITestClass>();
            Assert.That(test1, Is.Not.SameAs(test2));
        }

        [Test]
        public void ClearItself()
        {
            sut.Clear();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            sut.Dispose();
        }

        [Test]
        public void DisposeUsingStatement()
        {
            using (var container = Container.Default())
            {
            }
        }

    }
}
