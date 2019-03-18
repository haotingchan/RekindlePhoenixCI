using BusinessObjects;
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
   public class B30360Tests
   {
      private B30360 b30360;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30360.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30360_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b30360 = new B30360(destinationFilePath, "2018/10");
         //b30360 = new B30360(destinationFilePath, "2005/10");
      }

      [TestMethod()]
      public void Wf30361Test()
      {
         bool isCorrect = b30360.Wf30361();
         Assert.IsTrue(isCorrect);
      }

      [TestMethod()]
      public void Wf30362Test()
      {
         bool isCorrect = b30360.Wf30362();
         Assert.IsTrue(isCorrect);
      }

      [TestMethod()]
      public void Wf30363Test()
      {
         bool isCorrect = b30360.Wf30363();
         Assert.IsTrue(isCorrect);
      }

      [TestMethod()]
      public void Wf30366Test()
      {
         bool isCorrect = b30360.Wf30366();
         Assert.IsTrue(isCorrect);
      }

      [TestMethod()]
      public void Wf30367Test()
      {
         bool isCorrect = b30360.Wf30367();
         Assert.IsTrue(isCorrect);
      }

      [TestMethod()]
      public void Wf30368Test()
      {
         bool isCorrect = b30360.Wf30368();
         Assert.IsTrue(isCorrect);
      }
   }
}