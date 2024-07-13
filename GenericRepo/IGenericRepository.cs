using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Management_System.GenericRepo
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll();
        T GetById(object obj);
        void Insert(T obj);
        void Update(T obj);
        void Delete(int id);
        void Save();
    }

}
