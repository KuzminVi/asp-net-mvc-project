using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    
    public class ManageViewModel
    {
        [Key]
        public int Код_книги { get; set; }
        [Required]
        public string Статус { get; set; }
        [Required]
        public string Рейтинг { get; set; }
   
        public virtual Книги Книги { get; set; }
    }
}