using BiomechanicNetwork.Models;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Database.Repositories
{
    internal class ExerciseRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public ExerciseRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public DataTable GetMuscleGroups()
        {
            return _dbHelper.ExecuteQuery("SELECT id, name, image_public_id FROM muscle_groups ORDER BY name");
        }
        
        public DataTable GetExercises(int page = 1, int pageSize = 4)
        {
            var query = @"SELECT e.id, e.name, e.recommendations, m.name as muscle_group, e.video_public_id 
                      FROM exercises e 
                      JOIN muscle_groups m ON e.muscle_group_id = m.id 
                      ORDER BY m.name, e.name
                      LIMIT @pageSize OFFSET @offset";

            return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@pageSize", pageSize),
                new NpgsqlParameter("@offset", (page - 1) * pageSize)
            });
        }

        public bool AddExercise(Exercise exercise)
        {
            var query = @"INSERT INTO exercises (name, muscle_group_id, recommendations, video_public_id) 
                      VALUES (@name, @muscleGroupId, @recommendations, @videoPublicId)";

            var parameters = new NpgsqlParameter[]
            {
            new NpgsqlParameter("@name", exercise.Name),
            new NpgsqlParameter("@muscleGroupId", exercise.MuscleGroupId),
            new NpgsqlParameter("@recommendations", exercise.Recommendations),
            new NpgsqlParameter("@videoPublicId", exercise.VideoPublicId)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public bool DeleteExercise(int exerciseId)
        {
            // Переносим видео в "Другое" перед удалением
            var updateQuery = @"UPDATE video_posts 
                           SET exercise_id = (SELECT id FROM exercises WHERE name = 'Другое' LIMIT 1)
                           WHERE exercise_id = @exerciseId";

            _dbHelper.ExecuteNonQuery(updateQuery, new NpgsqlParameter[]
            {
            new NpgsqlParameter("@exerciseId", exerciseId)
            });

            // Удаляем упражнение
            return _dbHelper.ExecuteNonQuery(
                "DELETE FROM exercises WHERE id = @exerciseId",
                new NpgsqlParameter[] { new NpgsqlParameter("@exerciseId", exerciseId) }) > 0;
        }
    }
}
