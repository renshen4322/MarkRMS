using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VidaHouseManage.Entity
{

    public class UserResponseModel
    {
        public List<UserEntity> Data { get; set; }

        public int Count { get; set; }
    }

    public class UserRequestModel
    {
        public UserRequestModel()
        {
            Page = 1;
            PageSize = 10;
        }

        public string OrgId { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
    }


    public class UserEntity
    {

        public UserEntity()
        {
            Roles = new List<RolesEntity>();
        }
        public string Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public List<RolesEntity> Roles { get; set; }

        public string Scope { get; set; }

        public string OrganizationId { get; set; }

        public string OrganizationName { get; set; }

        public string OrganizationScope { get; set; }
    }
}
