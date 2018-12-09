﻿using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.Search;
using Lucene.Net.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Our.Umbraco.Look.Tests
{
    internal class TestHelper
    {
        private static string DirectoryPath => Path.GetTempPath() + "LookTestData";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">if supplied, allows test to use a specifc index</param>
        /// <returns></returns>
        internal static SearchingContext GetSearchingContext(string path = null)
        {
            if (path == null)
            {
                path = TestHelper.DirectoryPath; // use the default test index
            }

            return new SearchingContext()
            {
                Analyzer = new WhitespaceAnalyzer(),
                EnableLeadingWildcards = true,
                IndexSearcher = new IndexSearcher(new SimpleFSDirectory(new DirectoryInfo(path)), true)
            };
        }

        //internal static void GenerateTestData()
        //{
        //    // generate a load of random test data, just to bulk it out
        //}

        /// <summary>
        /// Add supplied collection into the test index
        /// </summary>
        /// <param name="things"></param>
        internal static void IndexThings(IEnumerable<Thing> things)
        {
            var nameStack = new Stack<string>(things.Select(x => x.Name));
            var dateStack = new Stack<DateTime?>(things.Select(x => x.Date));
            var textStack = new Stack<string>(things.Select(x => x.Text));
            var tagStack = new Stack<LookTag[]>(things.Select(x => x.Tags));
            var locationStack = new Stack<Location>(things.Select(x => x.Location));

            // setup indexers
            LookConfiguration.NameIndexer = x => nameStack.Pop();
            LookConfiguration.DateIndexer = x => dateStack.Pop();
            LookConfiguration.TextIndexer = x => textStack.Pop();
            LookConfiguration.TagIndexer = x => tagStack.Pop();
            LookConfiguration.LocationIndexer = x => locationStack.Pop();

            // null for IPublishedContent as not required
            var indexingContext = new IndexingContext(null, null);

            List<Document> documents = new List<Document>();

            foreach (var thing in things)
            {
                var document = new Document();

                LookService.Index(indexingContext, document);

                documents.Add(document);
            }

            // reset indexers
            LookConfiguration.NameIndexer = null;
            LookConfiguration.DateIndexer = null;
            LookConfiguration.TextIndexer = null;
            LookConfiguration.TagIndexer = null;
            LookConfiguration.LocationIndexer = null;

            TestHelper.IndexDocuments(documents);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="documents"></param>
        internal static void IndexDocuments(IEnumerable<Document> documents)
        {
            var luceneDirectory = FSDirectory.Open(System.IO.Directory.CreateDirectory(TestHelper.DirectoryPath));
            var analyzer = new WhitespaceAnalyzer();

            var indexWriter = new IndexWriter(luceneDirectory, analyzer, IndexWriter.MaxFieldLength.UNLIMITED);

            foreach (var document in documents)
            {
                indexWriter.AddDocument(document);
            }

            indexWriter.Optimize();

            indexWriter.Commit();
            
            indexWriter.Close();
        }

        /// <summary>
        /// Delete the test index from the file system
        /// </summary>
        internal static void DeleteIndex()
        {
            if (System.IO.Directory.Exists(TestHelper.DirectoryPath))
            {
                System.IO.Directory.Delete(TestHelper.DirectoryPath, true);
            }
        }
    }
}
