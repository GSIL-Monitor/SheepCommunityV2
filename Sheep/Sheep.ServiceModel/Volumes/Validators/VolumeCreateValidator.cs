using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     创建一个卷的校验器。
    /// </summary>
    public class VolumeCreateValidator : AbstractValidator<VolumeCreate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VolumeCreateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeCreateValidator()
        {
            RuleSet(ApplyTo.Post, () =>
                                  {
                                      RuleFor(x => x.BookId).NotEmpty().WithMessage(Resources.BookIdRequired);
                                      RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(Resources.VolumeNumberRequired);
                                      RuleFor(x => x.Title).NotEmpty().WithMessage(Resources.TitleRequired);
                                  });
        }
    }
}