using System.Collections.Generic;

namespace sem2.Models.ViewModels.HomeModels
{
    public class CatalogViewModel
    {
        public IEnumerable<FilmDTO> Films { get; set; }
        public string Query { get; set; }
    }
}