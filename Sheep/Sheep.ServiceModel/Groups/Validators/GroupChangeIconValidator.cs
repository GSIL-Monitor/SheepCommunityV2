using System;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     更改图标的校验器。
    /// </summary>
    public class GroupChangeIconValidator : AbstractValidator<GroupChangeIcon>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupChangeIconValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupChangeIconValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                     RuleFor(x => x.SourceIconUrl).Must(url => url.GetImageUrlExtension().IsImageExtension()).WithMessage(Resources.SourceIconUrlMismatch).When(x => !x.SourceIconUrl.IsNullOrEmpty());
                                 });
        }
    }
}