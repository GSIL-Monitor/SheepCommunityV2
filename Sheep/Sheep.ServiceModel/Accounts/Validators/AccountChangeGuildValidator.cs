using ServiceStack;
using ServiceStack.FluentValidation;
using Sheep.ServiceModel.Properties;

namespace Sheep.ServiceModel.Accounts.Validators
{
    /// <summary>
    ///     更改所属教会的校验器。
    /// </summary>
    public class AccountChangeGuildValidator : AbstractValidator<AccountChangeGuild>
    {
        /// <summary>
        ///     初始化一个新的<see cref="AccountChangeGuildValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public AccountChangeGuildValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                     RuleFor(x => x.Guild).Length(2, 128).WithMessage(x => string.Format(Resources.GuildLengthMismatch, 2, 128)).When(x => !x.Guild.IsNullOrEmpty());
                                 });
        }
    }
}