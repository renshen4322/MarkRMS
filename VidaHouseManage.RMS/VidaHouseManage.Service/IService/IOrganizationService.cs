using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;

namespace VidaHouseManage.Service.IService
{
    public interface IOrganizationService
    {
        ServiceResult<List<TreeNode>> GetOrganizationList();

        ServiceResult AddOrganization(OrganizationEntity entity);

        ServiceResult EditOrganization(OrganizationEntity entity);

        ServiceResult DelOrganization(string id, string userName);

    }
}
