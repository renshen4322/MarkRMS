namespace System.Web.Mvc
{
    public class PagerInfo
    {
        public int RecordCount { get; set; }

        public int CurrentPageIndex { get; set; }

        public int PageSize { get; set; }

        public string ViewCut { get; set; }

        public string Slug { get; set; }
    }
}