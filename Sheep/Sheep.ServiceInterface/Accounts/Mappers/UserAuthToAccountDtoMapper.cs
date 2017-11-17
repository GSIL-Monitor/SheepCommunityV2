using System;
using System.Collections.Generic;
using ServiceStack;
using ServiceStack.Auth;
using ServiceStack.Text;
using Sheep.ServiceModel.Accounts.Entities;

namespace Sheep.ServiceInterface.Accounts.Mappers
{
    public static class UserAuthToAccountDtoMapper
    {
        public static AccountDto MapToAccountDto(this IUserAuth userAuth)
        {
            if (userAuth.Meta == null)
            {
                userAuth.Meta = new Dictionary<string, string>();
            }
            var accountDto = new AccountDto
                             {
                                 Id = userAuth.Id,
                                 Type = userAuth.Meta.GetValueOrDefault("Type"),
                                 UserName = userAuth.UserName,
                                 Email = userAuth.Email,
                                 DisplayName = userAuth.DisplayName,
                                 FullName = userAuth.FullName,
                                 FullNameVerified = userAuth.Meta.GetValueOrDefault("FullNameVerified").To(false),
                                 IdImageUrl = userAuth.Meta.GetValueOrDefault("IdImageUrl"),
                                 Signature = userAuth.Meta.GetValueOrDefault("Signature"),
                                 Description = userAuth.Meta.GetValueOrDefault("Description"),
                                 AvatarUrl = userAuth.Meta.GetValueOrDefault("AvatarUrl"),
                                 CoverPhotoUrl = userAuth.Meta.GetValueOrDefault("CoverPhotoUrl"),
                                 BirthDate = userAuth.BirthDate?.ToString("yyyy-MM-dd"),
                                 Gender = userAuth.Gender,
                                 PrimaryEmail = userAuth.PrimaryEmail,
                                 PhoneNumber = userAuth.PhoneNumber,
                                 Country = userAuth.Country,
                                 State = userAuth.State,
                                 City = userAuth.City,
                                 Guild = userAuth.Meta.GetValueOrDefault("Guild"),
                                 Company = userAuth.Company,
                                 Address = userAuth.Address,
                                 Address2 = userAuth.Address2,
                                 MailAddress = userAuth.MailAddress,
                                 PostalCode = userAuth.PostalCode,
                                 TimeZone = userAuth.TimeZone,
                                 Language = userAuth.Language,
                                 PrivateMessagesSource = userAuth.Meta.GetValueOrDefault("PrivateMessagesSource"),
                                 ReceiveEmails = userAuth.Meta.GetValueOrDefault("ReceiveEmails").To<bool?>(),
                                 ReceiveSms = userAuth.Meta.GetValueOrDefault("ReceiveSms").To<bool?>(),
                                 ReceiveCommentNotifications = userAuth.Meta.GetValueOrDefault("ReceiveCommentNotifications").To<bool?>(),
                                 ReceiveConversationNotifications = userAuth.Meta.GetValueOrDefault("ReceiveConversationNotifications").To<bool?>(),
                                 TrackPresence = userAuth.Meta.GetValueOrDefault("TrackPresence").To<bool?>(),
                                 Status = userAuth.Meta.GetValueOrDefault("Status"),
                                 BanReason = userAuth.Meta.GetValueOrDefault("BanReason"),
                                 BannedUntilDate = userAuth.Meta.GetValueOrDefault("BannedUntilDate").To<DateTime?>()?.ToUnixTime(),
                                 RequireModeration = userAuth.Meta.GetValueOrDefault("RequireModeration").To<bool?>(),
                                 CreatedDate = userAuth.CreatedDate.ToUnixTime(),
                                 ModifiedDate = userAuth.ModifiedDate.ToUnixTime(),
                                 LockedDate = userAuth.LockedDate?.ToUnixTime(),
                                 Points = 0
                             };
            return accountDto;
        }
    }
}