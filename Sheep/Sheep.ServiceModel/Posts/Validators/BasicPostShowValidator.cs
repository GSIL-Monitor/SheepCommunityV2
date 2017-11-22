using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Posts.Validators
{
    /// <summary>
    ///     显示一个帖子基本信息的校验器。
    /// </summary>
    public class BasicPostShowValidator : AbstractValidator<BasicPostShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BasicPostShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicPostShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.PostId).NotEmpty().WithMessage(Resources.PostIdRequired);
                                 });
        }
    }
}