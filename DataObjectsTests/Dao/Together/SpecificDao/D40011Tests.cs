using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Config;
using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D40011Tests
   {
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);
      }

      [TestMethod()]
      public void GetFutR1DataTest()
      {
         DataTable data = new D40011().GetFutR1Data(new System.DateTime(2011, 04, 11));
         Assert.IsNotNull(data);
      }

      [TestMethod()]
      public void GetFutR2DataTest()
      {
         DataTable data = new D40011().GetFutR2Data(new System.DateTime(2011, 04, 11));
         Assert.IsNotNull(data);
      }
   }
}