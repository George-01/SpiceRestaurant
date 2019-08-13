using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpiceRestaurant.Data;

namespace SpiceRestaurant.Models
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public CategoryRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public IEnumerable<Category> GetAllCategories()
        {
            return _db.Category;
        }

        public Category GetCategoryById(int Id)
        {
            return _db.Category.FirstOrDefault(c => c.Id == Id);
        }
    }
}
