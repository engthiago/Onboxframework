using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using NUnit.Framework;
using Onbox.Abstractions.VDev;
using Onbox.Core.VDev.Json;
using Onbox.Di.VDev;

namespace Onbox.Revit.Tests.JsonService
{
    [Category("Json Service Extensions")]
    public class JsonExtensionsShould
    {
        private Container CreateContainer()
        {
            return new Container();
        }

        [Test]
        public void AddItselfToContainerAsAbstraction()
        {
            var sut = CreateContainer();
            sut.AddJson();

            var jsonService = sut.Resolve<IJsonService>();
            Assert.NotNull(jsonService);
        }


        [Test]
        public void AddItselfToContainerWithSettings()
        {
            var sut = CreateContainer();
            sut.AddJson(config =>
            {
                config.NullValueHandling = NullValueHandling.Ignore;
                config.DefaultValueHandling = DefaultValueHandling.Ignore;
                config.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });

            var settings = sut.Resolve<JsonSerializerSettings>();

            Assert.That(settings.NullValueHandling, Is.EqualTo(NullValueHandling.Ignore));
            Assert.That(settings.ReferenceLoopHandling, Is.EqualTo(ReferenceLoopHandling.Ignore));
        }

        [Test]
        public void AcceptNullConfigurations()
        {
            var sut = CreateContainer();
            sut.AddJson(null);

            var settings = sut.Resolve<JsonSerializerSettings>();

            Assert.That(settings.NullValueHandling, Is.EqualTo(NullValueHandling.Ignore));
            Assert.That(settings.ReferenceLoopHandling, Is.EqualTo(ReferenceLoopHandling.Ignore));
        }

        [Test]
        public void UseCamelCaseByDefault()
        {
            var sut = CreateContainer();
            sut.AddJson();

            var settings = sut.Resolve<JsonSerializerSettings>();
            var contractResolver = settings.ContractResolver;

            Assert.That(contractResolver.GetType(), Is.EqualTo(typeof(CamelCasePropertyNamesContractResolver)));
        }

    }
}
