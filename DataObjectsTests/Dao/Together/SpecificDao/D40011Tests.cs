﻿using DataObjects.Dao.Together.SpecificDao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Common.Config;
using BusinessObjects;
using System.Data;

namespace DataObjects.Dao.Together.SpecificDao.Tests
{
   [TestClass()]
   public class D40011Tests
   {

      [TestMethod()]
      public void GetRptLVTest()
      {
         int data = new D40011().GetRptLV("40011",1);
         Assert.IsNotNull(data);
      }
   }
}