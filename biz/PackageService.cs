using Demo2.Context;
using Demo2.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace Demo2.biz
{
    public class PackageService
    {
        private readonly IServiceProvider _serviceProvider;
        public List<PackageMenuCondition> _packageMenuCondition;

        public PackageService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _packageMenuCondition = LoadPackageMenuConditionAsync().GetAwaiter().GetResult();
        }

        private async Task<List<PackageMenuCondition>> LoadPackageMenuConditionAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<Package>();

                var packageList = await _context.tbl_package
                    .Include(p => p.package_menu_condition)
                    .ThenInclude(m => m.menuId)
                    .Include(u => u.package_menu_condition)
                    .ThenInclude(mf => mf.menu_features)
                    .SelectMany(u => u.package_menu_condition.Select(m => new PackageMenuCondition
                    {
                        package_name = u.Name,
                        menu_name = m.menuId.Name,
                        menu_feature_name = m.menu_features.name,
                        condition_value = m.condition_value,
                        description = m.menu_features.description
                    }))
                    .AsNoTracking()
                    .ToListAsync();

                return packageList;
            }
        }
        public List<PackageMenuCondition> GetPackageMenuCondition()
        {
            return _packageMenuCondition;
        }
    }


}
