using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Entity
{
    public class MaterialResponseEntity
    {
        /// <summary>
        /// Data
        /// </summary>
        public List<MaterialModel> data { get; set; }
        /// <summary>
        /// Count
        /// </summary>
        public int count { get; set; }
    }

    public class MaterialRoot
    {
        public MaterialModel Data { get; set; }
    }
    public class MaterialModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// MaterialId
        /// </summary>
        public int materialId { get; set; }
        /// <summary>
        /// name
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// images
        /// </summary>
        public string images { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string description { get; set; }
        /// <summary>
        /// 448
        /// </summary>
        public string template { get; set; }
        /// <summary>
        /// Params
        /// </summary>
        public string Params { get; set; }

        /// <summary>
        /// Dna
        /// </summary>
        public MaterialDna dna { get; set; }
        /// <summary>
        /// CustomAttributes
        /// </summary>
        public List<MaterialCustomAttributes> customAttributes { get; set; }
        /// <summary>
        /// CategoryId
        /// </summary>
        public int categoryId { get; set; }
        /// <summary>
        /// SupplierId
        /// </summary>
        public int supplierId { get; set; }
        /// <summary>
        /// 2017-01-18T13:46:15Z
        /// </summary>
        public string createdUtc { get; set; }
        /// <summary>
        /// 2017-01-18T13:49:00.8361721Z
        /// </summary>
        public string updatedUtc { get; set; }
        /// <summary>
        /// LsPublished
        /// </summary>
        public bool isPublished { get; set; }
        /// <summary>
        /// Latest
        /// </summary>
        public bool latest { get; set; }
        /// <summary>
        /// Revision
        /// </summary>
        public int revision { get; set; }
        /// <summary>
        /// IsLiked
        /// </summary>
        public bool isLiked { get; set; }
        /// <summary>
        /// LikedNumber
        /// </summary>
        public int likedNumber { get; set; }
        /// <summary>
        /// IsCollected
        /// </summary>
        public bool isCollected { get; set; }
        /// <summary>
        /// CollectedNumber
        /// </summary>
        public int collectedNumber { get; set; }
        public string tags { get; set; }
        public List<int> tagIds { get; set; }
        /// <summary>
        /// ExtraData
        /// </summary>
        public string extraData { get; set; }
    }
    public class MaterialDna
    {
        /// <summary>
        /// Style
        /// </summary>
        public int style { get; set; }
        /// <summary>
        /// Complexity
        /// </summary>
        public int complexity { get; set; }
        /// <summary>
        /// Metalness
        /// </summary>
        public int metalness { get; set; }
        /// <summary>
        /// Roughness
        /// </summary>
        public int roughness { get; set; }
        /// <summary>
        /// Size
        /// </summary>
        public int size { get; set; }
        /// <summary>
        /// Gender
        /// </summary>
        public int gender { get; set; }
        /// <summary>
        /// Age
        /// </summary>
        public int age { get; set; }
    }

    public class MaterialCustomAttributes
    {
        /// <summary>
        /// tBaseTexture
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// BaseTexture
        /// </summary>
        public string displayName { get; set; }
        /// <summary>
        /// DownloadFile
        /// </summary>
        public string valueType { get; set; }
        /// <summary>
        /// value
        /// </summary>
        public string value { get; set; }
    }
    
}
