using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Subjects.Validators
{
    /// <summary>
    ///     显示一个主题的校验器。
    /// </summary>
    public class SubjectShowValidator : AbstractValidator<SubjectShow>
    {
        /// <summary>
        ///     初始化一个新的<see cref="SubjectShowValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public SubjectShowValidator()
        {
            RuleSet(ApplyTo.Get, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                     RuleFor(x => x.SubjectNumber).NotEmpty().WithMessage(Resources.SubjectNumberRequired);
                                 });
        }
    }
}