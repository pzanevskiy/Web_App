using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Task5.DAL.Context;
using Task5.DAL.Repositories.Interfaces;

namespace Task5.DAL.Repositories
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        private SaleContext _context;
        private DbSet<TEntity> _entitySet;

        public GenericRepository(SaleContext context)
        {
            this._context = context;
            _entitySet = this._context.Set<TEntity>();
        }

        public IEnumerable<TEntity> Get()
        {
            return _entitySet.ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return _entitySet.FirstOrDefault(predicate);
        }

        public void Add(TEntity entity)
        {
            if (entity != null)
            {
                _entitySet.Add(entity);
            }
        }

        public void Delete(TEntity entity)
        {
            if (entity != null)
            {
                _entitySet.Remove(entity);
            }
        }

        public void Update(TEntity entity)
        {
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Modified;
            }
        }        
    }
}
