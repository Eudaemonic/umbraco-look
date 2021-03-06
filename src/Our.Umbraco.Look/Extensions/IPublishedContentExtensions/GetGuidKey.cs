﻿using System;
using Umbraco.Core.Models;
using Umbraco.Core.Models.PublishedContent;

namespace Our.Umbraco.Look.Extensions
{
    /// <summary>
    /// https://our.umbraco.com/forum/extending-umbraco-and-using-the-api/85776-umbraco-761-ipublishedcontentgetkey-returns-empty-guid
    /// </summary>
    internal static partial class IPublishedContentExtensions
    {
        /// <summary>
        /// Custom key key method
        /// </summary>
        /// <param name="publishedContent"></param>
        /// <returns></returns>
        internal static Guid GetGuidKey(this IPublishedContent publishedContent)
        {
            IPublishedContentWithKey withKey = null;

            if (publishedContent is PublishedContentWrapped)
            {
                withKey = ((PublishedContentWrapped)publishedContent).Unwrap() as IPublishedContentWithKey;
            }
            else
            {
                withKey = publishedContent as IPublishedContentWithKey;
            }

            if (withKey != null)
            {
                return withKey.Key;
            }

            return Guid.Empty;
        }
    }
}
