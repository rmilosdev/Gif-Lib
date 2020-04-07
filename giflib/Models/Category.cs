using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace giflib.Models
{
    public class Category
    {
        public Category() {

            Gifs = new List<Gif>();

        }
        public int Id { get; set; }
        [Required, MinLength(3), MaxLength(16)]
        public string Name { get; set; }
        [Required]
        public string ColorCode { get; set; }

        public ICollection<Gif> Gifs;


        public void AddGif(int gifId)
        {
            Gifs.Add(new Gif()
            {
                Id = gifId,
            });
        }

    }
}