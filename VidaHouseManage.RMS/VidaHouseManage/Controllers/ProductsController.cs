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
    public class ProductsController : Controller
    {
        private string allproductUrl = "https://api.vidahouse.com/designing/v1.0/Products?";//查询自己所有的物品url
        private string deleteProductUrl = "https://api.vidahouse.com/designing/v1.0/Products/"; //删除固有物品根据id
        private string VersionByIdUrl = "https://api.vidahouse.com/designing/v1.0/Products/version/"; //根据版本id获取该物品的所有详情
        private string putCategoryUrl = "https://api.vidahouse.com/designing/v1.0/Products/";  //更新物品数据
        private string updateAndPublishUrl = "https://api.vidahouse.com/designing/v1.0/Products/publish/";//更新并发布产品url
        //private string publicUrl = "https://api.vidahouse.com/designing/v1.0/Products/publish";
        private int PageSize = 10;
        private ServerRequestHelper serverHelper = new ServerRequestHelper();
        private ProductService _productService = new ProductService();
        // GET: Products
        public ActionResult Index(string strOrderBy = null, string strCategory = "-1", string parentId = "-2", string strOrder = null, string strKeyWord = null, string strView = "1", int PageIndex = 1)
        {
            if (Session["Token"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ViewCutInfo = strView;

            if (!string.IsNullOrEmpty(parentId))
                ViewBag.ParentCategoryInfo = parentId.Trim();

            PagerInfo pager = new PagerInfo();
            pager.PageSize = PageSize;
            pager.CurrentPageIndex = PageIndex;
            Dictionary<string, string> httpParams = new Dictionary<string, string>();
            httpParams = MergeParmInfo(strOrderBy, strCategory, strOrder, strKeyWord, httpParams);
            ProductResponseEntity requstParm = RequestHttpInfo(httpParams);
            List<ProductModel> listResponseEntity = new List<ProductModel>();
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
            pager.RecordCount = requstParm.Count;
            pager.ViewCut = strView;
            PagerQuery<PagerInfo, IList<ProductModel>> query = new PagerQuery<PagerInfo, IList<ProductModel>>(pager, listResponseEntity);
            ViewBag.DataList = query;
            return View("ProductIndex");
        }
        /// <summary>
        /// 添加数据到list
        /// </summary>
        /// <param name="requstParm"></param>
        /// <param name="listResponseEntity"></param>
        private static void AddDataToList(ProductResponseEntity requstParm, List<ProductModel> listResponseEntity)
        {
            foreach (var data in requstParm.Data)
            {
                ProductModel entity = new ProductModel();
                entity.Id = data.Id;
                entity.Name = data.Name;
                entity.Images = data.Images;
                entity.CategoryId = data.CategoryId;
                entity.OwnerId = data.OwnerId;
                entity.Description = data.Description;
                entity.Published = data.Published;
                entity.CollectedNumber = data.CollectedNumber;
                entity.Revision = data.Revision;
                entity.CreatedUtc = string.Format("{0}", Convert.ToDateTime(data.CreatedUtc));
                listResponseEntity.Add(entity);
            }
        }
        /// <summary>
        /// 根据参数请求http数据反序列为实体
        /// </summary>
        /// <param name="httpParams"></param>
        /// <returns></returns>
        private ProductResponseEntity RequestHttpInfo(Dictionary<string, string> httpParams)
        {
            var formDataString = HttpRequstHelper.FormatParams(httpParams);
            var url = allproductUrl + formDataString;
            var result = serverHelper.RequestGetReuse(url);
            var requstParm = JsonConvert.DeserializeObject<ProductResponseEntity>(result); //根据查询条件获得总条数，
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
        private Dictionary<string, string> MergeParmInfo(string strOrderBy = null, string strCategory = "0", string strOrder = null, string strKeyWord = null, Dictionary<string, string> httpParams = null)
        {

            if (!string.IsNullOrEmpty(strKeyWord))
            {
                httpParams.Add("Keyword", strKeyWord);
                ViewBag.KeywordInfo = strKeyWord.Trim();
            }
            if (!string.IsNullOrEmpty(strOrderBy))
            {
                httpParams.Add("Orderby", strOrderBy);
                ViewBag.OrderByInfo = strOrderBy;
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
            if (int.Parse(strCategory) > 0)
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
            string result = serverHelper.RequestDeleteReuse(deleteProductUrl + Id, null);
            var falg = result.Contains("errorCode");
            if (falg)
            {
                return -2;
            }
            else
            {
                LogHelper.Info("用户：" + Session["UserName"] + "删除了物品__" + Id);
                return 1;
            }

        }
        /// <summary>
        /// 修改分类后更新并发布一个新的物品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int PublishInfo(int id)
        {
            if (id <= 0)
                return -1;
            var entity = new ProductModel();
            var resultVersion = serverHelper.RequestGetReuse(VersionByIdUrl + id);
            var requstParm = JsonConvert.DeserializeObject<Root>(resultVersion);
            if (requstParm != null)
            {
                entity.Id = id;
                entity.ProductId = requstParm.Data.ProductId;
                entity.Name = requstParm.Data.Name;
                entity.CategoryId = requstParm.Data.CategoryId;
                entity.Style = requstParm.Data.Style;
                entity.Components = requstParm.Data.Components;
                entity.Size = requstParm.Data.Size;
                entity.FunctionType = requstParm.Data.FunctionType;
                entity.IsSharedModel = requstParm.Data.IsSharedModel;
                entity.IsVirtualProduct = requstParm.Data.IsVirtualProduct;
                entity.OwnerId = requstParm.Data.OwnerId;
                entity.Images = requstParm.Data.Images;
                entity.Description = requstParm.Data.Description;
                entity.CustomAttributes = requstParm.Data.CustomAttributes;
                entity.CreatedUtc = requstParm.Data.CreatedUtc;
                entity.UpdatedUtc = string.Format("{0:yyyy-MM-dd}t{0:hh:mm:ss}z", DateTime.Now).ToString().ToUpper();
                entity.Latest = requstParm.Data.Latest;
                entity.Revision = requstParm.Data.Revision + 1;
                entity.IsLiked = requstParm.Data.IsLiked;
                entity.LikedNumber = requstParm.Data.LikedNumber;
                entity.IsCollected = requstParm.Data.IsCollected;
                entity.CollectedNumber = requstParm.Data.CollectedNumber;
                entity.ExtraData = requstParm.Data.ExtraData;
                entity.Published = true;
                entity.BoundId = requstParm.Data.BoundId;
                entity.Tags = requstParm.Data.Tags;
                entity.TagIds = requstParm.Data.TagIds;
                entity.Listable = requstParm.Data.Listable;

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
                LogHelper.Info("用户：" + Session["UserName"] + "更新发布了物品__" + result);
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
                string result = serverHelper.RequestDeleteReuse(deleteProductUrl + id, null);
                if (result == "")
                {
                    LogHelper.Info("用户：" + Session["UserName"] + "删除了Id为" + id + "的物品");
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
            string cache_key = "product_key";
            var nodeObj = HttpRuntime.Cache[cache_key];
            if (nodeObj == null)
            {
                List<TreeNode> listResult = _productService.get();

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
            var entity = new ProductModel();
            var requstParm = JsonConvert.DeserializeObject<Root>(resultVersion);

            if (requstParm != null)
            {
                entity.Id = requstParm.Data.Id;
                entity.ProductId = requstParm.Data.ProductId;
                entity.Name = requstParm.Data.Name;
                entity.CategoryId = categoryId;
                entity.Style = requstParm.Data.Style;
                entity.Components = requstParm.Data.Components;
                entity.Size = requstParm.Data.Size;
                entity.FunctionType = requstParm.Data.FunctionType;
                entity.IsSharedModel = requstParm.Data.IsSharedModel;
                entity.IsVirtualProduct = requstParm.Data.IsVirtualProduct;
                entity.OwnerId = requstParm.Data.OwnerId;
                entity.Images = requstParm.Data.Images;
                entity.Description = nodeText;//requstParm.Data.Description;
                entity.CustomAttributes = requstParm.Data.CustomAttributes;
                entity.CreatedUtc = requstParm.Data.CreatedUtc;
                entity.UpdatedUtc = string.Format("{0:yyyy-MM-dd}t{0:hh:mm:ss}z", DateTime.Now).ToString().ToUpper();
                entity.Latest = requstParm.Data.Latest;
                var revision = requstParm.Data.Revision;
                entity.Revision = revision;
                entity.IsLiked = requstParm.Data.IsLiked;
                entity.LikedNumber = requstParm.Data.LikedNumber;
                entity.IsCollected = requstParm.Data.IsCollected;
                entity.CollectedNumber = requstParm.Data.CollectedNumber;
                entity.ExtraData = requstParm.Data.ExtraData;
                entity.Published = requstParm.Data.Published;
                entity.BoundId = requstParm.Data.BoundId;
                entity.Tags = requstParm.Data.Tags;
                entity.TagIds = requstParm.Data.TagIds;
                entity.Listable = requstParm.Data.Listable;

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
                LogHelper.Info("用户：" + Session["UserName"] + "修改物品" + id + "_的类型为" + categoryId + "_" + nodeText);
                return 1;
            }
        }
    }
}