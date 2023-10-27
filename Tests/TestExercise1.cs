using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace adonet
{
    [TestClass]
    public class TestExercise1
    {
        [TestMethod]
        public void Ex1_Search_All()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var result = r.Search(null);

            Assert.IsNotNull(result);
            Assert.AreEqual(10, result.Count);
            Assert.AreEqual(10, result.Select(p => p.Name).Distinct().Count());
        }

        [TestMethod]
        public void Ex1_Search_ByName()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var result = r.Search("leg");

            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(p => p.ID == 8));
        }

        [TestMethod]
        public void Ex1_FindByID_Found()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var result = r.FindById(4);


            Assert.IsNotNull(result);
            Assert.AreEqual(@"Fisher Price hammer toy", result.Name);
            Assert.AreEqual(27, result.VatPercentage);
            Assert.AreEqual(@"Months 18-24", result.CategoryName);
        }

        [TestMethod]
        public void Ex1_FindByID_NotFound()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var result = r.FindById(545647);

            Assert.IsNull(result);
        }

        [TestMethod]
        public void Ex1_Update()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var toUpdate = r.FindById(5);
            Assert.IsNotNull(toUpdate);

            var newName = System.Guid.NewGuid().ToString();
            toUpdate.Name = newName;
            r.Update(toUpdate);

            var result = r.FindById(5);
            Assert.IsNotNull(result);
            Assert.AreEqual(newName, result.Name);
            Assert.AreEqual(toUpdate.Price, result.Price);
        }

        [TestMethod]
        public void Ex1_Delete_Found()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var success = r.Delete(10);
            if (!success)
                Assert.Inconclusive("Deleting a record only works once... You should restore the database to test again.");
        }

        [TestMethod]
        public void Ex1_Delete_NotFound()
        {
            var r = new ProductRepository(TestConnectionStringHelper.SqlConnectionString);

            var success = r.Delete(545647);
            Assert.IsFalse(success);
        }
    }
}
