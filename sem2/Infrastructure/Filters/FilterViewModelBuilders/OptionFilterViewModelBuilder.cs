// using System.Collections.Generic;
// using System.Linq;
// using Newtonsoft.Json.Linq;
// using sem2_FSharp.ViewModels.CatalogModels;
// using sem2.Infrastructure.Filters.FilterDTOs;
// using sem2.Models.ViewModels;
// using sem2.Models.ViewModels.FilterViewModels;
//
// namespace sem2.Infrastructure.Filters.FilterDTOBuilders
// {
//     public class OptionFilterViewModelBuilder : IFilterViewModelBuilder<OptionFilterViewModel>
//     {
//         public OptionFilterViewModel BuildFilterViewModel(FilterViewModel filterViewModel, dynamic filterInfo, dynamic constraints, FilterDTO filterDto = null)
//         {
//             var model = (OptionFilterViewModel) filterViewModel;
//             JArray optionsJson = filterInfo["options"];
//             model.Options = optionsJson.ToObject<List<string>>();
//             if (filterDto != null && filterDto is OptionFilterDto dto)
//             {
//                 model.ChosenOptions = dto.Options.ToHashSet();
//             }
//             return model;
//         }
//     }
// }