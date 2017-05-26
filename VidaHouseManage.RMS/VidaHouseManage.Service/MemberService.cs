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
    public class MemberService : IMemberService
    {
        private HttpHelper serverHelper = new HttpHelper();

        #region 获取成员
        /// <summary>
        /// 获取成员列表
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <param name="organizationId"></param>
        /// <returns></returns>
        public PageListResult<List<UserEntity>> GetMemberList(int pageIndex, int pageSize, string organizationId)
        {
            var model = new UserRequestModel() { OrgId = organizationId, Page = pageIndex, PageSize = pageSize };

            var httpParams = new Dictionary<string, string>();

            httpParams.Add("OrgId", organizationId);

            httpParams.Add("Page", pageIndex.ToString());

            httpParams.Add("PageSize", pageSize.ToString());

            var formDataString = HttpRequstHelper.FormatParams(httpParams);

            var result = serverHelper.RequestGETReuse(URLConfig.MemberListUrl + formDataString);

            var responseResult = JsonConvert.DeserializeObject<UserResponseModel>(result.Data);

            var entitylist = new PagedList<UserEntity>(responseResult.Data, responseResult.Count, pageIndex - 1, pageSize);

            PageListResult<List<UserEntity>> query = responseResult.Data.ToPageResult<List<UserEntity>>(true, pageIndex, entitylist.PageSize, entitylist.TotalCount, entitylist.TotalPages);

            return query;
        }

        /// <summary>
        /// 获取成员详情
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public ServiceResult<UserEntity> GetMemberById(string Id)
        {
            var model = new UserEntity();

            var result = serverHelper.RequestGETReuse(URLConfig.MemberModelUrl + Id);

            if (!result.Success)
                return model.ToResult(success: result.Success, message: result.Message);

            model = JsonConvert.DeserializeObject<UserEntity>(result.Data); //获取所有权限列表;

            var selectIds = model.Roles.Select(x => x.Id);

            model.Roles = new RoleService().GetRoelsByOrganizationId(model.OrganizationId).Data;

            model.Roles.ForEach(x =>
            {
                if (selectIds.Any(y => y == x.Id))
                {
                    x.IsSelected = true;
                }

            });

            return model.ToResult(success: true);
        }


        #endregion

        #region 基础操作

        /// <summary>
        /// 添加成员
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult AddMember(string[] Ids, UserEntity entity)
        {

            if (Ids != null)
            {
                var roleList = Ids.ToList().Select(x =>
                {
                    return new RolesEntity() { Id = x };

                }).ToList();

                entity.Roles = roleList;
            }

            var json = JsonConvert.SerializeObject(entity);

            var result = serverHelper.RequestPostReuse(URLConfig.AddMemberUrl, json);

            return result.Success.ToResult(message: result.Data);
        }

        /// <summary>
        /// 编辑成员
        /// </summary>
        /// <param name="Ids"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        public ServiceResult EditMember(string[] Ids, UserEntity entity)
        {
            if (Ids != null)
            {
                var roleList = Ids.ToList().Select(x =>
                                    {
                                        return new RolesEntity() { Id = x };

                                    }).ToList();

                entity.Roles = roleList;
            }

            var json = JsonConvert.SerializeObject(entity);

            var result = serverHelper.RequestPutReuse(URLConfig.EditMemberUrl, json);

            return result.Success.ToResult(message: result.Data);
        }

        /// <summary>
        /// 删除成员
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResult DelMember(string id, string userName)
        {

            var result = serverHelper.RequestDeleteReuse(URLConfig.DelMemberUrl + id);

            if (result.Success)
                LogHelper.Info("用户：" + userName + "删除了成员__" + id);

            return result.Success.ToResult(message: result.Data);
        }

        #endregion

    }
}
