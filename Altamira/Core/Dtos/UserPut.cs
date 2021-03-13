using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Dtos
{
    public class UserPut
    {
        [Required(ErrorMessage = "Id Zorunlu")]
        public string Id { get; set; }
        [Required(ErrorMessage = "Kullanıcı Adı Zorunlu")]
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
