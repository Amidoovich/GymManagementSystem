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
    public class GenericRepostitory<TEntity> : IGenericRepostitory<TEntity> where TEntity : BaseEntity, new()
    {
        private readonly GymDbContext _dbContext;

        public GenericRepostitory(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }



        public void Add(TEntity entity) => _dbContext.Set<TEntity>().Add(entity);
       
            
        

        public void Delete(TEntity entity) => _dbContext.Set<TEntity>().Remove(entity);
           
        

        public IEnumerable<TEntity> GetAll(Func<TEntity, bool>? Conditon = null)
        {
            if (Conditon is null)
                return _dbContext.Set<TEntity>().AsNoTracking().ToList();

            return _dbContext.Set<TEntity>().AsNoTracking().Where(Conditon).ToList();
        }



        public TEntity? GetById(int id) => _dbContext.Set<TEntity>().Find(id);



        public void Update(TEntity entity) => _dbContext.Set<TEntity>().Update(entity);
          
        
    }
}
