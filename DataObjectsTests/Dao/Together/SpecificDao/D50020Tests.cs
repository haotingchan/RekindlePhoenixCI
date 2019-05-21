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
   public class D50020Tests
   {
      [TestMethod()]
      public void List50020Test()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.SumType = "M";
         d500Xx.SumSubType = "5";
         d500Xx.DataType = "R";
         d500Xx.SortType = "F";
         d500Xx.GbGroup = "rb_gall";
         DataTable data = new D50020().List50020(d500Xx);
         Assert.IsNotNull(data);
      }

      [TestMethod()]
      public void ListACCUTest()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.Sdate = "20181001";
         d500Xx.Edate = "20181101";
         d500Xx.SumType = "M";
         d500Xx.SumSubType = "5";
         d500Xx.DataType = "R";
         d500Xx.SortType = "F";
         d500Xx.GbGroup = "rb_gall";
         DataTable data = new D50020().ListACCU(d500Xx);
         Assert.IsNotNull(data);
      }

      [TestMethod()]
      public void ListACCUAHTest()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.Sdate = "20181001";
         d500Xx.Edate = "20181101";
         d500Xx.SumType = "M";
         d500Xx.SumSubType = "5";
         d500Xx.DataType = "R";
         d500Xx.SortType = "F";
         d500Xx.GbGroup = "rb_gall";
         DataTable data = new D50020().ListACCUAH(d500Xx);
         Assert.IsNotNull(data);
      }

      [TestMethod()]
      public void ListAHTest()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.SumType = "M";
         d500Xx.SumSubType = "5";
         d500Xx.DataType = "R";
         d500Xx.SortType = "F";
         d500Xx.GbGroup = "rb_gall";
         DataTable data = new D50020().ListAH(d500Xx);
         Assert.IsNotNull(data);
      }
   }
}