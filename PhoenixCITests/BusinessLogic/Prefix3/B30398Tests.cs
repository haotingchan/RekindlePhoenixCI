using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessObjects;
using Common.Config;
using DataObjects;
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
   public class B30398Tests
   {
      private B30398 b30398;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30398.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30398_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }

      [TestInitialize]
      public void Setup()
      {
         b30398 = new B30398(destinationFilePath, "2018/10");
      }
      [TestMethod()]
      public void Wf30398Test()
      {
         bool isCorrect = b30398.Wf30331();
         Assert.IsTrue(isCorrect);
      }

      [TestMethod()]
      public void Wf30398abcTest()
      {
         bool isCorrect = b30398.Wf30333();
         Assert.IsTrue(isCorrect);
      }
   }
}