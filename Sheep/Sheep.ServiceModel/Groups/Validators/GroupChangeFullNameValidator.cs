using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     更改真实姓名的校验器。
    /// </summary>
    public class GroupChangeFullNameValidator : AbstractValidator<GroupChangeFullName>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupChangeFullNameValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupChangeFullNameValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                     RuleFor(x => x.FullName).Length(2, 64).WithMessage(Resources.FullNameLengthMismatch, 2, 64).When(x => !x.FullName.IsNullOrEmpty());
                                     RuleFor(x => x.SourceIdImageUrl).Must(url => url.GetImageUrlExtension().IsImageExtension()).WithMessage(Resources.SourceIdImageUrlMismatch).When(x => !x.SourceIdImageUrl.IsNullOrEmpty());
                                 });
        }
    }
}