using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Entity
{
    public class OrganizationEntity
    {
        public OrganizationEntity()
        {
            SubOrganizations = new List<OrganizationEntity>();

            Name = string.Empty;

            Scope = string.Empty;

            BucketName = string.Empty;

            IsTopLevel = false;

            Description = string.Empty;
        }

        /// <summary>
        /// 组织标识
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        /// <summary>
        /// 组织名称
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        /// <summary>
        /// 组织作用范围
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }
        /// <summary>
        /// 组织的数据存储地址
        /// </summary>
        [JsonProperty("bucketName")]
        public string BucketName { get; set; }

        /// <summary>
        /// 是否顶级组织
        /// </summary>
        [JsonProperty("isTopLevel")]
        public bool IsTopLevel { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("subOrganizations")]
        public List<OrganizationEntity> SubOrganizations { get; set; }
    }

}
