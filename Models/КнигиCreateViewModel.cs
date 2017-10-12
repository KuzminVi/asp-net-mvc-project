using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class КнигиCreateViewModel
    {
        [Key]
        public int Код { get; set; }
        public string новАвтор { get; set; }
        [Required]
        public int сущАвтор { get; set; }
        [Required]
        [StringLength(255)]
        public string Название { get; set; }
        [StringLength(255)]
        public string Серия { get; set; }
        [StringLength(255)]
        public string Жанр { get; set; }
        [DisplayName("Год выпуска")]
        public Nullable<int> Год_выпуска { get; set; }
        [StringLength(1000)]
        [DisplayName("Краткое описание")]
        public string Краткое_описание { get; set; }
        [StringLength(1000)]
        [DisplayName("Ссылка на скачивание")]
        public string Ссылка_на_скачивание { get; set; }
        [DisplayName("Средний рейтинг")]
        public Nullable<float> Средний_рейтинг { get; set; }
    }
}