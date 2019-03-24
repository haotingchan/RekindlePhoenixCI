using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix4.Tests
{
   [TestClass()]
   public class B43033Tests
   {
      private B43033 b43033;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "43033.xls");
         destinationFilePath = Path.Combine(reportDirectoryPath, "43033_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xls");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b43033 = new B43033(destinationFilePath, "2019/02/27", "2019/03/15");
      }

      [TestMethod()]
      public void Wf43033Test()
      {
         string msgText = b43033.Wf43033();
         Assert.IsNotNull(msgText);
      }
   }
}