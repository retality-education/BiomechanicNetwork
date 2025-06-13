using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Database.Repositories
{
    using global::BiomechanicNetwork.Services;
    using Npgsql;
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Drawing;

    namespace BiomechanicNetwork.Database.Repositories
    {
        internal class AdminRepository
        {
            private readonly DatabaseHelper _dbHelper;
            private readonly CloudinaryService _cloudinary;

            public AdminRepository()
            {
                _dbHelper = new DatabaseHelper();
                _cloudinary = new CloudinaryService();
            }

            // 1. Управление пользователями
            public DataTable GetAllUsersExceptAdmin(int adminId)
            {
                var query = @"SELECT id, username, name, avatar_public_id, role_id, created_at 
                          FROM users 
                          WHERE id != @adminId
                          ORDER BY created_at DESC";

                return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@adminId", adminId)
                });
            }

            public bool UpdateUserRole(int userId, int roleId)
            {
                var query = "UPDATE users SET role_id = @roleId WHERE id = @userId";
                return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@roleId", roleId)
                }) > 0;
            }

            public DataTable GetAllRoles()
            {
                return _dbHelper.ExecuteQuery("SELECT id, name FROM roles ORDER BY id");
            }

            // 2. Управление предложениями
            public DataTable GetUnresolvedSuggestions()
            {
                var query = @"SELECT s.id, s.topic, s.comment, s.suggestion_date, 
                          s.is_about_users, u.name as user_name, u.id as user_id
                          FROM suggestions s
                          LEFT JOIN users u ON s.user_id = u.id
                          WHERE s.is_resolved = false
                          ORDER BY s.suggestion_date DESC";

                return _dbHelper.ExecuteQuery(query);
            }

            public bool MarkSuggestionAsResolved(int suggestionId, int resolvedBy)
            {
                var query = @"UPDATE suggestions 
                         SET is_resolved = true, 
                             resolved_date = @resolvedDate, 
                             resolved_by = @resolvedBy 
                         WHERE id = @suggestionId";

                return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@suggestionId", suggestionId),
                new NpgsqlParameter("@resolvedBy", resolvedBy),
                new NpgsqlParameter("@resolvedDate", DateTime.UtcNow)
                }) > 0;
            }

            // 3. Управление группами мышц и упражнениями
            public DataTable GetAllMuscleGroups()
            {
                return _dbHelper.ExecuteQuery("SELECT id, name, image_public_id FROM muscle_groups ORDER BY name");
            }

            public bool AddMuscleGroup(string name, string imagePublicId)
            {
                var query = "INSERT INTO muscle_groups (name, image_public_id) VALUES (@name, @imagePublicId)";
                return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@name", name),
                new NpgsqlParameter("@imagePublicId", imagePublicId ?? (object)DBNull.Value)
                }) > 0;
            }

            public bool UpdateMuscleGroup(int id, string name, string imagePublicId)
            {
                var query = @"UPDATE muscle_groups 
                          SET name = @name, 
                              image_public_id = @imagePublicId 
                          WHERE id = @id";

                return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@id", id),
                new NpgsqlParameter("@name", name),
                new NpgsqlParameter("@imagePublicId", imagePublicId ?? (object)DBNull.Value)
                }) > 0;
            }

            public DataTable GetExercisesByMuscleGroup(int muscleGroupId)
            {
                var query = @"SELECT id, name, recommendations, video_public_id 
                          FROM exercises 
                          WHERE muscle_group_id = @muscleGroupId
                          ORDER BY name";

                return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@muscleGroupId", muscleGroupId)
                });
            }

            public bool AddExercise(string name, int? muscleGroupId, string recommendations, string videoPublicId)
            {
                var query = @"INSERT INTO exercises (name, muscle_group_id, recommendations, video_public_id) 
                          VALUES (@name, @muscleGroupId, @recommendations, @videoPublicId)";

                return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@name", name),
                new NpgsqlParameter("@muscleGroupId", muscleGroupId ?? (object)DBNull.Value),
                new NpgsqlParameter("@recommendations", recommendations ?? (object)DBNull.Value),
                new NpgsqlParameter("@videoPublicId", videoPublicId ?? (object)DBNull.Value)
                }) > 0;
            }

            public bool UpdateExercise(int id, string name, int? muscleGroupId, string recommendations, string videoPublicId)
            {
                var query = @"UPDATE exercises 
                          SET name = @name, 
                              muscle_group_id = @muscleGroupId,
                              recommendations = @recommendations,
                              video_public_id = @videoPublicId
                          WHERE id = @id";

                return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@id", id),
                new NpgsqlParameter("@name", name),
                new NpgsqlParameter("@muscleGroupId", muscleGroupId ?? (object)DBNull.Value),
                new NpgsqlParameter("@recommendations", recommendations ?? (object)DBNull.Value),
                new NpgsqlParameter("@videoPublicId", videoPublicId ?? (object)DBNull.Value)
                }) > 0;
            }

            public Image GetImage(string publicId)
            {
                if (string.IsNullOrEmpty(publicId))
                    return null;

                try
                {
                    return _cloudinary.GetImage(publicId);
                }
                catch
                {
                    return null;
                }
            }
        }
    }
}
