using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.AnalyticsViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public AnalyticsViewModel GetAnalyticsData()
        {
            return new AnalyticsViewModel()
            {
                TotalMembers = _unitOfWork.GetRepository<Member>().GetAll().Count(),
                ActiveMembers = _unitOfWork.GetRepository<MemberShip>().GetAll(X => X.Status == "Active").Count(),
                TotalTrainers = _unitOfWork.GetRepository<Trainer>().GetAll().Count(),
                UpcomingSessions = _unitOfWork.SessionRepository.GetAll(X=> X.StartDate > DateTime.Now).Count(),
                OngoingSessions = _unitOfWork.SessionRepository.GetAll(X => X.StartDate <= DateTime.Now && X.EndDate > DateTime.Now).Count(),
                CompletedSessions = _unitOfWork.SessionRepository.GetAll(X => X.EndDate < DateTime.Now).Count()
            };
        }
    }
}
