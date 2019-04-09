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
   public class B40190Tests
   {
      private B40190 b40190;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "40190.xls");
         destinationFilePath = Path.Combine(reportDirectoryPath, "40190_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xls");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b40190 = new B40190(destinationFilePath, "2018/10/12");
      }



      [TestMethod()]
      public void Wf40191Test()
      {
         string msgText = b40190.Wf40191();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf40192Test()
      {
         string msgText = b40190.Wf40192();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf40193Test()
      {
         string msgText = b40190.Wf40193();
         Assert.IsNotNull(msgText);
      }

   }
}