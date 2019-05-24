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
using BaseGround.Shared;
using DevExpress.Spreadsheet;

namespace PhoenixCI.BusinessLogic.Prefix3.Tests
{
   [TestClass()]
   public class B30393Tests
   {
      private B30393 b30393;
      private static DateTime _StartDate, _EndDate;
      private static Workbook _workbook;
      private static string reportDirectoryPath, destinationFilePath;
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

         reportDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Report", DateTime.Now.ToString("yyyyMMdd"));
         Directory.CreateDirectory(reportDirectoryPath);

         string excelTemplateDirectoryPath = Path.Combine(Environment.CurrentDirectory.Replace("PhoenixCITests", "PhoenixCI"), "Excel_Template", "30393.xlsx");
         destinationFilePath = Path.Combine(reportDirectoryPath, "30393_" + DateTime.Now.ToString("yyyy.MM.dd") + "-" + DateTime.Now.ToString("hh.mm.ss") + "Test.xlsx");

         File.Copy(excelTemplateDirectoryPath, destinationFilePath, true);

         //前月倒數2天交易日
         _StartDate = PbFunc.f_get_last_day("AI3", "RHF", "2018/10", 2);
         //抓當月最後交易日
         _EndDate = PbFunc.f_get_end_day("AI3", "RHF", "2018/10");
         _workbook = new Workbook();
         //載入Excel
         _workbook.LoadDocument(destinationFilePath);
      }

      [TestInitialize]
      public void Setup()
      {
         b30393 = new B30393(_workbook, "2018/10");
         //b30393 = new B30393(destinationFilePath, "2005/10");
      }

      #region RHF
      [TestMethod()]
      public void Wf30393RHF_Test()
      {
         string isCorrect = b30393.Wf30393(_StartDate, _EndDate, "RHF", "30393_1(RHF)");
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf30393abcRHF_Test()
      {
         string isCorrect = b30393.Wf30393abc("RHF", "data_30393_1abc");
         Assert.IsNotNull(isCorrect);
      }
      #endregion

      #region RTF
      [TestMethod()]
      public void Wf30393RTF_Test()
      {
         string isCorrect = b30393.Wf30393(_StartDate, _EndDate, "RTF", "30393_2(RTF)");
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf30393abcRTF_Test()
      {
         string isCorrect = b30393.Wf30393abc("RTF", "data_30393_2abc");
         Assert.IsNotNull(isCorrect);
      }
      #endregion

      #region XEF
      [TestMethod()]
      public void Wf30393XEF_Test()
      {
         string isCorrect = b30393.Wf30393(_StartDate, _EndDate, "XEF", "30393_3(XEF)");
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf30393abcXEF_Test()
      {
         string isCorrect = b30393.Wf30393abc("XEF", "data_30393_3abc");
         Assert.IsNotNull(isCorrect);
      }
      #endregion

      #region XJF
      [TestMethod()]
      public void Wf30393XJF_Test()
      {
         string isCorrect = b30393.Wf30393(_StartDate, _EndDate, "XJF", "30393_4(XJF)");
         Assert.IsNotNull(isCorrect);
      }

      [TestMethod()]
      public void Wf30393abcXJF_Test()
      {
         string isCorrect = b30393.Wf30393abc("XJF", "data_30393_4abc");
         Assert.IsNotNull(isCorrect);
      }
      #endregion

      [TestCleanup]
      public void CleanAfterEachTestMethod()
      {
         //存檔
         _workbook.SaveDocument(destinationFilePath);
      }

   }
}