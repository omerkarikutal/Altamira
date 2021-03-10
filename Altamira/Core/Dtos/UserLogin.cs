using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Dtos
{
    public class UserLogin
    {
        [Required(ErrorMessage = "Kullanıcı Adı Zorunlu")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Şifre Zorunlu")]
        public string Password { get; set; }
    }
}
