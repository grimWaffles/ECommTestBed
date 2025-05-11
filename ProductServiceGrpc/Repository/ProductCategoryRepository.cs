using EfCoreTutorial.Entity.ECommerceModels;
using Microsoft.EntityFrameworkCore;
using ProductServiceGrpc.Database;

namespace ProductServiceGrpc.Repository
{
    public interface IProductCategoryRepository
    {
        Task<ProductCategoryModel> CreateCategoryAsync(ProductCategoryModel category);
        Task<ProductCategoryModel?> GetCategoryByIdAsync(int id);
        Task<List<ProductCategoryModel>> GetAllCategoriesAsync();
        Task<bool> UpdateCategoryAsync(ProductCategoryModel updatedCategory);
        Task<bool> DeleteCategoryAsync(int id, int modifiedBy);
    }

    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly AppDbContext _db;

        public ProductCategoryRepository(AppDbContext db)
        {
            _db = db;
        }

        // CREATE
        public async Task<ProductCategoryModel> CreateCategoryAsync(ProductCategoryModel category)
        {
            category.CreatedDate = DateTime.UtcNow;
            _db.ProductCategories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        // READ (Get by Id)
        public async Task<ProductCategoryModel?> GetCategoryByIdAsync(int id)
        {
            return await _db.ProductCategories
                            .Include(c => c.Products)
                            .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);
        }

        // READ (Get all)
        public async Task<List<ProductCategoryModel>> GetAllCategoriesAsync()
        {
            return await _db.ProductCategories
                            .Where(c => !c.IsDeleted)
                            .Include(c => c.Products)
                            .ToListAsync();
        }

        // UPDATE
        public async Task<bool> UpdateCategoryAsync(ProductCategoryModel updatedCategory)
        {
            var existingCategory = await _db.ProductCategories.FindAsync(updatedCategory.Id);
            if (existingCategory == null || existingCategory.IsDeleted)
                return false;

            existingCategory.CategoryName = updatedCategory.CategoryName;
            existingCategory.ModifiedBy = updatedCategory.ModifiedBy;
            existingCategory.ModifiedDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        // DELETE (Soft Delete)
        public async Task<bool> DeleteCategoryAsync(int id, int modifiedBy)
        {
            var category = await _db.ProductCategories.FindAsync(id);
            if (category == null || category.IsDeleted)
                return false;

            category.IsDeleted = true;
            category.ModifiedDate = DateTime.UtcNow;
            category.ModifiedBy = modifiedBy;

            await _db.SaveChangesAsync();
            return true;
        }
    }

}
