using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Entity
{
   public class CategoriesEntity
    {
        /// <summary>
        /// Data
        /// </summary>
        public List<SubCategories> data { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public int count { get; set; }
    }
    
    public class CategoryModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// ParentId
        /// </summary>
        public int parentId { get; set; }
        /// <summary>
        ///  桌子凳子
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// AA
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// Order
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// ExtraData
        /// </summary>
        public string extraData { get; set; }
        /// <summary>
        /// SubCategories
        /// </summary>
        public List<SubCategories> subCategories { get; set; }

        public List<TreeNode> getChi(List<SubCategories> da)
        {
            List<TreeNode> list = new List<TreeNode>();
            foreach (var d in da)
            {
                TreeNode node = new TreeNode();
                node.id = d.id.ToString();
                node.text = d.name;
                node.parentId = d.parentId.ToString();
                node.description = d.description;
                node.children = getChi(d.subCategories);
                list.Add(node);
            }
            return list;
        }
    }
    public class SubCategories
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// ParentId
        /// </summary>
        public int parentId { get; set; }
        /// <summary>
        /// 电脑桌
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 1903
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string image { get; set; }
        /// <summary>
        /// Order
        /// </summary>
        public int order { get; set; }
        /// <summary>
        /// ExtraData
        /// </summary>
        public string extraData { get; set; }
        /// <summary>
        /// SubCategories
        /// </summary>
        public List<SubCategories> subCategories { get; set; }

        public TreeNode ToNode()
        {
            TreeNode node = new TreeNode()
            {
                id = this.id.ToString(),
                text = this.name,
                parentId = this.parentId.ToString(),
                description = this.description,
                children = new List<TreeNode>()// getChi(this.subCategories)
            };
            return node;
        }
    }



}
