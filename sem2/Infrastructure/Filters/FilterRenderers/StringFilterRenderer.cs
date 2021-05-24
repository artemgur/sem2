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
//     [ForPropertyType(PropertyTypeEnum.Nominal)]
//     public class StringFilterRenderer : IFilterRenderer<StringFilterViewModel>
//     {
//         public IViewComponentResult Render(StringFilterViewModel model)
//         {
//             return new ViewViewComponentResult()
//             {
//                 ViewName = "StringFilterPartial"
//             };
//         }
//
//         public IViewComponentResult Render(FilterViewModel model)
//         {
//             return Render((StringFilterViewModel) model);
//         }
//     }
// }