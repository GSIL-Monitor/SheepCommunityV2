﻿using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Volumes.Validators
{
    /// <summary>
    ///     更新一条卷注释的校验器。
    /// </summary>
    public class VolumeAnnotationUpdateValidator : AbstractValidator<VolumeAnnotationUpdate>
    {
        /// <summary>
        ///     初始化一个新的<see cref="VolumeAnnotationUpdateValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public VolumeAnnotationUpdateValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.BookId).NotEmpty().WithMessage(x => string.Format(Resources.BookIdRequired));
                                     RuleFor(x => x.VolumeNumber).NotEmpty().WithMessage(x => string.Format(Resources.VolumeNumberRequired));
                                     RuleFor(x => x.AnnotationNumber).NotEmpty().WithMessage(x => string.Format(Resources.AnnotationNumberRequired));
                                     RuleFor(x => x.Annotation).NotEmpty().WithMessage(x => string.Format(Resources.AnnotationRequired));
                                 });
        }
    }
}