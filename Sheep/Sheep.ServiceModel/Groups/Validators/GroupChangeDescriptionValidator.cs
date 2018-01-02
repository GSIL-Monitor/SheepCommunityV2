using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     更改简介的校验器。
    /// </summary>
    public class GroupChangeDescriptionValidator : AbstractValidator<GroupChangeDescription>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupChangeDescriptionValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupChangeDescriptionValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(x => string.Format(Resources.GroupIdRequired));
                                     RuleFor(x => x.Description).Length(4, 8192).WithMessage(x => string.Format(Resources.DescriptionLengthMismatch, 4, 8192)).When(x => !x.Description.IsNullOrEmpty());
                                 });
        }
    }
}