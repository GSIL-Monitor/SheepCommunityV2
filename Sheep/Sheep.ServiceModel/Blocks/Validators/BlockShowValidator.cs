using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.Blocks.Validators
{
    /// <summary>
    ///     显示一个屏蔽的校验器。
    /// </summary>
    public class BlockShowValidator : AbstractValidator<BlockShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BlockShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BlockShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                 });
        }
    }
}