using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Models
{
    internal class Exercise
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int MuscleGroupId { get; set; }
        public string Recommendations { get; set; }
        public string VideoPublicId { get; set; } 
    }
}
