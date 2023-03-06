using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Metro.Repositories.Tests
{
    [TestClass()]
    public class MetroRepositoryTests
    {
        [TestMethod]
        [DeploymentItem(@"Data/metro.xlsx")]
        public void FileExist()
        {

            var myfile = "metro.xlsx";
            Assert.IsTrue(File.Exists(myfile));
        }

        [TestMethod]
        public void VonalakSzama()
        {
            var repo = new MetroRepository("metro.xlsx");
            Assert.AreEqual(4, repo.MetroVonalak.Count);
        }

        [TestMethod]
        public void ParkokSzama()
        {
            var repo = new MetroRepository("metro.xlsx");
            var parkMegallo = repo.Allomasok.FindAll(x => x.AllomasNev.Contains("tér")).Count;
            Assert.IsFalse(parkMegallo < 5);
        }
    }
}