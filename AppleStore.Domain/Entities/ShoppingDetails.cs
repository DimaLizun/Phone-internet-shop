using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AppleStore.Domain.Entities
{
    public class ShoppingDetails
    {
        [Required(ErrorMessage = "Введите имя")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Введите главный адресс")]
        [Display(Name = "Первый адрес")]
        public string MainAddress { get; set; }

        [Display(Name = "Второй адрес")]
        public string SecondAddress { get; set; }

        [Display(Name = "Третий адрес")]
        public string ThirdAddress { get; set; }

        [Required(ErrorMessage = "Введите город")]
        [Display(Name = "Город")]
        public string Сity { get; set; }

        [Required(ErrorMessage = "Введите страну")]
        [Display(Name = "Укажите страну")]
        public string Country{ get; set; }

        public bool Options { get; set; }
    }
}
