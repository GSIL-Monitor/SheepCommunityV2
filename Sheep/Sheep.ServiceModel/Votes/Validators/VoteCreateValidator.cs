using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Votes.Validators
{
    /// <summary>
    ///     新建一个投票的校验器。
    /// </summary>
    public class VoteCreateValidator : AbstractValidator<VoteCreate>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "评论"
                                                             };

        /// <summary>
        ///     初始化一个新的<see cref="VoteCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VoteCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ParentType).NotEmpty().WithMessage(Resources.ParentTypeRequired);
                                      RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(Resources.ParentTypeRangeMismatch, ParentTypes.Join(",")).When(x => !x.ParentType.IsNullOrEmpty());
                                      RuleFor(x => x.ParentId).NotEmpty().WithMessage(Resources.ParentIdRequired);
                                  });
        }
    }
}