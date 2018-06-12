using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace ImageServiceWeb.Models
{
    /// <summary>
    /// this is our student object so we can show our student data in a nice bootstrap looking table on home page
    /// </summary>
    [Serializable]
    [XmlRoot("Students"), XmlType("Students")]
    public class Student
    {
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "ID")]
        public int ID { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "FirstName")]
        public string FirstName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LastName")]
        public string LastName { get; set; }
    }
}