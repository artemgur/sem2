namespace sem2_FSharp.ViewModels.ProfileModels

open System.ComponentModel.DataAnnotations

type PasswordResetModel() =
    [<DefaultValue>]
    val mutable private userId: int
    [<DefaultValue>]
    val mutable private newPassword: string
    
    [<DefaultValue>]
    val mutable private confirmPassword: string
    
    [<Required(ErrorMessage = "Не указан пароль")>]
    [<DataType(DataType.Password)>]
    member public this.NewPassword with get() = this.newPassword
                                    and set p = this.newPassword <- p
                                    
    [<Required(ErrorMessage = "Пароль введен неверно")>]
    [<DataType(DataType.Password)>]
    member public this.ConfirmPassword with get() = this.confirmPassword
                                        and set p = this.confirmPassword <- p
  
     member public this.UserId with get() = this.userId
                               and set p = this.userId <- p
                          