using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Service
{
    public static class URLConfig
    {

        private static string host = string.IsNullOrEmpty(ConfigurationManager.AppSettings["WebApiHost"]) ? string.Empty : ConfigurationManager.AppSettings["WebApiHost"].ToString();

        #region  组织webapi

        //获取组织列表
        public static string OrganizationListUrl = host + "/users/v1.0/organizations";
        //添加组织
        public static string AddOrganizationUrl = host + "/users/v1.0/organizations?organizationId=";
        //编辑组织
        public static string EditOrganizationUrl = host + "/users/v1.0/organizations?organizationId=";
        //删除组织
        public static string DelOrganizationUrl = host + "/users/v1.0/organizations?organizationId=";

        #endregion

        #region 角色webapi

        //获取角色列表
        //public static string RoleListUrl = host + "/roles/v1.0/roles?orgId=";
        public static string RoleListUrl = "http://dev.api.vidaHouse.com/roles/v1.0/roles?orgId=";

        //获取单个角色
       // public static string RoleModelUrl = host + "/roles/v1.0/roles/";
        public static string RoleModelUrl = "http://dev.api.vidaHouse.com/roles/v1.0/roles/";

        //添加角色
        //public static string AddRoleUrl = host + "/roles/v1.0/roles?orgId=";
        public static string AddRoleUrl ="http://dev.api.vidaHouse.com/roles/v1.0/roles?orgId=";

        //编辑角色
        //public static string EditRoleUrl = host + "/roles/v1.0/roles/";
        public static string EditRoleUrl = "http://dev.api.vidaHouse.com/roles/v1.0/roles/";

        //删除角色
        //public static string DelRoleUrl = host + "/roles/v1.0/roles/";
        public static string DelRoleUrl = "http://dev.api.vidaHouse.com/roles/v1.0/roles/";

        #endregion

        #region 权限webapi

        //public static string AuthorityListUrl = host + "/roles/v1.0/permissions";
        public static string AuthorityListUrl = "http://dev.api.vidaHouse.com/roles/v1.0/permissions";

       // public static string AuthorityByRoleId = host + "/roles/v1.0/permissions/";
        public static string AuthorityByRoleId = "http://dev.api.vidaHouse.com/roles/v1.0/permissions/";

        #endregion

        #region 成员 webapi

        //获取成员列表
        // public static string MemberListUrl = host + "/users/v1.0/users?";
        public static string MemberListUrl = "http://dev.api.vidaHouse.com/users/v1.0/users?";

        //获取单个成员
        // public static string MemberModelUrl = host + "/users/v1.0/users?Id=";
        public static string MemberModelUrl = "http://dev.api.vidaHouse.com/users/v1.0/users?Id=";

        //添加成员
        // public static string AddMemberUrl = host + "/users/v1.0/users";
        public static string AddMemberUrl = "http://dev.api.vidaHouse.com/users/v1.0/users";

        //编辑成员
        // public static string EditMemberUrl = host + "/users/v1.0/users";
        public static string EditMemberUrl = "http://dev.api.vidaHouse.com/users/v1.0/users";

        //删除成员
        // public static string DelMemberUrl = host + "/users/v1.0/users?Id=";
        public static string DelMemberUrl = "http://dev.api.vidaHouse.com/users/v1.0/users?Id=";

        #endregion

    }
}
