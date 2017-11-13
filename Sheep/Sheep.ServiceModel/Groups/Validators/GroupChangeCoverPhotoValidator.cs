using System;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     更改封面图片的校验器。
    /// </summary>
    public class GroupChangeCoverPhotoValidator : AbstractValidator<GroupChangeCoverPhoto>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupChangeCoverPhotoValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupChangeCoverPhotoValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.GroupId).NotEmpty().WithMessage(Resources.GroupIdRequired);
                                     RuleFor(x => x.SourceCoverPhotoUrl).Must(url => url.GetImageUrlExtension().IsImageExtension()).WithMessage(Resources.SourceCoverPhotoUrlMismatch).When(x => !x.SourceCoverPhotoUrl.IsNullOrEmpty());
                                 });
        }
    }
}