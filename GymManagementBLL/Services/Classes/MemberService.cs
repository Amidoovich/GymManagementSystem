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
    internal class MemberService : IMemberService
    {
        private readonly IUnitOfWork _unitOfWork;

        public MemberService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public IEnumerable<MemberViewModel> GetAllMembers()
        {
            var members = _unitOfWork.GetRepository<Member>().GetAll();
            if (members == null || !members.Any()) return [];
            // Member - MemberViewModel => Manual Mapping
            //var MemberviewModels = new List<MemberViewModel>();

            #region Way01
            //foreach(var member in members)
            //{
            //    var MemberViewModel = new MemberViewModel()
            //    {
            //        Id = member.Id,
            //        Name = member.Name,
            //        Phone = member.Phone,
            //        Email = member.Email,
            //        Photo = member.Photo,
            //        Gender = member.Gender.ToString()
            //    };
            //    MemberviewModels.Add(MemberViewModel);
            //} 
            #endregion

            #region Way02

            var MemberViewModels = members.Select(member => new MemberViewModel()
            {
                Id = member.Id,
                Name = member.Name,
                Phone = member.Phone,
                Email = member.Email,
                Photo = member.Photo,
                Gender = member.Gender.ToString()
            }); 
            #endregion

            return MemberViewModels;

        }
        public bool CreateMember(CreateMemberViewModel createMember)
        {
            try
            {
                if (createMember == null) return false;


                if (IsEmailExists(createMember.Email) || IsPhoneExists(createMember.Phone)) return false;

                var member = new Member()
                {
                    Name = createMember.Name,
                    Phone = createMember.Phone,
                    Email = createMember.Email,
                    DateOfBirth = createMember.DateOfBirth,
                    Gender = createMember.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createMember.BuildingNumber,
                        Street = createMember.Street,
                        City = createMember.City,
                    },
                    HealthRecord = new HealthRecord()
                    {
                        Height = createMember.HealthRecordViewModel.Height,
                        Weight = createMember.HealthRecordViewModel.weight,
                        BloodType = createMember.HealthRecordViewModel.BloodType,
                        Note = createMember.HealthRecordViewModel.Note,
                    }
                };


                _unitOfWork.GetRepository<Member>().Add(member);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }



        }

        public MemberViewModel? GetMemberDetails(int memberId)
        {
            var member =  _unitOfWork.GetRepository<Member>().GetById(memberId);

            if (member == null) return null;


            var viewModel = new MemberViewModel()
            {
                Name = member.Name,
                Phone = member.Phone,
                Email = member.Email,
                Photo = member.Photo,
                Gender = member.Gender.ToString(),
                DateOfBirth = member.DateOfBirth.ToShortDateString(),
                Address = $"{member.Address.BuildingNumber} - {member.Address.Street} - {member.Address.City}",
            };



            var ActivememberShip = _unitOfWork.GetRepository<MemberShip>().GetAll(MS => MS.MemberId == member.Id && MS.Status == "Active").FirstOrDefault();

            if(ActivememberShip is not null)
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

            if(memberHealthRecord == null) return null;

            return new HealthRecordViewModel()
            {
                Height = memberHealthRecord.Height,
                weight = memberHealthRecord.Weight,
                BloodType = memberHealthRecord.BloodType,
                Note = memberHealthRecord.Note,
            };

           
        }

        public MemberToUpdateViewModel? GetMemberToUpdate(int id)
        {
            var member = _unitOfWork.GetRepository<Member>().GetById(id);

            if(member == null) return null;

            return new MemberToUpdateViewModel()
            {
                Email = member.Email,
                Phone = member.Phone,
                BuildingNumber = member.Address.BuildingNumber,
                Street = member.Address.Street,
                City = member.Address.City,
                Photo = member.Photo
            };



        }

        public bool UpdateMember(int id, MemberToUpdateViewModel memberToUpdate)
        {

            try
            {

                if (IsEmailExists(memberToUpdate.Email) || IsPhoneExists(memberToUpdate.Phone)) return false;


                var _memberRepository = _unitOfWork.GetRepository<Member>();

                var member = _memberRepository.GetById(id);

                if (member == null) return false;

                member.Email = memberToUpdate.Email;
                member.Phone = memberToUpdate.Phone;
                member.Address.BuildingNumber = memberToUpdate.BuildingNumber;
                member.Address.Street = memberToUpdate.Street;
                member.Address.City = memberToUpdate.City;
                member.UpdatedAt = DateTime.Now;



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

                var HasActiveMemberSession = _unitOfWork.GetRepository<MemberSession>().GetAll(MS => MS.MemberId == member.Id && MS.Session.StartDate > DateTime.Now).Any();

                if(HasActiveMemberSession) return false;


                var _memberShipRepository = _unitOfWork.GetRepository<MemberShip>();
                var MemberShips = _memberShipRepository.GetAll(MS => MS.MemberId == member.Id);



                if (MemberShips.Any())
                    foreach(var memberShip in MemberShips)
                        _memberShipRepository.Delete(memberShip);



                _memberRepository.Delete(member);

                return _unitOfWork.SaveChanges() > 0;


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
