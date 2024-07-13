using Student_Management_System.Models;
using Student_Management_System.GenericRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace Student_Management_System.Interface.Services.BlogPostServices
{
    public class BlogPostService : GenericRepository<BlogPost> , IBlogPostService,IDisposable
    {
        private StudentManagementSystemEntities _context = null;
        private DbSet<BlogPost> table = null;

        public BlogPostService()
        {
            _context = new StudentManagementSystemEntities();
            table = _context.Set<BlogPost>();
        }

        public BlogPostService(StudentManagementSystemEntities context)
        {
            _context = context;
            table = _context.Set<BlogPost>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<BlogPost> GetAllBlogPosts()
        {
            return _context.BlogPosts.Where(m => !m.Deleted).ToList();
        }
    }
}