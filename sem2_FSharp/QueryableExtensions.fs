namespace sem2_FSharp

open System.Linq
open System.Runtime.CompilerServices
open DomainModels

[<Extension>]
type QueryableExtensions =
    [<Extension>]
    static member inline public ById(userSet: IQueryable<User>, id: int) = userSet.Where(fun user -> user.Id = id)
    
//    [<Extension>]
//    static member inline public ById(categorySet: IQueryable<Category>, id: int) = categorySet.Where(fun cat -> cat.Id = id)
    
    [<Extension>]
    static member inline public ImageById(userSet: IQueryable<User>, id: int) = userSet.ById(id).Select(fun user -> user.Image)
    
    [<Extension>]
    static member inline public ById(filmSet: IQueryable<Film>, id: int) = filmSet.Where(fun user -> user.Id = id)
    
//    [<Extension>]
//    static member inline public LogoById(filmSet: IQueryable<Film>, id: int) = filmSet.ById(id).Select(fun film -> film.Logo)
//
//    [<Extension>]
//    static member inline public BackgroundById(filmSet: IQueryable<Film>, id: int) = filmSet.ById(id).Select(fun film -> film.Background)
    
//    [<Extension>]
//    static member inline public ById(productSet: IQueryable<Product>, id: int) = productSet.Where(fun product -> product.Id = id)
//    
//    [<Extension>]
//    static member inline public ImageById(productSet: IQueryable<Product>, id: int) = productSet.ById(id).Select(fun product -> product.Image)
