using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Config;
using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D30560Tests
   {

      [TestMethod()]
      public void List30561Test()
      {
         DataTable data = new D30560().List30561("2003", "2018", "201801", "201810");
         Assert.IsNotNull(data);
      }
   }
}