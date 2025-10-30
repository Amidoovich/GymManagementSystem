using AutoMapper;
using GymManagementBLL.AttachmentService;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Classes;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAttachmentService _attachmentService;

        public MemberService(IUnitOfWork unitOfWork, IMapper mapper ,IAttachmentService attachmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _attachmentService = attachmentService;
        }


        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members == null || !members.Any()) return [];

            var MemberViewModels = _mapper.Map<IEnumerable<MemberViewModel>>(members);

            return MemberViewModels;

        }
        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (createMember == null) return false;


                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;

                var PhotoName = _attachmentService.Upload("Members", createMember.PhotoFile);


                if(string.IsNullOrEmpty(PhotoName)) return false;
                    

                var member = _mapper.Map<Member>(createMember);
                member.Photo = PhotoName;


                _unitOfWork.GetRepository<Member>().Add(member);
                var IsCreated = _unitOfWork.SaveChanges() > 0;

                if(!IsCreated)
                    _attachmentService.Delete(PhotoName, "Members");
                   

                return IsCreated;
            }
            catch
            {
                return false;
            }



        }

        public MemberDetailsViewModel? GetMemberDetails(int memberId)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member == null) return null;



            var viewModel = _mapper.Map<MemberDetailsViewModel>(member);


            var ActivememberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(MS => MS.MemberId == member.Id && MS.Status == "Active").FirstOrDefault();

            if (ActivememberShip is not null)
            {
                viewModel.MemberShipStartDate = ActivememberShip.CreatedAt.ToShortDateString();
                viewModel.MemberShipEndDate = ActivememberShip.EndDate.ToShortDateString();
                var plan = _unitOfWork.GetRepository<Plan>().GetById(ActivememberShip.PlanId);
                viewModel.PlanName = plan?.Name;
            }


            return viewModel;




        }

        public HealthRecordViewModel? GetMemberHealthRecordDetails(int memberId)
        {
            var memberHealthRecord = _unitOfWork.GetRepository<HealthRecord>().GetById(memberId);

            if (memberHealthRecord == null) return null;

            var HealthRecordMapped = _mapper.Map<HealthRecordViewModel>(memberHealthRecord);

            return HealthRecordMapped;


        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);

            if (member == null) return null;

            var memberMappedViewModel = _mapper.Map<MemberToUpdateViewModel>(member);

            return memberMappedViewModel;


        }

        public bool UpdateMember(int id, MemberToUpdateViewModel memberToUpdate)
        {

            try
            {

                var EmailExists = _unitOfWork.GetRepository<Member>()
                    .GetAll(member => member.Email == memberToUpdate.Email && member.Id != id).Any();

                var PhoneExists = _unitOfWork.GetRepository<Member>()
                    .GetAll(member => member.Phone == memberToUpdate.Phone && member.Id != id).Any();

                if (EmailExists || PhoneExists) return false;


                var _memberRepository = _unitOfWork.GetRepository<Member>();

                var member = _memberRepository.GetById(id);

                if (member == null) return false;



                _mapper.Map(memberToUpdate, member);





                _memberRepository.Update(member);


                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }

        }

        public bool RemoveMember(int memberId)
        {
            try
            {
                var _memberRepository = _unitOfWork.GetRepository<Member>();
                var member = _memberRepository.GetById(memberId);

                if (member == null) return false;

                var SessionIds = _unitOfWork.GetRepository<MemberSession>().GetAll(X => X.MemberId == memberId).Select(X => X.SessionId);

                var HasActiveMemberSession = _unitOfWork.GetRepository<Session>().GetAll(X => SessionIds.Contains(X.Id) && X.StartDate > DateTime.Now).Any();



                if (HasActiveMemberSession) return false;


                var _memberShipRepository = _unitOfWork.GetRepository<MemberShip>();
                var MemberShips = _memberShipRepository.GetAll(MS => MS.MemberId == member.Id);



                if (MemberShips.Any())
                    foreach (var memberShip in MemberShips)
                        _memberShipRepository.Delete(memberShip);


                _memberRepository.Delete(member);

                var IsRemoved = _unitOfWork.SaveChanges() > 0;

                if (IsRemoved)
                    _attachmentService.Delete(member.Photo, "Members");

                return IsRemoved;

            }
            catch
            {
                return false;
            }
        }


        #region Helper

        private bool IsEmailExists(string email)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(member => member.Email == email).Any();
        }
        private bool IsPhoneExists(string phone)
        {
            return _unitOfWork.GetRepository<Member>().GetAll(member => member.Phone == phone).Any();
        }


        #endregion
    }
}
