using DataObjects.Dao.Together.SpecificDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Config;
using BusinessObjects;
using System;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D40040Tests
   {
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);
      }

      [TestMethod()]
      public void GetListTest()
      {
         DataTable dt = new D40040().ListData(new DateTime(2018, 10, 12), "1%");
         //2018/10/12的資料有15筆
         Assert.AreEqual(15, dt.Rows.Count);
      }

      [TestMethod()]
      public void GetDateLastSheet1Test()
      {
         DateTime dateTime = new D40040().GetDateLast(new DateTime(2018, 10, 12), 1);
         //2018/10/12的前一交易日為2018/10/11
         Assert.AreEqual("2018/10/11", dateTime.ToString("yyyy/MM/dd"));
      }

      [TestMethod()]
      public void GetDateLastSheet2Test()
      {
         DateTime dateTime = new D40040().GetDateLast(new DateTime(2018, 10, 12), 1);
         //2018/10/12的前一交易日為2018/10/11
         Assert.AreEqual(new DateTime(2018, 10, 11), dateTime);
      }

      [TestMethod()]
      public void ListMg6DataTest()
      {
         DateTime dateTime = new D40040().GetDateLast(new DateTime(2018, 10, 12), 0);
         DataTable dt = new D40040().ListMg6Data(new DateTime(2018, 10, 12), dateTime, "%");
         //2018/10/12的資料有20筆
         Assert.AreEqual(20, dt.Rows.Count);
      }

      [TestMethod()]
      public void ListMg8DataTest()
      {
         DataTable dt = new D40040().ListMg8Data(new DateTime(2018, 10, 12), "1%");
         //2018/10/12的資料有8筆
         Assert.AreEqual(8, dt.Rows.Count);
      }

      [TestMethod()]
      public void ListDayDataTest()
      {
         DataTable dt = new D40040().ListDayData(new DateTime(2018, 10, 12));
         //2018/10/12的資料有36筆
         Assert.AreEqual(36, dt.Rows.Count);
      }

      [TestMethod()]
      public void ListEtfMg6DataTest()
      {
         DataTable dt = new D40040().ListEtfMg6Data(new DateTime(2018, 10, 12));
         //2018/10/12的資料有16筆
         Assert.AreEqual(16, dt.Rows.Count);
      }

      [TestMethod()]
      public void ListEtfDataTest()
      {
         DateTime dateTime = new D40040().GetDateLast(new DateTime(2018, 10, 12), 1);
         DataTable dt = new D40040().ListEtfData(new DateTime(2018, 10, 12), dateTime, "1%");
         //2018/10/12的資料有4筆
         Assert.AreEqual(4, dt.Rows.Count);
      }

      [TestMethod()]
      public void ListSpanSvDataTest()
      {
         //Excel讀取Value
         string ExcelCellVal = "2014/12/1";
         DateTime dateTime = DateTime.MinValue;
         DateTime.TryParse(ExcelCellVal, out dateTime);
         DataTable dt = new D40040().ListSpanSvData(new DateTime(2018, 10, 12), dateTime, "1%");
         //2018/10/12的資料有7筆
         Assert.AreEqual(7, dt.Rows.Count);
      }

      [TestMethod()]
      public void ListSpanSdDataTest()
      {
         //Excel讀取Value
         string ExcelCellVal = "2014/12/1";
         DateTime dateTime = DateTime.MinValue;
         DateTime.TryParse(ExcelCellVal, out dateTime);
         DataTable dt = new D40040().ListSpanSdData(new DateTime(2018, 10, 12), dateTime, "1%");
         //2018/10/12的資料有9筆
         Assert.AreEqual(9, dt.Rows.Count);
      }
   }
}