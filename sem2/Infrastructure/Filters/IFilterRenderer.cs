﻿using Microsoft.AspNetCore.Mvc;
using sem2_FSharp.ViewModels.CatalogModels;
using sem2.Models.ViewModels;

namespace sem2.Infrastructure.Filters
{
    public interface IFilterRenderer
    {
        IViewComponentResult Render(FilterViewModel model);
    }

    public interface IFilterRenderer<in T> : IFilterRenderer where T : FilterViewModel
    {
        IViewComponentResult Render(T model);
    }
}