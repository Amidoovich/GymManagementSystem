using AutoMapper;
using GymManagementBLL.ViewModels.PlanViewModels;
using GymManagementDAL.Entities;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.MappingProfiles
{
    internal class PlanProfile : Profile
    {
        public PlanProfile()
        {
            CreateMap<Plan, PlanViewModel>();

            CreateMap<Plan, UpdatePlanViewModel>()
                .ForMember(dest => dest.PlanName, opt => opt.MapFrom(src => src.Name));


            CreateMap<UpdatePlanViewModel, Plan>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PlanName))
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());

        }
    }
}
