using Student_Management_System.GenericRepo;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Student_Management_System.Interface.Services.Contacts
{
    public class ContactService : GenericRepository<Contact>, IContactService, IDisposable
    {
        private StudentManagementSystemEntities _context = null;
        private DbSet<Contact> table = null;

        public ContactService()
        {
            _context = new StudentManagementSystemEntities();
            table = _context.Set<Contact>();
        }

        public ContactService(StudentManagementSystemEntities context)
        {
            _context = context;
            table = _context.Set<Contact>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<Contact> GetAllContacts()
        {
            return _context.Contacts.ToList();
        }
    }
}