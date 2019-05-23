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
   public class D50032Tests
   {
      [TestMethod()]
      public void List50032Test()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.Sbrkno = "       ";
         d500Xx.Ebrkno = "ZZZZZZZ";
         d500Xx.ProdCategory = "%";
         d500Xx.ProdKindIdSto = "%";
         d500Xx.Sdate = "201808";
         d500Xx.Edate = "201809";
         DataTable data = new D50032().List50032(d500Xx);
         Assert.IsNotNull(data);
      }
   }
}