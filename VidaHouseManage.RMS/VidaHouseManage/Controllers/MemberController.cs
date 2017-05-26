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
    public class MemberController : BaseController
    {
        private IOrganizationService organizationService = new OrganizationService();

        private IMemberService memberService = new MemberService();

        private IRoleService roleService = new RoleService();

        #region 成员

        public ActionResult MemberList()
        {
            return View();
        }

        [HttpPost]
        public ActionResult MemberList(int Page = 1, int PageSize = 10, string OrganizationId = "")
        {
            var result = memberService.GetMemberList(Page, PageSize, OrganizationId);

            return Json(result);

        }

        [HttpPost]
        public ActionResult AddMember(string[] Ids, UserEntity entity)
        {
            var result = memberService.AddMember(Ids, entity);

            return Json(result);
        }

        [HttpPost]
        public ActionResult GetMemberById(string Id)
        {
            var result = memberService.GetMemberById(Id);

            return Json(result);
        }

        [HttpPost]
        public ActionResult EditMember(string[] Ids, UserEntity entity)
        {
            var result = memberService.EditMember(Ids, entity);

            return Json(result);
        }

        [HttpPost]

        public ActionResult DeleteMember(string Id)
        {
            var result = memberService.DelMember(Id,Session["UserName"].ToString());

            return Json(result);
        }

        #endregion

        #region 组织角色

        [HttpGet]
        public ActionResult GetOrganizationList()
        {
            var result = organizationService.GetOrganizationList();

            return Json(result.Data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GetRolesByOrganizationId(string Id)
        {
            var result = roleService.GetRoelsByOrganizationId(Id);

            return Json(result);
        }

        #endregion

    }
}