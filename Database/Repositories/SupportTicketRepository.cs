using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiomechanicNetwork.Database.Repositories
{
    using Npgsql;
    using System;
    using System.Data;

    namespace BiomechanicNetwork.Database.Repositories
    {
        internal class SupportTicketRepository
        {
            private readonly DatabaseHelper _dbHelper;

            public SupportTicketRepository()
            {
                _dbHelper = new DatabaseHelper();
            }

            public bool CreateSupportTicket(int userId, string topic, string message, bool isAboutUsers = false)
            {
                var query = @"
                INSERT INTO suggestions (user_id, topic, comment, is_about_users, suggestion_date)
                VALUES (@userId, @topic, @message, @isAboutUsers, @date)";

                var parameters = new NpgsqlParameter[]
                {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@topic", topic),
                new NpgsqlParameter("@message", message),
                new NpgsqlParameter("@isAboutUsers", isAboutUsers),
                new NpgsqlParameter("@date", DateTime.UtcNow)
                };

                return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }

            public DataTable GetUserTickets(int userId, int page = 1, int pageSize = 10)
            {
                var query = @"
                SELECT id, topic, comment, suggestion_date, is_resolved, resolved_date
                FROM suggestions
                WHERE user_id = @userId
                ORDER BY suggestion_date DESC
                LIMIT @pageSize OFFSET @offset";

                return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@userId", userId),
                new NpgsqlParameter("@pageSize", pageSize),
                new NpgsqlParameter("@offset", (page - 1) * pageSize)
                });
            }

            public DataTable GetAllTickets(bool onlyUnresolved = false, int page = 1, int pageSize = 20)
            {
                var whereClause = onlyUnresolved ? "WHERE is_resolved = false" : "";

                var query = $@"
                SELECT s.id, s.topic, s.comment, s.suggestion_date, 
                       s.is_resolved, s.resolved_date, u.name as user_name
                FROM suggestions s
                JOIN users u ON s.user_id = u.id
                {whereClause}
                ORDER BY s.suggestion_date DESC
                LIMIT @pageSize OFFSET @offset";

                return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@pageSize", pageSize),
                new NpgsqlParameter("@offset", (page - 1) * pageSize)
                });
            }

            public bool ResolveTicket(int ticketId, int resolvedByUserId)
            {
                var query = @"
                UPDATE suggestions
                SET is_resolved = true,
                    resolved_date = @date,
                    resolved_by = @resolvedBy
                WHERE id = @ticketId";

                var parameters = new NpgsqlParameter[]
                {
                new NpgsqlParameter("@ticketId", ticketId),
                new NpgsqlParameter("@resolvedBy", resolvedByUserId),
                new NpgsqlParameter("@date", DateTime.UtcNow)
                };

                return _dbHelper.ExecuteNonQuery(query, parameters) > 0;
            }

            public int GetTicketCount(int userId = 0, bool onlyUnresolved = false)
            {
                var whereClause = userId == 0
                    ? (onlyUnresolved ? "WHERE is_resolved = false" : "")
                    : (onlyUnresolved ? "WHERE user_id = @userId AND is_resolved = false"
                                      : "WHERE user_id = @userId");

                var query = $"SELECT COUNT(*) FROM suggestions {whereClause}";

                var parameters = userId == 0
                    ? null
                    : new NpgsqlParameter[] { new NpgsqlParameter("@userId", userId) };

                return Convert.ToInt32(_dbHelper.ExecuteScalar(query, parameters));
            }

            public DataTable GetTicketDetails(int ticketId)
            {
                var query = @"
                SELECT s.*, u.name as user_name, 
                       ru.name as resolved_by_name
                FROM suggestions s
                JOIN users u ON s.user_id = u.id
                LEFT JOIN users ru ON s.resolved_by = ru.id
                WHERE s.id = @ticketId";

                return _dbHelper.ExecuteQuery(query, new NpgsqlParameter[]
                {
                new NpgsqlParameter("@ticketId", ticketId)
                });
            }
        }
    }
}
