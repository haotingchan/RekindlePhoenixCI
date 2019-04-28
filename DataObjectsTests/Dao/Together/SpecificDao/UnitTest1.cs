﻿using System;
using BusinessObjects;
using Common.Config;
using DataObjects;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataObjectsTests.Dao.Together.SpecificDao
{
   [TestClass]
   public class UnitTest1
   {
      [AssemblyInitialize]
      public static void DBTest(TestContext testContext)
      {
         ConnectionInfo connectionInfo = SettingDragons.Instance.GetConnectionInfo(SettingDragons.Instance.Setting.Database.CiUserAp);
         GlobalDaoSetting.Set(connectionInfo);
      }
   }
}
