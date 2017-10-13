namespace Sheep.Common.Infrastructure
{
    /// <summary>
    ///     分页列表的接口。
    /// </summary>
    public interface IPaged
    {
        /// <summary>
        ///     当前分页号。
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        ///     单页行数。
        /// </summary>
        int PageSize { get; }

        /// <summary>
        ///     总行数。
        /// </summary>
        long TotalCount { get; }
    }
}