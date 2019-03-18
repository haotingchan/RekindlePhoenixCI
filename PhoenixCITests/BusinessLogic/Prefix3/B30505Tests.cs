using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30505Tests
   {
      private B30505 b30505;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         destinationFilePath = Path.Combine(reportDirectoryPath, $@"30505_{DateTime.Now.ToString("yyyy.MM.dd")}Test.csv");

      }
      [TestInitialize]
      public void Setup()
      {
         b30505 = new B30505(destinationFilePath, "2018/10/11", "2018/10/21");
      }
      [TestMethod()]
      public void Wf30505Test()
      {
         string msgText = b30505.Wf30505();
         Assert.IsNotNull(msgText);
      }
   }
}