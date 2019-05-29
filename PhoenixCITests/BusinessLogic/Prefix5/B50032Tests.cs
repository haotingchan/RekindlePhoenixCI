using BusinessObjects;
using Common.Config;
using DataObjects;
using DataObjects.Dao.Together.SpecificDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoenixCI.BusinessLogic.Prefix5;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixCI.BusinessLogic.Prefix5.Tests
{
   [TestClass()]
   public class B50032Tests
   {
      private DataTable _data;
      private B50032 b50032;

      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);
      }

      [TestInitialize]
      public void Setup()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.Sbrkno = "       ";
         d500Xx.Ebrkno = "ZZZZZZZ";
         d500Xx.ProdCategory = "%";
         d500Xx.ProdKindIdSto = "%";
         d500Xx.Sdate = "201808";
         d500Xx.Edate = "201809";
         _data = new D50032().List50032(d500Xx);
         b50032 = new B50032();
      }

      [TestMethod()]
      public void CompareDataTest()
      {
         DataTable dt = b50032.CompareData(_data, "2");
         Assert.AreEqual(1945, dt.Rows.Count);
      }

      [TestMethod()]
      public void FilterDataTest()
      {
         DataTable dt = b50032.FilterData(_data, b50032.CompareData(_data, "2"));
         Assert.AreEqual(3890, dt.Rows.Count);
      }

      [TestMethod()]
      public void CompareDataByParallelTest()
      {
         DataTable dt = b50032.CompareDataByParallel(_data, "2");
         Assert.AreEqual(1945, dt.Rows.Count);
      }

      [TestMethod()]
      public void FilterDataByParallelTest()
      {
         DataTable dt = b50032.FilterDataByParallel(_data, b50032.CompareDataByParallel(_data, "2"));
         Assert.AreEqual(3890, dt.Rows.Count);
      }

      [TestMethod(), Timeout(5000)]
      public void 連續造市2個月平行處理時間測試Test()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.Sbrkno = "       ";
         d500Xx.Ebrkno = "ZZZZZZZ";
         d500Xx.ProdCategory = "%";
         d500Xx.ProdKindIdSto = "%";
         d500Xx.Sdate = "201808";
         d500Xx.Edate = "201809";
         DataTable datasource = new D50032().List50032(d500Xx);
         DataTable ids1 = b50032.CompareDataByParallel(datasource, "2");
         DataTable dt = b50032.FilterDataByParallel(datasource, ids1);
         Assert.AreEqual(3890,dt.Rows.Count);
      }

      [TestMethod(), Timeout(7000)]
      public void 連續造市3個月平行處理時間測試Test()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.Sbrkno = "       ";
         d500Xx.Ebrkno = "ZZZZZZZ";
         d500Xx.ProdCategory = "%";
         d500Xx.ProdKindIdSto = "%";
         d500Xx.Sdate = "201808";
         d500Xx.Edate = "201810";
         DataTable datasource = new D50032().List50032(d500Xx);
         DataTable ids1 = b50032.CompareDataByParallel(datasource, "3");
         DataTable dt = b50032.FilterDataByParallel(datasource, ids1);
         Assert.AreEqual(5604, dt.Rows.Count);
      }

      [TestMethod(), Timeout(12000)]
      public void 連續造市4個月平行處理時間測試Test()
      {
         D500xx d500Xx = new D500xx();
         d500Xx.Sbrkno = "       ";
         d500Xx.Ebrkno = "ZZZZZZZ";
         d500Xx.ProdCategory = "%";
         d500Xx.ProdKindIdSto = "%";
         d500Xx.Sdate = "201808";
         d500Xx.Edate = "201811";
         DataTable datasource = new D50032().List50032(d500Xx);
         DataTable ids1 = b50032.CompareDataByParallel(datasource, "4");
         DataTable dt = b50032.FilterDataByParallel(datasource, ids1);
         Assert.AreEqual(7404, dt.Rows.Count);
      }

   }
}