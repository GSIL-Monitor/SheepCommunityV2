using System.Collections.Generic;
using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.Model.Friendship.Entities;
using Sheep.ServiceInterface.Users.Mappers;
using Sheep.ServiceModel.Blocks.Entities;

namespace Sheep.ServiceInterface.Blocks.Mappers
{
    public static class BlockToBlockDtoMapper
    {
        public static BlockDto MapToBlockDto(this Block block, IUserAuth blockee, IUserAuth blocker)
        {
            if (block.Meta == null)
            {
                block.Meta = new Dictionary<string, string>();
            }
            var blockDto = new BlockDto
                           {
                               Blockee = blockee?.MapToUserDto(),
                               Blocker = blocker?.MapToUserDto(),
                               CreatedDate = block.CreatedDate.ToUnixTime(),
                               ModifiedDate = block.ModifiedDate.ToUnixTime()
                           };
            return blockDto;
        }

        public static BlockOfBlockeeDto MapToBlockOfBlockeeDto(this Block block, IUserAuth blockee)
        {
            if (block.Meta == null)
            {
                block.Meta = new Dictionary<string, string>();
            }
            var blockDto = new BlockOfBlockeeDto
                           {
                               Blockee = blockee?.MapToUserDto(),
                               CreatedDate = block.CreatedDate.ToUnixTime(),
                               ModifiedDate = block.ModifiedDate.ToUnixTime()
                           };
            return blockDto;
        }

        public static BlockOfBlockerDto MapToBlockOfBlockerDto(this Block block, IUserAuth blocker)
        {
            if (block.Meta == null)
            {
                block.Meta = new Dictionary<string, string>();
            }
            var blockDto = new BlockOfBlockerDto
                           {
                               Blocker = blocker?.MapToUserDto(),
                               CreatedDate = block.CreatedDate.ToUnixTime(),
                               ModifiedDate = block.ModifiedDate.ToUnixTime()
                           };
            return blockDto;
        }
    }
}