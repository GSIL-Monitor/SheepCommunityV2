using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.Blocks.Validators
{
    /// <summary>
    ///     新建一个屏蔽的校验器。
    /// </summary>
    public class BlockCreateValidator : AbstractValidator<BlockCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BlockCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BlockCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                  });
        }
    }
}