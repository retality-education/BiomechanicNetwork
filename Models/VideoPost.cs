using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Models
{
    internal class VideoPost
    {
        public int Id { get; set; }
        public int MuscleId { get; set; }
        public int UserId { get; set; }
        public int ExerciseId { get; set; }
        public DateTime PostDate { get; set; }
        public string Description { get; set; }
        public string VideoPublicId { get; set; } 
    }
}
