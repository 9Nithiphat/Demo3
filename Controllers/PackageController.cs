using Demo2.biz;
using Demo2.Context;
using Demo2.Model;
using Demo2.Model.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace Demo2.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly PackageService _packageService;
        private readonly IConfiguration _configuration;

        public PackageController(PackageService packageService, IConfiguration configuration)
        {
            _packageService = packageService;
            _configuration = configuration;
        }
        [HttpGet("test")]
        public string GetTest()
        {
            return "API is Good!";
        }

        [HttpPost("GetUsersByPackage2")]
        public ActionResult<List<PackageMenuCondition>> GetUsersByPackage2(PackageReq args)
        {

            var packageMenuConditions = _packageService._packageMenuCondition;
            List<string> packages = packageMenuConditions
            .Select(p => p.package_name)
            .Distinct()
            .ToList();
            if (!packages.Contains(args.packageName))
            {
                args.packageName = _configuration["packageDefault"];
            }

            List<PackageInfo> packageFeature = packageMenuConditions
                                            .Where(u => u.package_name == args.packageName)
                                            .Select(u => new PackageInfo { condition_value = u.condition_value, menu_feature_name = u.menu_feature_name})
                                            .ToList();

            return Ok(packageFeature);

        }
    }
}
