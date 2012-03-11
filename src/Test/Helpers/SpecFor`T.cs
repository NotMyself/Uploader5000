using NUnit.Framework;

namespace Test.Helpers
{
    // ReSharper disable InconsistentNaming

    [TestFixture]
    public abstract class SpecFor<T>
    {
        protected T subject;

        public abstract void Context();
        public abstract void Because();
        public virtual void CleanUp() { }

        [SetUp]
        public void SetUp()
        {
            Context();
            Because();
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

    }
}
