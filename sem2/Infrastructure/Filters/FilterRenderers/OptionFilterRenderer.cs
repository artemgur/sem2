// using DomainModels;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Mvc.ViewComponents;
// using sem2_FSharp.ViewModels.CatalogModels;
// using sem2.Infrastructure.Filters.FilterDTOs;
// using sem2.Models.ViewModels;
// using sem2.Models.ViewModels.FilterViewModels;
//
// namespace sem2.Infrastructure.Filters.FilterRenderers
// {
//     [ForPropertyType(PropertyTypeEnum.Option)]
//     public class OptionFilterRenderer : IFilterRenderer<OptionFilterViewModel>
//     {
//         public IViewComponentResult Render(OptionFilterViewModel model)
//         {
//             return new ViewViewComponentResult()
//             {
//                 ViewName = "OptionFilterPartial"
//             };        
//         }
//
//         public IViewComponentResult Render(FilterViewModel model)
//         {
//             return Render((OptionFilterViewModel) model);
//         }
//     }
// }