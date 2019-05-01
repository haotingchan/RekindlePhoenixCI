using PhoenixCI.BusinessLogic.Prefix3;
using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30290Tests
   {
      private B30290 b30290;
      private static string reportDirectoryPath, destinationFilePath;

      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30290.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30290_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }

      [TestInitialize]
      public void Setup()
      {
         b30290 = new B30290("30290");
      }

      [TestMethod()]
      public void Wf30290gbfTest()
      {
         string msgText = b30290.Wf30290gbf(destinationFilePath, "20181101", "2018/09/28");
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf30290NStockTest()
      {
         string msgText = b30290.Wf30290NStock(destinationFilePath, "20181101", "2018/09/28");
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf30290StockTest()
      {
         string msgText = b30290.Wf30290Stock(destinationFilePath, "20181101", "2018/09/28");
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void LastQuarterTest()
      {
         string ymd = b30290.LastQuarter(new DateTime(2018, 10, 11));
         Assert.AreEqual("2018/09/28", ymd);
      }

   }
}