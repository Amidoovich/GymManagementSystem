using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class SessionRepository : GenericRepostitory<Session>, ISessionRepository
    {
        private readonly GymDbContext _dbContext;

        public SessionRepository(GymDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }



        public IEnumerable<Session> GetAllSessionsWithTrainersAndCategories() => _dbContext.Sessions.Include(S => S.SessionTrainer)
                                                                                                    .Include(S => S.SessionCategory)
                                                                                                    .AsNoTracking().ToList();

        public int GetCountOfBookedSlots(int sessionId) => _dbContext.MemberSessions.Count(S => S.SessionId == sessionId);

        public Session? GetSessionWithTrainerAndCategory(int sessionId)
        {
            return _dbContext.Sessions.Include(S => S.SessionTrainer).Include(S => S.SessionCategory).FirstOrDefault(S => S.Id == sessionId);
        }
    }
}
