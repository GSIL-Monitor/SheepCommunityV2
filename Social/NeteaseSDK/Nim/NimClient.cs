using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceStack;
using ServiceStack.Extensions;
using ServiceStack.Logging;
using ServiceStack.Text;

namespace Netease.Nim
{
    /// <summary>
    ///     网易云通信服务客户端封装库。
    /// </summary>
    public class NimClient : INimClient
    {
        #region 静态变量

        /// <summary>
        ///     相关的日志记录器。
        /// </summary>
        protected static readonly ILog Log = LogManager.GetLogger(typeof(NimClient));

        #endregion

        #region 属性

        /// <summary>
        ///     申请应用时分配的应用程序唯一标识。
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        ///     申请应用时分配的应用程序密钥。
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        ///     创建用户帐号的地址。
        /// </summary>
        public string UserCreateUrl { get; set; }

        /// <summary>
        ///     用户帐号更新的地址。
        /// </summary>
        public string UserUpdateUrl { get; set; }

        /// <summary>
        ///     封禁用户帐号的地址。
        /// </summary>
        public string UserBlockUrl { get; set; }

        /// <summary>
        ///     解禁用户帐号的地址。
        /// </summary>
        public string UserUnBlockUrl { get; set; }

        /// <summary>
        ///     更新用户名片的地址。
        /// </summary>
        public string UserUpdateInfoUrl { get; set; }

        /// <summary>
        ///     获取用户名片的地址。
        /// </summary>
        public string UserGetInfosUrl { get; set; }

        /// <summary>
        ///     设置黑名单静音的地址。
        /// </summary>
        public string UserSetSpecialRelationUrl { get; set; }

        /// <summary>
        ///     加好友的地址。
        /// </summary>
        public string FriendAddUrl { get; set; }

        /// <summary>
        ///     更新好友的地址。
        /// </summary>
        public string FriendUpdateUrl { get; set; }

        /// <summary>
        ///     删除好友的地址。
        /// </summary>
        public string FriendDeleteUrl { get; set; }

        /// <summary>
        ///     发送普通消息的地址。
        /// </summary>
        public string MessageSendUrl { get; set; }

        /// <summary>
        ///     批量发送点对点普通消息的地址。
        /// </summary>
        public string MessageSendBatchUrl { get; set; }

        /// <summary>
        ///     发送自定义系统通知的地址。
        /// </summary>
        public string MessageSendAttachUrl { get; set; }

        /// <summary>
        ///     批量发送点对点自定义系统通知的地址。
        /// </summary>
        public string MessageSendBatchAttachUrl { get; set; }

        /// <summary>
        ///     文件上传的地址。
        /// </summary>
        public string MessageFileUploadUrl { get; set; }

        /// <summary>
        ///     消息撤回的地址。
        /// </summary>
        public string MessageRecallUrl { get; set; }

        /// <summary>
        ///     创建群的地址。
        /// </summary>
        public string TeamCreateUrl { get; set; }

        /// <summary>
        ///     拉人入群的地址。
        /// </summary>
        public string TeamAddMemberUrl { get; set; }

        /// <summary>
        ///     踢人出群的地址。
        /// </summary>
        public string TeamKickMemberUrl { get; set; }

        /// <summary>
        ///     解散群的地址。
        /// </summary>
        public string TeamRemoveUrl { get; set; }

        /// <summary>
        ///     编辑群资料的地址。
        /// </summary>
        public string TeamUpdateUrl { get; set; }

        /// <summary>
        ///     群信息与成员列表查询的地址。
        /// </summary>
        public string TeamQueryUrl { get; set; }

        /// <summary>
        ///     移交群主的地址。
        /// </summary>
        public string TeamChangeOwnerUrl { get; set; }

        /// <summary>
        ///     任命管理员的地址。
        /// </summary>
        public string TeamAddManagerUrl { get; set; }

        /// <summary>
        ///     移除管理员的地址。
        /// </summary>
        public string TeamRemoveManagerUrl { get; set; }

        /// <summary>
        ///     获取某用户所加入的群信息的地址。
        /// </summary>
        public string TeamGetJoinedUrl { get; set; }

        /// <summary>
        ///     修改群昵称的地址。
        /// </summary>
        public string TeamUpdateMemberNickUrl { get; set; }

        /// <summary>
        ///     修改消息提醒开关的地址。
        /// </summary>
        public string TeamUpdateMemberMuteUrl { get; set; }

        /// <summary>
        ///     禁言群成员的地址。
        /// </summary>
        public string TeamMuteMemberUrl { get; set; }

        /// <summary>
        ///     主动退群的地址。
        /// </summary>
        public string TeamLeaveMemberUrl { get; set; }

        /// <summary>
        ///     将群组整体禁言的地址。
        /// </summary>
        public string TeamMuteUrl { get; set; }

