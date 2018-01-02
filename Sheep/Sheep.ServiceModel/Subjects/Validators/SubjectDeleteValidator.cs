using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Subjects.Validators
{
    /// <summary>
    ///     删除一个主题的校验器。
    /// </summary>
    public class SubjectDeleteValidator : AbstractValidator<SubjectDelete>
    {
        /// <summary>
        ///     初始化一个新的<see cref="SubjectDeleteValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public SubjectDeleteValidator()
        {
            RuleSet(ApplyTo.Delete, () =>
                                    {
                                        RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                        RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                        RuleFor(x => x.SubjectNumber).NotEmpty().WithMessage(x => string.Format(Resources.SubjectNumberRequired));
                                    });
        }
    }
}