using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Web;

namespace TestProjectManagerApp.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        [MaxLength(2000)]
        public string Description { get; set; }
        public string Organization { get; set; }
        [DataType(DataType.Date)]
        public DateTime Start { get; set; }
        [DataType(DataType.Date)]
        public DateTime End { get; set; }
        [Required]
        public string Role { get; set; }
        [DataType(DataType.Url)]
        public string Link { get; set; }
        public string Skills { get; set; }
        public virtual ICollection<File> Attachments { get; set; }
        [Required]
        public Types Type { get; set; }
        [Required]
        public DateTime Created { get; set; }
        [Required]
        public DateTime Updated { get; set; }
    }

    public enum Types
    {
        Work,
        Book,
        Cource,
        Blog,
        Other
    }
}