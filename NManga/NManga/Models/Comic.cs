using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NManga.Models
{
    public class Comic : BaseEntity
    {
        public string AltText { get; set; }
        public DateTime TimePublished { get; set; }

        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Content { get; set; }
        public int Ordinal { get; set; }
    }
}