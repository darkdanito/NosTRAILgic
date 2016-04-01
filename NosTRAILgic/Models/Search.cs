using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NosTRAILgic.Models
{
    /************************************************************************************
     * Description: This model handle the creation of search DB and storing the         *
     *              information that the user searches in the homepage                  *
     *                                                                                  *
     ************************************************************************************/
    public class Search
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SearchID { get; set; }

        public string Keyword { get; set; }

        public DateTime Date { get; set; }
    }
}