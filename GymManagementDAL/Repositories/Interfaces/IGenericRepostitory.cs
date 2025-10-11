﻿using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Repositories.Interfaces
{
    public interface IGenericRepostitory<TEntity> where TEntity : BaseEntity , new()  
    {

        IEnumerable<TEntity> GetAll(Func<TEntity,bool>? Conditon = null);
        TEntity? GetById(int id);
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);


    }
}
