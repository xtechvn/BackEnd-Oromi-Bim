using Azure.Core;
using Caching.Elasticsearch;
using Entities.ViewModels;
using Entities.ViewModels.Comment;
using Entities.ViewModels.Mongo;
using HuloToys_Service.ElasticSearch.NewEs;
using Microsoft.AspNetCore.Mvc;
using PdfSharp;
using Repositories.IRepositories;
using Repositories.Repositories;
using WEB.CMS.Service;

namespace WEB.CMS.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _IcommentRepository;
        private readonly ClientESService _ClientESService;
        private LogCacheFilterMongoService _logCacheFilterMongoService;
        public CommentController(ICommentRepository icommentRepository,IConfiguration configuration)
        {
            _IcommentRepository = icommentRepository;
            _ClientESService = new ClientESService(configuration["DataBaseConfig:Elastic:Host"], configuration);
            _logCacheFilterMongoService = new LogCacheFilterMongoService(configuration);

        }
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> ListComment(CommentMongoSearchViewModel searchModel)
        {
            long total = 0;
            var lst = _logCacheFilterMongoService.GetListComment(searchModel, out total, searchModel.pageIndex, searchModel.pageSize);
            ViewBag.total = total;
            ViewBag.CurrentPage = searchModel.pageIndex;
            ViewBag.PageSize = searchModel.pageSize;
            ViewBag.TotalPage = (int)Math.Ceiling((double)total / searchModel.pageSize);
            return PartialView(lst);
        }

        //public async Task<GenericViewModel<CommentViewModel>> GetAllComment(CommentParamRequest request) 
        //{
        //    return await _IcommentRepository.GetAllComment(request);
        //}

        //public async Task<IActionResult> Client(string phoneOrName) 
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(phoneOrName))
        //            return Ok(new
        //            {
        //                is_success = true,
        //                data = _ClientESService.GetClientByNameOrPhone(phoneOrName)
        //            });
        //    }
        //    catch
        //    {

        //    }
        //    return Ok(new
        //    {
        //        is_success = false
        //    });
        //}
    }
}
