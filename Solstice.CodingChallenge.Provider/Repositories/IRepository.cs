using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Solstice.CodingChallenge.Provider.Repositories
{
    internal interface IRepository<T> where T : class
    {
        T Find(int id);

        IEnumerable<T> All();
        IQueryable<T> Get(Expression<Func<T, bool>> predicate);
        IEnumerable<T> GetList(Expression<Func<T, bool>> predicate);

        void Insert(T entity);
        void InsertBulk(IEnumerable<T> entities);

        void Delete(T entity);
        void DeleteBulk(IEnumerable<T> entities);
    }
}
