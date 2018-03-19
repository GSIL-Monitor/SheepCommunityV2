using ServiceStack;
using ServiceStack.FluentValidation;

namespace Sheep.Job.ServiceModel.Groups.Validators
{
    /// <summary>
    ///     查询并统计一组群组排行的校验器。
    /// </summary>
    public class GroupRankCountValidator : AbstractValidator<GroupRankCount>
    {
        /// <summary>
        ///     初始化一个新的<see cref="GroupRankCountValidator" />对象。
        ///     创建规则集合。
        /// </summary>
        public GroupRankCountValidator()
        {
            RuleSet(ApplyTo.Put, () =>
                                 {
                                 });
        }
    }
}