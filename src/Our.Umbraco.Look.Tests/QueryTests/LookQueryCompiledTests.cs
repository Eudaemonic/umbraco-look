﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Our.Umbraco.Look.Tests.QueryTests
{
    [TestClass]
    public class LookQueryCompiled
    {
        [TestMethod]
        public void New_Query_Not_Compiled()
        {
            var lookQuery = new LookQuery();

            Assert.IsNull(lookQuery.Compiled);
        }

        /// <summary>
        /// 
        /// </summary>
        [TestMethod]
        public void New_Query_No_Clauses_Not_Compiled()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext());

            var lookResult = LookService.RunQuery(lookQuery);

            Assert.IsNull(lookQuery.Compiled);
        }

        [TestMethod]
        public void New_Query_Executed_To_Make_Compiled()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NodeQuery = new NodeQuery("thing") };

            var lookResult = LookService.RunQuery(lookQuery);

            Assert.IsNotNull(lookQuery.Compiled);
        }


        [TestMethod]
        public void Invalidate_Compiled_By_Raw_Query_Change()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NodeQuery = new NodeQuery("thing") };

            var lookResult = LookService.RunQuery(lookQuery);

            lookQuery.RawQuery = "+field:value";

            Assert.IsNull(lookQuery.Compiled);
        }

        [TestMethod]
        public void Invalidate_Compiled_By_Node_Query_Change()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NodeQuery = new NodeQuery("thing") };

            var lookResult = LookService.RunQuery(lookQuery);

            lookQuery.NodeQuery = new NodeQuery();

            Assert.IsNull(lookQuery.Compiled);
        }

        [TestMethod]
        public void Invalidate_Compiled_By_Name_Query_Change()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NodeQuery = new NodeQuery("thing") };

            var lookResult = LookService.RunQuery(lookQuery);

            lookQuery.NameQuery = new NameQuery();
            lookQuery.NameQuery.StartsWith = "new value";

            Assert.IsNull(lookQuery.Compiled);
        }

        [TestMethod]
        public void Invalidate_Compiled_By_Date_Query_Change()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NodeQuery = new NodeQuery("thing") };

            var lookResult = LookService.RunQuery(lookQuery);

            lookQuery.DateQuery = new DateQuery();
            lookQuery.DateQuery.Before = DateTime.MaxValue;

            Assert.IsNull(lookQuery.Compiled);
        }

        [TestMethod]
        public void Invalidate_Compiled_By_Text_Query_Change()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NodeQuery = new NodeQuery("thing") };

            var lookResult = LookService.RunQuery(lookQuery);

            lookQuery.TextQuery = new TextQuery();
            lookQuery.TextQuery.GetHighlight = true;

            Assert.IsNull(lookQuery.Compiled);
        }

        [TestMethod]
        public void Invalidate_Compiled_By_Tag_Query_Change()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NodeQuery = new NodeQuery("thing") };

            var lookResult = LookService.RunQuery(lookQuery);

            lookQuery.TagQuery = new TagQuery();
            lookQuery.TagQuery.FacetOn = new TagFacetQuery();

            Assert.IsNull(lookQuery.Compiled);
        }

        [TestMethod]
        public void Invalidate_Compiled_By_Location_Query_Change()
        {
            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NodeQuery = new NodeQuery("thing") };

            var lookResult = LookService.RunQuery(lookQuery);

            lookQuery.LocationQuery = new LocationQuery();
            lookQuery.LocationQuery.MaxDistance = new Distance(1, DistanceUnit.Miles);

            Assert.IsNull(lookQuery.Compiled);
        }

        [TestMethod]
        public void Re_Execute_Compiled_Expect_Same_Results()
        {
            TestHelper.IndexThings(new Thing[] { new Thing() { Name = "thing" } });

            var lookQuery = new LookQuery(TestHelper.GetSearchingContext()) { NameQuery = new NameQuery("thing") };

            Assert.IsNull(lookQuery.Compiled);

            var lookResults = LookService.RunQuery(lookQuery);
            var total = lookResults.TotalItemCount;

            Assert.IsNotNull(lookQuery.Compiled);
            Assert.IsTrue(total > 0);

            Assert.AreEqual(total, LookService.RunQuery(lookQuery).TotalItemCount);            
        }
    }
}
