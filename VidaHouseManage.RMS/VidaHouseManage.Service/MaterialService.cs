using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaHouseManage.Common;
using VidaHouseManage.Entity;

namespace VidaHouseManage.Service
{
   public class MaterialService
    {
        private string ProCategoriesUrl = "https://api.vidahouse.com/designing/v1.0/Categories?type=1";
        private ServerRequestHelper serverHelper = new ServerRequestHelper();

        public List<TreeNode> get()
        {
            var result = serverHelper.RequestGetReuse(ProCategoriesUrl);
            var rets = JsonConvert.DeserializeObject<CategoriesEntity>(result); //根据查询条件获得总条数， 
            List<TreeNode> list = new List<TreeNode>();


            recursion2(rets.data, list, "0");

            return list;
        }

        private void recursion2(List<SubCategories> data, IList<TreeNode> list, string pid)
        {
            foreach (var item in data)
            {
                if (item.parentId.ToString() == pid)
                {
                    TreeNode node = item.ToNode();
                    list.Add(node);
                    recursion2(item.subCategories, node.children, node.id);
                }
            }
        }

    }
}
