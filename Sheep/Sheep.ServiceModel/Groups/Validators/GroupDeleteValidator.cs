using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     删除一个群组的校验器。
    /// </summary>
    public class GroupDeleteValidator : AbstractValidator<GroupDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                    });
        }
    }
}