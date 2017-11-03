using System;
using System.Runtime.Serialization;
using Sheep.ServiceModel.Achievements.Entities;
using Sheep.ServiceModel.BasicUsers.Entities;
using Sheep.ServiceModel.Users.Entities;

namespace Sheep.ServiceModel.UserAchievements.Entities
{
    /// <summary>
    ///     用户的成就信息。
    /// </summary>
    [DataContract]
    public class UserAchievementDto
    {
        /// <summary>
        ///     成就信息。
        /// </summary>
        [DataMember(Order = 1)]
        public AchievementDto Achievement { get; set; }

        /// <summary>
        ///     用户信息。
        /// </summary>
        [DataMember(Order = 2)]
        public BasicUserDto User { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 3)]
        public DateTime CreatedDate { get; set; }
    }
}