using System.ComponentModel.DataAnnotations.Schema;

namespace EFC_Course.Entities
{
    
        [Table("tblUsers")]
        public class User
        {
            public int UserId { get; set; }
            public string Username { get; set; }
        }
    
}
