using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BD.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }


        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }


        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        
        
        [Column(TypeName = "int")]
        public int Duration { get; set; }


        [Column(TypeName = "varchar(30)")]
        public string Author { get; set; }


        [Column(TypeName = "TEXT")]
        public string Description { get; set; }
        
        
        [Column(TypeName = "varchar(30)")]
        public string Category { get; set; }
        
        
        [Column(TypeName = "int")]
        public double Rate { get; set; }

        
        [Column(TypeName = "int")]
        public int VotesNum { get; set; }
    }
}
