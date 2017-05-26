using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VidaHouseManage.Common;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;
using VidaHouseManage.Service.IService;

namespace VidaHouseManage.Service
{
    public class OrganizationService: IOrganizationService
    {

        private HttpHelper serverHelper = new HttpHelper();

        #region 获取组织
        /// <summary>
        /// 获取组织树
        /// </summary>
        /// <returns></returns>
        public ServiceResult<List<TreeNode>> GetOrganizationList()
        {
            var result = serverHelper.RequestGETReuse(URLConfig.OrganizationListUrl);

            var list = new List<TreeNode>();

            if (result.Success)
            {
                var entity = JsonConvert.DeserializeObject<OrganizationEntity>(result.Data); //获取组织列表;

                list = entity.GetModelList("0");
            }

            return list.ToResult(success: true);

        }

        #endregion

        #region 基础操作
        /// <summary>
        /// 添加组织
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult AddOrganization(OrganizationEntity entity)
        {
            var parentId = entity.Id;

            entity.Id = string.Empty;

            var json = JsonConvert.SerializeObject(entity);

            var result = serverHelper.RequestPostReuse(URLConfig.AddOrganizationUrl + parentId, json);

            return result.Success.ToResult(message: result.Data);
        }

        /// <summary>
        /// 编辑组织
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult EditOrganization(OrganizationEntity entity)
        {
            var entityJson = JsonConvert.SerializeObject(entity);

            var result = serverHelper.RequestPutReuse(URLConfig.EditOrganizationUrl + entity.Id, entityJson);

            return result.Success.ToResult(message: result.Data);
        }

        /// <summary>
        /// 删除组织
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult DelOrganization(string id, string userName)
        {
            var result = serverHelper.RequestDeleteReuse(URLConfig.DelOrganizationUrl + id);

            if (result.Success)
                LogHelper.Info("用户：" + userName + "删除了组织__" + id);

            return result.Success.ToResult(message: result.Data);
        }

        #endregion

    }
}
