using System.Collections.Generic;
using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改显示语言的校验器。
    /// </summary>
    public class AccountChangeLanguageValidator : AbstractValidator<AccountChangeLanguage>
    {
        public static readonly HashSet<string> Languages = new HashSet<string>
                                                           {
                                                               "简体中文",
                                                               "繁体中文",
                                                               "英语",
                                                               "法语",
                                                               "西班牙语",
                                                               "日语",
                                                               "韩语"
                                                           };

        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeLanguageValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeLanguageValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Language).Must(language => Languages.Contains(language)).WithMessage(Resources.LanguageRangeMismatch, Languages.Join(",")).When(x => !x.Language.IsNullOrEmpty());
                                 });
        }
    }
}