using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DomainModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sem2.Models.ViewModels.AdminPanelModels;
using sem2_FSharp;
using sem2_FSharp.ViewModels;
using sem2_FSharp.ViewModels.AdminPanelModels;

namespace sem2.Controllers
{
    [Authorize(Policy = "HasAdminPanelAccess")]
    public class AdminPanelController : Controller
    {
        private readonly ApplicationContext _dbContext;
        private readonly IWebHostEnvironment _appEnvironment;

        public AdminPanelController(ApplicationContext dbContext, IWebHostEnvironment appEnvironment)
        {
            _dbContext = dbContext;
            _appEnvironment = appEnvironment;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*
        [HttpGet]
        public IActionResult CommandLine()
        {
            return View(CommandLineResponse.Empty());
        }
        
        [HttpPost]
        public IActionResult CommandLine(string command)
        {
            var response = CommandLineResponse.Empty();
            if (!_commandService.TryExecuteCommand(command, out var message))
                response = CommandLineResponse.Failure(message);
            else response = CommandLineResponse.Success(message);
            return View(response);
        }*/
        
        [HttpGet]
        public IActionResult CreateFilm()
        {
            return View(new CreateFilmModel());
        }

        [HttpPost]
        public async Task<IActionResult> CreateFilm(CreateFilmModel data)
        {
            if (!ModelState.IsValid)
                return View(data);

            var film = new Film()
            {
                Actors = data.Actors,
                Info = "{}",
                LongDescription = data.LongDescription,
                ShortDescription = data.ShortDescription,
                Name = data.Name,
                OriginalName = data.OriginalName,
                Producer = data.Producer,
                Rating = 0,
            };

            _dbContext.Films.Add(film);
            await _dbContext.SaveChangesAsync();

            film.LogoImagePath = await CreateFilmLogoImage(data.LogoImage, film.Id);
            film.BackgroundImagePath = await CreateFilmBackgroundImage(data.BackgroundImage, film.Id);
            film.VideoPath = await CreateFilmVideo(data.Video, film.Id);

            await _dbContext.SaveChangesAsync();
            
            return Redirect(Url.Content($"~/AboutFilm/{film.Id}"));
        }
        
        // public IActionResult Products(string category, string query, int filterOption = 0)
        // {
        //     var model = new AdminPanelProductsViewModel();
        //     model.Category = category;
        //     model.Query = query;
        //     model.FilterOption = filterOption;
        //
        //     var categories = _dbContext.Categories.Select(c => new CategoryDTO()
        //     {
        //         Id = c.Id,
        //         Name = c.Name
        //     }).ToList();
        //
        //     model.Categories = categories;
        //
        //     var productsQuery = _dbContext.Products.Select(x => x);
        //     if (category != null && category != "all" && int.TryParse(category, out var categoryId))
        //         productsQuery = productsQuery.Where(p => p.CategoryId == categoryId);
        //
        //     if (!string.IsNullOrWhiteSpace(query))
        //     {
        //         if (filterOption == 0)
        //             productsQuery = productsQuery.Where(p => p.Name.ToLower().Contains(query.ToLower()));
        //         else if (filterOption == 1 && double.TryParse(query, out var ratingQuery))
        //             productsQuery = productsQuery.Where(p => p.Rating == (decimal) ratingQuery);
        //         else if (filterOption == 2 && double.TryParse(query, out var priceQuery))
        //             productsQuery = productsQuery.Where(p => p.Price == (decimal) priceQuery);
        //     }
        //
        //     var products = productsQuery.Select(p => new AdminPanelProductDTO()
        //     {
        //         Id = p.Id,
        //         Name = p.Name,
        //         CategoryName = p.Category.Name,
        //         Price = (double)p.Price,
        //         Rating = (double)p.Rating
        //     }).ToList();
        //
        //     model.Products = products;
        //     
        //     return View(model);
        // }

        // [HttpGet]
        // public IActionResult CreateProduct()
        // {
        //     var model = new AdminPanelCreateProductViewModel();
        //     var categories = _dbContext.Categories.Select(c => new CategoryDTO()
        //     {
        //         Id = c.Id,
        //         Name = c.Name
        //     }).ToList();
        //
        //     model.Categories = categories;
        //     
        //     return View(model);
        // }

        // [HttpGet("~/adminpanel/api/deleteProduct")]
        // public async Task<IActionResult> DeleteProduct(int productId)
        // {
        //     var imageData = _dbContext.Products.ImageById(productId).FirstOrDefault();
        //
        //     if (imageData != null)
        //     {
        //         var imagePath = Path.Combine(_appEnvironment.WebRootPath, imageData.ImagePath);
        //         if (System.IO.File.Exists(imagePath))
        //             System.IO.File.Delete(imagePath);
        //     }
        //
        //     var product = _dbContext.Products.ById(productId).FirstOrDefault();
        //     if (product != null)
        //     {
        //         _dbContext.Products.Remove(product);
        //         _dbContext.RemoveRange(_dbContext.Reviews
        //             .Where(review => review.ProductId == product.Id)
        //             .ToList());
        //     }
        //
        //     if(imageData != null)
        //         _dbContext.ImageMetadata.Remove(imageData);
        //     
        //     await _dbContext.SaveChangesAsync();
        //
        //     return Ok();
        // }

        // [HttpPost]
        // public async Task<IActionResult> CreateProduct(AdminPanelCreateProductViewModel data)
        // {
        //     var categories = _dbContext.Categories.Select(c => new CategoryDTO()
        //     {
        //         Id = c.Id,
        //         Name = c.Name
        //     }).ToList();
        //
        //     data.Categories = categories;
        //
        //     if(data.Image == null)
        //         ModelState.AddModelError("", "Фото товара должно быть установлено");
        //     if (!ModelState.IsValid)
        //         return View(data);
        //
        //     var product = new Product()
        //     {
        //         CategoryId = data.Category.Value,
        //         Name = data.ProductName,
        //         Description = data.ProductDescription,
        //         Price = (decimal) data.ProductPrice.Value,
        //         Rating = 0
        //     };
        //
        //     var imageData = await CreateProductImageMetadata(data.Image);
        //     await _dbContext.ImageMetadata.AddAsync(imageData);
        //
        //     await _dbContext.SaveChangesAsync();
        //
        //     product.ImageId = imageData.Id;
        //     var attributeValues = new Dictionary<string, string>();
        //
        //     foreach (var propertyInfo in data.PropertyInfos)
        //         attributeValues[propertyInfo.PropertyId.ToString()] = propertyInfo.Value;
        //
        //     var jsonDoc = JsonDocument.Parse(JsonSerializer.SerializeToUtf8Bytes(attributeValues));
        //     product.AttributeValues = jsonDoc;
        //
        //     await _dbContext.Products.AddAsync(product);
        //     await _dbContext.SaveChangesAsync();
        //     
        //     return RedirectToAction("Products");
        // }

        public async Task<ImageMetadata> CreateImageMetadata(IFormFile imageFile, string name, string folderName)
        {
            var imageName = name + Path.GetExtension(imageFile.FileName);
            var virtualImagePath = Path.Combine($"applicationData/{folderName}", imageName);
            var imagePath = Path.Combine(_appEnvironment.WebRootPath, virtualImagePath);
        
            await using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }
        
            var imageData = new ImageMetadata()
            {
                ImagePath = virtualImagePath,
                ContentType = imageFile.ContentType
            };
        
            return imageData;
        }

