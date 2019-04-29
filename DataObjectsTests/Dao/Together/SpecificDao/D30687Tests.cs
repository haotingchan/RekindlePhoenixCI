using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Config;
using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D30687Tests
   {

      [TestMethod()]
      public void ListRuNewDataTest()
      {
         DataTable data = new D30687().ListRuNewData("20181011","20181011","%%","%","A");
         Assert.IsNotNull(data);
      }
   }
}