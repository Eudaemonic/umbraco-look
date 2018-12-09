using System.Net.Http.Formatting;
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
            var treeNodeCollection = new TreeNodeCollection();
            
            if (id == Constants.System.Root.ToInvariantString())
            {

                treeNodeCollection.Add(CreateTreeNode("1", "-1", queryStrings, "Look Tree Node"));
            }

            return treeNodeCollection;
        }

        protected override MenuItemCollection GetMenuForNode(string id, FormDataCollection queryStrings)
        {
            throw new System.NotImplementedException();
        }

    }
}
