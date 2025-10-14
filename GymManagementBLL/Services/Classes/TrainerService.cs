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
    internal class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                if (createTrainer is null) return false;


                if (IsEmailExists(createTrainer.Email) || IsPhoneExists(createTrainer.Phone)) return false;

                var trainer = new Trainer()
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    Gender = createTrainer.Gender,
                    Address = new Address()
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        Street = createTrainer.Street,
                        City = createTrainer.City,
                    },
                    Specialties = createTrainer.Specialties,
                };

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

            return Trainers.Select(T => new TrainerViewModel()
            {
                Id = T.Id,
                Name = T.Name,
                Email = T.Email,
                Phone = T.Phone,
                Specialties = T.Specialties.ToString(),

            });

        }

        public TrainerViewModel? GetTrainerDetails(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);


            if(trainer is null) return null;


            return new TrainerViewModel()
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
            };

        }

        public TrianerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);

            if(trainer is null) return null;

            return new TrianerToUpdateViewModel()
            {
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specialties,
            };
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

                if(IsEmailExists(updateTrianer.Email) || IsPhoneExists(updateTrianer.Phone)) return false;


                var TrainerRepo = _unitOfWork.GetRepository<Trainer>();

                var trainer = TrainerRepo.GetById(id);

                if(trainer is null) return false;

                trainer.Email = updateTrianer.Email;
                trainer.Phone = updateTrianer.Phone;
                trainer.Address.BuildingNumber = updateTrianer.BuildingNumber;
                trainer.Address.Street = updateTrianer.Street;
                trainer.Address.City = updateTrianer.City;
                trainer.Specialties = updateTrianer.Specialties;
                trainer.UpdatedAt = DateTime.Now;

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
