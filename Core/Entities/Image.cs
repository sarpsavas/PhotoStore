using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Image
    {
        public Guid ListingId { get; set; }
        [Key]
        public string? Url { get; set; }

        public Image()
        {
            
        }
        public Image(Guid listingId, string url)
        {
            ListingId = listingId;
            Url = url;
        }
    }
}
