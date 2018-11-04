﻿using Examine;
using Our.Umbraco.Look.Models;
using Umbraco.Core.Logging;
using UmbracoExamine;

namespace Our.Umbraco.Look.Services
{
    public partial class LookService
    {
        /// <summary>
        /// Get the configuration details of an Exmaine searcher, so Lucene can be queried in the same way,
        /// consumer needs to know Lucene directory, the analyser (for the text field) and whether to use wildcards)
        /// TODO: move validation logic out into initialize, so quicker to get content during a query
        /// </summary>
        /// <param name="searcherName">The name of the Examine seracher (see ExamineSettings.config)</param>
        /// <returns></returns>
        private static SearcherContext GetSearcherContext(string searcherName)
        {
            var searcher = !string.IsNullOrWhiteSpace(searcherName) 
                            ? ExamineManager.Instance.SearchProviderCollection[searcherName]
                            : ExamineManager.Instance.DefaultSearchProvider;

            if (searcher == null)
            {
                LogHelper.Debug(typeof(LookService), $"Unable to find Examine searcher '{ searcherName }'");
            }
            else
            {
                // TODO: other searcher types ?

                if (!(searcher is UmbracoExamineSearcher))
                {
                    LogHelper.Debug(typeof(LookService), $"Examine searcher of unexpected type '{ searcher.GetType() }'");
                }
                else
                {
                    var umbracoExamineSearcher = (UmbracoExamineSearcher)searcher;
                    
                    return new SearcherContext()
                    {
                        //SearcherName = searcherName,
                        IndexSetName = umbracoExamineSearcher.IndexSetName,
                        //LuceneIndexFolder = umbracoExamineSearcher.LuceneIndexFolder,
                        Analyzer = umbracoExamineSearcher.IndexingAnalyzer,
                        EnableLeadingWildcards = umbracoExamineSearcher.EnableLeadingWildcards
                    };
                }
            }

            return null;
        }
    }
}
