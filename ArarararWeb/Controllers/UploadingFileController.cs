using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ArarararWeb.DataModels;
using Microsoft.AspNetCore.Mvc;

namespace ArarararWeb.Controllers
{
    public class UploadingFileController : Controller
    {
        private readonly MyDBContext _dbContext;

        public UploadingFileController(MyDBContext dBContext)
        {
            _dbContext = dBContext;
        }
        public IActionResult Index()
        {
            var files = _dbContext.Files.ToList(); // Получаем все файлы из базы данных

            return View(files);
            
        }
    }
}