using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;

namespace VidaHouseManage.Service.IService
{
    public interface IRoleService
    {

        ServiceResult<List<RolesEntity>> GetRoelsByOrganizationId(string organizationId);

        ServiceResult<RolesEntity> GetRolesById(long Id);

        ServiceResult CreateRoles(string[] names, RolesEntity entity);

        ServiceResult EditRoles(string[] names, RolesEntity entity);

        ServiceResult DelRoles(string Id, string userName);

    }
}
