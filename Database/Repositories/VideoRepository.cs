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

        public DataTable GetFeed( int page = 1, int pageSize = 5)
        {
            var query = @"SELECT v.id, v.description, v.post_date, u.id as user_id, u.name as user_name, 
                  e.name as exercise_name, mg.name as muscle_group_name,
                  v.video_public_id
                  FROM video_posts v 
                  JOIN users u ON v.user_id = u.id 
                  JOIN exercises e ON v.exercise_id = e.id
                  JOIN muscle_groups mg ON e.muscle_group_id = mg.id
                  ORDER BY v.post_date DESC 
                  LIMIT @pageSize OFFSET @offset";

            return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
            {
        new NpgsqlParameter("@pageSize", pageSize),
        new NpgsqlParameter("@offset", (page - 1) * pageSize)
            });
        }

        public DataTable GetUserVideos(int userId, int page = 1, int pageSize = 5)
        {
            var query = @"SELECT v.id, v.description, v.post_date, 
                  e.name as exercise_name, mg.name as muscle_group_name,
                  v.video_public_id
                  FROM video_posts v 
                  JOIN exercises e ON v.exercise_id = e.id
                  JOIN muscle_groups mg ON e.muscle_group_id = mg.id
                  WHERE v.user_id = @userId
                  ORDER BY v.post_date DESC 
                  LIMIT @pageSize OFFSET @offset";

            return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
            {
        new NpgsqlParameter("@userId", userId),
        new NpgsqlParameter("@pageSize", pageSize),
        new NpgsqlParameter("@offset", (page - 1) * pageSize)
            });
        }
        public bool AddVideo(int userId, int exerciseId, string publicId, string description = "")
        {
            var query = @"INSERT INTO video_posts (user_id, exercise_id, video_public_id, description) 
                  VALUES (@userId, @exerciseId, @publicId, @description)";

            var parameters = new NpgsqlParameter[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@exerciseId", exerciseId),
                new NpgsqlParameter("@publicId", publicId),
                new NpgsqlParameter("@description", description ?? "")
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
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

        public bool AddComment(int videoPostId, int userId, string text)
        {
            var query = "INSERT INTO comments (video_post_id, user_id, text) VALUES (@postId, @userId, @text)";
            var parameters = new NpgsqlParameter[]
            {
            new NpgsqlParameter("@postId", videoPostId),
            new NpgsqlParameter("@userId", userId),
            new NpgsqlParameter("@text", text)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable GetComments(int videoPostId)
        {
            var query = @"SELECT c.id, c.text, c.comment_date, u.name as user_name 
                      FROM comments c 
                      JOIN users u ON c.user_id = u.id 
                      WHERE c.video_post_id = @postId 
                      ORDER BY c.comment_date DESC";

            return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
            {
            new NpgsqlParameter("@postId", videoPostId)
            });
        }

        public bool AddLike(int videoPostId, int userId)
        {
            var query = @"INSERT INTO likes (video_post_id, user_id) 
                      VALUES (@postId, @userId)
                      ON CONFLICT (video_post_id, user_id) DO NOTHING";

            var parameters = new NpgsqlParameter[]
            {
            new NpgsqlParameter("@postId", videoPostId),
            new NpgsqlParameter("@userId", userId)
            };

            return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public int GetLikeCount(int videoPostId)
        {
            var query = "SELECT COUNT(*) FROM likes WHERE video_post_id = @postId";
            return Convert.ToInt32(_dbHelper.ExecuteScalar(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@postId", videoPostId)
            }));
        }
    }
}   
