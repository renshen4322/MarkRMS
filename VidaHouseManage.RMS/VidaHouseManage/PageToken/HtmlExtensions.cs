using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace VidaHouseManage.PageToken
{
    public static class HtmlExtensions
    {
        public static HtmlString GenerateVerficationToken(this HtmlHelper htmlhelper)
        {
            string formValue = Utility.Encrypt(HttpContext.Current.Session.SessionID + DateTime.Now.Ticks.ToString());
            HttpContext.Current.Session[PageTokenViewBase.SessionMyToken] = formValue;

            string fieldName = PageTokenViewBase.HiddenTokenName;
            TagBuilder builder = new TagBuilder("input");
            builder.Attributes["type"] = "hidden";
            builder.Attributes["name"] = fieldName;
            builder.Attributes["value"] = formValue;
            return new HtmlString(builder.ToString(TagRenderMode.SelfClosing));
        }

        #region 文本输入
        /// <summary>
        /// 输出大文本字段[目前采用PRE标签]
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MvcHtmlString LargeText(this HtmlHelper helper, string value)
        {
            // 生成与标题链接有关的HTML代码
            TagBuilder text = new TagBuilder("pre");
            text.AddCssClass("pre");
            text.InnerHtml = value;
            return MvcHtmlString.Create(text.ToString());
        }
        /// <summary>
        /// 输出大文本字段[目前采用PRE标签]
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString LargeTextFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression)
        {
            object data = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expression, helper.ViewData).Model;
            if (data != null)
            {
                TagBuilder text = new TagBuilder("pre");
                text.InnerHtml = data.ToString();
                text.AddCssClass("pre");
                return MvcHtmlString.Create(text.ToString());
            }
            return null;

        }
        #endregion

        #region 文本显示


        /// <summary>
        /// 文本字段[处理日期值]
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static MvcHtmlString Text(this HtmlHelper helper, DateTime? value, string format = "yyyy-MM-dd")
        {
            if (value == null)
            {
                return null;
            }
            return MvcHtmlString.Create(value.Value.ToString(format));
        }

        #endregion

        #region 按纽
        /// <summary>
        /// 输出按钮的HTML
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonId">按钮ID</param>
        /// <param name="buttonText">按钮文本</param>
        /// <param name="cssClass">按钮样式名</param>
        /// <param name="buttonTitle">按钮标题（光标移到按钮上显示的文本）</param>
        /// <param name="onClick">单击事件</param>
        /// <returns>按钮HTML</returns>
        public static MvcHtmlString Button(this HtmlHelper helper, string buttonId, string buttonText, string cssClass, string buttonTitle = null, string onClick = null)
        {
            StringBuilder render = new StringBuilder();
            render.AppendFormat("<input type=\"button\" id=\"{0}\" value=\"{1}\" class=\"ui-btn {2}\" onmouseover=\"this.className='ui-btn {2}-hover'\" onmouseout=\"this.className='ui-btn {2}'\" ", buttonId, buttonText, cssClass);

            if (!string.IsNullOrEmpty(buttonTitle))
            {
                render.Append(String.Format("title=\"{0}\" ", buttonTitle));
            }

            if (onClick != null)
            {
                render.Append(String.Format("onclick=\"{0}\" ", onClick));
            }

            render.Append("/>");
            return MvcHtmlString.Create(render.ToString());
        }

        /// <summary>
        /// 输出连接按纽
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="buttonId">按扭id</param>
        /// <param name="text">控纽文本</param>
        /// <param name="icon">按纽小图标</param>
        /// <returns></returns>
        public static MvcHtmlString LinkButton(this HtmlHelper helper, string buttonId, string text, string icon, string url)
        {
            string onclick = "javascript:window.location.href = '" + url + "'";
            return Button(helper, buttonId, text, icon, onclick);

        }

        #endregion
        #region DropDownList
        /// <summary>
        /// 根据字典类型绑定DropDownList
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="helper"></param>
        /// <param name="expression"></param>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        public static MvcHtmlString DictionaryItemDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> helper, Expression<Func<TModel, TProperty>> expression, string dictionaryType)
        {
            var selectList = GetSelectList(dictionaryType);
            return helper.DropDownList(ExpressionHelper.GetExpressionText(expression), selectList, new { style = "width:160px" });
        }

        /// <summary>
        /// 根据字典类型绑定DropDownList
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="name"></param>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        public static MvcHtmlString DictionaryItemDropDownList(this HtmlHelper helper, string name, string dictionaryType)
        {
            var selectList = GetSelectList(dictionaryType);
            return helper.DropDownList(name, selectList, new { style = "width:160px" });
        }
        /// <summary>
        /// 根据字典类型获取SelectList
        /// </summary>
        /// <param name="dictionaryType"></param>
        /// <returns></returns>
        private static SelectList GetSelectList(string dictionaryType)
        {
            Dictionary<string, string> dic = null;
            if (dictionaryType == "SGType")//SG类型
            {
                dic = new Dictionary<string, string>(){
                    {"","--请选择--"},
                    {"电气仪控设备","电气仪控设备"},
                    {"核燃料元件","核燃料元件"},
                    {"机械设备","机械设备"},
                    {"通用","通用"}
                 };
            }
            else if (dictionaryType == "SGTechLine")//SG技术路线
            {
                dic = new Dictionary<string, string>(){
                {"","--请选择--"},
                {"CPR","CPR"},
                {"AP100","AP100"},
                {"EPR","EPR"}
                };
            }
            else if (dictionaryType == "Language")//语言
            {
                dic = new Dictionary<string, string>(){
               {"","--请选择--"},
                {"中文","中文"},
                {"英文","英文"},
                {"法文","法文"}
                };
            }
            else if (dictionaryType == "SGFrequancy")//频度
            {
                dic = new Dictionary<string, string>(){
                {"","--请选择--"},
                {"P","P"},
                {"R","R"},
                {"Yn","Yn"},
                {"O","O"},
                {"E","E"},
                {"Sd","Sd"},
                {"Sf","Sf"},
                {"Sq","Sq"},
                {"Y","Y"},
                };

            }
            else if (dictionaryType == "SSPListType")//SSP类型
            {
                dic = new Dictionary<string, string>(){
                {"","--请选择--"},
                {"CPR","CPR"},
                {"EPR","EPR"}
                };

            }
            else if (dictionaryType == "MakeLevel")//制造级别
            {
                dic = new Dictionary<string, string>(){
                {"","--请选择--"},
                {"A","A"},
                {"B","B"}
                };

            }
            else
            {
                //字典类型
                dic = new Dictionary<string, string>() { { "", "--请选择--" } };
            }

            var selectList = new SelectList(dic, "Key", "Value");
            return selectList;
        }
        #endregion

        #region 下拉列表

        public static MvcHtmlString DropDownList(this HtmlHelper htmlHelper, string name, string optionLabel = null, string htmlAttributes = null)
        {
            var selectList = htmlHelper.ViewData.Eval(name) as IEnumerable<SelectListItem>;
            if (selectList == null) throw new ArgumentNullException();

            StringBuilder builder = new StringBuilder();
            builder.AppendFormat("<select name=\"{0}\" {1}>", name, htmlAttributes);
            if (optionLabel != null) builder.AppendFormat("<option value=\"\">{0}</option>", optionLabel);
            foreach (var item in selectList)
            {
                builder.AppendFormat("<option value=\"{0}\"{1}>{2}</option>",
                    item.Value,
                    item.Selected ? " selected=\"selected\"" : null,
                    item.Text);
            }
            builder.Append("</select>");
            return MvcHtmlString.Create(builder.ToString());
        }

        #endregion
    }
}