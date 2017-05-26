using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace VidaHouseManage.Entity
{
    public class JsTreeNode
    {
        public string id { get; set; }
        public string text { get; set; }
        public State state { get; set; }
        public string description { get; set; }
        public string parentId { get; set; }
        public List<Children> children { get; set; }
    }
    public class JsTreeModel
    {
        public string id { get; set; }
        public string parent { get; set; }
        public string text { get; set; }
        public bool children { get; set; } // if node has sub-nodes set true or not set false
    }
}
