// using sem2_FSharp.ViewModels.CatalogModels;
// using sem2.Infrastructure.Filters.FilterDTOs;
// using sem2.Models.ViewModels;
// using sem2.Models.ViewModels.FilterViewModels;
//
// namespace sem2.Infrastructure.Filters.FilterDTOBuilders
// {
//     public class StringFilterViewModelBuilder : IFilterViewModelBuilder<StringFilterViewModel>
//     {
//         public StringFilterViewModel BuildFilterViewModel(FilterViewModel filterViewModel, dynamic filterInfo, dynamic constraints, FilterDTO filterDto = null)
//         {
//             var model = (StringFilterViewModel) filterViewModel;
//             if (filterDto != null && filterDto is StringFilterDto dto)
//                 model.Query = dto.Query;
//             return model;
//         }
//     }
// }