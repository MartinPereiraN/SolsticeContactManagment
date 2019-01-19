using Solstice.CodingChallenge.Domain.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Solstice.CodingChallenge.Provider.Repositories
{
    internal class Repository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void DeleteBulk(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
        }

        public IQueryable<T> Get(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? _context.Set<T>().Where(predicate) : _context.Set<T>();
        }

        public IEnumerable<T> GetList(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null ? _context.Set<T>().Where(predicate).AsEnumerable() : _context.Set<T>().AsEnumerable();
        }

        public void Insert(T entity)
        {
            _context.Set<T>().Add(entity);
        }

        public void InsertBulk(IEnumerable<T> entities)
        {
            _context.Set<T>().AddRange(entities);
        }

        public IEnumerable<T> All()
        {
            return _context.Set<T>().AsEnumerable();
        }

        public T Find(int id)
        {
            return _context.Set<T>().Find(id);
        }
    }
}
