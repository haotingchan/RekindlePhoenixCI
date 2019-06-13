using PhoenixCI.BusinessLogic.Prefix4;
using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix4.Tests
{
   [TestClass()]
   public class B40012Tests
   {
      private B40012 b40012;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "40012old.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "40012_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b40012 = new B40012("40012", destinationFilePath, "2018/10/11");
      }


      [TestMethod()]
      public void Wf40012FutureSheetTest()
      {
         string msgText = b40012.WfFutureSheet();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf40012OptionSheetTest()
      {
         string msgText = b40012.WfOptionSheet();
         Assert.IsNotNull(msgText);
      }
   }
}