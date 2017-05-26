using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using VidaHouseManage.Common;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;

namespace VidaHouseManage.Controllers
{
    public class SchemesController : Controller
    {
        private string allSchemeUrl = "https://api.vidahouse.com/designing/v1.0/Schemes/Owner?";//查询自己所有的方案url
        private string deleteSchemeUrl = "https://api.vidahouse.com/designing/v1.0/Schemes/";  //删除某个指定的方案
        private string shardeSchemeUrl = "https://api.vidahouse.com/designing/v1.0/Schemes/shared?schemeVersionId="; //分享方案到好友圈
        private string cancelShardeUrl = "https://api.vidahouse.com/designing/v1.0/Schemes/shared?schemeVersionId= "; //取消分享
        private int PageSize = 12;
        private ServerRequestHelper serverHelper = new ServerRequestHelper();
       
        // GET: Products
        public ActionResult Index(string strOrderBy = null, string strOrder = null, string strKeyWord = null, string strView = "1", int PageIndex = 1)
        {
            if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ViewCutInfo = strView;
            PagerInfo pager = new PagerInfo();
            pager.PageSize = PageSize;
            pager.CurrentPageIndex = PageIndex;
            Dictionary<string, string> httpParams = new Dictionary<string, string>();
            httpParams = MergeParmInfo(strOrderBy,  strOrder, strKeyWord, httpParams);
            SchemeResponseEntity requstParm = RequestHttpInfo(httpParams);
            List<SchemeModel> listResponseEntity = new List<SchemeModel>();
            if (requstParm.count > 0)
            {
                var totalCount = requstParm.count; 
                var getModule = totalCount % PageSize; 
                var divCount = totalCount / PageSize; 
                var pageCount = (getModule == 0) ? divCount : (divCount + 1); 
                if (requstParm.count <= PageSize)
                {
                    httpParams = new Dictionary<string, string>();
                    httpParams.Add("Index", PageIndex.ToString());
                    httpParams.Add("Size", totalCount.ToString());  
                    httpParams = MergeParmInfo(strOrderBy, strOrder, strKeyWord, httpParams);
                    requstParm = RequestHttpInfo(httpParams);
                    AddDataToList(requstParm, listResponseEntity);
                }
                else
                {
                    httpParams = new Dictionary<string, string>();
                    httpParams.Add("Index", PageIndex.ToString());
                    httpParams.Add("Size", PageSize.ToString());
                    httpParams = MergeParmInfo(strOrderBy, strOrder, strKeyWord, httpParams);
                    requstParm = RequestHttpInfo(httpParams);
                    AddDataToList(requstParm, listResponseEntity);
                }
            }
            pager.RecordCount = requstParm.count;
            pager.ViewCut = strView;
            PagerQuery<PagerInfo, IList<SchemeModel>> query = new PagerQuery<PagerInfo, IList<SchemeModel>>(pager, listResponseEntity);
            ViewBag.DataList = query;
            return View("SchemeIndex");
        }
        /// <summary>
        /// 添加数据到list
        /// </summary>
        /// <param name="requstParm"></param>
        /// <param name="listResponseEntity"></param>
        private static void AddDataToList(SchemeResponseEntity requstParm, List<SchemeModel> listResponseEntity)
        {
            foreach (var data in requstParm.data)
            {
                SchemeModel entity = new SchemeModel();
                entity.id = data.id;
                entity.name = data.name;
                entity.images = data.images;
                data.images = Regex.Replace(data.images, @"(\r\n|\t+)", "");

                if (Regex.IsMatch(data.images, @"{[\s\S]*}"))
                {
                    entity.schemePic = JsonConvert.DeserializeObject<SchemePic>(data.images);

                    entity.images = entity.schemePic.screenshots.Count > 0 ? entity.schemePic.screenshots[0] : "";
                }
                else
                {
                    entity.images = data.images;
                }
                entity.description = data.description;
                entity.published = data.published;
                entity.revision = data.revision;
                entity.freezed = data.freezed;
                entity.isShared = data.isShared;
                entity.collectedNumber = data.collectedNumber;
                entity.createdUtc = string.Format("{0}", Convert.ToDateTime(data.createdUtc));
                listResponseEntity.Add(entity);
            }
        }
        /// <summary>
        /// 根据参数请求http数据反序列为实体
        /// </summary>
        /// <param name="httpParams"></param>
        /// <returns></returns>
        private SchemeResponseEntity RequestHttpInfo(Dictionary<string, string> httpParams)
        {
            var formDataString = HttpRequstHelper.FormatParams(httpParams);
            var url = allSchemeUrl + formDataString;
            var result = serverHelper.RequestGetReuse(url);
            var requstParm = JsonConvert.DeserializeObject<SchemeResponseEntity>(result); //根据查询条件获得总条数，
            return requstParm;
        }
        /// <summary>
        /// 组合参数
        /// </summary>
        /// <param name="strKeyWord"></param>
        /// <param name="strOrderBy"></param>
        /// <param name="strOrder"></param>
        /// <param name="strCategory"></param>       
        /// <returns></returns>
        private Dictionary<string, string> MergeParmInfo(string strOrderBy = null, string strOrder = null, string strKeyWord = null, Dictionary<string, string> httpParams = null)
        {

            if (!string.IsNullOrEmpty(strKeyWord))
            {
                httpParams.Add("Keyword", strKeyWord);
                ViewBag.KeywordInfo = strKeyWord.Trim();
            }
            if (!string.IsNullOrEmpty(strOrderBy))
            {
                httpParams.Add("Orderby", strOrderBy);
                ViewBag.OrderByInfo = strOrderBy.Trim();
            }
            //else
            //{
            //    httpParams.Add("Orderby", "CreatedUtc");
            //    ViewBag.OrderByInfo = "CreatedUtc";
            //}
           
            if (!string.IsNullOrEmpty(strOrder))
            {
                httpParams.Add("Order", strOrder);
                ViewBag.OrderInfo = strOrder;
            }
            return httpParams;
        }

