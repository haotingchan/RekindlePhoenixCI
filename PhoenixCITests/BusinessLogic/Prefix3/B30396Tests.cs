﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using BusinessObjects;
using Common.Config;
using DataObjects;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30396Tests
   {
      private B30396 b30396;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30396.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30396_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }

      [TestInitialize]
      public void Setup()
      {
         b30396 = new B30396(destinationFilePath, "2018/10");
      }

      [TestMethod()]
      public void Wf30396Test()
      {
         string isCorrect = b30396.Wf30396();
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf30396abcTest()
      {
         string isCorrect = b30396.Wf30396abc();
         Assert.IsNotNull(isCorrect);
      }
   }
}