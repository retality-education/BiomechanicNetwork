using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Models
{
    internal class Suggestion
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Topic { get; set; }
        public string Comment { get; set; }
        public DateTime SuggestionDate { get; set; }
        public bool IsAboutUsers { get; set; }
        public bool IsClosed { get; set; }
    }
}
