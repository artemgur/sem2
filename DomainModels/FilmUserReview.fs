namespace DomainModels

open System.Collections.Generic;
open System.ComponentModel.DataAnnotations

[<CLIMutable>]
type Film =
    {
        [<Key>]
        Id:int
        Name:string
        LongDescription:string
        ShortDescription:string
        Info:string//2021, приключения, боевик&ensp;  Россия&ensp;  2 ч 16 мин&ensp;
//        Price:decimal
//        Rating:decimal
        
        
//        LogoId:int
//        Logo:ImageMetadata
//        
//        BackgroundId:int
//        Background:ImageMetadata
        
        InFavoritesOfUsers:ICollection<User>
        
        Producer:string
        OriginalName:string
        Actors:string
        
        
        //Reviews:ICollection<Review>
        
        
        //Genres: ICollection<Genre>
    }
and [<CLIMutable>] User =
    {
        [<Key>]
        Id:int
        
        Name:string
        Surname:string
        Email:string
        HashedPassword:string

        ImageId:int
        Image:ImageMetadata
        
        Role:UserRole
        RoleId:int
        
        FavoriteFilms:ICollection<Film>
        
        TotalPayment:decimal
        
//        Subscriptions:ICollection<UserSubscription>
        
        IsConfirmed:bool
    }
// and [<CLIMutable>] Review =
//        {
//            [<Key>]
//            Id:int
//            
//            FilmId:int
//            Film:Film
//            
//            UserId:int
//            User:User
//            
//            Content:string
//            Rating:int
//            Date:DateTime
//        }
//and [<CLIMutable>] UserSubscription =
//    {
//        Subscription: Subscription
//        User: User
//        EndDate: DateTime
//    }