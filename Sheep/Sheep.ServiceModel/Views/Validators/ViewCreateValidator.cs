using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Views.Validators
{
    /// <summary>
    ///     新建一个查看的校验器。
    /// </summary>
    public class ViewCreateValidator : AbstractValidator<ViewCreate>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "帖子",
                                                                 "章",
                                                                 "节"
                                                             };

        /// <summary>
        ///     初始化一个新的<see cref="ViewCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewCreateValidator()
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