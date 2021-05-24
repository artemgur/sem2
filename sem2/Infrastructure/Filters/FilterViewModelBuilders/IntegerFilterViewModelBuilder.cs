// using sem2_FSharp.ViewModels.CatalogModels;
// using sem2.Infrastructure.Filters.FilterDTOs;
// using sem2.Models.ViewModels;
// using sem2.Models.ViewModels.FilterViewModels;
//
// namespace sem2.Infrastructure.Filters.FilterDTOBuilders
// {
//     public class IntegerFilterViewModelBuilder : IFilterViewModelBuilder<IntegerFilterViewModel>
//     {
//         public IntegerFilterViewModel BuildFilterViewModel(FilterViewModel filterViewModel, dynamic filterInfo, dynamic constraints, FilterDTO filterDto = null)
//         {
//             var model = (IntegerFilterViewModel) filterViewModel;
//             model.MinConstraint = constraints["minValue"] ?? long.MinValue;
//             model.MaxConstraint = constraints["maxValue"] ?? long.MaxValue;
//             if (filterDto != null && filterDto is IntegerFilterDto dto)
//             {
//                 model.Min = dto.Min;
//                 model.Max = dto.Max;
//             }
//             return model;
//         }
//     }
// }