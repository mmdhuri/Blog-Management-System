using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Student_Management_System.GenericRepo
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private StudentManagementSystemEntities _context = null;
        private DbSet<T> table = null;

        public GenericRepository()
        {
            this._context = new StudentManagementSystemEntities();
            table = _context.Set<T>();
        }

        public GenericRepository(StudentManagementSystemEntities _context)
        {
            this._context = _context;
            table = _context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return table.ToList();
        }

        public T GetById(object obj)
        {
            return table.Find(obj);
        }

        public void Insert(T obj)
        {
            table.Add(obj);
            _context.SaveChanges();
        }

        public void Update(T obj)
        {
            table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            T existing = table.Find(id);
            table.Remove(existing);
            _context.SaveChanges();
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}