using Dapper;
using MemeSource.Interfaces;
using MemeSource.Models;
using System.Data;

namespace MemeSource.DAL.Repository
{
    public class TwitterRepository: ITwitterRepository
    {
        /// <summary>
        /// The database
        /// </summary>
        private readonly IDatabaseHelper _databaseHelper;

        /// <summary>
        ///
        /// </summary>
        /// <param name="databaseHelper"></param>
        public TwitterRepository(IDatabaseHelper databaseHelper)
        {
            this._databaseHelper = databaseHelper;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="twitter">twitter 實體</param>
        /// <returns></returns>
        public async Task<int> AddAsync(TwitterConfig twitter)
        {
            using (IDbConnection conn = this._databaseHelper.GetConnection())
            {
                string sql = @"
                            INSERT INTO TwitterConfig
                            VALUES (
                                @APIKey,
                                @APIKeySecret,
                                @AccessToken,
                                @AccessTokenSecret
                            );";

                var count = await conn.ExecuteAsync(
                    sql,
                    new
                    {
                        twitter.APIKey,
                        twitter.APIKeySecret,
                        twitter.AccessToken, 
                        twitter.AccessTokenSecret
                    });

                return count;
            }
        }

        /// <summary>
        /// 查詢
        /// </summary>
        /// <param name="twitterQuery">查詢條件</param>
        /// <returns></returns>
        public async Task<IEnumerable<TwitterConfig>> GetAsync(TwitterConfig twitterQuery)
        {
            using (IDbConnection conn = this._databaseHelper.GetConnection())
            {
                string sql = @"
                            SELECT
                                APIKey,
                                APIKeySecret,
                                AccessToken, 
                                AccessTokenSecret
                            FROM TwitterConfig
                            WHERE
                                APIKey = @APIKey"
                ;

                var twitters = await conn.QueryAsync<TwitterConfig>(
                    sql,
                    new
                    {
                        twitterQuery.APIKey
                    });

                return twitters;
            }
        }

        /// <summary>
        /// 刪除
        /// </summary>
        /// <param name="twitter">twitter 實體</param>
        /// <returns></returns>
        public async Task<int> RemoveAsync(TwitterConfig twitter)
        {
            using (IDbConnection conn = this._databaseHelper.GetConnection())
            {
                string sql = @"DELETE FROM TwitterConfig 
                               WHERE APIKey = @APIKey";

                var count = await conn.ExecuteAsync(
                    sql,
                    new
                    {
                        twitter.APIKey
                    });

                return count;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="twitter">twitter 實體</param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(TwitterConfig twitter)
        {
            using (IDbConnection conn = this._databaseHelper.GetConnection())
            {
                string sql = @"
                            UPDATE TwitterConfig
                            SET APIKey = @APIKey,
                                APIKeySecret = @APIKeySecret,
                                AccessToken = @AccessToken,
                                AccessTokenSecret = @AccessTokenSecret
                            WHERE Id = @id ";

                var count = await conn.ExecuteAsync(
                    sql,
                    new
                    {
                        twitter.APIKey,
                        twitter.APIKeySecret,
                        twitter.AccessToken,
                        twitter.AccessTokenSecret
                    });

                return count;
            }
        }
    }
}
