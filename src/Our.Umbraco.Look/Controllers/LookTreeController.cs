using Examine;
using Examine.Providers;
using System;
using System.Net.Http.Formatting;
using System.Web;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;
using System.Linq;

namespace Our.Umbraco.Look.Controllers
{
    [Tree("developer", "lookTree", "Look")]
    public class LookTreeController : TreeController
    {

        protected override TreeNodeCollection GetTreeNodes(string id, FormDataCollection queryStrings)
        {
            var tree = new TreeNodeCollection();

            var node = new LookTreeNode(id);

            switch (node.Type)
            {
                case LookTreeNodeType.Root:

                    var searchProviders = ExamineManager.Instance.SearchProviderCollection;

                    foreach(var searchProvider in searchProviders)
                    {
                        var baseSearchProvider = searchProvider as BaseSearchProvider;

                        if (baseSearchProvider != null)
                        {
                            // if searcher has any items with tags
                            var hasTaggedItems = 
                                new LookQuery(baseSearchProvider.Name)
                                {
                                    RawQuery = "+" + LookConstants.HasTagsField + ":" + Boolean.TrueString.ToLower()
                                }
                                .Query()
                                .TotalItemCount > 0;

                            tree.Add(this.CreateTreeNode("searchProvider-" + baseSearchProvider.Name, id, queryStrings, baseSearchProvider.Name, "icon-files", hasTaggedItems));
                        }
                    }

                    break;

                case LookTreeNodeType.SearchProvider:

                    var tagGroups = new LookQuery(node.SearchProvider) { RawQuery = "+" + LookConstants.HasTagsField + ":" + Boolean.TrueString.ToLower() }
                                        .Query()
                                        .Matches
                                        .SelectMany(x => x.Tags.Select(y => y.Group))
                                        .Distinct();

                    foreach(var tagGroup in tagGroups)
                    {
                        tree.Add(this.CreateTreeNode(id + "-tagGroup-" + tagGroup, id, queryStrings, tagGroup != "" ? tagGroup : "Default", "icon-tags", true));
                    }
                    
                    break;

                case LookTreeNodeType.TagGroup:

                    var tags = new LookQuery(node.SearchProvider) { RawQuery = "+" + LookConstants.HasTagsField + ":" + Boolean.TrueString.ToLower() }
                                        .Query()
                                        .Matches
                                        .SelectMany(x => x.Tags.Where(y => y.Group == node.TagGroup))
                                        .Select(x => x.Name)
                                        .Distinct();

                    foreach (var tag in tags)
                    {
                        tree.Add(this.CreateTreeNode(id + "-tag-" + tag, id, queryStrings, tag, "icon-tag", false));
                    }

                    break;
            }

            return tree;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// helper class to parse string id
        /// /// </summary>
        private class LookTreeNode
        {
            internal LookTreeNodeType Type
            {
                get
                {
                    if (this.Tag != null)
                    {
                        return LookTreeNodeType.Tag;
                    }

                    if (this.TagGroup != null)
                    {
                        return LookTreeNodeType.TagGroup;
                    }

                    if (this.SearchProvider != null)
                    {
                        return LookTreeNodeType.SearchProvider;
                    }

                    return LookTreeNodeType.Root;
                }
            }

            internal string SearchProvider { get; }

            internal string TagGroup { get; }

            internal string Tag { get; }

            /// <summary>
            /// index-External
            /// </summary>
            /// <param name="id"></param>
            internal LookTreeNode(string id)
            {
                if (id == "-1") { return; }

                var chopped = id.Split('-');

                if (chopped.Length >= 2)
                {
                    this.SearchProvider = chopped[1];
                }

                if (chopped.Length >= 4)
                {
                    this.TagGroup = chopped[3];
                }

                if (chopped.Length >= 6)
                {
                    this.Tag = chopped[5];
                }
            }
        }

        private enum LookTreeNodeType
        {
            Root,

            SearchProvider,

            TagGroup,

            Tag
        }
    }
}
