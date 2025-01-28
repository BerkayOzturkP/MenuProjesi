using Microsoft.AspNetCore.Mvc;

namespace MenuProjesi.Models
{
    public class Categories 
    {
        public int Id { get; set; }

        public string CategoryName { get; set; }

        public int? ProductCount { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
