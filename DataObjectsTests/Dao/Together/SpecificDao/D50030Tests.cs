using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D50030Tests
   {
      [TestMethod()]
      public void ListD50030Test()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.SumType = "M";
         d500Xx.SumSubType = "5";
         d500Xx.DataType = "Q";
         d500Xx.SortType = "F";
         d500Xx.Sdate = "201810";
         d500Xx.Edate = "201810";
         DataTable data = new D50030().List50030(d500Xx);
         Assert.IsNotNull(data);
      }
   }
}