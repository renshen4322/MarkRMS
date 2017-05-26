using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;
using VidaHouseManage.Service;
using VidaHouseManage.Service.IService;

namespace VidaHouseManage.Controllers
{
    public class OrganizationController : BaseController
    {
        private IOrganizationService organizationService = new OrganizationService();

        private IRoleService roleService = new RoleService();

        private IAuthorityService authorityService = new AuthorityService();

        #region 组织管理

        public ActionResult OrganizationList()
        {

            return View();
        }

        [HttpGet]
        public ActionResult GetOrganizationList()
        {
            var result = organizationService.GetOrganizationList();

            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public ActionResult AddOrganization(OrganizationEntity entity)
        {
            var result = organizationService.AddOrganization(entity);

            return Json(result);
        }

        [HttpPost]
        public ActionResult EditOrganization(OrganizationEntity entity)
        {
            var result = organizationService.EditOrganization(entity);

            return Json(result);
        }

        [HttpPost]
        public ActionResult DelOrganization(string id)
        {
            var result = organizationService.DelOrganization(id, Session["UserName"].ToString());

            return Json(result);
        }

        #endregion

        #region 组织角色管理

        [HttpPost]
        public ActionResult GetRolesByOrganization(string Id)
        {
            var result= roleService.GetRoelsByOrganizationId(Id);

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetAuthorityList()
        {
            var result = authorityService.GetAuthorityList();

            return Json(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult CreateRole(string[] Names, RolesEntity model)
        {
            var result = roleService.CreateRoles(Names, model);

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetRoleById(long Id)
        {
            var result = roleService.GetRolesById(Id);

            return Json(result);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult EditRole(string[] Names, RolesEntity model)
        {
            var result = roleService.EditRoles(Names, model);

            return Json(result);
        }

        [HttpPost]
        public ActionResult DelRole(string Id)
        {
            var result = roleService.DelRoles(Id, Session["UserName"].ToString());

            return Json(result);
        }

        #endregion
    }
}