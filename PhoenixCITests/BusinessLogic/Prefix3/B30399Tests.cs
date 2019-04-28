using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30399Tests
   {
      private B30399 b30399;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30399.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30399_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }

      [TestInitialize]
      public void Setup()
      {
         b30399 = new B30399(destinationFilePath, "2018/10");
      }

      [TestMethod()]
      public void Wf30331Test()
      {
         string isCorrect = b30399.Wf30331();
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf30333Test()
      {
         string isCorrect = b30399.Wf30333();
         Assert.IsNotNull(isCorrect);
      }
   }
}