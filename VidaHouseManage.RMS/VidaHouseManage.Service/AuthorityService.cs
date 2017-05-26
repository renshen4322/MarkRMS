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

    public class AuthorityService: IAuthorityService
    {
        private HttpHelper serverHelper = new HttpHelper();

        #region 获取权限列表

        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        public ServiceResult<List<AuthorityEntity>> GetAuthorityList()
        {
            var result = serverHelper.RequestGETReuse(URLConfig.AuthorityListUrl);

            var list = new List<AuthorityEntity>();

            if (result.Success)
            {
                list = JsonConvert.DeserializeObject<List<AuthorityEntity>>(result.Data); //获取所有权限列表;
            }

            return list.ToResult(success: result.Success);
        }

        /// <summary>
        /// 获取角色权限
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>

        public ServiceResult<List<AuthorityEntity>> GetAuthorityListByRoleId(long roleId)
        {
            var result = serverHelper.RequestGETReuse(URLConfig.AuthorityByRoleId + roleId);

            var list = new List<AuthorityEntity>();

            if (result.Success)
            {

                list = JsonConvert.DeserializeObject<List<AuthorityEntity>>(result.Data); //获取所有权限列表;
            }

            return list.ToResult(success: result.Success);

        }

        #endregion

    }
}
