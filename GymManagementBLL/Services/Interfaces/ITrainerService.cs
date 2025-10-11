using GymManagementBLL.ViewModels.TrainerViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Interfaces
{
    internal interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();

        bool CreateTrainer(CreateTrainerViewModel createTrainer);

        TrainerViewModel? GetTrainerDetails(int id);

        TrianerToUpdateViewModel? GetTrainerToUpdate(int id);

        bool UpdateTrainer(int id , TrianerToUpdateViewModel updateTrianer);

        bool RemoveTrainer(int id);

    }
}
