using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.ServiceModel.Blocks.Validators
{
    /// <summary>
    ///     取消一个屏蔽的校验器。
    /// </summary>
    public class BlockDeleteValidator : AbstractValidator<BlockDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BlockDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BlockDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                    });
        }
    }
}