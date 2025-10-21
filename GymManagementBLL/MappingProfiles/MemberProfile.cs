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

            CreateMap<Member, MemberViewModel>().ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender.ToString()));


            CreateMap<Member, MemberDetailsViewModel>()
            .ForMember(dest => dest.DateOfBirth, options => options.MapFrom(src => src.DateOfBirth.ToShortDateString()))
            .ForMember(dest => dest.Gender, options => options.MapFrom(src => src.Gender.ToString()))
            .ForMember(dest => dest.Address, options => options.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"))
            .ForMember(dest => dest.MemberShipStartDate, options => options.Ignore())
            .ForMember(dest => dest.MemberShipEndDate, options => options.Ignore())
            .ForMember(dest => dest.PlanName, options => options.Ignore());


            //CreateMap<CreateMemberViewModel, Member>()
            //    .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
            //    .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
            //    .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
            //    .ForPath(dest => dest.HealthRecord.BloodType, opt => opt.MapFrom(src => src.HealthRecordViewModel.BloodType))
            //    .ForPath(dest => dest.HealthRecord.Height, opt => opt.MapFrom(src => src.HealthRecordViewModel.Height))
            //    .ForPath(dest => dest.HealthRecord.Weight, opt => opt.MapFrom(src => src.HealthRecordViewModel.weight))
            //    .ForPath(dest => dest.HealthRecord.Note, opt => opt.MapFrom(src => src.HealthRecordViewModel.Note ?? ""));


            CreateMap<CreateMemberViewModel, Member>()
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src))
                .ForMember(dest => dest.HealthRecord, opt => opt.MapFrom(src => src.HealthRecordViewModel));
                

            CreateMap<CreateMemberViewModel, Address>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.City));


            CreateMap<HealthRecord, HealthRecordViewModel>().ReverseMap();



            CreateMap<Member, MemberToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City))
                .ForMember(dest => dest.Photo, opt => opt.Ignore());
         




            CreateMap<MemberToUpdateViewModel, Member>()
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForMember(dest => dest.Name, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Photo, opt => opt.Ignore());



        }
    }
}
