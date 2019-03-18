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
   public class B30370Tests
   {
      private B30370 b30370;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30370.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30370_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }

      [TestInitialize]
      public void Setup()
      {
         //b30370 = new B30370(destinationFilePath, "2018/10");
         b30370 = new B30370(destinationFilePath, "2005/10");
         //b30370 = new B30370(destinationFilePath, "2005/12");
      }

      [TestMethod()]
      public void Wf30371Test()
      {
         bool isCorrect = b30370.Wf30371();
         Assert.IsTrue(isCorrect);
      }

      [TestMethod()]
      public void Wf30375Test()
      {
         bool isCorrect = b30370.Wf30375();
         Assert.IsTrue(isCorrect);
      }
   }
}