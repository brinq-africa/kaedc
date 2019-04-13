using System;
using System.Collections.Generic;

namespace kaedc.Models
{
    public partial class Images
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public sbyte IsDeleted { get; set; }
        public DateTime? Createdat { get; set; }
        public string Createdby { get; set; }
        public DateTime? Updatedat { get; set; }
        public string Updatedby { get; set; }
        public int KaedcUser { get; set; }

        public Kaedcuser KaedcUserNavigation { get; set; }
    }
}
