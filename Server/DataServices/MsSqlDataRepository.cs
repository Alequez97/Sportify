﻿using DomainEntities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataServices
{
    public class MsSqlDataRepository<T> : IDataRepository<T> where T : EntityBase, new()
    {
        private readonly SportifyDbContext _dbContext;
        private readonly DbSet<T> _entities;

        public MsSqlDataRepository(SportifyDbContext dbContext)
        {
            _dbContext = dbContext;
            _entities = _dbContext.Set<T>();
        }


        public T Read(int id)
        {
            return _entities.SingleOrDefault(s => s.Id == id);
        }

        public List<T> ReadAll()
        {
            return _entities.ToList();
        }
        
        public bool Create(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _entities.Add(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                // TODO Add exception logger
                return false;
            }
        }

        public bool Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                // TODO Add exception logger
                return false;
            }
        }

        public bool Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            try
            {
                _entities.Remove(entity);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                // TODO Add exception logger
                return false;
            }
        }

        public List<T> FindByExpression(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] properties)
        {
            var query = _entities as IQueryable<T>;
            query = properties?.Aggregate(query, (current, property) => current.Include(property));
            var result = query.Where(expression).ToList();
            return result;
        }
    }
}