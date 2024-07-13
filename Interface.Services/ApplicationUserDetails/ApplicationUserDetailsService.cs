using Student_Management_System.GenericRepo;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Student_Management_System.Interface.Services.ApplicationUserDetails
{
    public class ApplicationUserDetailsService : GenericRepository<ApplicationUsersDetail>, IApplicationUserDetailsService, IDisposable
    {
        private StudentManagementSystemEntities _context = null;
        private DbSet<ApplicationUsersDetail> table = null;

        public ApplicationUserDetailsService()
        {
            _context = new StudentManagementSystemEntities();
            table = _context.Set<ApplicationUsersDetail>();
        }

        public ApplicationUserDetailsService(StudentManagementSystemEntities context)
        {
            _context = context;
            table = _context.Set<ApplicationUsersDetail>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<ApplicationUsersDetail> GetAllApplicationUsers()
        {
            return _context.ApplicationUsersDetails.Where(m => !m.Deleted).ToList();
        }

        public ApplicationUsersDetail GetAppUserById(string AspNetUserId)
        {
            return _context.ApplicationUsersDetails.Where(m => !m.Deleted && m.AspNetUserId == AspNetUserId).FirstOrDefault();
        }
    }
}