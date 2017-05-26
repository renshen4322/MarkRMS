using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Common.Common
{
    public static class ServiceResultExtensions
    {
        public static ServiceResult<T> ToResult<T>(this T result, bool success = false, string message = "")
        {
            return new ServiceResult<T>()
            {
                Data = result,
                Success = success,
                Message = message
            };
        }

        public static ServiceResult ToResult(this bool success, string message = "", int code = 0)
        {
            return new ServiceResult(success, message, code) { };
        }

        public static PageListResult<T> ToPageResult<T>(this T rseult,bool success, int pageIndex = 0, int pageSize = 0, int totalCount = 0, int totalPages = 0,string message = "")
        {
            var SourceModel = new PageListResult<T>(pageIndex, pageSize, totalCount, totalPages, success,message) { Data= rseult };

            return SourceModel;
        }

    }
}
