using Demo2.biz;
using Demo2.Context;
using Demo2.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Demo2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class package_controller : Controller {
        private readonly PackageService _packageService;
        private readonly Package _context;

        public package_controller(PackageService packageService, Package context)
        {
            _packageService = packageService;
            _context = context;
        }
        [HttpGet("GetUsers")]
        public async Task<ActionResult<IEnumerable<tbl_user>>> GetUsers()
        {
            var userList = await _context.tbl_user
            .Include(u => u.user_package)
            .ThenInclude(p => p.packageId)
            .Include(u => u.user_package)
            .ThenInclude(m => m.menuId)
            .Select(u => new
            {
                u.Id,
                u.name,
                UserPackages = u.user_package.Select(pm => new
            {
                pm.package_id,
                PackageName = pm.packageId.Name,
                pm.menu_id,
                MenuName = pm.menuId.Name
            }).ToList()
            })
            .AsNoTracking()
            .ToListAsync();

            return Ok(userList);
        }
            [HttpGet("GetPackgeInfo")]
            public ActionResult<IEnumerable<PackageMenuCondition>> GetPackageInfo() 
            {
                var packageConditions = _packageService.GetPackageMenuCondition();
                return Ok(packageConditions);
        }

        [HttpPost("AddUserPackage")]
        public async Task<ActionResult> AddUserPackage([FromBody] AddUserPackageDto dto)
        {
            if (dto.PackageId < 1 || dto.PackageId > 7 || dto.MenuId < 1 || dto.MenuId > 7)
            {
                return BadRequest("PackageId and MenuId must be between 1 and 7.");
            }

            var userExists = await _context.tbl_user.AnyAsync(u => u.Id == dto.UserId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var existingUserPackage = await _context.tbl_user_package
                .FirstOrDefaultAsync(up => up.user_id == dto.UserId && up.package_id == dto.PackageId);

            if (existingUserPackage != null)
            {
                _context.tbl_user_package.Remove(existingUserPackage);
                await _context.SaveChangesAsync();
            }

            var userPackage = new tbl_user_package
            {
                user_id = dto.UserId,
                package_id = dto.PackageId,
                menu_id = dto.MenuId
            };

            await _context.tbl_user_package.AddAsync(userPackage);
            await _context.SaveChangesAsync();

            return Ok("User package added successfully.");
        }

        [HttpGet("GetUsersByPackage/{packageId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetUsersByPackage(int packageId)
        {
            if (packageId < 1 || packageId > 7)
            {
                return BadRequest("PackageId must be between 1 and 7.");
            }

            var userList = await _context.tbl_user
                .Where(u => u.user_package.Any(up => up.package_id == packageId))
                .Select(u => new
                {
                    u.Id,
                    u.name,
                    UserPackages = u.user_package.Where(up => up.package_id == packageId)
                        .Select(up => $"Package ID: {up.package_id}, Package Name: {up.packageId.Name}, Menu ID: {up.menu_id}, Menu Name: {up.menuId.Name}")
                        .ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            var formattedUsers = userList.Select(u =>
                $"ID: {u.Id}, Name: {u.name}, UserPackages: {string.Join(", ", u.UserPackages)}"
            ).ToList();

            return Ok(formattedUsers);
        }
        [HttpGet("GetUsersByPackage2/{packageId}")]
        public async Task<ActionResult<IEnumerable<tbl_user>>> GetUsersByPackage2(int packageId)
        {
            if (packageId < 1 || packageId > 7)
            {
                return BadRequest("PackageId must be between 1 and 7.");
            }

            var userList = await _context.tbl_user
                .Where(u => u.user_package.Any(up => up.package_id == packageId))
                .Select(u => new
                {
                    u.Id,
                    u.name,
                    UserPackages = u.user_package.Select(up => new
                    {
                        up.package_id,
                        PackageName = up.packageId.Name,
                        up.menu_id,
                        MenuName = up.menuId.Name
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();

            return Ok(userList);
        }
        //[HttpGet("GetPackagesByUser/{userId}")]
        //public async Task<ActionResult<IEnumerable<object>>> GetPackagesByUser(int userId)
        //{
        //    var userExists = await _context.tbl_user.AnyAsync(u => u.Id == userId);
        //    if (!userExists)
        //    {
        //        return NotFound("User not found.");
        //    }

        //    var packages = await _context.tbl_user_package
        //        .Where(up => up.user_id == userId)
        //        .Select(up => new
        //        {
        //            up.package_id,
        //            PackageName = up.packageId.Name,
        //            up.menu_id,
        //            MenuName = up.menuId.Name
        //        })
        //        .AsNoTracking()
        //        .ToListAsync();

        //    return Ok(packages);
        //}
        [HttpGet("GetPackagesByUser2/{userId}")]
        public async Task<ActionResult<IEnumerable<string>>> GetPackagesByUser2(int userId)
        {
            var userExists = await _context.tbl_user.AnyAsync(u => u.Id == userId);
            if (!userExists)
            {
                return NotFound("User not found.");
            }

            var packages = await _context.tbl_user_package
                .Where(up => up.user_id == userId)
                .Select(up => $"package {up.package_id}: {up.packageId.Name}, menu {up.menu_id}: {up.menuId.Name}")
                .AsNoTracking()
                .ToListAsync();

            return Ok(packages);
        }
    }
}
