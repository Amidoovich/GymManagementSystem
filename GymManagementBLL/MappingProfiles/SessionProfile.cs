using AutoMapper;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.MappingProfiles
{
    public class SessionProfile : Profile
    {
        public SessionProfile()
        {
            CreateMap<Session, SessionViewModel>()
            .ForMember(dest => dest.TrainerName, options => options.MapFrom(src => src.SessionTrainer.Name))
            .ForMember(dest => dest.CategoryName, options => options.MapFrom(src => src.SessionCategory.CategoryName))
            .ForMember(dest => dest.AvailableSlots, options => options.Ignore());

            CreateMap<CreateSessionViewModel, Session>();

            CreateMap<Session, SessionToUpdateViewModel>().ReverseMap();

            CreateMap<Trainer, TrainerSelectViewModel>();

            CreateMap<Category, CategorySelectViewModel>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));
        }
    }
}
