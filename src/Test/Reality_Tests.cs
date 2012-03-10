using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Shouldly;

namespace Test
{
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
