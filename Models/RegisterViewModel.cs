using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class RegisterViewModel
    {
        [Required][MinLength(5)][MaxLength(15)][Key]
        public string Имя { get; set; }
        [Required][MinLength(6)]
        [MaxLength(15)]
        [DataType(DataType.Password)]
        public string Пароль { get; set; }
        [Required][Compare("Пароль")]
        [DataType(DataType.Password)]
        public string потверждениеПароль { get; set; }
        [Required]
        [MaxLength(50)]
        [DataType(DataType.EmailAddress)]
        public string Почта { get; set; }

    }
}