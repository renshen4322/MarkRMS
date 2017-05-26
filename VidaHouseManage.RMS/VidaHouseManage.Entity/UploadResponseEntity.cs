using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Entity
{
   public class UploadRoot
    {
        public UploadModel Data { get; set; }
    }

    public class UploadModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// sample string 2
        /// </summary>
        public string FileName { get; set; }
        /// <summary>
        /// sample string 3
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// sample string 4
        /// </summary>
        public string MediaType { get; set; }
        /// <summary>
        /// Size
        /// </summary>
        public int Size { get; set; }

        public string shiftSize { get; set; }
        /// <summary>
        /// sample string 6
        /// </summary>
        public string HashCode { get; set; }
        /// <summary>
        /// IsPublic
        /// </summary>
        public bool IsPublic { get; set; }
        /// <summary>
        /// 2017-02-14T15:05:29.2355424+08:00
        /// </summary>
        public string CreatedUtc { get; set; }
        /// <summary>
        /// 2017-02-14T15:05:29.2355424+08:00
        /// </summary>
        public string UpdatedUtc { get; set; }
        /// <summary>
        /// OwnerId
        /// </summary>
        public int OwnerId { get; set; }
        /// <summary>
        /// sample string 9
        /// </summary>
        public string ExtraData { get; set; }
    }

    public class UploadResponseEntity
    {
        /// <summary>
        /// Data
        /// </summary>
        public List<UploadModel> Data { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public int Count { get; set; }
    }

}
