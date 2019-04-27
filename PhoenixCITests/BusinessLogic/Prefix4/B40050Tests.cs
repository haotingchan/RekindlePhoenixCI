using BusinessObjects;
using Common.Config;
using DataObjects;
using DataObjects.Dao.Together.SpecificDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix4.Tests
{
   [TestClass()]
   public class B40050Tests
   {
      private B40050 b40050;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "40050.xls");
         destinationFilePath = Path.Combine(reportDirectoryPath, "40050_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xls");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b40050 = new B40050(destinationFilePath, new D40050().GetData(new DateTime(2018, 10, 11), new DateTime(2018, 01, 01), "1%"), 0);
      }

      [TestMethod()]
      public void Wf40051Test()
      {
         string isCorrect = b40050.Wf40051();
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf40052Test()
      {
         string isCorrect = b40050.Wf40052();
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf40053Test()
      {
         string isCorrect = b40050.Wf40053();
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf40054Test()
      {
         string isCorrect = b40050.Wf40054();
         Assert.IsNotNull(isCorrect);
      }
   }
}