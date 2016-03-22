using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NosTRAILgic.Models
{
    public class Search
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SearchID { get; set; }

        public string Keyword { get; set; }

        public DateTime Date { get; set; }
    }
}