        public async Task<string> CreateFilmLogoImage(IFormFile image, int filmId)
        {
            var metadata = await CreateImageMetadata(image, $"filmLogo_{filmId}", "film_logos");
            return metadata.ImagePath;
        }

        public async Task<string> CreateFilmBackgroundImage(IFormFile image, int filmId)
        {
            var metadata = await CreateImageMetadata(image, $"filmBackground_{filmId}", "film_backgrounds");
            return metadata.ImagePath;
        }
        
        public async Task<string> CreateFilmVideo(IFormFile video, int filmId)
        {
            var metadata = await CreateImageMetadata(video, $"film_{filmId}", "films");
            return metadata.ImagePath;
        }
        
        //
        // [HttpGet("~/adminpanel/api/getproperties")]
        // public IActionResult GetPropertyInfos(int categoryId)
        // {
        //     if (!_dbContext.Categories.Any(c => c.Id == categoryId))
        //         return BadRequest();
        //
        //     var properties = _dbContext.Categories
        //         .Where(c => c.Id == categoryId)
        //         .SelectMany(c => c.Properties)
        //         .Select(p => new
        //         {
        //             Id = p.Id,
        //             Name = p.Name,
        //             PropertyType = p.Type.Name,
        //             FilterInfo = p.FilterInfo.ToJsonString(),
        //             Constraints = p.Constraints.ToJsonString()
        //         }).ToList();
        //
        //     return Json(properties);
        // }
        //
        // public IActionResult GetAllProductPictures()
        // {
        //     using (var memoryStream = new MemoryStream())
        //     {
        //         using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
        //         {
        //             foreach (var filePath in Directory.EnumerateFiles(Path.Combine(_appEnvironment.WebRootPath,
        //                 "applicationData/productImages")))
        //             {
        //                 var file = archive.CreateEntry(Path.GetFileName(filePath));
        //                 using var streamWriter = file.Open();
        //                 using var fileReader = System.IO.File.OpenRead(filePath);
        //                     fileReader.CopyTo(streamWriter);
        //             }
        //         }
        //
        //         return File(memoryStream.ToArray(), "application/zip", "Images.zip");
        //     }
        // }
        //
        // public IActionResult Orders()
        // {
        //     return BadRequest();
        // }
    }
}