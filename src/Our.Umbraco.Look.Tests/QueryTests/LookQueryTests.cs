﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Our.Umbraco.Look.Models;
using Our.Umbraco.Look.Services;
using Umbraco.Core.Models;

namespace Our.Umbraco.Look.Tests.QueryTests
{
    [TestClass]
    public class LookQueryTests
    {
        [TestMethod]
        public void Null_Query()
        {
            var lookResult = LookService.Query(null);

            Assert.IsNotNull(lookResult);
            Assert.IsFalse(lookResult.Success);
            Assert.IsTrue(lookResult.Total == 0);
        }

        [TestMethod]
        public void Empty_Query()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext());

            var lookResult = LookService.Query(lookQuery);

            Assert.IsNotNull(lookResult);
            Assert.IsFalse(lookResult.Success);
            Assert.IsTrue(lookResult.Total == 0);
        }

        [TestMethod]
        public void Query_With_Node_Type_Clause()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext());

            lookQuery.NodeQuery.Types = new PublishedItemType[] { PublishedItemType.Content };

            var lookResult = LookService.Query(lookQuery);

            Assert.IsNotNull(lookResult);
            Assert.IsTrue(lookResult.Success);
        }
    }
}
