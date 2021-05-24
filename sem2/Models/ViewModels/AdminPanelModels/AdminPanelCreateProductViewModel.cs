// using System.Collections.Generic;
// using System.ComponentModel.DataAnnotations;
// using Microsoft.AspNetCore.Http;
// using Microsoft.AspNetCore.Mvc;
// using sem2_FSharp.ViewModels;
// using sem2.Infrastructure.AdminPanel;
//
// namespace sem2.Models.ViewModels.AdminPanelModels
// {
//     [ModelBinder(BinderType = typeof(CreateProductPropertyInfoBinder))]
//     public class CreateProductPropertyInfo
//     {
//         public int PropertyId { get; set; }
//         public string Value { get; set; }
//     }
//     
//     public class AdminPanelCreateProductViewModel
//     {
//         [Required(ErrorMessage = "Категория должна быть указана")] 
//         public int? Category { get; set; } = null;
//         [Required(ErrorMessage = "Имя товара должно быть указано")]
//         [MaxLength(64)]
//         public string ProductName { get; set; }
//         [MaxLength(1024)]
//         public string ProductDescription { get; set; }
//         [Required(ErrorMessage = "Цена товара должна быть указана")]
//         [Range(0, 10000000000, ErrorMessage = "Цена должна быть положительным числом, не превосходящим 10 000 000 000")]
//         public double? ProductPrice { get; set; } = null;
//
//         public IEnumerable<CategoryDTO> Categories;
//
//         public IFormFile Image { get; set; }
//         public List<CreateProductPropertyInfo> PropertyInfos { get; set; }
//     }
// }