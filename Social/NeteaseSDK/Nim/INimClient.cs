using System.Threading.Tasks;

namespace Netease.Nim
{
    /// <summary>
    ///     网易云通信服务客户端封装库的接口定义。
    /// </summary>
    public interface INimClient
    {
        #region 用户

        /// <summary>
        ///     将创建网易云通信用户帐号。
        /// </summary>
        UserCreateResponse Post(UserCreateRequest request);

        /// <summary>
        ///     异步将创建网易云通信用户帐号。
        /// </summary>
        Task<UserCreateResponse> PostAsync(UserCreateRequest request);

        /// <summary>
        ///     更新网易云通信用户帐号。
        /// </summary>
        UserUpdateResponse Post(UserUpdateRequest request);

        /// <summary>
        ///     异步更新网易云通信用户帐号。
        /// </summary>
        Task<UserUpdateResponse> PostAsync(UserUpdateRequest request);

        /// <summary>
        ///     封禁网易云通信用户帐号。
        /// </summary>
        UserBlockResponse Post(UserBlockRequest request);

        /// <summary>
        ///     异步封禁网易云通信用户帐号。
        /// </summary>
        Task<UserBlockResponse> PostAsync(UserBlockRequest request);

        /// <summary>
        ///     解禁网易云通信用户帐号。
        /// </summary>
        UserUnBlockResponse Post(UserUnBlockRequest request);

        /// <summary>
        ///     异步解禁网易云通信用户帐号。
        /// </summary>
        Task<UserUnBlockResponse> PostAsync(UserUnBlockRequest request);

        /// <summary>
        ///     更新用户名片。
        /// </summary>
        UserUpdateInfoResponse Post(UserUpdateInfoRequest request);

        /// <summary>
        ///     异步更新用户名片。
        /// </summary>
        Task<UserUpdateInfoResponse> PostAsync(UserUpdateInfoRequest request);

        /// <summary>
        ///     获取用户名片。
        /// </summary>
        UserGetInfosResponse Post(UserGetInfosRequest request);

        /// <summary>
        ///     异步获取用户名片。
        /// </summary>
        Task<UserGetInfosResponse> PostAsync(UserGetInfosRequest request);

        /// <summary>
        ///     设置黑名单静音。
        /// </summary>
        UserSetSpecialRelationResponse Post(UserSetSpecialRelationRequest request);

        /// <summary>
        ///     异步设置黑名单静音。
        /// </summary>
        Task<UserSetSpecialRelationResponse> PostAsync(UserSetSpecialRelationRequest request);

        #endregion

        #region 好友

        /// <summary>
        ///     加好友。
        /// </summary>
        FriendAddResponse Post(FriendAddRequest request);

        /// <summary>
        ///     异步加好友。
        /// </summary>
        Task<FriendAddResponse> PostAsync(FriendAddRequest request);

        /// <summary>
        ///     更新好友。
        /// </summary>
        FriendUpdateResponse Post(FriendUpdateRequest request);

        /// <summary>
        ///     异步更新好友。
        /// </summary>
        Task<FriendUpdateResponse> PostAsync(FriendUpdateRequest request);

        /// <summary>
        ///     删除好友。
        /// </summary>
        FriendDeleteResponse Post(FriendDeleteRequest request);

        /// <summary>
        ///     异步删除好友。
        /// </summary>
        Task<FriendDeleteResponse> PostAsync(FriendDeleteRequest request);

        #endregion

        #region 消息

        /// <summary>
        ///     发送普通消息。
        /// </summary>
        MessageSendResponse Post(MessageSendRequest request);

        /// <summary>
        ///     异步发送普通消息。
        /// </summary>
        Task<MessageSendResponse> PostAsync(MessageSendRequest request);

        /// <summary>
        ///     批量发送点对点普通消息。
        /// </summary>
        MessageSendBatchResponse Post(MessageSendBatchRequest request);

        /// <summary>
        ///     异步批量发送点对点普通消息。
        /// </summary>
        Task<MessageSendBatchResponse> PostAsync(MessageSendBatchRequest request);

        /// <summary>
        ///     发送自定义系统通知。
        /// </summary>
        MessageSendAttachResponse Post(MessageSendAttachRequest request);

        /// <summary>
        ///     异步发送自定义系统通知。
        /// </summary>
        Task<MessageSendAttachResponse> PostAsync(MessageSendAttachRequest request);

        /// <summary>
        ///     批量发送点对点自定义系统通知。
        /// </summary>
        MessageSendBatchAttachResponse Post(MessageSendBatchAttachRequest request);

        /// <summary>
        ///     异步批量发送点对点自定义系统通知。
        /// </summary>
        Task<MessageSendBatchAttachResponse> PostAsync(MessageSendBatchAttachRequest request);

        /// <summary>
        ///     文件上传。
        /// </summary>
        MessageFileUploadResponse Post(MessageFileUploadRequest request);

        /// <summary>
        ///     异步文件上传。
        /// </summary>
        Task<MessageFileUploadResponse> PostAsync(MessageFileUploadRequest request);

        /// <summary>
        ///     消息撤回。
        /// </summary>
        MessageRecallResponse Post(MessageRecallRequest request);

