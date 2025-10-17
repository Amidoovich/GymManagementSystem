using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.SessionViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    internal class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork ,IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                if (createSession is null) return false;


                if (!IsTainerExists(createSession.TrainerId) || !IsCategoryExists(createSession.CategoryId) || !IsValidDateRange(createSession.StartDate, createSession.EndDate)) return false;


                var session = _mapper.Map<Session>(createSession);

                _unitOfWork.SessionRepository.Add(session);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessionRepo = _unitOfWork.SessionRepository;

            var sessions = sessionRepo.GetAllSessionsWithTrainersAndCategories();

            if (sessions is null || !sessions.Any()) return [];

            var MappedSessions = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in MappedSessions)
                session.AvailableSlots = session.Capacity - sessionRepo.GetCountOfBookedSlots(session.Id);
            
            return MappedSessions;

        }

        public SessionViewModel? GetSessionById(int sessionId)
        {
            
            var sessionRepo = _unitOfWork.SessionRepository;
            
            var session = sessionRepo.GetSessionWithTrainerAndCategory(sessionId);


            if(session is null) return null;

            var sessionMapped =  _mapper.Map<SessionViewModel>(session);

            sessionMapped.AvailableSlots = sessionRepo.GetCountOfBookedSlots(sessionId);

            return sessionMapped;

        }


        public SessionToUpdateViewModel? GetSessionToUpdate(int sessionId)
        {
            var session = _unitOfWork.SessionRepository.GetById(sessionId);
            if(session is null || !IsSessionAvailableForUpdating(session)) return null;

            return _mapper.Map<SessionToUpdateViewModel>(session);
        }

        public bool UpdateSession(int sessionId, SessionToUpdateViewModel updateSession)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(sessionId);

                if (session is null) return false;

                if (!IsTainerExists(updateSession.TrainerId) || !IsSessionAvailableForUpdating(session) || !IsValidDateRange(session.StartDate,session.EndDate)) return false;


                session = _mapper.Map<Session>(updateSession);
                session.UpdatedAt = DateTime.Now;

                _unitOfWork.SessionRepository.Update(session);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }
        }

        public bool RemoveSession(int sessionId)
        {
            try
            {
                var Repo = _unitOfWork.GetRepository<Session>();

                var session = Repo.GetById(sessionId);

                if (session is null || !IsSessionAvailableForRemoving(session)) return false;

                Repo.Delete(session);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch 
            { 
                return false; 
            }


        }
        #region Helper Methods


        private bool IsTainerExists(int trainerId)
        {
            return _unitOfWork.GetRepository<Trainer>().GetAll(T => T.Id == trainerId).Any();
        }
        private bool IsCategoryExists(int CategoryId)
        {
            return _unitOfWork.GetRepository<Category>().GetAll(C => C.Id == CategoryId).Any();
        }

        private bool IsValidDateRange(DateTime strartDate , DateTime endDate)
        {
            return strartDate < endDate && strartDate > DateTime.Now;
        }

        private bool IsSessionAvailableForUpdating(Session session)
        {
            if(session is null) return false;

            if(session.EndDate < DateTime.Now)
                return false;

            if(session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return false;

            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBooking) return false;


            return true;
        }
        private bool IsSessionAvailableForRemoving(Session session)
        {
            if(session is null) return false;

            if(session.StartDate > DateTime.Now)
                return false;

            if(session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return false;

            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;

            if (HasActiveBooking) return false;


            return true;
        }

       

        #endregion


    }
}
