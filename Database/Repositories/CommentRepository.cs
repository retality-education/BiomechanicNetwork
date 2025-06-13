using BiomechanicNetwork.Models;
using Npgsql;
using System;
using System.Data;

namespace BiomechanicNetwork.Database.Repositories
{
    internal class CommentRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public CommentRepository()
        {
            _dbHelper = new DatabaseHelper();
        }

        public DataTable GetVideoComments(int videoId)
        {
            string query = @"
                SELECT c.id, c.user_id, u.name as user_name, u.role_id as user_role, c.text, c.created_at 
                FROM video_comments c
                JOIN users u ON u.id = c.user_id
                WHERE c.video_id = @videoId
                ORDER BY c.created_at DESC";

            return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@videoId", videoId)
            });
        }

        public DataTable GetExerciseComments(int exerciseId)
        {
            string query = @"
                SELECT c.id, c.user_id, u.name as user_name, u.role_id as user_role, c.text, c.created_at 
                FROM exercise_comments c
                JOIN users u ON u.id = c.user_id
                WHERE c.exercise_id = @exerciseId
                ORDER BY c.created_at DESC";

            return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@exerciseId", exerciseId)
            });
        }

        public int GetCommentsCount(int contentId, bool isExercise = false)
        {
            string tableName = isExercise ? "exercise_comments" : "video_comments";
            string columnName = isExercise ? "exercise_id" : "video_id";

            string query = $@"
                SELECT COUNT(*) 
                FROM {tableName}
                WHERE {columnName} = @contentId";

            var result = _dbHelper.ExecuteScalar(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@contentId", contentId)
            });

            return Convert.ToInt32(result);
        }

        public bool AddVideoComment(int userId, int videoId, string text)
        {
            string query = @"
                INSERT INTO video_comments (user_id, video_id, text, created_at)
                VALUES (@userId, @videoId, @text, @createdAt)";

            return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@videoId", videoId),
                new NpgsqlParameter("@text", text),
                new NpgsqlParameter("@createdAt", DateTime.UtcNow)
            }) > 0;
        }

        public bool AddExerciseComment(int userId, int exerciseId, string text)
        {
            string query = @"
                INSERT INTO exercise_comments (user_id, exercise_id, text, created_at)
                VALUES (@userId, @exerciseId, @text, @createdAt)";

            return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@exerciseId", exerciseId),
                new NpgsqlParameter("@text", text),
                new NpgsqlParameter("@createdAt", DateTime.UtcNow)
            }) > 0;
        }

        public bool DeleteComment(int commentId, bool isExerciseComment)
        {
            string tableName = isExerciseComment ? "exercise_comments" : "video_comments";
            string query = $"DELETE FROM {tableName} WHERE id = @commentId";

            return _dbHelper.ExecuteNonQuery(query, new NpgsqlParameter[]
            {
                new NpgsqlParameter("@commentId", commentId)
            }) > 0;
        }
    }
}