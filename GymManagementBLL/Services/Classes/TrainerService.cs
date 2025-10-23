using AutoMapper;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels.TrainerViewModels;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementBLL.Services.Classes
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                if (createTrainer is null) return false;


                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone)) return false;

                var trainer = _mapper.Map<Trainer>(createTrainer);

                _unitOfWork.GetRepository<Trainer>().Add(trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch
            {
                return false;
            }

        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var Trainers = _unitOfWork.GetRepository<Trainer>().GetAll();

            if (Trainers is null || !Trainers.Any()) return [];

            var trainersMappedViewModel = _mapper.Map<IEnumerable<TrainerViewModel>>(Trainers);

            return trainersMappedViewModel;

        }

        public TrainerDetailsViewModel? GetTrainerDetails(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);


            if(trainer is null) return null;
            var TrainerMapped = _mapper.Map<TrainerDetailsViewModel>(trainer);

            return TrainerMapped;

        }

        public TrianerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);

            if(trainer is null) return null;

            var TrainerToUpdate = _mapper.Map<TrianerToUpdateViewModel>(trainer);


            return TrainerToUpdate;
        }

        public bool RemoveTrainer(int id)
        {
            try
            {
                
                var trainerRepo = _unitOfWork.GetRepository<Trainer>();
                var trainer = trainerRepo.GetById(id);

                if(trainer == null) return false;

                var hasFutureSession = _unitOfWork.GetRepository<Session>().GetAll(S => S.TrainerId == trainer.Id && S.StartDate > DateTime.Now).Any();

                if(hasFutureSession) return false;


                trainerRepo.Delete(trainer);
                return _unitOfWork.SaveChanges() > 0;

            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTrainer(int id, TrianerToUpdateViewModel updateTrianer)
        {
            try
            {
                if (updateTrianer == null) return false;
                var TrainerRepo = _unitOfWork.GetRepository<Trainer>();

                var trainer = TrainerRepo.GetById(id);

                var IsEmailExists = TrainerRepo.GetAll(T => T.Email == updateTrianer.Email && T.Id != id ).Any();
                var IsPhoneExists = TrainerRepo.GetAll(T => T.Phone == updateTrianer.Phone && T.Id != id ).Any();

                if (trainer is null || IsEmailExists || IsPhoneExists) return false;

                _mapper.Map(updateTrianer, trainer);



                TrainerRepo.Update(trainer);

                return _unitOfWork.SaveChanges() > 0;
                
            }
            catch
            {
                return false;
            }
        }


        #region Helper Methods

        bool IsEmailExists(string email)  => _unitOfWork.GetRepository<Trainer>().GetAll(T => T.Email == email).Any();
        bool IsPhoneExists(string phone)  => _unitOfWork.GetRepository<Trainer>().GetAll(T => T.Phone == phone).Any();





        #endregion
    }
}
