using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.Job.ServiceModel.Users.Validators
{
    /// <summary>
    ///     查询并统计一组用户排行的校验器。
    /// </summary>
    public class UserRankCountValidator : AbstractValidator<UserRankCount>
    {
        /// <summary>
        ///     初始化一个新的<see cref="UserRankCountValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public UserRankCountValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                 });
        }
    }
}