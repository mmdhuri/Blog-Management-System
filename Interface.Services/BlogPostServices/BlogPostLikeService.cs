using Student_Management_System.GenericRepo;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Student_Management_System.Interface.Services.BlogPostServices
{
    public class BlogPostLikeService : GenericRepository<BlogPost_Likes>, IBlogPostLikeService, IDisposable
    {
        private StudentManagementSystemEntities _context = null;
        private DbSet<BlogPost_Likes> table = null;

        public BlogPostLikeService()
        {
            _context = new StudentManagementSystemEntities();
            table = _context.Set<BlogPost_Likes>();
        }

        public BlogPostLikeService(StudentManagementSystemEntities context)
        {
            _context = context;
            table = _context.Set<BlogPost_Likes>();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public List<BlogPost_Likes> GetAllBlogPostFavourites()
        {
            return _context.BlogPost_Likes.ToList();
        }
    }
}