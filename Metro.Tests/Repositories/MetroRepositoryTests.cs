using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Metro.Repositories.Tests
{
    [TestClass()]
    [DeploymentItem(@"Data/metro.xlsx")]
    public class MetroRepositoryTests
    {
        [TestMethod("#1 Fájlt megtalálja az egységteszt projekt")]
        public void FileExists()
        {

            string myfile = "metro.xlsx";
            bool exists = File.Exists(myfile);
            Assert.IsTrue(exists);
        }

        [TestMethod("#2 Állomások tér névvel teszt")]
        public void AllomasTezt()
        {
            var repo = new MetroRepository("metro.xlsx");
            var parkMegallo = repo.Allomasok.FindAll(x => x.AllomasNev.Contains("tér")).Count;
            Assert.IsFalse(parkMegallo < 5);
        }


        [TestMethod("#3 Vonalak számának tesztje")]
        public void VonalTest()
        {
            var repo = new MetroRepository("metro.xlsx");
            int elvart = 4;
            int eredmeny = repo.MetroVonalak.Count;
            Assert.AreEqual(elvart, eredmeny);
        }

    }
}