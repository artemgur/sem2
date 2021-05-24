// using Microsoft.Extensions.DependencyInjection;
// using sem2_FSharp.ViewModels.CatalogModels;
// using sem2.Models.ViewModels;
//
// namespace sem2.Infrastructure.Filters
// {
//     public static class ServiceCollectionFilterExtensions
//     {
//         public static IServiceCollection AddFilters(this IServiceCollection services)
//         {
//             services.AddSingleton<FilterMapper<FilterDTO>>();
//             services.AddSingleton<FilterMapper<FilterViewModel>>();
//             services.AddSingleton<FilterMapper<IFilterRenderer>>();
//             services.AddSingleton<FilterViewModelProvider>();
//             return services;
//         }
//     }
// }