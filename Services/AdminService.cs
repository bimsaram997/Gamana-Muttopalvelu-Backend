using Gamana_Muttopalvelu_Backend.Data;
using Gamana_Muttopalvelu_Backend.DTO.Admin;
using Microsoft.EntityFrameworkCore;

namespace Gamana_Muttopalvelu_Backend.Services
{
    public interface IAdminService
    {
        // Key Services
        Task<List<AdminKeyServiceResponseDto>> GetAllKeyServicesAsync();
        Task<AdminKeyServiceResponseDto?> GetKeyServiceByIdAsync(int id);
        Task<int> SaveKeyServiceAsync(AdminKeyServiceUpsertDto dto, int? id = null);
        Task<bool> DeleteKeyServiceAsync(int id);

        // Process Steps
        Task<List<AdminProcessStepResponseDto>> GetAllProcessStepsAsync();
        Task<AdminProcessStepResponseDto?> GetProcessStepByIdAsync(int id);
        Task<int> SaveProcessStepAsync(AdminProcessStepUpsertDto dto, int? id = null);
        Task<bool> DeleteProcessStepAsync(int id);

        // Detailed Services
        Task<List<AdminDetailedServiceResponseDto>> GetAllDetailedServicesAsync();
        Task<AdminDetailedServiceResponseDto?> GetDetailedServiceByIdAsync(int id);
        Task<int> SaveDetailedServiceAsync(AdminDetailedServiceUpsertDto dto, int? id = null);
        Task<bool> DeleteDetailedServiceAsync(int id);

        // Packages
        Task<List<AdminPackageResponseDto>> GetAllPackagesAsync();
        Task<AdminPackageResponseDto?> GetPackageByIdAsync(int id);
        Task<int> SavePackageAsync(AdminPackageUpsertDto dto, int? id = null);
        Task<bool> DeletePackageAsync(int id);

        // Reviews
        Task<List<AdminReviewResponseDto>> GetAllReviewsAsync();
        Task<AdminReviewResponseDto?> GetReviewByIdAsync(int id);
        Task<int> SaveReviewAsync(AdminReviewUpsertDto dto, int? id = null);
        Task<bool> DeleteReviewAsync(int id);

        // Form Options
        Task<List<AdminFormOptionResponseDto>> GetAllFormOptionsAsync();
        Task<AdminFormOptionResponseDto?> GetFormOptionByIdAsync(int id);
        Task<int> SaveFormOptionAsync(AdminFormOptionUpsertDto dto, int? id = null);
        Task<bool> DeleteFormOptionAsync(int id);
    }

    public class AdminService : IAdminService
    {
        private readonly AppDbContext _context;

        public AdminService(AppDbContext context)
        {
            _context = context;
        }

        // --- 1. Key Services ---
        public async Task<List<AdminKeyServiceResponseDto>> GetAllKeyServicesAsync()
        {
            return await _context.KeyServices.Include(x => x.Translations)
                .Select(x => new AdminKeyServiceResponseDto
                {
                    Id = x.Id,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new KeyServiceTranslationDto { LanguageCode = t.LanguageCode, Name = t.Name }).ToList()
                }).ToListAsync();
        }

