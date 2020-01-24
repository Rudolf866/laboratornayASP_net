using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace laboratornayASP_net.Models
{
    public class Comment
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Не указано имя")]
        public string UserName { get; set; }
        public DateTime CommentDate { get; set; } = DateTime.UtcNow;
        [Required(ErrorMessage = "Не указан текст")]
        public string CommentText { get; set; }
        [HiddenInput]
        public bool Hidden { get; set; }

        public ClientModel ClientModel { get; set; }

    }
}
