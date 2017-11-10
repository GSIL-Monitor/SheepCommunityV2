using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     显示一个群组基本信息的校验器。
    /// </summary>
    public class BasicGroupShowValidator : AbstractValidator<BasicGroupShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BasicGroupShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicGroupShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                 });
        }
    }

    /// <summary>
    ///     根据关联的第三方编号显示一个群组基本信息的校验器。
    /// </summary>
    public class BasicGroupShowByRefIdValidator : AbstractValidator<BasicGroupShowByRefId>
    {
        /// <summary>
        ///     初始化一个新的<see cref="BasicGroupShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public BasicGroupShowByRefIdValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.RefId).NotEmpty().WithMessage(Resources.RefIdRequired);
                                 });
        }
    }
}