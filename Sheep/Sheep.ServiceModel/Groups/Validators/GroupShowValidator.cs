using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     显示一个群组的校验器。
    /// </summary>
    public class GroupShowValidator : AbstractValidator<GroupShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                 });
        }
    }

    /// <summary>
    ///     根据显示名称显示一个群组的校验器。
    /// </summary>
    public class GroupShowByDisplayNameValidator : AbstractValidator<GroupShowByDisplayName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupShowByDisplayNameValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.DisplayName).NotEmpty().WithMessage(Resources.DisplayNameRequired);
                                 });
        }
    }
}