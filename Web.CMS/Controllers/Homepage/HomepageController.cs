using Caching.RedisWorker;
using Entities.ConfigModels;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Repositories.IRepositories;
using Repositories.Repositories;
using System.Security.Claims;
using Utilities;
using Utilities.Common;
using Utilities.Contants;
using WEB.CMS.Customize;

namespace Web.CMS.Controllers.Homepage
{
    [CustomAuthorize]
    public class HomepageController : Controller
    {
      
         private readonly IConfiguration _configuration;
        private readonly RedisConn _redisConn;
        private readonly IAllCodeRepository _allCodeRepository;
        private readonly string _UrlStaticImage;
        public HomepageController(IConfiguration configuration, RedisConn redisConn, IAllCodeRepository allCodeRepository)
        {
            _configuration = configuration;
            _redisConn = redisConn;
            _redisConn.Connect();
            _allCodeRepository = allCodeRepository;
            _UrlStaticImage = _configuration["DomainConfig:ImageStatic"];
        }
        public async Task<IActionResult> Index()
        {
            int max_slide = 3;
            int max_sub = 3;
            ViewBag.BannerSlide = new List<AllCode>();
            ViewBag.BannerSub = new List<AllCode>();
            ViewBag.MaxSlide = max_slide;
            ViewBag.MaxSub = max_sub;
            ViewBag.MaxSupplier = 3;
            string static_domain = _configuration["DomainConfig:ImageStatic"];
            ViewBag.Static = static_domain;
            try
            {

                ViewBag.BannerSlide = _allCodeRepository.GetListByType("HOMEPAGE_SLIDE");
                ViewBag.BannerSub = _allCodeRepository.GetListByType("HOMEPAGE_SUBBANNER");
                ViewBag.Supplier = _allCodeRepository.GetListByType("HOMEPAGE_SUPPLIER");
            }
            catch
            {

            }
            return View();
        }
        public async Task<IActionResult> Summit(List<AllCode> banner_main, List<AllCode> banner_sub, List<AllCode> trending_main = null)
        {

            try
            {
                int _UserId = 0;

                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                if (banner_main != null && banner_main.Count > 0)
                {
                    foreach (var banner in banner_main)
                    {
                        banner.Type = "HOMEPAGE_SLIDE";
                        banner.UpdateTime = DateTime.Now;
                        banner.CreateDate = DateTime.Now;
                        banner.CreatedBy = _UserId;
                        banner.UpdatedBy = _UserId;
                        if (banner.Description == null) banner.Description = "";
                        string static_domain = _configuration["DomainConfig:ImageStatic"];
                        banner.Description = banner.Description.Replace(static_domain, "");
                        banner.Description = await ImageResizerLegacy.DownloadAndOptimizeImageAsync(banner.Description, _UrlStaticImage);

                        if (banner.Id > 0)
                        {
                            await _allCodeRepository.Update(banner);
                        }
                    }
                }
                if (banner_sub != null && banner_sub.Count > 0)
                {
                    foreach (var banner in banner_sub)
                    {
                        banner.Type = "HOMEPAGE_SUBBANNER";
                        banner.UpdateTime = DateTime.Now;
                        banner.CreateDate = DateTime.Now;
                        banner.CreatedBy = _UserId;
                        banner.UpdatedBy = _UserId;
                        if (banner.Description == null) banner.Description = "";
                        string static_domain = _configuration["DomainConfig:ImageStatic"];
                        banner.Description = banner.Description.Replace(static_domain, "");
                        banner.Description = await ImageResizerLegacy.DownloadAndOptimizeImageAsync(banner.Description, _UrlStaticImage);

                        if (banner.Id > 0)
                        {
                            await _allCodeRepository.Update(banner);
                        }
                    }
                }
                if (trending_main != null && trending_main.Count > 0)
                {
                    foreach (var banner in trending_main)
                    {
                        banner.Type = "HOMEPAGE_TRENDINGMAIN";
                        banner.UpdateTime = DateTime.Now;
                        banner.CreateDate = DateTime.Now;
                        banner.CreatedBy = _UserId;
                        banner.UpdatedBy = _UserId;
                        if (banner.Description == null) banner.Description = "";
                        string static_domain = _configuration["DomainConfig:ImageStatic"];
                        banner.Description = banner.Description.Replace(static_domain, "");
                        banner.Description = await ImageResizerLegacy.DownloadAndOptimizeImageAsync(banner.Description, _UrlStaticImage);

                        if (banner.Id > 0)
                        {
                            await _allCodeRepository.Update(banner);
                        }
                    }
                }
              
                _allCodeRepository.DeleteEmptyAllcodeDescription("HOMEPAGE_TRENDINGSUB");
                _allCodeRepository.DeleteEmptyAllcodeDescription("HOMEPAGE_TRENDINGMAIN");
                _allCodeRepository.DeleteEmptyAllcodeDescription("HOMEPAGE_SLIDE");

                _redisConn.clear(CacheName.OMORI_HOMEPAGE_SLIDE, Convert.ToInt32(_configuration["Redis:Database:db_common"]));
                return Ok(new
                {
                    is_success = true,
                    message = "Cập nhật thành công"
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("Summit - HomepageController: " + ex);
            }
            return Ok(new
            {
                is_success = false,
                message = "Cập nhật thất bại"

            });
        }
        [HttpPost]
        public async Task<IActionResult> DeleteById(int id)
        {

            try
            {
                int _UserId = 0;

                if (HttpContext.User.FindFirst(ClaimTypes.NameIdentifier) != null)
                {
                    _UserId = Convert.ToInt32(HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
                }
                if (id <= 0)
                {
                    return Ok(new
                    {
                        is_success = false,
                        message = "FAILED"
                    });
                }

                await _allCodeRepository.Delete(id);

                _redisConn.clear(CacheName.OMORI_HOMEPAGE_SLIDE, Convert.ToInt32(_configuration["Redis:Database:db_common"]));
                return Ok(new
                {
                    is_success = true,
                    message = "SUCCESS"
                });
            }
            catch (Exception ex)
            {
                LogHelper.InsertLogTelegram("DeleteById - HomepageController: " + ex);
            }
            return Ok(new
            {
                is_success = false,
                message = "Cập nhật thất bại"

            });
        }
    }
}
