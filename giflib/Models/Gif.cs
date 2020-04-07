using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace giflib.Models
{
    public class Gif
    {
        public Gif()
        {
            Favorite = false;
            DateUploaded = DateTime.Now;

            Favorites = new List<Favorite>();
        }
        public int Id { get; set; }
        public byte[] Bytes { get; set; }
        [Required]
        public string Description { get; set; }
        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public DateTime DateUploaded { get; set; }
        public string Username { get; set; }
        public bool Favorite { get; set; }
        public string Hash { get; set; }
        public string UserId { get; set; }

        public Category Category { get; set; }
        public User User { get; set; }
        public ICollection<Favorite> Favorites { get; set; }

        public string getTimeSinceUploaded()
        {
            string unit;
            DateTime now = DateTime.Now;
            double diff;


            if ((diff = (now - DateUploaded).TotalSeconds) < 60)
            {
                unit = "secs ago ";
            }
            else if ((diff = (now - DateUploaded).TotalMinutes) < 60)
            {
                unit = "mins ago ";
            }
            else if ((diff = (now - DateUploaded).TotalHours) < 24)
            {
                unit = "hours ago ";
            }
            else if ((diff = (now - DateUploaded).TotalDays) < 30) {
                unit = "days ago ";
            }
            //else if(diff = (DateUploaded - now).TotalM) < 24)
            //{
            //    unit = "months";
            //}
            else
            {
                return "";
            }

            return String.Format("{0:0} {1}", diff, unit);
        }

    

        public void addCategory(Category category)
        {
            Category = category;
        }

    }
}