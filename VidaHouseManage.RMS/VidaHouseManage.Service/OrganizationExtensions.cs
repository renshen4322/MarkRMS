using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaHouseManage.Entity;

namespace VidaHouseManage.Service
{
    public static class OrganizationExtensions
    {
        /// <summary>
        /// 获取组织树
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="parentid"></param>
        /// <returns></returns>
        public static List<TreeNode> GetModelList(this OrganizationEntity entity, string parentid)
        {
            if (entity == null)
                return new List<TreeNode>();

            var list = new List<TreeNode>();

            var node = new TreeNode() { id = entity.Id, text = entity.Name, parentId = parentid, description = entity.Description, a_attr = new NodeDes() { Description = entity.Description }, state = new State() { opened = true } };

            if (entity.SubOrganizations.Count > 0)
            {
                foreach (var item in entity.SubOrganizations)
                {
                    node.children.AddRange(item.GetModelList(entity.Id));
                }
            }
            list.Add(node);
            return list;

        }

        //平级list
        public static List<OrganizationEntity> GetSameLevelList(this OrganizationEntity entity)
        {

            if (entity == null)
                return new List<OrganizationEntity>();

            var list = new List<OrganizationEntity>();

            var node = new OrganizationEntity() { Id = entity.Id, Name = entity.Name, Description = entity.Description };

            if (entity.SubOrganizations.Count > 0)
            {
                foreach (var item in entity.SubOrganizations)
                {
                    list.AddRange(item.GetSameLevelList());
                }
            }

            list.Add(node);
            return list;
        }

    }
}
