using System.Collections.Generic;
using sem2_FSharp.ViewModels.CatalogModels;
using sem2.Infrastructure.Filters;

namespace sem2.Models.ViewModels
{
    public class SearchViewModel
    {
        public IEnumerable<FilterViewModel> Filters;
    }
}