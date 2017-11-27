using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Likes.Validators
{
    /// <summary>
    ///     取消一个点赞的校验器。
    /// </summary>
    public class LikeDeleteValidator : AbstractValidator<LikeDelete>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "帖子"
                                                             };

        /// <summary>
        ///     初始化一个新的<see cref="LikeDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public LikeDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.ParentType).NotEmpty().WithMessage(Resources.ParentTypeRequired);
                                        RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(Resources.ParentTypeRangeMismatch, ParentTypes.Join(","));
                                        RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                    });
        }
    }
}