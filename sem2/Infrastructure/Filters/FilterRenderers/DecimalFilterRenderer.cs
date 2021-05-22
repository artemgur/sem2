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
//     [ForPropertyType(PropertyTypeEnum.Decimal)]
//     public class DecimalFilterRenderer : IFilterRenderer<DecimalFilterViewModel>
//     {
//         public IViewComponentResult Render(DecimalFilterViewModel model)
//         {
//             return new ViewViewComponentResult()
//             {
//                 ViewName = "DecimalFilterPartial"
//             };
//         }
//
//         public IViewComponentResult Render(FilterViewModel model)
//         {
//             return Render((DecimalFilterViewModel) model);
//         }
//     }
// }