using System.Linq;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using ServiceStack.Text;
using Sheep.Common.Auth;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Blocks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Blocks;

namespace Sheep.ServiceInterface.Blocks
{
    /// <summary>
    ///     列举一组被屏蔽者的屏蔽信息服务接口。
    /// </summary>
    [CompressResponse]
    public class ListBlockOfBlockeeService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ListBlockOfBlockeeService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置列举一组被屏蔽者的校验器。
        /// </summary>
        public IValidator<BlockListOfBlockee> BlockListOfBlockeeValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置屏蔽的存储库。
        /// </summary>
        public IBlockRepository BlockRepo { get; set; }

        #endregion

        #region 列举一组被屏蔽者

        /// <summary>
        ///     列举一组被屏蔽者。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(BlockListOfBlockee request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    BlockListOfBlockeeValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var existingBlocks = await BlockRepo.FindBlocksByBlockerAsync(request.BlockerId, request.CreatedSince?.FromUnixTime(), request.ModifiedSince?.FromUnixTime(), request.OrderBy, request.Descending, request.Skip, request.Limit);
            if (existingBlocks == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BlocksNotFound));
            }
            var blockeesMap = (await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthsAsync(existingBlocks.Select(block => block.BlockeeId.ToString()).Distinct().ToList())).ToDictionary(userAuth => userAuth.Id, userAuth => userAuth);
            var blocksDto = existingBlocks.Select(block => block.MapToBlockOfBlockeeDto(blockeesMap.GetValueOrDefault(block.BlockeeId))).ToList();
            return new BlockListOfBlockeeResponse
                   {
                       Blocks = blocksDto
                   };
        }

        #endregion
    }
}