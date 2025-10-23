using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.MappingProfiles
{
    internal class MemberProfile : Profile
    {
        public MemberProfile()
        {
           
            
            
            #region Get All Members
           
            CreateMap<Member, MemberViewModel>().ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender.ToString())); 

            #endregion

            #region Get Member Details

            CreateMap<Member, MemberDetailsViewModel>()
            .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToShortDateString()))
            .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.Address, options => options.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
            .ForMember(dest => dest.MemberShipStartDate, options => options.Ignore())
            .ForMember(dest => dest.MemberShipEndDate, options => options.Ignore())
            .ForMember(dest => dest.PlanName, options => options.Ignore()); 


            #endregion

            #region Create Member
            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));


            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));


            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();
            #endregion

            #region Update Member
            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));



            CreateMap<MemberToUpdateViewModel, Member>()
                            .ForMember(dest => dest.Name, opt => opt.Ignore())
                            .ForMember(dest => dest.Photo, opt => opt.Ignore())
                            .AfterMap((src, dest) =>
                            {
                                dest.Address.BuildingNumber = src.BuildingNumber;
                                dest.Address.City = src.City;
                                dest.Address.Street = src.Street;
                                dest.UpdatedAt = DateTime.Now;
                            });


            #endregion




        }
    }
}