        /// <summary>
        ///     异步消息撤回。
        /// </summary>
        Task<MessageRecallResponse> PostAsync(MessageRecallRequest request);

        #endregion

        #region 群组

        /// <summary>
        ///     创建群。
        /// </summary>
        TeamCreateResponse Post(TeamCreateRequest request);

        /// <summary>
        ///     异步创建群。
        /// </summary>
        Task<TeamCreateResponse> PostAsync(TeamCreateRequest request);

        /// <summary>
        ///     拉人入群。
        /// </summary>
        TeamAddMemberResponse Post(TeamAddMemberRequest request);

        /// <summary>
        ///     异步拉人入群。
        /// </summary>
        Task<TeamAddMemberResponse> PostAsync(TeamAddMemberRequest request);

        /// <summary>
        ///     踢人出群。
        /// </summary>
        TeamKickMemberResponse Post(TeamKickMemberRequest request);

        /// <summary>
        ///     异步踢人出群。
        /// </summary>
        Task<TeamKickMemberResponse> PostAsync(TeamKickMemberRequest request);

        /// <summary>
        ///     解散群。
        /// </summary>
        TeamRemoveResponse Post(TeamRemoveRequest request);

        /// <summary>
        ///     异步解散群。
        /// </summary>
        Task<TeamRemoveResponse> PostAsync(TeamRemoveRequest request);

        /// <summary>
        ///     编辑群资料。
        /// </summary>
        TeamUpdateResponse Post(TeamUpdateRequest request);

        /// <summary>
        ///     异步编辑群资料。
        /// </summary>
        Task<TeamUpdateResponse> PostAsync(TeamUpdateRequest request);

        /// <summary>
        ///     群信息与成员列表查询。
        /// </summary>
        TeamQueryResponse Post(TeamQueryRequest request);

        /// <summary>
        ///     异步群信息与成员列表查询。
        /// </summary>
        Task<TeamQueryResponse> PostAsync(TeamQueryRequest request);

        /// <summary>
        ///     移交群主。
        /// </summary>
        TeamChangeOwnerResponse Post(TeamChangeOwnerRequest request);

        /// <summary>
        ///     异步移交群主。
        /// </summary>
        Task<TeamChangeOwnerResponse> PostAsync(TeamChangeOwnerRequest request);

        /// <summary>
        ///     任命管理员。
        /// </summary>
        TeamAddManagerResponse Post(TeamAddManagerRequest request);

        /// <summary>
        ///     异步任命管理员。
        /// </summary>
        Task<TeamAddManagerResponse> PostAsync(TeamAddManagerRequest request);

        /// <summary>
        ///     移除管理员。
        /// </summary>
        TeamRemoveManagerResponse Post(TeamRemoveManagerRequest request);

        /// <summary>
        ///     异步移除管理员。
        /// </summary>
        Task<TeamRemoveManagerResponse> PostAsync(TeamRemoveManagerRequest request);

        /// <summary>
        ///     获取某用户所加入的群信息。
        /// </summary>
        TeamGetJoinedResponse Post(TeamGetJoinedRequest request);

        /// <summary>
        ///     异步获取某用户所加入的群信息。
        /// </summary>
        Task<TeamGetJoinedResponse> PostAsync(TeamGetJoinedRequest request);

        /// <summary>
        ///     修改群昵称。
        /// </summary>
        TeamUpdateMemberNickResponse Post(TeamUpdateMemberNickRequest request);

        /// <summary>
        ///     异步修改群昵称。
        /// </summary>
        Task<TeamUpdateMemberNickResponse> PostAsync(TeamUpdateMemberNickRequest request);

        /// <summary>
        ///     修改消息提醒开关。
        /// </summary>
        TeamUpdateMemberMuteResponse Post(TeamUpdateMemberMuteRequest request);

        /// <summary>
        ///     异步修改消息提醒开关。
        /// </summary>
        Task<TeamUpdateMemberMuteResponse> PostAsync(TeamUpdateMemberMuteRequest request);

        /// <summary>
        ///     禁言群成员。
        /// </summary>
        TeamMuteMemberResponse Post(TeamMuteMemberRequest request);

        /// <summary>
        ///     异步禁言群成员。
        /// </summary>
        Task<TeamMuteMemberResponse> PostAsync(TeamMuteMemberRequest request);

        /// <summary>
        ///     主动退群。
        /// </summary>
        TeamLeaveMemberResponse Post(TeamLeaveMemberRequest request);

        /// <summary>
        ///     异步主动退群。
        /// </summary>
        Task<TeamLeaveMemberResponse> PostAsync(TeamLeaveMemberRequest request);

        /// <summary>
        ///     将群组整体禁言。
        /// </summary>
        TeamMuteResponse Post(TeamMuteRequest request);

        /// <summary>
        ///     异步将群组整体禁言。
        /// </summary>
        Task<TeamMuteResponse> PostAsync(TeamMuteRequest request);

        /// <summary>
        ///     获取群组禁言列表。
        /// </summary>
        TeamGetMutedMembersResponse Post(TeamGetMutedMembersRequest request);

        /// <summary>
        ///     异步获取群组禁言列表。
        /// </summary>
        Task<TeamGetMutedMembersResponse> PostAsync(TeamGetMutedMembersRequest request);

        #endregion
    }
}