using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    public interface IMemberService
    {
        IEnumerable<MemberViewModel> GetAllMembers();

        bool CreateMember(CreateMemberViewModel createMember);

        MemberDetailsViewModel? GetMemberDetails(int memberId);

        HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId);

        MemberToUpdateViewModel? GetMemberToUpdate(int id);

        bool UpdateMember(int id, MemberToUpdateViewModel memberToUpdate);

        bool RemoveMember(int memberId);

    }
}
