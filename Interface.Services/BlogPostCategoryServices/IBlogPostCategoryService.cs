using Student_Management_System.GenericRepo;
using Student_Management_System.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Student_Management_System.Interface.Services.BlogPostCategoryServices
{
    public interface IBlogPostCategoryService : IGenericRepository<BlogCategory>
    {
        List<BlogCategory> GetAllBlogCategories();
    }
}
