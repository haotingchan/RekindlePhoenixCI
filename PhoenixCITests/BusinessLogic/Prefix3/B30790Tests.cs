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
   public class B30790Tests
   {
      private B30790 b30790;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30790.xls");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30790_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xls");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b30790 = new B30790(destinationFilePath, "2017/10/01", "2017/10/11", "2017/10/01", "2017/10/11");
      }

      [TestMethod()]
      public void Wf30790Test()
      {
         string msgText = b30790.Wf30790();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf30790fourTest()
      {
         string msgText = b30790.Wf30790four();
         Assert.IsNotNull(msgText);
      }

   }
}