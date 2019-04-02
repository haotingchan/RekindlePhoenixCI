using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessObjects;
using Common.Config;
using DataObjects;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30506Tests
   {
      private B30506 b30506;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30506.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30506_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }

      [TestInitialize]
      public void Setup()
      {
         b30506 = new B30506(reportDirectoryPath, destinationFilePath, "2018/08", "2018/10");
      }

      [TestMethod()]
      public void WF30506Test()
      {
         string msgText = b30506.WF30506();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf30507Test()
      {
         string msgText = b30506.WF30507();
         Assert.IsNotNull(msgText);
      }

   }
}