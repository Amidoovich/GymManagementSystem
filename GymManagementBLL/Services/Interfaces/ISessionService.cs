using GymManagementBLL.ViewModels.SessionViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface ISessionService
    {
        IEnumerable<SessionViewModel> GetAllSessions();

        SessionViewModel? GetSessionById(int sessionId);

        bool CreateSession(CreateSessionViewModel createSession);

        SessionToUpdateViewModel? GetSessionToUpdate(int sessionId);

        bool UpdateSession(int sessionId, SessionToUpdateViewModel updateSession);

        bool RemoveSession(int sessionId);

    }
}