        /// <summary>
        ///     获取群组禁言列表的地址。
        /// </summary>
        public string TeamGetMutedMembersUrl { get; set; }

        #endregion

        #region IIMClient 接口实现

        #region 用户

        /// <summary>
        ///     将创建网易云通信用户帐号。
        /// </summary>
        public UserCreateResponse Post(UserCreateRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步将创建网易云通信用户帐号。
        /// </summary>
        public async Task<UserCreateResponse> PostAsync(UserCreateRequest request)
        {
            try
            {
                var responseJson = await UserCreateUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<UserCreateResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UserCreateResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     更新网易云通信用户帐号。
        /// </summary>
        public UserUpdateResponse Post(UserUpdateRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步更新网易云通信用户帐号。
        /// </summary>
        public async Task<UserUpdateResponse> PostAsync(UserUpdateRequest request)
        {
            try
            {
                var responseJson = await UserUpdateUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<UserUpdateResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UserUpdateResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     封禁网易云通信用户帐号。
        /// </summary>
        public UserBlockResponse Post(UserBlockRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步封禁网易云通信用户帐号。
        /// </summary>
        public async Task<UserBlockResponse> PostAsync(UserBlockRequest request)
        {
            try
            {
                var responseJson = await UserBlockUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<UserBlockResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UserBlockResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     解禁网易云通信用户帐号。
        /// </summary>
        public UserUnBlockResponse Post(UserUnBlockRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步解禁网易云通信用户帐号。
        /// </summary>
        public async Task<UserUnBlockResponse> PostAsync(UserUnBlockRequest request)
        {
            try
            {
                var responseJson = await UserUnBlockUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<UserUnBlockResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UserUnBlockResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     更新用户名片。
        /// </summary>
        public UserUpdateInfoResponse Post(UserUpdateInfoRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步更新用户名片。
        /// </summary>
        public async Task<UserUpdateInfoResponse> PostAsync(UserUpdateInfoRequest request)
        {
            try
            {
                var responseJson = await UserUpdateInfoUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<UserUpdateInfoResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UserUpdateInfoResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     获取用户名片。
        /// </summary>
        public UserGetInfosResponse Post(UserGetInfosRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步获取用户名片。
        /// </summary>
        public async Task<UserGetInfosResponse> PostAsync(UserGetInfosRequest request)
        {
            try
            {
                var responseJson = await UserGetInfosUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<UserGetInfosResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UserGetInfosResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     设置黑名单静音。
        /// </summary>
        public UserSetSpecialRelationResponse Post(UserSetSpecialRelationRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步设置黑名单静音。
        /// </summary>
        public async Task<UserSetSpecialRelationResponse> PostAsync(UserSetSpecialRelationRequest request)
        {
            try
            {
                var responseJson = await UserSetSpecialRelationUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<UserSetSpecialRelationResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new UserSetSpecialRelationResponse
                       {
                           Code = -1
                       };
            }
        }

        #endregion

        #region 好友

        /// <summary>
        ///     加好友。
        /// </summary>
        public FriendAddResponse Post(FriendAddRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步加好友。
        /// </summary>
        public async Task<FriendAddResponse> PostAsync(FriendAddRequest request)
        {
            try
            {
                var responseJson = await FriendAddUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<FriendAddResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new FriendAddResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     更新好友。
        /// </summary>
        public FriendUpdateResponse Post(FriendUpdateRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步更新好友。
        /// </summary>
        public async Task<FriendUpdateResponse> PostAsync(FriendUpdateRequest request)
        {
            try
            {
                var responseJson = await FriendUpdateUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<FriendUpdateResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new FriendUpdateResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     删除好友。
        /// </summary>
        public FriendDeleteResponse Post(FriendDeleteRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步删除好友。
        /// </summary>
        public async Task<FriendDeleteResponse> PostAsync(FriendDeleteRequest request)
        {
            try
            {
                var responseJson = await FriendDeleteUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<FriendDeleteResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new FriendDeleteResponse
                       {
                           Code = -1
                       };
            }
        }

        #endregion

        #region 消息

        /// <summary>
        ///     发送普通消息。
        /// </summary>
        public MessageSendResponse Post(MessageSendRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步发送普通消息。
        /// </summary>
        public async Task<MessageSendResponse> PostAsync(MessageSendRequest request)
        {
            try
            {
                var responseJson = await MessageSendUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<MessageSendResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new MessageSendResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     批量发送点对点普通消息。
        /// </summary>
        public MessageSendBatchResponse Post(MessageSendBatchRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步批量发送点对点普通消息。
        /// </summary>
        public async Task<MessageSendBatchResponse> PostAsync(MessageSendBatchRequest request)
        {
            try
            {
                var responseJson = await MessageSendBatchUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<MessageSendBatchResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new MessageSendBatchResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     发送自定义系统通知。
        /// </summary>
        public MessageSendAttachResponse Post(MessageSendAttachRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步发送自定义系统通知。
        /// </summary>
        public async Task<MessageSendAttachResponse> PostAsync(MessageSendAttachRequest request)
        {
            try
            {
                var responseJson = await MessageSendAttachUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<MessageSendAttachResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new MessageSendAttachResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     批量发送点对点自定义系统通知。
        /// </summary>
        public MessageSendBatchAttachResponse Post(MessageSendBatchAttachRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步批量发送点对点自定义系统通知。
        /// </summary>
        public async Task<MessageSendBatchAttachResponse> PostAsync(MessageSendBatchAttachRequest request)
        {
            try
            {
                var responseJson = await MessageSendBatchAttachUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<MessageSendBatchAttachResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new MessageSendBatchAttachResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     文件上传。
        /// </summary>
        public MessageFileUploadResponse Post(MessageFileUploadRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步文件上传。
        /// </summary>
        public async Task<MessageFileUploadResponse> PostAsync(MessageFileUploadRequest request)
        {
            try
            {
                var responseJson = await MessageFileUploadUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<MessageFileUploadResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new MessageFileUploadResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     消息撤回。
        /// </summary>
        public MessageRecallResponse Post(MessageRecallRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步消息撤回。
        /// </summary>
        public async Task<MessageRecallResponse> PostAsync(MessageRecallRequest request)
        {
            try
            {
                var responseJson = await MessageRecallUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<MessageRecallResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new MessageRecallResponse
                       {
                           Code = -1
                       };
            }
        }

        #endregion

        #region 群组

        /// <summary>
        ///     创建群。
        /// </summary>
        public TeamCreateResponse Post(TeamCreateRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步创建群。
        /// </summary>
        public async Task<TeamCreateResponse> PostAsync(TeamCreateRequest request)
        {
            try
            {
                var responseJson = await TeamCreateUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamCreateResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamCreateResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     拉人入群。
        /// </summary>
        public TeamAddMemberResponse Post(TeamAddMemberRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步拉人入群。
        /// </summary>
        public async Task<TeamAddMemberResponse> PostAsync(TeamAddMemberRequest request)
        {
            try
            {
                var responseJson = await TeamAddMemberUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamAddMemberResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamAddMemberResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     踢人出群。
        /// </summary>
        public TeamKickMemberResponse Post(TeamKickMemberRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步踢人出群。
        /// </summary>
        public async Task<TeamKickMemberResponse> PostAsync(TeamKickMemberRequest request)
        {
            try
            {
                var responseJson = await TeamKickMemberUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamKickMemberResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamKickMemberResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     解散群。
        /// </summary>
        public TeamRemoveResponse Post(TeamRemoveRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步解散群。
        /// </summary>
        public async Task<TeamRemoveResponse> PostAsync(TeamRemoveRequest request)
        {
            try
            {
                var responseJson = await TeamRemoveUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamRemoveResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamRemoveResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     编辑群资料。
        /// </summary>
        public TeamUpdateResponse Post(TeamUpdateRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步编辑群资料。
        /// </summary>
        public async Task<TeamUpdateResponse> PostAsync(TeamUpdateRequest request)
        {
            try
            {
                var responseJson = await TeamUpdateUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamUpdateResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamUpdateResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     群信息与成员列表查询。
        /// </summary>
        public TeamQueryResponse Post(TeamQueryRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步群信息与成员列表查询。
        /// </summary>
        public async Task<TeamQueryResponse> PostAsync(TeamQueryRequest request)
        {
            try
            {
                var responseJson = await TeamQueryUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamQueryResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamQueryResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     移交群主。
        /// </summary>
        public TeamChangeOwnerResponse Post(TeamChangeOwnerRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步移交群主。
        /// </summary>
        public async Task<TeamChangeOwnerResponse> PostAsync(TeamChangeOwnerRequest request)
        {
            try
            {
                var responseJson = await TeamChangeOwnerUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamChangeOwnerResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamChangeOwnerResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     任命管理员。
        /// </summary>
        public TeamAddManagerResponse Post(TeamAddManagerRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步任命管理员。
        /// </summary>
        public async Task<TeamAddManagerResponse> PostAsync(TeamAddManagerRequest request)
        {
            try
            {
                var responseJson = await TeamAddManagerUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamAddManagerResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamAddManagerResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     移除管理员。
        /// </summary>
        public TeamRemoveManagerResponse Post(TeamRemoveManagerRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步移除管理员。
        /// </summary>
        public async Task<TeamRemoveManagerResponse> PostAsync(TeamRemoveManagerRequest request)
        {
            try
            {
                var responseJson = await TeamRemoveManagerUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamRemoveManagerResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamRemoveManagerResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     获取某用户所加入的群信息。
        /// </summary>
        public TeamGetJoinedResponse Post(TeamGetJoinedRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步获取某用户所加入的群信息。
        /// </summary>
        public async Task<TeamGetJoinedResponse> PostAsync(TeamGetJoinedRequest request)
        {
            try
            {
                var responseJson = await TeamGetJoinedUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamGetJoinedResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamGetJoinedResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     修改群昵称。
        /// </summary>
        public TeamUpdateMemberNickResponse Post(TeamUpdateMemberNickRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步修改群昵称。
        /// </summary>
        public async Task<TeamUpdateMemberNickResponse> PostAsync(TeamUpdateMemberNickRequest request)
        {
            try
            {
                var responseJson = await TeamUpdateMemberNickUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamUpdateMemberNickResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamUpdateMemberNickResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     修改消息提醒开关。
        /// </summary>
        public TeamUpdateMemberMuteResponse Post(TeamUpdateMemberMuteRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步修改消息提醒开关。
        /// </summary>
        public async Task<TeamUpdateMemberMuteResponse> PostAsync(TeamUpdateMemberMuteRequest request)
        {
            try
            {
                var responseJson = await TeamUpdateMemberMuteUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamUpdateMemberMuteResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamUpdateMemberMuteResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     禁言群成员。
        /// </summary>
        public TeamMuteMemberResponse Post(TeamMuteMemberRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步禁言群成员。
        /// </summary>
        public async Task<TeamMuteMemberResponse> PostAsync(TeamMuteMemberRequest request)
        {
            try
            {
                var responseJson = await TeamMuteMemberUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamMuteMemberResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamMuteMemberResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     主动退群。
        /// </summary>
        public TeamLeaveMemberResponse Post(TeamLeaveMemberRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步主动退群。
        /// </summary>
        public async Task<TeamLeaveMemberResponse> PostAsync(TeamLeaveMemberRequest request)
        {
            try
            {
                var responseJson = await TeamLeaveMemberUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamLeaveMemberResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamLeaveMemberResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     将群组整体禁言。
        /// </summary>
        public TeamMuteResponse Post(TeamMuteRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步将群组整体禁言。
        /// </summary>
        public async Task<TeamMuteResponse> PostAsync(TeamMuteRequest request)
        {
            try
            {
                var responseJson = await TeamMuteUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamMuteResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamMuteResponse
                       {
                           Code = -1
                       };
            }
        }

        /// <summary>
        ///     获取群组禁言列表。
        /// </summary>
        public TeamGetMutedMembersResponse Post(TeamGetMutedMembersRequest request)
        {
            return Nito.AsyncEx.AsyncContext.Run(() => PostAsync(request));
        }

        /// <summary>
        ///     异步获取群组禁言列表。
        /// </summary>
        public async Task<TeamGetMutedMembersResponse> PostAsync(TeamGetMutedMembersRequest request)
        {
            try
            {
                var responseJson = await TeamGetMutedMembersUrl.HttpPostStringAsync(request.ToQueryString(), "application/x-www-form-urlencoded", GetRequestHeaders());
                var response = responseJson.FromJson<TeamGetMutedMembersResponse>();
                if (response != null && response.Code != 200)
                {
                    Log.ErrorFormat("{0} {1} Error: {2}-{3}-{4}", GetType().Name, request.GetType().Name, response.Code, response.Code.ToErrorDescription(), response.Description);
                }
                return response;
            }
            catch (Exception ex)
            {
                var errorMessage = ex.GetInnerMostException().Message;
                Log.ErrorFormat("{0} {1} Error: {2}", GetType().Name, request.GetType().Name, errorMessage);
                return new TeamGetMutedMembersResponse
                       {
                           Code = -1
                       };
            }
        }

        #endregion

        #endregion

        #region 添加HTTP头

        /// <summary>
        ///     获取请求的HTTP头。
        /// </summary>
        /// <returns>HTTP头的字典。</returns>
        public Dictionary<string, string> GetRequestHeaders()
        {
            var headers = new Dictionary<string, string>(4)
                          {
                              ["AppKey"] = AppKey,
                              ["Nonce"] = Guid.NewGuid().ToString("N"),
                              ["CurTime"] = DateTime.UtcNow.ToUnixTime().ToString()
                          };
            headers["CheckSum"] = string.Format("{0}{1}{2}", AppSecret, headers["Nonce"], headers["CurTime"]).ToSha1HashString();
            return headers;
        }

        #endregion
    }
}