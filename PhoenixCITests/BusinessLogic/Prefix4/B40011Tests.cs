﻿using PhoenixCI.BusinessLogic.Prefix4;
using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix4.Tests
{
   [TestClass()]
   public class B40011Tests
   {
      private B40011 b40011;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "40011.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "40011_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }
      [TestInitialize]
      public void Setup()
      {
         b40011 = new B40011("40011", destinationFilePath, "2019/05/22");
      }


      [TestMethod()]
      public void Wf40011FutureSheetTest()
      {
         string msgText = b40011.WfFutureSheet();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void Wf40011OptionSheetTest()
      {
         string msgText = b40011.WfOptionSheet();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void WfStatFTest()
      {
         string msgText = b40011.WfStat("F", "fut_3index");
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void WfStatOTest()
      {
         string msgText = b40011.WfStat("O", "opt_3index");
         Assert.IsNotNull(msgText);
      }
   }
}