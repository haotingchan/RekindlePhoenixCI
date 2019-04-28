using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Config;
using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D4001xTests
   {

      [TestMethod()]
      public void ConcreteDaoTest()
      {
         ID4001x d4001X = new D4001x().ConcreteDao("40011");
         Assert.IsNotNull(d4001X);
      }
   }
}