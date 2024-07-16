using MemeSource.Models;

namespace MemeSource.Interfaces;

public interface ITwitterRepository
{
    /// <summary>
    /// 新增
    /// </summary>
    /// <param name="twitter">twitter 實體</param>
    Task<int> AddAsync(TwitterConfig twitter);

    /// <summary>
    /// 查詢
    /// </summary>
    /// <param name="twitterQuery">查詢條件</param>
    Task<IEnumerable<TwitterConfig>> GetAsync(TwitterConfig twitterQuery);

    /// <summary>
    /// 刪除
    /// </summary>
    /// <param name="twitter">twitter 實體</param>
    Task<int> RemoveAsync(TwitterConfig twitter);

    /// <summary>
    /// 更新
    /// </summary>
    /// <param name="twitter">twitter 實體</param>
    Task<int> UpdateAsync(TwitterConfig twitter);
}
