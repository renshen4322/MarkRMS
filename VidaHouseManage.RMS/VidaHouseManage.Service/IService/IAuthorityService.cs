using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;

namespace VidaHouseManage.Service.IService
{
    public interface IAuthorityService
    {

        ServiceResult<List<AuthorityEntity>> GetAuthorityList();

        ServiceResult<List<AuthorityEntity>> GetAuthorityListByRoleId(long roleId);

    }
}
