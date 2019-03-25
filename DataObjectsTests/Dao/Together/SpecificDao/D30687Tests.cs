using Microsoft.VisualStudio.TestTools.UnitTesting;
using DataObjects.Dao.Together.SpecificDao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Config;
using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D30687Tests
   {
      [ClassInitialize]
      public static void MyClassInitialize(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);
      }

      [TestMethod()]
      public void ListRuNewDataTest()
      {
         DataTable data = new D30687().ListRuNewData("20181011","20181011","%%","%","A");
         Assert.IsNotNull(data);
      }
   }
}