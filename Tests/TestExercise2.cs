using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace adonet
{
    [TestClass]
    public class TestExercise2
    {
        [TestMethod]
        public void Ex2_NoConcurrency()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var toUpdate = r.FindById(6);
            Assert.IsNotNull(toUpdate);

            var newName = System.Guid.NewGuid().ToString();
            toUpdate.Name = newName;
            Assert.IsTrue(r.UpdateWithConcurrencyCheck(toUpdate));

            var result = r.FindById(6);
            Assert.IsNotNull(result);
            Assert.AreEqual(newName, result.Name);
            Assert.AreEqual(toUpdate.Price, result.Price);
        }

        [TestMethod]
        public void Ex2_WithConcurrency()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var productForUserA = r.FindById(7);
            Assert.IsNotNull(productForUserA);
            var productForUserB = r.FindById(7);
            Assert.IsNotNull(productForUserB);

            // first modification by user A
            var newNameForUserA = System.Guid.NewGuid().ToString();
            productForUserA.Name = newNameForUserA;
            Assert.IsTrue(r.UpdateWithConcurrencyCheck(productForUserA));

            // second ("concurrent") modification by user B
            var newNameForUserB = System.Guid.NewGuid().ToString();
            productForUserB.Name = newNameForUserB;
            Assert.IsFalse(r.UpdateWithConcurrencyCheck(productForUserB));

            // database state equals with state after change by user A
            var result = r.FindById(7);
            Assert.IsNotNull(result);
            Assert.AreEqual(newNameForUserA, result.Name);
        }
    }
}
