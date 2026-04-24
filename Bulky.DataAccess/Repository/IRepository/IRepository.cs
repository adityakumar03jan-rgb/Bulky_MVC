using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Bulky.DataAccess.Repository.IRepository
{
    //Generic repository interface
    //T is a placeholder for the type of entity that the repository will manage
    //The where T : class constraint ensures that T must be a reference type (class)
    //This allows the repository to work with any class type, such as Category, Product, etc.
    //This interface will define the common operations that can be performed on any entity type, such as Add, Update, Remove, Get, etc.        
    public interface IRepository<T> where T : class
    {
        //Method signatures for common CRUD operations
        //T - Category
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T, bool>>filter);
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entity);

    }
}
