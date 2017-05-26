using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VidaHouseManage.Common;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;
using VidaHouseManage.Service;

namespace VidaHouseManage.Controllers
{
    public class MaterialsController : Controller
    {
        private string allMaterialUrl = "https://api.vidahouse.com/designing/v1.0/Materials?";//查询自己所有的材质url
        private string deleteMaterialUrl = "https://api.vidahouse.com/designing/v1.0/Materials/";//删除固有材质根据id
        private string VersionByIdUrl = "https://api.vidahouse.com/designing/v1.0/Materials/version/"; //根据版本id获取该材质的所有详情
        private string putCategoryUrl = "https://api.vidahouse.com/designing/v1.0/Materials/";  //更新材质数据
        private string updateAndPublishUrl = "https://api.vidahouse.com/designing/v1.0/Materials/publish/";//更新并发布产品url
        private int PageSize = 10;
        private ServerRequestHelper serverHelper = new ServerRequestHelper();
        private MaterialService _materialService = new MaterialService();
        // GET: Products
        public ActionResult Index(string strOrderBy = null, string strCategory = "-1", string parentId = "-2", string strOrder = null, string strKeyWord = null, string strView = "1", int PageIndex = 1)
        {
            if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ViewCutInfo = strView.Trim();


            if (!string.IsNullOrEmpty(parentId))
                ViewBag.ParentCategoryInfo = parentId.Trim();

            PagerInfo pager = new PagerInfo();
            pager.PageSize = PageSize;
            pager.CurrentPageIndex = PageIndex;
            Dictionary<string, string> httpParams = new Dictionary<string, string>();
            httpParams = MergeParmInfo(strOrderBy, strCategory, strOrder, strKeyWord, httpParams);
            MaterialResponseEntity requstParm = RequestHttpInfo(httpParams);
            List<MaterialModel> listResponseEntity = new List<MaterialModel>();
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
                    httpParams = MergeParmInfo(strOrderBy, strCategory, strOrder, strKeyWord, httpParams);
                    requstParm = RequestHttpInfo(httpParams);
                    AddDataToList(requstParm, listResponseEntity);
                }
                else
                {
                    httpParams = new Dictionary<string, string>();
                    httpParams.Add("Index", PageIndex.ToString());
                    httpParams.Add("Size", PageSize.ToString());
                    httpParams = MergeParmInfo(strOrderBy, strCategory, strOrder, strKeyWord, httpParams);
                    requstParm = RequestHttpInfo(httpParams);
                    AddDataToList(requstParm, listResponseEntity);
                }
            }
            pager.RecordCount = requstParm.count;
            pager.ViewCut = strView;
            PagerQuery<PagerInfo, IList<MaterialModel>> query = new PagerQuery<PagerInfo, IList<MaterialModel>>(pager, listResponseEntity);
            ViewBag.DataList = query;
            return View("MaterialsIndex");
        }
        /// <summary>
        /// 添加数据到list
        /// </summary>
        /// <param name="requstParm"></param>
        /// <param name="listResponseEntity"></param>
        private static void AddDataToList(MaterialResponseEntity requstParm, List<MaterialModel> listResponseEntity)
        {
            foreach (var data in requstParm.data)
            {
                MaterialModel entity = new MaterialModel();
                entity.id = data.id;
                entity.name = data.name;
                entity.images = data.images;
                entity.categoryId = data.categoryId;
                entity.description = data.description;
                entity.collectedNumber = data.collectedNumber;
                entity.revision = data.revision;
                entity.isPublished = data.isPublished;
                entity.supplierId = data.supplierId;
                entity.createdUtc = string.Format("{0}", Convert.ToDateTime(data.createdUtc));
                listResponseEntity.Add(entity);
            }
        }
        /// <summary>
        /// 根据参数请求http数据反序列为实体
        /// </summary>
        /// <param name="httpParams"></param>
        /// <returns></returns>
        private MaterialResponseEntity RequestHttpInfo(Dictionary<string, string> httpParams)
        {
            var formDataString = HttpRequstHelper.FormatParams(httpParams);
            var url = allMaterialUrl + formDataString;
            var result = serverHelper.RequestGetReuse(url);
            var requstParm = JsonConvert.DeserializeObject<MaterialResponseEntity>(result); //根据查询条件获得总条数，
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
        private Dictionary<string, string> MergeParmInfo(string strOrderBy = null, string strCategory = null, string strOrder = null, string strKeyWord = null, Dictionary<string, string> httpParams = null)
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

            if (string.IsNullOrEmpty(strCategory))
            {
                strCategory = "-1";
            }

            if (int.Parse(strCategory)>0)
            {
                httpParams.Add("Category", strCategory);
               
            }
            ViewBag.CategoryInfo = strCategory.Trim();
            return httpParams;
        }

        public int Delete(int Id)
        {
            if (Id <= 0)
                return -1;
            string result = serverHelper.RequestDeleteReuse(deleteMaterialUrl + Id, null);
            var falg = result.Contains("errorCode");
            if (falg)
            {
                return -2;
            }
            else
            {
                LogHelper.Info("用户：" + Session["UserName"] + "删除了id为_" + Id + "的材质");
                return 1;
            }
        }
        /// <summary>
        /// 修改分类后更新并发布一个新的材质
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int PublishInfo(int id)
        {
            if (id <= 0)
                return -1;
            var entity = new MaterialModel();
            var resultVersion = serverHelper.RequestGetReuse(VersionByIdUrl + id);
            var requstParm = JsonConvert.DeserializeObject<MaterialRoot>(resultVersion);
            if (requstParm != null)
            {
                entity.id = id;
                entity.materialId = requstParm.Data.materialId;
                entity.name = requstParm.Data.name;
                entity.images = requstParm.Data.images;
                entity.description = requstParm.Data.description;
                entity.template = requstParm.Data.template;
                entity.Params = requstParm.Data.Params;
                entity.dna = requstParm.Data.dna;
                entity.customAttributes = requstParm.Data.customAttributes;
                entity.categoryId = requstParm.Data.categoryId;
                entity.supplierId = requstParm.Data.supplierId;
                entity.createdUtc = requstParm.Data.createdUtc;
                entity.updatedUtc = string.Format("{0:yyyy-MM-dd}t{0:hh:mm:ss}z", DateTime.Now).ToString().ToUpper();
                entity.isPublished = true;
                entity.latest = requstParm.Data.latest;
                entity.revision = requstParm.Data.revision + 1;
                entity.isLiked = requstParm.Data.isLiked;
                entity.likedNumber = requstParm.Data.likedNumber;
                entity.isCollected = requstParm.Data.isCollected;
                entity.collectedNumber = requstParm.Data.collectedNumber;
                entity.tags = requstParm.Data.tags;
                entity.tagIds = requstParm.Data.tagIds;
                entity.extraData = requstParm.Data.extraData;

            }
            var entityJson = JsonConvert.SerializeObject(entity);
            string result = serverHelper.RequestPutReuse(updateAndPublishUrl + id, entityJson);
            var falg = result.Contains("errorCode");
            if (falg)
            {
                return -2;
            }
            else
            {
                LogHelper.Info("用户：" + Session["UserName"] + "更新发布了Id为" + result + "的材质");
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
            return this.RedirectToAction("Index", new { pageIndex = pageIndex, strView = strView });
        }

        private void DeleteSysMsg(string id)
        {
            if (!string.IsNullOrEmpty(id) && id != "undefined")
            {
                string result = serverHelper.RequestDeleteReuse(deleteMaterialUrl + id, null);
                if (result == "")
                {
                    LogHelper.Info("用户：" + Session["UserName"] + "删除了Id为" + id + "的材质");
                }
            }
        }

        private void SetNodeNotOpen(TreeNode treeNodes)
        {

            treeNodes.state = null;

            if (treeNodes.children.Count > 0)
            {
                treeNodes.children.ForEach(x =>
                {
                    SetNodeNotOpen(x);
                });
            }

        }

        [HttpGet]
        public JsonResult TreeNodes(string categoryId = "0", string parentId = "0")
        {
            string cache_key = "mater_key";
            var nodeObj = HttpRuntime.Cache[cache_key];
            if (nodeObj == null)
            {
                List<TreeNode> listResult = _materialService.get();

                TreeNode Tree = new TreeNode();

                Tree.children = listResult;
                Tree.id = "-1";
                Tree.parentId = "-2";
                Tree.text = "全部";
                Tree.state = new State() { opened = true };

                CacheHelper.Insert(cache_key, Tree, 30);
            }
            nodeObj = HttpRuntime.Cache[cache_key];

            var TreeNode = (TreeNode)nodeObj;

            SetNodeNotOpen(TreeNode);


            if (categoryId == "-1")
            {
                TreeNode.state = new State() { selected = true, opened = true };
            }

            else if (int.Parse(categoryId) > 0 && parentId == "-1")
            {
                var nodelDetail = TreeNode.children.FirstOrDefault(x => x.id == categoryId);

                if (nodelDetail != null)
                    nodelDetail.state = new State() { opened = true, selected = true };

            }
            else if (int.Parse(categoryId) > 0 && int.Parse(parentId) > 0)
            {
                var nodelDetail = TreeNode.children.FirstOrDefault(x => x.id == parentId);

                if (nodelDetail != null)
                {
                    nodelDetail.state = new State() { opened = true };

                    var children = nodelDetail.children.FirstOrDefault(x => x.id == categoryId);

                    if (children != null)
                        children.state = new State() { opened = true, selected = true };
                }
            }

            return Json(TreeNode, JsonRequestBehavior.AllowGet);

        }

        /// <summary>
        /// 修改分类类型
        /// </summary>
        /// <param name="id"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        public int UpdateCategorie(int id, int categoryId, string nodeText)
        {
            var resultVersion = serverHelper.RequestGetReuse(VersionByIdUrl + id);
            var entity = new MaterialModel();
            var requstParm = JsonConvert.DeserializeObject<MaterialRoot>(resultVersion);
            if (requstParm != null)
            {
                entity.id = requstParm.Data.id;
                entity.materialId = requstParm.Data.materialId;
                entity.name = requstParm.Data.name;
                entity.categoryId = categoryId;
                entity.images = requstParm.Data.images;
                entity.description = nodeText;//requstParm.Data.description;
                entity.template = requstParm.Data.template;
                entity.Params = requstParm.Data.Params;
                entity.dna = requstParm.Data.dna;
                entity.customAttributes = requstParm.Data.customAttributes;
                entity.supplierId = requstParm.Data.supplierId;
                entity.createdUtc = requstParm.Data.createdUtc;
                entity.updatedUtc = string.Format("{0:yyyy-MM-dd}t{0:hh:mm:ss}z", DateTime.Now).ToString().ToUpper();
                entity.latest = requstParm.Data.latest;
                entity.revision = requstParm.Data.revision;
                entity.isLiked = requstParm.Data.isLiked;
                entity.likedNumber = requstParm.Data.likedNumber;
                entity.isCollected = requstParm.Data.isCollected;
                entity.isPublished = requstParm.Data.isPublished;
                entity.collectedNumber = requstParm.Data.collectedNumber;
                entity.tags = requstParm.Data.tags;
                entity.tagIds = requstParm.Data.tagIds;
                entity.extraData = requstParm.Data.extraData;

            }
            var entityJson = JsonConvert.SerializeObject(entity);
            string result = serverHelper.RequestPutReuse(putCategoryUrl + id, entityJson);
            var falg = result.Contains("errorCode");
            if (falg)
            {
                return -2;
            }
            else
            {
                LogHelper.Info("用户：" + Session["UserName"] + "修改材质" + id + "_的类型为" + categoryId + "_" + nodeText);
                return 1;
            }
        }

    }

}
