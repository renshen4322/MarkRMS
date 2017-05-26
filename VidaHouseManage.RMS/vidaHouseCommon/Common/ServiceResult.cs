using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VidaHouseManage.Common.Common
{
    
    [Serializable, JsonObject(MemberSerialization.OptOut)]
    public class ServiceResult
    {
        public ServiceResult()
        {
            this.Success = false;
            this.Message = string.Empty;
            this.Data = null;
            this.Code = 0;
        }

        public ServiceResult(bool success)
        {
            this.Success = success;
            this.Message = string.Empty;
            this.Data = null;
            this.Code = 0;
        }

        public ServiceResult(bool success, string message)
        {
            this.Success = success;
            this.Message = message;
            this.Data = null;
            this.Code = 0;
        }

        public ServiceResult(bool success, string message, int code)
        {
            this.Success = success;
            this.Message = message;
            this.Data = null;
            this.Code = code;
        }


        public bool Success { get; set; }

        public int Code { get; set; }

        public object Data { get; set; }

        public string Message { get; set; }
    }

    [Serializable, JsonObject(MemberSerialization.OptOut)]
    public class ServiceResult<T>
    {
        public ServiceResult()
        {
            Success = false;
            Message = string.Empty;
            Code = 0;
        }

        public bool Success { get; set; }

        public T Data { get; set; }

        public string Message { get; set; }

        public int Code { get; set; }

    }

    [Serializable, JsonObject(MemberSerialization.OptOut)]
    public class PageListResult<T>
    {
        public PageListResult( int pageIndex, int pageSize, int totalCount, int totalPages,bool success,string message)
        {
            this.PageIndex = pageIndex;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            this.TotalPages = totalPages;
            this.Success = success;
            this.Message = message;
        }
        public int PageIndex { get;  set; }
        public int PageSize { get;  set; }
        public int TotalCount { get;  set; }
        public int TotalPages { get;  set; }
        public T Data { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }
    }

    [Serializable]
    public class PagedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public int TotalPages { get; private set; }

        public bool HasPreviousPage
        {
            get { return (PageIndex > 0); }
        }
        public bool HasNextPage
        {
            get { return (PageIndex + 1 < TotalPages); }
        }

        public PagedList(IList<T> source, int total, int pageIndex, int pageSize)
        {
            TotalCount = total;
            TotalPages = TotalCount / pageSize;
            if (TotalCount % pageSize > 0)
                TotalPages++;

            this.PageSize = pageSize;
            this.PageIndex = pageIndex;

            if (pageIndex > TotalPages)
                this.PageIndex = TotalPages;

            this.AddRange(source);
        }

    }



}
