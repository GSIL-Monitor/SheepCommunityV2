﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sheep.Model.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheep.Model.Properties.Resources", typeof(Resources).Assembly);
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
        ///   查找类似 电子邮件地址已存在。 的本地化字符串。
        /// </summary>
        internal static string EmailAlreadyExists {
            get {
                return ResourceManager.GetString("EmailAlreadyExists", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 错误的平台用户编号。 的本地化字符串。
        /// </summary>
        internal static string InvalidOpenUserId {
            get {
                return ResourceManager.GetString("InvalidOpenUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 错误的验证码。 的本地化字符串。
        /// </summary>
        internal static string InvalidSecurityToken {
            get {
                return ResourceManager.GetString("InvalidSecurityToken", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 错误的用户名称或电子邮件地址及登录密码。 的本地化字符串。
        /// </summary>
        internal static string InvalidUserNameOrEmailOrPassword {
            get {
                return ResourceManager.GetString("InvalidUserNameOrEmailOrPassword", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 平台用户编号必须输入。 的本地化字符串。
        /// </summary>
        internal static string OpenUserIdRequired {
            get {
                return ResourceManager.GetString("OpenUserIdRequired", resourceCulture);
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
        ///   查找类似 手机号码必须为11位中国地区手机号。 的本地化字符串。
        /// </summary>
        internal static string PhoneNumberFormatMismatch {
            get {
                return ResourceManager.GetString("PhoneNumberFormatMismatch", resourceCulture);
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
        ///   查找类似 用户的帐号已锁定。 的本地化字符串。
        /// </summary>
        internal static string UserAccountLocked {
            get {
                return ResourceManager.GetString("UserAccountLocked", resourceCulture);
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
        ///   查找类似 用户名称或电子邮件地址必须输入。 的本地化字符串。
        /// </summary>
        internal static string UserNameOrEmailRequired {
            get {
                return ResourceManager.GetString("UserNameOrEmailRequired", resourceCulture);
            }
        }
    }
}
