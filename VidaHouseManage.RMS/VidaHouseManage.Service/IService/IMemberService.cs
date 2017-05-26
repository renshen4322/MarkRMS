using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VidaHouseManage.Common.Common;
using VidaHouseManage.Entity;

namespace VidaHouseManage.Service.IService
{
    public interface IMemberService
    {
        PageListResult<List<UserEntity>> GetMemberList(int pageIndex, int pageSize, string organizationId);

        ServiceResult<UserEntity> GetMemberById(string Id);

        ServiceResult AddMember(string[] Ids, UserEntity entity);

        ServiceResult EditMember(string[] Ids, UserEntity entity);

        ServiceResult DelMember(string id, string userName);

    }
}
