using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DAL_Demo.Models
{
    public class Post
    {
        public int Id { get; set; }

        [Display(Name = "Destinataire")]
        [StringLength(50), Required]
        public string Sender { get; set; }

        public DateTime CreationDate { get; set; }

        [Display(Name = "Titre")]
        [StringLength(50), Required]
        public string Title { get; set; }

        [Display(Name = "Message")]
        [Required]
        public string Content { get; set; }

        public Post()
        {
            Id = 0;
            Sender = "";
            CreationDate = DateTime.Now;
            Title = "";
            Content = "";
        }
    }
    public class Posts : DAL.RecordsDB<Post>
    {
        public Posts(DAL.DataBase dataBase) : base(dataBase)
        {
            SetCache(true, "SELECT * FROM POSTS ORDER BY CreationDate DESC");
        }
    }
}