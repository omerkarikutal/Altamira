using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dtos
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Username { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
