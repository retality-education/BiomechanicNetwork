using BiomechanicNetwork.Models;
using BiomechanicNetwork.Services;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Database.Repositories
{
    internal class MuscleGroupRepository
    {
        private readonly DatabaseHelper _dbHelper;
        private readonly CloudinaryService _cloudinary;

        public MuscleGroupRepository()
        {
            _dbHelper = new DatabaseHelper();
            _cloudinary = new CloudinaryService();
        }

        public List<MuscleGroupWithExercises> GetAllWithExercises(bool excludeOtherGroup = false)
        {
            var result = new List<MuscleGroupWithExercises>();

            string s = "";
            if (!excludeOtherGroup)
                s = "WHERE mg.name <> 'Другое'";
            var query = $@"SELECT mg.id, mg.name, mg.image_public_id, 
                         e.id as exercise_id, e.name as exercise_name, e.video_public_id as video_public_id
                         FROM muscle_groups mg
                         LEFT JOIN exercises e ON e.muscle_group_id = mg.id
                         {s}
                         ORDER BY mg.name, e.name";

            var parameters = new NpgsqlParameter[]
            {
                 new NpgsqlParameter("@excludeOther", excludeOtherGroup),
            };

            var dataTable = _dbHelper.ExecuteQuery(query, parameters);

            MuscleGroupWithExercises currentGroup = null;
            foreach (DataRow row in dataTable.Rows)
            {
                var groupId = Convert.ToInt32(row["id"]);
                var groupName = row["name"].ToString();

                if (currentGroup == null || currentGroup.Id != groupId)
                {
                    currentGroup = new MuscleGroupWithExercises
                    {
                        Id = groupId,
                        Name = groupName,
                        Image = GetGroupImage(row["image_public_id"].ToString()),
                        Exercises = new List<Exercise>()
                    };
                    result.Add(currentGroup);
                }

                if (!row.IsNull("exercise_id"))
                {
                    currentGroup.Exercises.Add(new Exercise
                    {
                        Id = Convert.ToInt32(row["exercise_id"]),
                        Name = row["exercise_name"].ToString(),
                        VideoPublicId = row["video_public_id"].ToString()
                    });
                }
            }

            return result;
        }

        public Image GetGroupImage(string publicId)
        {
            if (string.IsNullOrEmpty(publicId))
                return Properties.Resources.cancel;

            try
            {
                return _cloudinary.GetImage(publicId);
            }
            catch
            {
                return Properties.Resources.cancel;
            }
        }
    }

    internal class MuscleGroupWithExercises
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Image Image { get; set; }
        public List<Exercise> Exercises { get; set; }
    }
}