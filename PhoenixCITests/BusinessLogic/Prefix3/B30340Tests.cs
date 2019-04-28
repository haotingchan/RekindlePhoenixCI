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
   public class B30340Tests
   {
      private B30340 b30340;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30340.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30340_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b30340 = new B30340(destinationFilePath, "2005/10");
      }

      [TestMethod()]
      public void Wf30341Test()
      {
         string isCorrect = b30340.Wf30341();
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf30343Test()
      {
         string isCorrect = b30340.Wf30343();
         Assert.IsNotNull(isCorrect);
      }
   }
}