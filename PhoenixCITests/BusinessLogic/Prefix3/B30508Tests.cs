using BusinessObjects;
using Common;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoenixCI.BusinessLogic.Prefix3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30508Tests
   {
      private B30508 b30508;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         destinationFilePath = Path.Combine(reportDirectoryPath, $@"30508(買)_{DateTime.Now.ToString("yyyy.MM.dd")}Test.csv");

      }
      [TestInitialize]
      public void Setup()
      {
         b30508 = new B30508(destinationFilePath, "2018/10/11", "2018/10/11");
      }
      [TestMethod()]
      public void Wf30508Test()
      {
         bool isCorrect = b30508.Wf30508();
         Assert.IsTrue(isCorrect);
      }
   }
}