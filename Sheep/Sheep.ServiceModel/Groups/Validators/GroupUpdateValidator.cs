using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     更新群组的校验器。
    /// </summary>
    public class GroupUpdateValidator : AbstractValidator<GroupUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                     RuleFor(x => x.DisplayName).NotEmpty().WithMessage(Resources.DisplayNameRequired);
                                 });
        }
    }
}