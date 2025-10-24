using AutoMapper;
using GymManagementBLL.ViewModels.MemberViewModel;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.MappingProfiles
{
    internal class TrainerProfile : Profile
    {
        public TrainerProfile()
        {
            CreateMap<Trainer, TrainerViewModel>()
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialties.ToString()));

            CreateMap<Trainer, TrainerDetailsViewModel>()
                .ForMember(dest => dest.Specialties, opt => opt.MapFrom(src => src.Specialties.ToString()))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth.ToShortDateString()))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => $"{src.Address.BuildingNumber} - {src.Address.Street} - {src.Address.City}"));


            CreateMap<CreateTrainerViewModel, Trainer>()
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City));

            CreateMap<Trainer, TrianerToUpdateViewModel>()
                .ForMember(dest => dest.BuildingNumber, opt => opt.MapFrom(src => src.Address.BuildingNumber))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.Address.City));


            CreateMap<TrianerToUpdateViewModel, Trainer>()
                .ForPath(dest => dest.Address.BuildingNumber, opt => opt.MapFrom(src => src.BuildingNumber))
                .ForPath(dest => dest.Address.Street, opt => opt.MapFrom(src => src.Street))
                .ForPath(dest => dest.Address.City, opt => opt.MapFrom(src => src.City))
                .ForPath(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Name, opt => opt.Ignore());
                
        }
    }
}
