using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Entity
{
    public class RolesEntity
    {
        public RolesEntity()
        {
            Name = string.Empty;

            Description = string.Empty;

            Permissions = new List<string>();

            AuthorList = new List<AuthorityEntity>();

            IsSelected = false;
        }

        public string Id { get; set; }

        public string Name { get; set; }

        [JsonIgnore]
        public string OrganizationId { get; set; }

        public string Description { get; set; }

        [JsonIgnore]
        public bool IsSelected { get; set; }

        public List<string> Permissions { get; set; }

        [JsonIgnore]
        public List<AuthorityEntity> AuthorList { get; set; }
    }
}
