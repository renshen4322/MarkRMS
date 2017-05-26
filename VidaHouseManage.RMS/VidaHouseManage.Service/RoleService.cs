using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaHouseManage.Common;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;
using VidaHouseManage.Service.IService;

namespace VidaHouseManage.Service
{

    public class RoleService: IRoleService
    {
        private HttpHelper serverHelper = new HttpHelper();

        #region 根据组织获取角色
        /// <summary>
        /// 根据组织获取角色
        /// </summary>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public ServiceResult<List<RolesEntity>> GetRoelsByOrganizationId(string organizationId)
        {
            var list = new List<RolesEntity>();

            var result = serverHelper.RequestGETReuse(URLConfig.RoleListUrl + organizationId);

            if (result.Success)
                list = JsonConvert.DeserializeObject<List<RolesEntity>>(result.Data); //获取组织列表;

            return list.ToResult(success: true);

        }

        #endregion

        #region 角色管理
        /// <summary>
        /// 获取角色详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ServiceResult<RolesEntity> GetRolesById(long Id)
        {
            var result = serverHelper.RequestGETReuse(URLConfig.RoleModelUrl + Id);

            if (!result.Success)
                return new RolesEntity().ToResult(success: result.Success, message: result.Message);

            var model = JsonConvert.DeserializeObject<RolesEntity>(result.Data);

            var selectNames = new List<string>();

            var authorResult = new AuthorityService().GetAuthorityListByRoleId(Id);

            if (authorResult.Success)
                selectNames = authorResult.Data.Select(x => x.Name).ToList();

            model.AuthorList = new AuthorityService().GetAuthorityList().Data;

            model.AuthorList.ForEach(x =>
            {
                if (selectNames.Any(y => y == x.Name))
                {
                    x.IsSelected = true;
                }

            });

            return model.ToResult(success: true);

        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult CreateRoles(string[] names, RolesEntity entity)
        {
            if (names != null)
                entity.Permissions = names.ToList();

            var json = JsonConvert.SerializeObject(entity);

            var result = serverHelper.RequestPostReuse(URLConfig.AddRoleUrl + entity.OrganizationId, json);

            return result.Success.ToResult(message:result.Data);
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult EditRoles(string[] names, RolesEntity entity)
        {
            if (names != null)
                entity.Permissions = names.ToList();

            var json = JsonConvert.SerializeObject(entity);

            var result = serverHelper.RequestPutReuse(URLConfig.EditRoleUrl, json);

            return result.Success.ToResult(message: result.Data);

        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ServiceResult DelRoles(string Id, string userName)
        {
            var result = serverHelper.RequestDeleteReuse(URLConfig.DelRoleUrl + Id);

            if (result.Success)
                LogHelper.Info("用户：" + userName + "删除了角色__" + Id);

            return result.Success.ToResult(message: result.Data);
        }

        #endregion

    }
}
