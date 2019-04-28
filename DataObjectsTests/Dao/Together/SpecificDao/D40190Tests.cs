using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Config;
using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D40190Tests
   {

      [TestMethod()]
      public void Get49191FUTTest()
      {
         DataTable data = new D40190().Get49191FUT(new System.DateTime(2018,10,12));
         Assert.IsNotNull(data);
      }
   }
}