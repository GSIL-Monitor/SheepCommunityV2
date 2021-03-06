﻿using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Groups.Entities
{
    /// <summary>
    ///     群组信息。
    /// </summary>
    [DataContract]
    public class GroupDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     显示名称。
        /// </summary>
        [DataMember(Order = 2)]
        public string DisplayName { get; set; }

        /// <summary>
        ///     真实组织全称。
        /// </summary>
        [DataMember(Order = 3)]
        public string FullName { get; set; }

        /// <summary>
        ///     真实组织全称是否已通过认证。
        /// </summary>
        [DataMember(Order = 4)]
        public bool FullNameVerified { get; set; }

        /// <summary>
        ///     简介。
        /// </summary>
        [DataMember(Order = 5)]
        public string Description { get; set; }

        /// <summary>
        ///     图像网址。
        /// </summary>
        [DataMember(Order = 6)]
        public string IconUrl { get; set; }

        /// <summary>
        ///     封面图像地址。
        /// </summary>
        [DataMember(Order = 7)]
        public string CoverPhotoUrl { get; set; }

        /// <summary>
        ///     创建日期。
        /// </summary>
        [DataMember(Order = 8)]
        public long CreatedDate { get; set; }

        /// <summary>
        ///     更新日期。
        /// </summary>
        [DataMember(Order = 9)]
        public long ModifiedDate { get; set; }
    }
}