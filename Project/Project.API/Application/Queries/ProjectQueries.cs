using System;
using System.Threading.Tasks;
using Dapper;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Project.Infrastructure;

namespace Project.API.Application.Queries
{
    public class ProjectQueries : IProjectQueries
    {
        private readonly ProjectContext _projectContext;

        public ProjectQueries(ProjectContext projectContext)
        {
            _projectContext = projectContext;
        }
        public async Task<dynamic> GetProjectByUserIdAsync(int userId)
        {
            using (var conn = _projectContext.Database.GetDbConnection())
            {
                string sql = @"SELECT a.*
                               FROM Projects as a
                               WHERE a.UserId = @userId";
                var result = await conn.QueryAsync(sql, new { userId });
                return result;
            }
        }

        public async Task<dynamic> GetProjectDetailAsync(int projectId)
        {
            using (var conn = _projectContext.Database.GetDbConnection())
            {
                conn.Open();
                string sql = @"select * from Projects a 
                               inner join ProjectVisibleRules b
                               on a.Id=b.ProjectId
                               where a.id=@projectId";
                var result = await conn.QueryAsync(sql, new { projectId });
                return result;
            }
        }
    }
}