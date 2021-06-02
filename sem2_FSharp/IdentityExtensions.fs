namespace sem2_FSharp

open System
open System.Runtime.CompilerServices
open System.Security.Claims

[<Extension>]
type IdentityExtensions =
    [<Extension>]
    static member inline public GetName(principal: ClaimsPrincipal) =
        if principal.Identity.IsAuthenticated then
            let first = principal.FindFirst("username")
            if first = null then null else first.Value
        else null
