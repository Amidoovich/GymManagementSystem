using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using GymManagementDAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Classes
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly GymDbContext _dbContext;

        public UnitOfWork(GymDbContext dbContext,ISessionRepository sessionRepository) 
        {
            _dbContext = dbContext;
            SessionRepository = sessionRepository;
        }



        private readonly Dictionary<Type,object> _repositories = new();

        public ISessionRepository SessionRepository { get; }

        // Key => Member , Trainer
        // Value => GenericRepostitory<Member>() , GenericRepostitory<Trainer>()

        public IGenericRepostitory<TEntity> GetRepository<TEntity>() where TEntity : BaseEntity, new()
        {
            
            
            
            var EntityType = typeof(TEntity);
            if (_repositories.ContainsKey(EntityType))
                return (IGenericRepostitory<TEntity>)_repositories[EntityType];

            var newRepo = new GenericRepostitory<TEntity>(_dbContext);

            _repositories.Add(EntityType, newRepo);

            return newRepo;
            
            
            //return new GenericRepostitory<TEntity>(_dbContext);
        }
        // Create many numbers of objects
        //GenericRepostitory<Member>
        //GenericRepostitory<Member>
        //GenericRepostitory<Member>
        //GenericRepostitory<Member>
        //GenericRepostitory<Member>
        //GenericRepostitory<Member>

        public int SaveChanges()
        {
            return _dbContext.SaveChanges();
        }


    }
}
