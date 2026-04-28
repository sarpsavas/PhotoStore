using Core.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Listing
    {
        public Guid ListingId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string ListingName { get; set; }
        public string? ListingDescription { get; set; }
        public string Address { get; set; }
        public DateTime ListingDate { get; set; }
        public List<string> ImageUrls { get; set; }
        public decimal Price { get; set; }
        public string Contact { get; set; }
        public ListingCategories Category { get; set; }
    }
}
