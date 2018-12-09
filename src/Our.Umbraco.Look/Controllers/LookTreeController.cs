using Examine;
using Examine.Providers;
using System;
using System.Net.Http.Formatting;
using System.Web;
using Umbraco.Core;
using Umbraco.Web.Models.Trees;
using Umbraco.Web.Trees;

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
                            tree.Add(this.CreateTreeNode("searchProvider=" + baseSearchProvider.Name, id, queryStrings, baseSearchProvider.Name, "icon-files", true));
                        }
                    }

                    break;

                case LookTreeNodeType.SearchProvider:

                    // get all tag groups in this index
                    var lookQuery = new LookQuery(node.SearchProvider);

                    //lookQuery.RawQuery = "+" + LookConstants.HasTagsField + ":" + Boolean.TrueString.ToLower(); // raw query is analyzed - case changed



                    var lookResult = lookQuery.Query();

                    foreach(var march in lookResult.Matches)
                    {

                    }
                    


                    //tree.Add(this.CreateTreeNode(id + "-", id, queryStrings, index.Name, "icon-truck", true));


                    break;

                case LookTreeNodeType.TagGroup:
                    break;

                case LookTreeNodeType.Tag:
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

                var parsedId = HttpUtility.ParseQueryString(id);

                this.SearchProvider = parsedId["searchProvider"];
                this.TagGroup = parsedId["tagGroup"];
                this.Tag = parsedId["tag"];
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
