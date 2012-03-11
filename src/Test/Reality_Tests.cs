using NUnit.Framework;
using Shouldly;

namespace Test
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public class Reality_Tests
    {
        [Test]
        public void true_should_be_true()
        {
            true.ShouldBe(true);
        }
    }
}