        public int Delete(int Id)
        {
            if (Id <= 0)
                return -1;
            string result = serverHelper.RequestDeleteReuse(deleteSchemeUrl + Id, null);
            var falg = result.Contains("errorCode");
            if (falg)
            {
                return -2;
            }
            else
            {
                LogHelper.Info("用户：" + Session["UserName"] + "删除了Id为_" + Id + "的方案");
                return 1;
            }
        }

        /// <summary>
        /// 删除
        /// Author: mark.dong
        /// 支持复选
        /// </summary>
        /// <param name="needDelIds"></param>
        public RedirectToRouteResult DeleteSelect(string needDelIds, int pageIndex = 1, string strView = "1")
        {
            if (!string.IsNullOrEmpty(needDelIds) && needDelIds != "undefined")
            {
                if (needDelIds.Contains(","))
                {
                    needDelIds = needDelIds.Substring(1, needDelIds.Length - 1);
                }
                var ids = needDelIds.Split(',');
                foreach (var item in ids)
                {
                    DeleteSysMsg(item);
                   
                }
            }
            return this.RedirectToAction("Index", new { pageIndex = pageIndex,strView = strView });
        }
        private void DeleteSysMsg(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "undefined")
            {
                string result = serverHelper.RequestDeleteReuse(deleteSchemeUrl + id, null);
                if (result == "")
                {
                    LogHelper.Info("用户：" + Session["UserName"] + "删除了Id为" + id + "的方案");
                }
            }
        }
        /// <summary>
        /// 取消分享
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int CancelShardInfo(int id)
        {
            if (id <= 0)
                return -1;

            var helper = new HttpHelper();

            var result = helper.RequestDeleteReuse(cancelShardeUrl + id);
           
            if (result.Success)
            {
                return 1;
            }
            else
            {

                return -2;
            }
        }

        public int ShardInfo(int id)
        {
            if (id <= 0)
                return -1;
            string result = serverHelper.RequestPutReuse(shardeSchemeUrl + id, null);
            var falg = result.Contains("errorCode");
            if (falg)
            {
                return -2;
            }
            else
            {
                return 1;
            }
        }
    }
}