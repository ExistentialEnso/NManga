using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NManga.Models
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? TimeCreated { get; set; }
        public DateTime? TimeUpdated { get; set; }
    }
}