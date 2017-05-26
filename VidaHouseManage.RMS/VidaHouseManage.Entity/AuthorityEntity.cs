using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Entity
{
    public class AuthorityEntity
    {
        public AuthorityEntity()
        {
            IsSelected = false;
        }

        public string Name { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        public bool IsSelected { get; set; }

    }
}
