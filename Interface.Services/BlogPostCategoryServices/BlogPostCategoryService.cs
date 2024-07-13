using Student_Management_System.GenericRepo;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Student_Management_System.Interface.Services.BlogPostCategoryServices
{
    public class BlogPostCategoryService : GenericRepository<BlogCategory>, IBlogPostCategoryService, IDisposable
    {
        private StudentManagementSystemEntities _context = null;
        private DbSet<BlogCategory> table = null;

        public BlogPostCategoryService()
        {
            _context = new StudentManagementSystemEntities();
            table = _context.Set<BlogCategory>();
        }

        public BlogPostCategoryService(StudentManagementSystemEntities context)
        {
            _context = context;
            table = _context.Set<BlogCategory>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<BlogCategory> GetAllBlogCategories()
        {
            return _context.BlogCategories.Where(m => !m.Deleted).ToList();
        }
    }
}