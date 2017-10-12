using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class LoginViewModel
    {
        [Key]
        public string Имя { get; set; }
        [DataType(DataType.Password)]
        public string Пароль { get; set; }
        public int tries { get; set; }
    }
}