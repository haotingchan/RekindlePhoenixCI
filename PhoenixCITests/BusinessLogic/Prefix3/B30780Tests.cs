using PhoenixCI.BusinessLogic.Prefix3;
using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30780Tests
   {
      private B30780 b30780;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30780.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30780_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);
      }

      [TestMethod()]
      public void WF30780one全部Test()
      {
         b30780 = new B30780(destinationFilePath, "2018/10", "%", new DateTime(2018, 11, 09));
         string msgText = b30780.WF30780one();
         Assert.IsNotNull(msgText);
      }


      [TestMethod()]
      public void WF30780two全部Test()
      {
         b30780 = new B30780(destinationFilePath, "2018/10", "%", new DateTime(2018, 11, 09));
         string msgText = b30780.WF30780two();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void WF30780four全部Test()
      {
         b30780 = new B30780(destinationFilePath, "2018/10", "%", new DateTime(2018, 11, 09));
         string msgText = b30780.WF30780four();
         Assert.IsNotNull(msgText);
      }

      [TestMethod()]
      public void WF30780five全部Test()
      {
         b30780 = new B30780(destinationFilePath, "2018/10", "%", new DateTime(2018, 11, 09));
         string msgText = b30780.WF30780five();
         Assert.IsNotNull(msgText);
      }
   }
}