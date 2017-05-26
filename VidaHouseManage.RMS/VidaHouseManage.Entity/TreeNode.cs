using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VidaHouseManage.Entity
{
    
    public class TreeNode
    {
        public TreeNode()
        {
            children = new List<TreeNode>();

            a_attr = new NodeDes();
        }

        public string id { get; set; }
        public string text { get; set; }
        public State state { get; set; }
        public string description { get; set; }
        public string parentId { get; set; }
        public NodeDes a_attr { get; set; }
        public List<TreeNode> children { get; set; }
    }


    public class NodeDes
    {
        public string Description { get; set; }
    }

    public class State
    {
        public bool opened { get; set; }
        public bool disabled { get; set; }
        public bool selected { get; set; }
    }
    public class Children
    {
        public string id { get; set; }
        public string parentId { get; set; }
        public string text { get; set; }
        public string description { get; set; }
        public List<Children> children { get; set; }
    }

    
}
