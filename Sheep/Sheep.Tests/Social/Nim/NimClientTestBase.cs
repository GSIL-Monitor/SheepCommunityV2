using Netease.Nim;
using NUnit.Framework;
using ServiceStack.Configuration;
using Sheep.Common.Settings;

namespace Sheep.Tests.Social.Nim
{
    public class NimClientTestBase
    {
        #region 变量

        public INimClient NimClient;
        public IAppSettings AppSettings;

        #endregion

        #region 启动与结束设置

        [OneTimeSetUp]
        public virtual void OnBeforeTest()
        {
            AppSettings = new AppSettings();
            NimClient = new NimClient
                        {
                            AppKey = AppSettings.GetString(AppSettingsNeteaseNimNames.AppKey),
                            AppSecret = AppSettings.GetString(AppSettingsNeteaseNimNames.AppSecret),
                            UserCreateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserCreateUrl),
                            UserUpdateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserUpdateUrl),
                            UserBlockUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserBlockUrl),
                            UserUnBlockUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserUnBlockUrl),
                            UserUpdateInfoUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserUpdateInfoUrl),
                            UserGetInfosUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserGetInfosUrl),
                            UserSetSpecialRelationUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.UserSetSpecialRelationUrl),
                            FriendAddUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.FriendAddUrl),
                            FriendUpdateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.FriendUpdateUrl),
                            FriendDeleteUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.FriendDeleteUrl),
                            MessageSendUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageSendUrl),
                            MessageSendBatchUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageSendBatchUrl),
                            MessageSendAttachUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageSendAttachUrl),
                            MessageSendBatchAttachUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageSendBatchAttachUrl),
                            MessageFileUploadUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageFileUploadUrl),
                            MessageRecallUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.MessageRecallUrl),
                            TeamCreateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamCreateUrl),
                            TeamAddMemberUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamAddMemberUrl),
                            TeamKickMemberUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamKickMemberUrl),
                            TeamRemoveUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamRemoveUrl),
                            TeamUpdateUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamUpdateUrl),
                            TeamQueryUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamQueryUrl),
                            TeamChangeOwnerUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamChangeOwnerUrl),
                            TeamAddManagerUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamAddManagerUrl),
                            TeamRemoveManagerUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamRemoveManagerUrl),
                            TeamGetJoinedUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamGetJoinedUrl),
                            TeamUpdateMemberNickUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamUpdateMemberNickUrl),
                            TeamUpdateMemberMuteUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamUpdateMemberMuteUrl),
                            TeamMuteMemberUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamMuteMemberUrl),
                            TeamLeaveMemberUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamLeaveMemberUrl),
                            TeamMuteUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamMuteUrl),
                            TeamGetMutedMembersUrl = AppSettings.GetString(AppSettingsNeteaseNimNames.TeamGetMutedMembersUrl)
                        };
        }

        [OneTimeTearDown]
        public virtual void OnAfterTest()
        {
        }

        [SetUp]
        public virtual void OnBeforeEachTest()
        {
        }

        [TearDown]
        public virtual void OnAfterEachTest()
        {
        }

        #endregion
    }
}