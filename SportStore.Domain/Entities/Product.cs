using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SportStore.Domain.Entities
{
   public class Product
    {
        [HiddenInput(DisplayValue =false)]
        public int ProductID { get; set; }
        //[Required(ErrorMessage ="Please enter a product name")]
        public string Name { get; set; }
        [DataType(DataType.MultilineText)/*,Required(ErrorMessage ="please enter a description")*/]
        public string Descrpition { get; set; }
        [Required,Range(0.01,double.MaxValue,ErrorMessage ="Please enter a posivite price")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "Please specify a category")]
        public string Category { get; set; }
        [Column(TypeName ="varbinary")]
        public byte[] ImageData { get; set; }
        [Column(TypeName ="varchar"),MaxLength(50)] 
        public string ImageMimeType { get; set; }
    }
}
