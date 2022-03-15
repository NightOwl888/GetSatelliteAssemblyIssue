using NUnit.Framework;
using System.Globalization;
using System.IO;

namespace ICU4N.Tests
{
    [TestFixture]
    public class TestSatelliteAssemblies
    {
        [Test]
        public void TestExistingSatelliteAssembly()
        {
            // The normal case where the satellite assembly exists works as expected
            string culture = "en-US";
            var assembly = typeof(ICU4N.Class1).Assembly.GetSatelliteAssembly(new CultureInfo(culture));
            Assert.AreEqual(culture, assembly.GetName().CultureName);
        }

        [Test]
        public void TestDocumentedBehavior()
        {
            // Per the docs, loading a satellite assembly that doesn't exist should throw FileNotFoundException
            Assert.Throws<FileNotFoundException>(() => typeof(ICU4N.Class1).Assembly.GetSatelliteAssembly(new CultureInfo("en-CA")));
        }

        [Test]
        public void TestDesiredBehavior()
        {
            // Ideally, assembly would be null so we can implement our own fallback logic.
            // Throwing an exception only to catch it and return null is expensive.
            Assert.IsNull(typeof(ICU4N.Class1).Assembly.GetSatelliteAssembly(new CultureInfo("en-CA")));
        }

        [Test]
        public void TestActualBehavior()
        {
            // Actual behavior is illogical. We don't fallback to "en". We don't get an exception as per the docs.
            // We don't even get null (as would be ideal). Instead, we fall back to invariant culture.
            var invariantAssembly = typeof(ICU4N.Class1).Assembly.GetSatelliteAssembly(CultureInfo.InvariantCulture);
            var assembly = typeof(ICU4N.Class1).Assembly.GetSatelliteAssembly(new CultureInfo("en-CA"));
            Assert.AreEqual(invariantAssembly, assembly);
        }
    }
}
