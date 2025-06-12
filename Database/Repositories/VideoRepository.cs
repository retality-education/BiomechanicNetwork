using CloudinaryDotNet.Actions;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Database.Repositories
{
    internal class VideoRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public VideoRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public DataTable GetFeed(int userId, int page = 1, int pageSize = 5)
        {
            var query = @"SELECT v.id, v.description, v.post_date, u.id as user_id, u.name as user_name, 
                  e.name as exercise_name, mg.name as muscle_group_name,
                  v.video_public_id,
                  EXISTS(SELECT 1 FROM video_views vv WHERE vv.video_id = v.id AND vv.user_id = @userId) as is_viewed,
                  EXISTS(SELECT 1 FROM video_likes vl WHERE vl.video_id = v.id AND vl.user_id = @userId) as is_liked
                  FROM video_posts v 
                  JOIN users u ON v.user_id = u.id 
                  JOIN exercises e ON v.exercise_id = e.id
                  JOIN muscle_groups mg ON e.muscle_group_id = mg.id
                  ORDER BY v.post_date DESC 
                  LIMIT @pageSize OFFSET @offset";

            return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@pageSize", pageSize),
                new NpgsqlParameter("@offset", (page - 1) * pageSize)
            });
        }

        public DataTable GetUserVideos(int userId, int currentUserId, int page = 1, int pageSize = 5)
        {
            var query = @"SELECT v.id, v.description, v.post_date, 
                  e.name as exercise_name, mg.name as muscle_group_name,
                  v.video_public_id,
                  EXISTS(SELECT 1 FROM video_views vv WHERE vv.video_id = v.id AND vv.user_id = @currentUserId) as is_viewed,
                  EXISTS(SELECT 1 FROM video_likes vl WHERE vl.video_id = v.id AND vl.user_id = @currentUserId) as is_liked
                  FROM video_posts v 
                  JOIN exercises e ON v.exercise_id = e.id
                  JOIN muscle_groups mg ON e.muscle_group_id = mg.id
                  WHERE v.user_id = @userId
                  ORDER BY v.post_date DESC 
                  LIMIT @pageSize OFFSET @offset";

            return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@currentUserId", currentUserId),
                new NpgsqlParameter("@pageSize", pageSize),
                new NpgsqlParameter("@offset", (page - 1) * pageSize)
            });
        }

        public bool AddVideo(int userId, int exerciseId, string publicId, string description = "")
        {
            var time = DateTime.Now;

            var query = @"INSERT INTO video_posts (user_id, exercise_id, video_public_id, description, post_date) 
                  VALUES (@userId, @exerciseId, @publicId, @description, @time)";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@exerciseId", exerciseId),
                new NpgsqlParameter("@publicId", publicId),
                new NpgsqlParameter("@description", description ?? ""),
                new NpgsqlParameter("@time", time)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }
        public int GetCommentsCount(int contentId, bool isExercise = false)
        {
            string tableName = isExercise ? "exercise_comments" : "video_comments";
            string columnName = isExercise ? "exercise_id" : "video_id";

            string query = $@" SELECT COUNT(*) FROM {tableName} WHERE {columnName} = @contentId";

            var parameters = new NpgsqlParameter[]
           {
                new NpgsqlParameter("@contentId", contentId),
           };
            var result = _dbHelper.ExecuteScalar(query, parameters);
            return Convert.ToInt32(result);
        }
        public int GetVideoCount(int userId = 0)
        {
            var query = userId == 0
                ? "SELECT COUNT(*) FROM video_posts"
                : "SELECT COUNT(*) FROM video_posts WHERE user_id = @userId";

            var parameters = userId == 0
                ? null
                : new NpgsqlParameter[] { new NpgsqlParameter("@userId", userId) };

            return Convert.ToInt32(_dbHelper.ExecuteScalar(query, parameters));
        }

        public bool AddView(int videoId, int userId)
        {
            // Добавляем просмотр только если его еще нет
            var query = @"INSERT INTO video_views (video_id, user_id, view_date)
                      VALUES (@videoId, @userId, @viewDate)
                      ON CONFLICT (video_id, user_id) DO NOTHING";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@videoId", videoId),
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@viewDate", DateTime.UtcNow)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public int GetViewsCount(int videoId)
        {
            var query = "SELECT COUNT(*) FROM video_views WHERE video_id = @videoId";
            return Convert.ToInt32(_dbHelper.ExecuteScalar(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@videoId", videoId)
            }));
        }

        public bool ToggleLike(int videoId, int userId)
        {
            // Проверяем, есть ли уже лайк
            var checkQuery = "SELECT COUNT(*) FROM video_likes WHERE video_id = @videoId AND user_id = @userId";
            var hasLike = Convert.ToInt32(_dbHelper.ExecuteScalar(checkQuery, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@videoId", videoId),
                new NpgsqlParameter("@userId", userId)
            })) > 0;

            if (hasLike)
            {
                // Удаляем лайк
                var deleteQuery = "DELETE FROM video_likes WHERE video_id = @videoId AND user_id = @userId";
                return _dbHelper.ExecuteNonQuery(deleteQuery, new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@videoId", videoId),
                    new NpgsqlParameter("@userId", userId)
                }) > 0;
            }
            else
            {
                // Добавляем лайк
                var insertQuery = "INSERT INTO video_likes (video_id, user_id, like_date) VALUES (@videoId, @userId, @likeDate)";
                return _dbHelper.ExecuteNonQuery(insertQuery, new NpgsqlParameter[]
                {
                    new NpgsqlParameter("@videoId", videoId),
                    new NpgsqlParameter("@userId", userId),
                    new NpgsqlParameter("@likeDate", DateTime.UtcNow)
                }) > 0;
            }
        }

        public int GetLikeCount(int videoId)
        {
            var query = "SELECT COUNT(*) FROM video_likes WHERE video_id = @videoId";
            return Convert.ToInt32(_dbHelper.ExecuteScalar(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@videoId", videoId)
            }));
        }

        public bool IsLiked(int videoId, int userId)
        {
            var query = "SELECT COUNT(*) FROM video_likes WHERE video_id = @videoId AND user_id = @userId";
            return Convert.ToInt32(_dbHelper.ExecuteScalar(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@videoId", videoId),
                new NpgsqlParameter("@userId", userId)
            })) > 0;
        }

        public bool IsViewed(int videoId, int userId)
        {
            var query = "SELECT COUNT(*) FROM video_views WHERE video_id = @videoId AND user_id = @userId";
            return Convert.ToInt32(_dbHelper.ExecuteScalar(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@videoId", videoId),
                new NpgsqlParameter("@userId", userId)
            })) > 0;
        }
    }
}