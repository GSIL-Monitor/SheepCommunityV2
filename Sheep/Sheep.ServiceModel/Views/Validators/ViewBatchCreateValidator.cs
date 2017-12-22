using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Views.Validators
{
    /// <summary>
    ///     新建一组查看的校验器。
    /// </summary>
    public class ViewBatchCreateValidator : AbstractValidator<ViewBatchCreate>
    {
        public static readonly HashSet<string> ParentTypes = new HashSet<string>
                                                             {
                                                                 "帖子",
                                                                 "章",
                                                                 "节"
                                                             };

        /// <summary>
        ///     初始化一组新的<see cref="ViewBatchCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public ViewBatchCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.ParentType).NotEmpty().WithMessage(Resources.ParentTypeRequired);
                                      RuleFor(x => x.ParentType).Must(contentType => ParentTypes.Contains(contentType)).WithMessage(Resources.ParentTypeRangeMismatch, ParentTypes.Join(",")).When(x => !x.ParentType.IsNullOrEmpty());
                                      RuleFor(x => x.ParentIds).NotEmpty().WithMessage(Resources.ParentIdsRequired);
                                  });
        }
    }
}