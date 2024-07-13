using Student_Management_System.GenericRepo;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Management_System.Interface.Services.BlogPostServices
{
    public interface IBlogPostService : IGenericRepository<BlogPost>
    {
        List<BlogPost> GetAllBlogPosts();
    }
}
