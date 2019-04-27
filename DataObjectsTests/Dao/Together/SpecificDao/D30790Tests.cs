using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Config;
using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D30790Tests
   {
      [TestMethod()]
      public void MaxDateTest()
      {
         string data = new D30790().MaxDate();
         Assert.IsNotNull(data);
      }
   }
}