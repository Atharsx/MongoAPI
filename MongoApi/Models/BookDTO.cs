using System.ComponentModel.DataAnnotations;

namespace MongoApi.Models
{
    public class BookDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
