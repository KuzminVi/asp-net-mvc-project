using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class MessageViewModel
    {
        public int Код { get; set; }
        public string Пользователь { get; set; }
        [DisplayName("Дата отправки")]
        public System.DateTime Дата_отправки { get; set; }
        public string Содержание { get; set; }
    }
}