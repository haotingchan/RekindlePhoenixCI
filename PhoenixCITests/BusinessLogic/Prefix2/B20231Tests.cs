using Microsoft.VisualStudio.TestTools.UnitTesting;
using PhoenixCI.BusinessLogic.Prefix2;
using System;
using BusinessObjects;
using Common.Config;
using DataObjects;
using System.IO;
using System.Data;
using BaseGround.Shared;
using DataObjects.Dao.Together.SpecificDao;

namespace PhoenixCI.BusinessLogic.Prefix2.Tests
{
   [TestClass()]
   public class B20231Tests
   {
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);

      }

      [TestMethod()]
      public void TxtWriteToDataTableTest()
      {
         Stream openFile = FileToStream(Path.Combine(Environment.CurrentDirectory, "Excel_Template", "20231.txt"));
         DataTable dtReadTxt = new D20231().List20231("20180329").Clone();
         DataTable dataTable = new B20231().TxtWriteToDataTable(openFile, dtReadTxt);
         Assert.IsNotNull(dataTable);
      }

      public Stream FileToStream(string fileName)
      {
         // 打开文件
         FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
         // 读取文件的 byte[]
         byte[] bytes = new byte[fileStream.Length];
         fileStream.Read(bytes, 0, bytes.Length);
         fileStream.Close();
         // 把 byte[] 转换成 Stream
         Stream stream = new MemoryStream(bytes);
         return stream;
      }
   }
}