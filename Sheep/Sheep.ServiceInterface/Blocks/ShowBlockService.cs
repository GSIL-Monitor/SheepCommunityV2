using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Friendship;
using Sheep.ServiceInterface.Blocks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Blocks;

namespace Sheep.ServiceInterface.Blocks
{
    /// <summary>
    ///     显示一个屏蔽服务接口。
    /// </summary>
    [CompressResponse]
    public class ShowBlockService : Service
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(ShowBlockService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     获取及设置显示一个屏蔽的校验器。
        /// </summary>
        public IValidator<BlockShow> BlockShowValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置屏蔽的存储库。
        /// </summary>
        public IBlockRepository BlockRepo { get; set; }

        #endregion

        #region 显示一个屏蔽

        /// <summary>
        ///     显示一个屏蔽。
        /// </summary>
        //[CacheResponse(Duration = 3600)]
        public async Task<object> Get(BlockShow request)
        {
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    BlockShowValidator.ValidateAndThrow(request, ApplyTo.Get);
            //}
            var blockee = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.BlockeeId.ToString());
            if (blockee == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.BlockeeId));
            }
            var blocker = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.BlockerId.ToString());
            if (blocker == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.BlockerId));
            }
            var existingBlock = await BlockRepo.GetBlockAsync(request.BlockeeId, request.BlockerId);
            if (existingBlock == null)
            {
                throw HttpError.NotFound(string.Format(Resources.BlockNotFound, request.BlockeeId));
            }
            var blockDto = existingBlock.MapToBlockDto(blockee, blocker);
            return new BlockShowResponse
                   {
                       Block = blockDto
                   };
        }

        #endregion
    }
}