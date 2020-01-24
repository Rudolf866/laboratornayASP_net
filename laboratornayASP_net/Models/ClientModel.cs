using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace laboratornayASP_net.Models
{
    public class ClientModel
    {
        public int Id { get; set; }
        [RegularExpression(@"^[А-ЯЁ][а-яё]+ [А-ЯЁ][а-яё]+ [А-ЯЁ][а-яё]+$", ErrorMessage = "Некорректные данные")]
        public string FIO { get; set; }
        [Required(ErrorMessage = "Не указан адрес")]
        public string Address { get; set; }
        [RegularExpression(@"^[0-9]{3}-[0-9]{3}-[0-9]{2}-[0-9]{2}$", ErrorMessage = "Некорректный телефон:xxx-xxx-xx-xx")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Не указан Email")]
        public string Email { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage = "Не указан Логин")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Не указан Пароль")]
        public string Password { get; set; }
        [NotMapped]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароль введен неверно")]
        public string ConfirmPassword { get; set; }
        public List<Comment> Comments { get; set; }


    }
}
