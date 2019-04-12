using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30520Tests
   {
      private B30520 b30520;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30520.xls");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30520_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xls");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b30520 = new B30520(destinationFilePath, "2018/08");
      }
      [TestMethod()]
      public void Wf30521Test()
      {
         string msgText = b30520.Wf30521();
         Assert.IsNotNull(msgText);
      }
   }
}