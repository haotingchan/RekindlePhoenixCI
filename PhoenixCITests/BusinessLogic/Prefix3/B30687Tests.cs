using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30687Tests
   {
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         destinationFilePath = Path.Combine(reportDirectoryPath, $@"30687_{DateTime.Now.ToString("yyyy.MM.dd")}Test.csv");
      }
      [TestMethod()]
      public void WF30687RuNew全部盤別全部時段Test()
      {
         string msgText = new B30687(destinationFilePath, "2018/10/11", "2018/10/11","",2,2).WF30687RuNew();
         Assert.IsNotNull(msgText);
      }
   }
}