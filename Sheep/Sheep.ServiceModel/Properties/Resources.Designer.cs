﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sheep.ServiceModel.Properties {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheep.ServiceModel.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   查找类似 授权码必须输入。 的本地化字符串。
        /// </summary>
        internal static string AccessTokenRequired {
            get {
                return ResourceManager.GetString("AccessTokenRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 帐户状态的有效范围为:{0}。 的本地化字符串。
        /// </summary>
        internal static string AccountStatusRangeMismatch {
            get {
                return ResourceManager.GetString("AccountStatusRangeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 出生日期的有效范围为{0}至{1}。 的本地化字符串。
        /// </summary>
        internal static string BirthDateRangeMismatch {
            get {
                return ResourceManager.GetString("BirthDateRangeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 城市编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string CityIdRequired {
            get {
                return ResourceManager.GetString("CityIdRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 代码必须输入。 的本地化字符串。
        /// </summary>
        internal static string CodeRequired {
            get {
                return ResourceManager.GetString("CodeRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 国家编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string CountryIdRequired {
            get {
                return ResourceManager.GetString("CountryIdRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 输入的国家/地区不在列表中。 的本地化字符串。
        /// </summary>
        internal static string CountryRangeMismatch {
            get {
                return ResourceManager.GetString("CountryRangeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 简介的长度至少为{0}位并且最多为为{1}位。 的本地化字符串。
        /// </summary>
        internal static string DescriptionLengthMismatch {
            get {
                return ResourceManager.GetString("DescriptionLengthMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 显示名称已经存在。 的本地化字符串。
        /// </summary>
        internal static string DisplayNameAlreadyExists {
            get {
                return ResourceManager.GetString("DisplayNameAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 显示名称必须输入。 的本地化字符串。
        /// </summary>
        internal static string DisplayNameRequired {
            get {
                return ResourceManager.GetString("DisplayNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 电子邮件地址已经存在。 的本地化字符串。
        /// </summary>
        internal static string EmailAlreadyExists {
            get {
                return ResourceManager.GetString("EmailAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 电子邮件地址格式错误。 的本地化字符串。
        /// </summary>
        internal static string EmailFormatMismatch {
            get {
                return ResourceManager.GetString("EmailFormatMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 电子邮件地址的长度至少为{0}位并且最多为为{1}位。 的本地化字符串。
        /// </summary>
        internal static string EmailLengthMismatch {
            get {
                return ResourceManager.GetString("EmailLengthMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 真实姓名的长度至少为{0}位并且最多为为{1}位。 的本地化字符串。
        /// </summary>
        internal static string FullNameLengthMismatch {
            get {
                return ResourceManager.GetString("FullNameLengthMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 性别的有效范围为:{0}。 的本地化字符串。
        /// </summary>
        internal static string GenderRangeMismatch {
            get {
                return ResourceManager.GetString("GenderRangeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 群组编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string GroupIdRequired {
            get {
                return ResourceManager.GetString("GroupIdRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 所属教会的长度至少为{0}位并且最多为为{1}位。 的本地化字符串。
        /// </summary>
        internal static string GuildLengthMismatch {
            get {
                return ResourceManager.GetString("GuildLengthMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 加入方式的有效范围为:{0}。 的本地化字符串。
        /// </summary>
        internal static string JoinModeRangeMismatch {
            get {
                return ResourceManager.GetString("JoinModeRangeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 语言的有效范围为:{0}。 的本地化字符串。
        /// </summary>
        internal static string LanguageRangeMismatch {
            get {
                return ResourceManager.GetString("LanguageRangeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 名称必须输入。 的本地化字符串。
        /// </summary>
        internal static string NameRequired {
            get {
                return ResourceManager.GetString("NameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 排序的字段的有效范围为:{0}。 的本地化字符串。
        /// </summary>
        internal static string OrderByRangeMismatch {
            get {
                return ResourceManager.GetString("OrderByRangeMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 登录密码的长度至少为{0}位并且最多为为{1}位。 的本地化字符串。
        /// </summary>
        internal static string PasswordLengthMismatch {
            get {
                return ResourceManager.GetString("PasswordLengthMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 登录密码必须输入。 的本地化字符串。
        /// </summary>
        internal static string PasswordRequired {
            get {
                return ResourceManager.GetString("PasswordRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 手机号码已经存在。 的本地化字符串。
        /// </summary>
        internal static string PhoneNumberAlreadyExists {
            get {
                return ResourceManager.GetString("PhoneNumberAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 手机号码必须为11位中国地区手机号。 的本地化字符串。
        /// </summary>
        internal static string PhoneNumberFormatMismatch {
            get {
                return ResourceManager.GetString("PhoneNumberFormatMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 手机号码不存在。 的本地化字符串。
        /// </summary>
        internal static string PhoneNumberNotExists {
            get {
                return ResourceManager.GetString("PhoneNumberNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 手机号码必须输入。 的本地化字符串。
        /// </summary>
        internal static string PhoneNumberRequired {
            get {
                return ResourceManager.GetString("PhoneNumberRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用途必须输入。 的本地化字符串。
        /// </summary>
        internal static string PurposeRequired {
            get {
                return ResourceManager.GetString("PurposeRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 QQ用户编号已经存在。 的本地化字符串。
        /// </summary>
        internal static string QQUserIdAlreadyExists {
            get {
                return ResourceManager.GetString("QQUserIdAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 QQ用户编号不存在。 的本地化字符串。
        /// </summary>
        internal static string QQUserIdNotExists {
            get {
                return ResourceManager.GetString("QQUserIdNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 QQ用户编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string QQUserIdRequired {
            get {
                return ResourceManager.GetString("QQUserIdRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 关联的第三方编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string RefIdRequired {
            get {
                return ResourceManager.GetString("RefIdRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 验证码有效长度必须为{0}位。 的本地化字符串。
        /// </summary>
        internal static string SecurityTokenLengthMismatch {
            get {
                return ResourceManager.GetString("SecurityTokenLengthMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 验证码必须输入。 的本地化字符串。
        /// </summary>
        internal static string SecurityTokenRequired {
            get {
                return ResourceManager.GetString("SecurityTokenRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 签名的长度至少为{0}位并且最多为为{1}位。 的本地化字符串。
        /// </summary>
        internal static string SignatureLengthMismatch {
            get {
                return ResourceManager.GetString("SignatureLengthMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 来源头像地址必须为图像地址（以.jpg或.png为后缀名）。 的本地化字符串。
        /// </summary>
        internal static string SourceAvatarUrlMismatch {
            get {
                return ResourceManager.GetString("SourceAvatarUrlMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 来源封面图片地址必须为图像地址（以.jpg或.png为后缀名）。 的本地化字符串。
        /// </summary>
        internal static string SourceCoverPhotoUrlMismatch {
            get {
                return ResourceManager.GetString("SourceCoverPhotoUrlMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 来源图标地址必须为图像地址（以.jpg或.png为后缀名）。 的本地化字符串。
        /// </summary>
        internal static string SourceIconUrlMismatch {
            get {
                return ResourceManager.GetString("SourceIconUrlMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 省份编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string StateIdRequired {
            get {
                return ResourceManager.GetString("StateIdRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string UserIdRequired {
            get {
                return ResourceManager.GetString("UserIdRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名称已经存在。 的本地化字符串。
        /// </summary>
        internal static string UserNameAlreadyExists {
            get {
                return ResourceManager.GetString("UserNameAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名称的长度至少为{0}位并且最多为为{1}位。 的本地化字符串。
        /// </summary>
        internal static string UserNameLengthMismatch {
            get {
                return ResourceManager.GetString("UserNameLengthMismatch", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名称或电子邮件地址必须输入。 的本地化字符串。
        /// </summary>
        internal static string UserNameOrEmailRequired {
            get {
                return ResourceManager.GetString("UserNameOrEmailRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 用户名称必须输入。 的本地化字符串。
        /// </summary>
        internal static string UserNameRequired {
            get {
                return ResourceManager.GetString("UserNameRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 微博用户编号已经存在。 的本地化字符串。
        /// </summary>
        internal static string WeiboUserIdAlreadyExists {
            get {
                return ResourceManager.GetString("WeiboUserIdAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 微博用户编号不存在。 的本地化字符串。
        /// </summary>
        internal static string WeiboUserIdNotExists {
            get {
                return ResourceManager.GetString("WeiboUserIdNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 微博用户编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string WeiboUserIdRequired {
            get {
                return ResourceManager.GetString("WeiboUserIdRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 微信用户编号已经存在。 的本地化字符串。
        /// </summary>
        internal static string WeixinUserIdAlreadyExists {
            get {
                return ResourceManager.GetString("WeixinUserIdAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 微信用户编号不存在。 的本地化字符串。
        /// </summary>
        internal static string WeixinUserIdNotExists {
            get {
                return ResourceManager.GetString("WeixinUserIdNotExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 微信用户编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string WeixinUserIdRequired {
            get {
                return ResourceManager.GetString("WeixinUserIdRequired", resourceCulture);
            }
        }
    }
}
