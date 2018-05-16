using System.Collections.Generic;
using System.Threading.Tasks;
using Netease.Nim;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.Logging;
using Sheep.Common.Auth;
using Sheep.Model.Friendship;
using Sheep.Model.Friendship.Entities;
using Sheep.ServiceInterface.Blocks.Mappers;
using Sheep.ServiceInterface.Properties;
using Sheep.ServiceModel.Blocks;

namespace Sheep.ServiceInterface.Blocks
{
    /// <summary>
    ///     新建一个屏蔽服务接口。
    /// </summary>
    public class CreateBlockService : ChangeBlockService
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(CreateBlockService));

        #endregion

        #region 属性 

        /// <summary>
        ///     获取及设置相关的应用程序设置器。
        /// </summary>
        public IAppSettings AppSettings { get; set; }

        /// <summary>
        ///     网易云通信服务客户端。
        /// </summary>
        public INimClient NimClient { get; set; }

        /// <summary>
        ///     获取及设置新建一个屏蔽的校验器。
        /// </summary>
        public IValidator<BlockCreate> BlockCreateValidator { get; set; }

        /// <summary>
        ///     获取及设置用户身份的存储库。
        /// </summary>
        public IUserAuthRepository AuthRepo { get; set; }

        /// <summary>
        ///     获取及设置屏蔽的存储库。
        /// </summary>
        public IBlockRepository BlockRepo { get; set; }

        #endregion

        #region 新建一个屏蔽

        /// <summary>
        ///     新建一个屏蔽。
        /// </summary>
        public async Task<object> Post(BlockCreate request)
        {
            if (!IsAuthenticated)
            {
                throw HttpError.Unauthorized(Resources.LoginRequired);
            }
            //if (HostContext.GlobalRequestFilters == null || !HostContext.GlobalRequestFilters.Contains(ValidationFilters.RequestFilter))
            //{
            //    BlockCreateValidator.ValidateAndThrow(request, ApplyTo.Post);
            //}
            var blockee = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(request.BlockeeId.ToString());
            if (blockee == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, request.BlockeeId));
            }
            var blockerId = GetSession().UserAuthId.ToInt(0);
            var blocker = await ((IUserAuthRepositoryExtended) AuthRepo).GetUserAuthAsync(blockerId.ToString());
            if (blocker == null)
            {
                throw HttpError.NotFound(string.Format(Resources.UserNotFound, blockerId));
            }
            var existingBlock = await BlockRepo.GetBlockAsync(request.BlockeeId, blockerId);
            if (existingBlock != null)
            {
                return new BlockCreateResponse
                       {
                           Block = existingBlock.MapToBlockDto(blockee, blocker)
                       };
            }
            var newBlock = new Block
                           {
                               Meta = new Dictionary<string, string>(),
                               BlockeeId = request.BlockeeId,
                               BlockerId = blockerId
                           };
            var block = await BlockRepo.CreateBlockAsync(newBlock);
            ResetCache(block);
            await NimClient.PostAsync(new UserSetSpecialRelationRequest
                                      {
                                          AccountId = blockerId.ToString(),
                                          TargetAccountId = request.BlockeeId.ToString(),
                                          Type = 1,
                                          Value = 1
                                      });
            return new BlockCreateResponse
                   {
                       Block = block.MapToBlockDto(blockee, blocker)
                   };
        }

        #endregion
    }
}