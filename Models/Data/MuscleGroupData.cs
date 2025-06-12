using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Models.Data
{
    internal class MuscleGroupData
    {
        public string Name { get; set; }
        public List<string> Exercises { get; set; }
        public Image Image { get; set; }

        public MuscleGroupData()
        {
            Exercises = new List<string>();
        }
    }
}
