﻿using Examine;
using Our.Umbraco.Look.Extensions;
using System;
using System.Web;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace Our.Umbraco.Look
{
    public class LookMatch : SearchResult
    {
        private Lazy<IPublishedContent> _item;

        /// <summary>
        /// Lazy evaluation of Item for IPublishedContent
        /// </summary>
        public IPublishedContent Item => this._item.Value;

        /// <summary>
        /// The custom name field
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// The custom date field
        /// </summary>
        public DateTime? Date { get; }

        /// <summary>
        /// The full text (only returned if specified)
        /// </summary>
        public string Text { get; }

        /// <summary>
        /// Highlight text (containing search text) extracted from from the full text
        /// </summary>
        public IHtmlString Highlight { get; }

        /// <summary>
        /// Tag collection (only returned if specified)
        /// </summary>
        public LookTag[] Tags { get; }

        /// <summary>
        /// The custom location (lat|lng) field
        /// </summary>
        public Location Location { get; }

        /// <summary>
        /// Temp field for calculated results
        /// </summary>
        public double? Distance { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="docId"></param>
        /// <param name="score"></param>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="text"></param>
        /// <param name="highlight"></param>
        /// <param name="tags"></param>
        /// <param name="location"></param>
        /// <param name="distance"></param>
        /// <param name="publishedItemType"></param>
        /// <param name="umbracoHelper"></param>
        internal LookMatch(
                    int docId,
                    float score,
                    int id,
                    string name,
                    DateTime? date,
                    string text,
                    IHtmlString highlight,
                    LookTag[] tags,
                    Location location,
                    double? distance,
                    PublishedItemType publishedItemType,
                    UmbracoHelper umbracoHelper)
        {
            //this.DocId = docId; // not in Examine 0.1.70, but in more recent versions
            this.Score = score;
            this.Id = id;
            this.Name = name;
            this.Date = date;
            this.Text = text;
            this.Highlight = highlight;
            this.Tags = tags;
            this.Location = location;
            this.Distance = distance;

            this._item = new Lazy<IPublishedContent>(() => {

                if (umbracoHelper != null) // will be null for unit tests (as not initialized via Umbraco startup)
                {
                    switch (publishedItemType)
                    {
                        case PublishedItemType.Content: return umbracoHelper.TypedContent(id);
                        case PublishedItemType.Media: return umbracoHelper.TypedMedia(id);
                        case PublishedItemType.Member: return umbracoHelper.SafeTypedMember(id);
                    }
                }

                return null;
            });
        }
    }
}