        public async Task<AdminKeyServiceResponseDto?> GetKeyServiceByIdAsync(int id)
        {
            return await _context.KeyServices.Include(x => x.Translations)
                .Where(x => x.Id == id)
                .Select(x => new AdminKeyServiceResponseDto
                {
                    Id = x.Id,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new KeyServiceTranslationDto { LanguageCode = t.LanguageCode, Name = t.Name }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<int> SaveKeyServiceAsync(AdminKeyServiceUpsertDto dto, int? id = null)
        {
            KeyService entity = id.HasValue
                ? await _context.KeyServices.Include(x => x.Translations).FirstAsync(x => x.Id == id)
                : new KeyService();

            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = dto.IsActive;
            entity.Translations = dto.Translations.Select(t => new KeyServiceTranslation { LanguageCode = t.LanguageCode, Name = t.Name }).ToList();

            if (!id.HasValue) _context.KeyServices.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteKeyServiceAsync(int id)
        {
            var item = await _context.KeyServices.FindAsync(id);
            if (item == null) return false;
            _context.KeyServices.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- 2. Process Steps ---
        public async Task<List<AdminProcessStepResponseDto>> GetAllProcessStepsAsync()
        {
            return await _context.ProcessSteps.Include(x => x.Translations)
                .Select(x => new AdminProcessStepResponseDto
                {
                    Id = x.Id,
                    StepNumber = x.StepNumber,
                    Icon = x.Icon,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new ProcessStepTranslationDto { LanguageCode = t.LanguageCode, Title = t.Title, Description = t.Description }).ToList()
                }).ToListAsync();
        }

        public async Task<AdminProcessStepResponseDto?> GetProcessStepByIdAsync(int id)
        {
            return await _context.ProcessSteps.Include(x => x.Translations)
                .Where(x => x.Id == id)
                .Select(x => new AdminProcessStepResponseDto
                {
                    Id = x.Id,
                    StepNumber = x.StepNumber,
                    Icon = x.Icon,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new ProcessStepTranslationDto { LanguageCode = t.LanguageCode, Title = t.Title, Description = t.Description }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<int> SaveProcessStepAsync(AdminProcessStepUpsertDto dto, int? id = null)
        {
            ProcessStep entity = id.HasValue
                ? await _context.ProcessSteps.Include(x => x.Translations).FirstAsync(x => x.Id == id)
                : new ProcessStep();

            entity.StepNumber = dto.StepNumber;
            entity.Icon = dto.Icon;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = dto.IsActive;
            entity.Translations = dto.Translations.Select(t => new ProcessStepTranslation { LanguageCode = t.LanguageCode, Title = t.Title, Description = t.Description }).ToList();

            if (!id.HasValue) _context.ProcessSteps.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteProcessStepAsync(int id)
        {
            var item = await _context.ProcessSteps.FindAsync(id);
            if (item == null) return false;
            _context.ProcessSteps.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- 3. Detailed Services ---
        public async Task<List<AdminDetailedServiceResponseDto>> GetAllDetailedServicesAsync()
        {
            return await _context.DetailedServices.Include(x => x.Translations).Include(x => x.Highlights).ThenInclude(h => h.Translations)
                .Select(x => new AdminDetailedServiceResponseDto
                {
                    Id = x.Id,
                    Icon = x.Icon,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new DetailedServiceTranslationDto { LanguageCode = t.LanguageCode, Title = t.Title, Subtitle = t.Subtitle, Description = t.Description }).ToList(),
                    Highlights = x.Highlights.Select(h => new AdminHighlightResponseDto
                    {
                        Id = h.Id,
                        DisplayOrder = h.DisplayOrder,
                        Translations = h.Translations.Select(ht => new HighlightTranslationDto { LanguageCode = ht.LanguageCode, Text = ht.Text }).ToList()
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<AdminDetailedServiceResponseDto?> GetDetailedServiceByIdAsync(int id)
        {
            return await _context.DetailedServices.Include(x => x.Translations).Include(x => x.Highlights).ThenInclude(h => h.Translations)
                .Where(x => x.Id == id)
                .Select(x => new AdminDetailedServiceResponseDto
                {
                    Id = x.Id,
                    Icon = x.Icon,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new DetailedServiceTranslationDto { LanguageCode = t.LanguageCode, Title = t.Title, Subtitle = t.Subtitle, Description = t.Description }).ToList(),
                    Highlights = x.Highlights.Select(h => new AdminHighlightResponseDto
                    {
                        Id = h.Id,
                        DisplayOrder = h.DisplayOrder,
                        Translations = h.Translations.Select(ht => new HighlightTranslationDto { LanguageCode = ht.LanguageCode, Text = ht.Text }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<int> SaveDetailedServiceAsync(AdminDetailedServiceUpsertDto dto, int? id = null)
        {
            DetailedService entity = id.HasValue
                ? await _context.DetailedServices.Include(x => x.Translations).Include(x => x.Highlights).ThenInclude(h => h.Translations).FirstAsync(x => x.Id == id)
                : new DetailedService();

            entity.Icon = dto.Icon;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = dto.IsActive;
            entity.Translations = dto.Translations.Select(t => new DetailedServiceTranslation { LanguageCode = t.LanguageCode, Title = t.Title, Subtitle = t.Subtitle, Description = t.Description }).ToList();
            entity.Highlights = dto.Highlights.Select(h => new DetailedServiceHighlight
            {
                DisplayOrder = h.DisplayOrder,
                Translations = h.Translations.Select(ht => new DetailedServiceHighlightTranslation { LanguageCode = ht.LanguageCode, Text = ht.Text }).ToList()
            }).ToList();

            if (!id.HasValue) _context.DetailedServices.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteDetailedServiceAsync(int id)
        {
            var item = await _context.DetailedServices.FindAsync(id);
            if (item == null) return false;
            _context.DetailedServices.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- 4. Pricing Packages ---
        public async Task<List<AdminPackageResponseDto>> GetAllPackagesAsync()
        {
            return await _context.PricingPackages.Include(x => x.Translations).Include(x => x.Features).ThenInclude(f => f.Translations)
                .Select(x => new AdminPackageResponseDto
                {
                    Id = x.Id,
                    RatePerHour = x.RatePerHour,
                    IsPopular = x.IsPopular,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new PackageTranslationDto { LanguageCode = t.LanguageCode, Title = t.Title, PriceDisplay = t.PriceDisplay, Unit = t.Unit, Description = t.Description }).ToList(),
                    Features = x.Features.Select(f => new AdminFeatureResponseDto
                    {
                        Id = f.Id,
                        DisplayOrder = f.DisplayOrder,
                        Translations = f.Translations.Select(ft => new FeatureTranslationDto { LanguageCode = ft.LanguageCode, Text = ft.Text }).ToList()
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<AdminPackageResponseDto?> GetPackageByIdAsync(int id)
        {
            return await _context.PricingPackages.Include(x => x.Translations).Include(x => x.Features).ThenInclude(f => f.Translations)
                .Where(x => x.Id == id)
                .Select(x => new AdminPackageResponseDto
                {
                    Id = x.Id,
                    RatePerHour = x.RatePerHour,
                    IsPopular = x.IsPopular,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new PackageTranslationDto { LanguageCode = t.LanguageCode, Title = t.Title, PriceDisplay = t.PriceDisplay, Unit = t.Unit, Description = t.Description }).ToList(),
                    Features = x.Features.Select(f => new AdminFeatureResponseDto
                    {
                        Id = f.Id,
                        DisplayOrder = f.DisplayOrder,
                        Translations = f.Translations.Select(ft => new FeatureTranslationDto { LanguageCode = ft.LanguageCode, Text = ft.Text }).ToList()
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<int> SavePackageAsync(AdminPackageUpsertDto dto, int? id = null)
        {
            PricingPackage entity = id.HasValue
                ? await _context.PricingPackages.Include(x => x.Translations).Include(x => x.Features).ThenInclude(f => f.Translations).FirstAsync(x => x.Id == id)
                : new PricingPackage();

            entity.RatePerHour = dto.RatePerHour;
            entity.IsPopular = dto.IsPopular;
            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = dto.IsActive;
            entity.Translations = dto.Translations.Select(t => new PricingPackageTranslation { LanguageCode = t.LanguageCode, Title = t.Title, PriceDisplay = t.PriceDisplay, Unit = t.Unit, Description = t.Description }).ToList();
            entity.Features = dto.Features.Select(f => new PricingPackageFeature
            {
                DisplayOrder = f.DisplayOrder,
                Translations = f.Translations.Select(ft => new PricingPackageFeatureTranslation { LanguageCode = ft.LanguageCode, Text = ft.Text }).ToList()
            }).ToList();

            if (!id.HasValue) _context.PricingPackages.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeletePackageAsync(int id)
        {
            var item = await _context.PricingPackages.FindAsync(id);
            if (item == null) return false;
            _context.PricingPackages.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- 5. Customer Reviews ---
        public async Task<List<AdminReviewResponseDto>> GetAllReviewsAsync()
        {
            return await _context.CustomerReviews.Include(x => x.Translations)
                .Select(x => new AdminReviewResponseDto
                {
                    Id = x.Id,
                    Author = x.Author,
                    Location = x.Location,
                    Rating = x.Rating,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new ReviewTranslationDto { LanguageCode = t.LanguageCode, Comment = t.Comment, DateDisplay = t.DateDisplay, ServiceUsed = t.ServiceUsed }).ToList()
                }).ToListAsync();
        }

        public async Task<AdminReviewResponseDto?> GetReviewByIdAsync(int id)
        {
            return await _context.CustomerReviews.Include(x => x.Translations)
                .Where(x => x.Id == id)
                .Select(x => new AdminReviewResponseDto
                {
                    Id = x.Id,
                    Author = x.Author,
                    Location = x.Location,
                    Rating = x.Rating,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new ReviewTranslationDto { LanguageCode = t.LanguageCode, Comment = t.Comment, DateDisplay = t.DateDisplay, ServiceUsed = t.ServiceUsed }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<int> SaveReviewAsync(AdminReviewUpsertDto dto, int? id = null)
        {
            CustomerReview entity = id.HasValue
                ? await _context.CustomerReviews.Include(x => x.Translations).FirstAsync(x => x.Id == id)
                : new CustomerReview();

            entity.Author = dto.Author;
            entity.Location = dto.Location;
            entity.Rating = dto.Rating;
            entity.IsActive = dto.IsActive;
            entity.Translations = dto.Translations.Select(t => new CustomerReviewTranslation { LanguageCode = t.LanguageCode, Comment = t.Comment, DateDisplay = t.DateDisplay, ServiceUsed = t.ServiceUsed }).ToList();

            if (!id.HasValue) _context.CustomerReviews.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteReviewAsync(int id)
        {
            var item = await _context.CustomerReviews.FindAsync(id);
            if (item == null) return false;
            _context.CustomerReviews.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        // --- 6. Form Options ---
        public async Task<List<AdminFormOptionResponseDto>> GetAllFormOptionsAsync()
        {
            return await _context.FormServiceOptions.Include(x => x.Translations)
                .Select(x => new AdminFormOptionResponseDto
                {
                    Id = x.Id,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new FormOptionTranslationDto { LanguageCode = t.LanguageCode, Label = t.Label }).ToList()
                }).ToListAsync();
        }

        public async Task<AdminFormOptionResponseDto?> GetFormOptionByIdAsync(int id)
        {
            return await _context.FormServiceOptions.Include(x => x.Translations)
                .Where(x => x.Id == id)
                .Select(x => new AdminFormOptionResponseDto
                {
                    Id = x.Id,
                    DisplayOrder = x.DisplayOrder,
                    IsActive = x.IsActive,
                    Translations = x.Translations.Select(t => new FormOptionTranslationDto { LanguageCode = t.LanguageCode, Label = t.Label }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<int> SaveFormOptionAsync(AdminFormOptionUpsertDto dto, int? id = null)
        {
            FormServiceOption entity = id.HasValue
                ? await _context.FormServiceOptions.Include(x => x.Translations).FirstAsync(x => x.Id == id)
                : new FormServiceOption();

            entity.DisplayOrder = dto.DisplayOrder;
            entity.IsActive = dto.IsActive;
            entity.Translations = dto.Translations.Select(t => new FormServiceOptionTranslation { LanguageCode = t.LanguageCode, Label = t.Label }).ToList();

            if (!id.HasValue) _context.FormServiceOptions.Add(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteFormOptionAsync(int id)
        {
            var item = await _context.FormServiceOptions.FindAsync(id);
            if (item == null) return false;
            _context.FormServiceOptions.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
