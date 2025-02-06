using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;


namespace BookCatalogue
{
    public class Book
    {
        public string Title { get; set; }
        public int Pages { get; set; }
        public string Publisher { get; set; }
        public int ReleaseYear { get; set; }
        public string Summary { get; set; }
        public string ISBN { get; set; }
    }
}
