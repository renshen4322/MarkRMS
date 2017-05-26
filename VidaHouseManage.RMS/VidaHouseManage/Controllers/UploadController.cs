using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidaHouseManage.Common;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;

namespace VidaHouseManage.Controllers
{
    public class UploadController : Controller
    {
        private string allUploadUrl = "https://api.vidahouse.com/designing/v1.0/Upload/Owner?";//查询所有自己的资源url
        private string deleteUploadUrl = "https://api.vidahouse.com/designing/v1.0/Upload/"; //删除固有资源根据id
        private string publicUrl = "https://api.vidahouse.com/designing/v1.0/Upload/";

        private int PageSize = 12;
        private ServerRequestHelper serverHelper = new ServerRequestHelper();

        // GET: Products
        public ActionResult Index(string strView = "1", int PageIndex = 1)
        {
            if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic = LoadDict(dic);
            ViewBag.DictInfo = dic;
            ViewBag.ViewPlacement = strView;
            return View("UploadIndex");
        }
        public Dictionary<string, string> LoadDict(Dictionary<string, string> dic = null)
        {
            dic.Add("mesh", "模型");
            dic.Add("basetexture", "漫反射贴图");
            dic.Add("normal", "法线贴图");
            dic.Add("mask", "遮罩贴图");
            dic.Add("materialicon", "材料贴图");
            dic.Add("itemicon", "物品图标");
            dic.Add("p360", "360全景图");
            dic.Add("p360shot", "360全景图截图");
            dic.Add("designshot", "方案截图");
            dic.Add("dnashot", "DNA截图");
            dic.Add("headicon", "账号头像");
            dic.Add("generic", "通用资源");
            dic.Add("static", "静态存储");
            dic.Add("item360", "物品360°环绕图");
            dic.Add("fbxsource", "用户上传的fbx源文件");
            dic.Add("picsource", "用户上传的图片源文件");
            dic.Add("screenshot", "屏幕截图");
            dic.Add("babylon", "babylon");

            return dic;
        }
        public ActionResult ShowUpload(string strSlug = "normal", string strKeyWord = null, string strView = "1", int PageIndex = 1)
        {
            if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic = LoadDict(dic);
            ViewBag.DictInfo = dic;
            ViewBag.ViewCutInfo = strView;
            PagerInfo pager = new PagerInfo();
            pager.PageSize = PageSize;
            pager.CurrentPageIndex = PageIndex;
            Dictionary<string, string> httpParams = new Dictionary<string, string>();
            httpParams = MergeParmInfo(strSlug, strKeyWord, httpParams);
            UploadResponseEntity requstParm = RequestHttpInfo(httpParams);
            List<UploadModel> listResponseEntity = new List<UploadModel>();
            if (requstParm.Count > 0)
            {
                var totalCount = requstParm.Count; 
                var getModule = totalCount % PageSize; 
                var divCount = totalCount / PageSize; 
                var pageCount = (getModule == 0) ? divCount : (divCount + 1); 
                if (requstParm.Count <= PageSize)
                {
                    httpParams = new Dictionary<string, string>();
                    httpParams.Add("Index", PageIndex.ToString());
                    httpParams.Add("Size", totalCount.ToString());  
                    httpParams = MergeParmInfo(strSlug, strKeyWord, httpParams);
                    requstParm = RequestHttpInfo(httpParams);
                    AddDataToList(requstParm, listResponseEntity);
                }
                else
                {
                    httpParams = new Dictionary<string, string>();
                    httpParams.Add("Index", PageIndex.ToString());
                    httpParams.Add("Size", PageSize.ToString());
                    httpParams = MergeParmInfo(strSlug, strKeyWord, httpParams);
                    requstParm = RequestHttpInfo(httpParams);
                    AddDataToList(requstParm, listResponseEntity);
                }
            }
            pager.RecordCount = requstParm.Count;
            pager.ViewCut = strView;
            pager.Slug = strSlug;
            PagerQuery<PagerInfo, IList<UploadModel>> query = new PagerQuery<PagerInfo, IList<UploadModel>>(pager, listResponseEntity);
            ViewBag.DataList = query;
            return View("ShowUpload");
        }
        /// <summary>
        /// 添加数据到list
        /// </summary>
        /// <param name="requstParm"></param>
        /// <param name="listResponseEntity"></param>
        private void AddDataToList(UploadResponseEntity requstParm, List<UploadModel> listResponseEntity)
        {
            foreach (var data in requstParm.Data)
            {
                UploadModel entity = new UploadModel();
                entity.Id = data.Id;
                entity.FileName = data.FileName.Length>25?data.FileName.Substring(0,25):data.FileName;
                entity.Url = data.Url;
                entity.MediaType = data.MediaType;
                entity.OwnerId = data.OwnerId;
                entity.HashCode = data.HashCode;
                entity.shiftSize = GetResouseSize(data.Size);
                entity.IsPublic = data.IsPublic;
                entity.UpdatedUtc = data.UpdatedUtc;
                entity.CreatedUtc = string.Format("{0}", Convert.ToDateTime(data.CreatedUtc));
                listResponseEntity.Add(entity);
            }
        }
        private string GetResouseSize(int size)
        {
            int sizeGb = 1024 * 1024;
            string resize = "";
            if (size > sizeGb)
            {
                resize = size / sizeGb + "(GB)";
            }
            else if (size > 1024 & size < sizeGb)
            {
                resize = size / 1024 + "(MB)";
            }
            else
            {
                resize = size + "(KB)";
            }
            return resize;
        }
        /// <summary>
        /// 根据参数请求http数据反序列为实体
        /// </summary>
        /// <param name="httpParams"></param>
        /// <returns></returns>
        private UploadResponseEntity RequestHttpInfo(Dictionary<string, string> httpParams)
        {
            var formDataString = HttpRequstHelper.FormatParams(httpParams);
            var url = allUploadUrl + formDataString;
            var result = serverHelper.RequestGetReuse(url);
            var requstParm = JsonConvert.DeserializeObject<UploadResponseEntity>(result); //根据查询条件获得总条数，
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
        private Dictionary<string, string> MergeParmInfo(string strSlug = null, string strKeyWord = null, Dictionary<string, string> httpParams = null)
        {
            if (!string.IsNullOrEmpty(strSlug))
            {
                httpParams.Add("Slug", strSlug);
                ViewBag.SlugInfo = strSlug.Trim();
            }

            if (!string.IsNullOrEmpty(strKeyWord))
            {
                httpParams.Add("Keyword", strKeyWord);
                ViewBag.KeywordInfo = strKeyWord.Trim();
            }

            return httpParams;
        }

        public int Delete(int Id)
        {
            if (Id <= 0)
                return -1;
            string result = serverHelper.RequestDeleteReuse(deleteUploadUrl + Id, null);
            var falg = result.Contains("errorCode");
            if (falg)
            {
                return -2;
            }
            else
            {
                LogHelper.Info("用户：" + Session["UserName"] + "删除了Id为_" + Id + "的资源文件");
                return 1;
            }

        }
        /// <summary>
        /// 公开
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isPublic"></param>
        /// <returns></returns>
        public int publicInfo(int id)
        {

            string url = publicUrl + id + "?isPublic=" + true;
            string result = serverHelper.RequestPostReusestr(url, null);
            if (result.Contains("errorCode"))
            {
                return -1;
            }
            else
            {
                return 1;
            }


        }
        public int CancelPublicInfo(int id)
        {
            string url = publicUrl + id + "?isPublic=" + false;
            string result = serverHelper.RequestPostReusestr(url, null);
            if (result.Contains("errorCode"))
            {
                return -2;
            }
            else
            {
                return 2;
            }
        }

        /// <summary>
        /// 删除
        /// Author: mark.dong
        /// 支持复选
        /// </summary>
        /// <param name="needDelIds"></param>
        public RedirectToRouteResult DeleteSelect(string needDelIds, int pageIndex = 1, string strView = "1", string strSlug = "normal")
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
            return this.RedirectToAction("ShowUpload", new { pageIndex = pageIndex, strView = strView, strSlug = strSlug });
        }
        private void DeleteSysMsg(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "undefined")
            {
                string result = serverHelper.RequestDeleteReuse(deleteUploadUrl + id, null);
                if (result == "")
                {
                    LogHelper.Info("用户：" + Session["UserName"] + "删除了Id为" + id + "的资源文件");
                }
            }
        }
    }
}