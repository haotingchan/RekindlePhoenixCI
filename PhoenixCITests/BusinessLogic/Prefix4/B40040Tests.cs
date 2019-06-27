using PhoenixCI.BusinessLogic.Prefix4;
using BusinessObjects;
using Common.Config;
using DataObjects;
using DataObjects.Dao.Together.SpecificDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix4.Tests
{
   [TestClass()]
   public class B40040Tests
   {
      private B40040 b40040;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "40040.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "40040_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b40040 = new B40040(destinationFilePath, "2018/10/12", "1");
      }

      [TestMethod()]
      public void Wf40040_Test()
      {
         string msgText = b40040.WfSheetOne();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf40040ETF_Test()
      {
         string msgText = b40040.WfSheetTwo();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf40040SPAN_Test()
      {
         string msgText = b40040.Wf40040SPAN();
         Assert.IsNotNull(msgText);
      }

   }
}