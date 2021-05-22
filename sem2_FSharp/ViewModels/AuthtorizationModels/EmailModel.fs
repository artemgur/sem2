namespace sem2_FSharp.ViewModels.AuthtorizationModels

open System.ComponentModel.DataAnnotations

type EmailModel() =
    [<DefaultValue>]
    val mutable private email: string

    [<Required(ErrorMessage = "Не указан Email")>]
    member public this.Email with get() = this.email
                                    and set p = this.email <- p
