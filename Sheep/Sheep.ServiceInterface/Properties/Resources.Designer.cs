﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Sheep.ServiceInterface.Properties {
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
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Sheep.ServiceInterface.Properties.Resources", typeof(Resources).Assembly);
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
        ///   查找类似 无法找到指定条件的城市/区域列表。 的本地化字符串。
        /// </summary>
        internal static string CitiesNotFound {
            get {
                return ResourceManager.GetString("CitiesNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的城市/区域{0}。 的本地化字符串。
        /// </summary>
        internal static string CityNotFound {
            get {
                return ResourceManager.GetString("CityNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定条件的国家/地区列表。 的本地化字符串。
        /// </summary>
        internal static string CountriesNotFound {
            get {
                return ResourceManager.GetString("CountriesNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的国家/地区{0}。 的本地化字符串。
        /// </summary>
        internal static string CountryNotFound {
            get {
                return ResourceManager.GetString("CountryNotFound", resourceCulture);
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
        ///   查找类似 无法找到指定的关注{0}。 的本地化字符串。
        /// </summary>
        internal static string FollowNotFound {
            get {
                return ResourceManager.GetString("FollowNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的关注列表。 的本地化字符串。
        /// </summary>
        internal static string FollowsNotFound {
            get {
                return ResourceManager.GetString("FollowsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 生成验证码时产生了错误。 的本地化字符串。
        /// </summary>
        internal static string GenerateTokenError {
            get {
                return ResourceManager.GetString("GenerateTokenError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的群组{0}。 的本地化字符串。
        /// </summary>
        internal static string GroupNotFound {
            get {
                return ResourceManager.GetString("GroupNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 操作的帐户必须为群组所有者。 的本地化字符串。
        /// </summary>
        internal static string GroupOwnerRequired {
            get {
                return ResourceManager.GetString("GroupOwnerRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定条件的群组列表。 的本地化字符串。
        /// </summary>
        internal static string GroupsNotFound {
            get {
                return ResourceManager.GetString("GroupsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的图像文件。 的本地化字符串。
        /// </summary>
        internal static string ImageFileNotFound {
            get {
                return ResourceManager.GetString("ImageFileNotFound", resourceCulture);
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
        ///   查找类似 错误的用户编号{0}。 的本地化字符串。
        /// </summary>
        internal static string InvalidUserId {
            get {
                return ResourceManager.GetString("InvalidUserId", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的点赞。 的本地化字符串。
        /// </summary>
        internal static string LikeNotFound {
            get {
                return ResourceManager.GetString("LikeNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定条件的点赞列表。 的本地化字符串。
        /// </summary>
        internal static string LikesNotFound {
            get {
                return ResourceManager.GetString("LikesNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 必须先使用作者身份登录。 的本地化字符串。
        /// </summary>
        internal static string LoginAsAuthorRequired {
            get {
                return ResourceManager.GetString("LoginAsAuthorRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 必须先登录。 的本地化字符串。
        /// </summary>
        internal static string LoginRequired {
            get {
                return ResourceManager.GetString("LoginRequired", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的帖子{0}。 的本地化字符串。
        /// </summary>
        internal static string PostNotFound {
            get {
                return ResourceManager.GetString("PostNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定条件的帖子列表。 的本地化字符串。
        /// </summary>
        internal static string PostsNotFound {
            get {
                return ResourceManager.GetString("PostsNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 在已登录的状态下不允许进行重复登录，请先退出登录。 的本地化字符串。
        /// </summary>
        internal static string ReLoginNotAllowed {
            get {
                return ResourceManager.GetString("ReLoginNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 在已登录的状态下不允许进行注册，请先退出登录。 的本地化字符串。
        /// </summary>
        internal static string ReRegisterNotAllowed {
            get {
                return ResourceManager.GetString("ReRegisterNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 发送验证码时产生了错误。 的本地化字符串。
        /// </summary>
        internal static string SendTokenError {
            get {
                return ResourceManager.GetString("SendTokenError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的省份/直辖市/州{0}。 的本地化字符串。
        /// </summary>
        internal static string StateNotFound {
            get {
                return ResourceManager.GetString("StateNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定条件的省份/直辖市/州列表。 的本地化字符串。
        /// </summary>
        internal static string StatesNotFound {
            get {
                return ResourceManager.GetString("StatesNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法更新指定用户{0}的显示名称。 的本地化字符串。
        /// </summary>
        internal static string UserDisplayNameNotUpdated {
            get {
                return ResourceManager.GetString("UserDisplayNameNotUpdated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定的用户{0}。 的本地化字符串。
        /// </summary>
        internal static string UserNotFound {
            get {
                return ResourceManager.GetString("UserNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法更新指定的用户{0}。 的本地化字符串。
        /// </summary>
        internal static string UserNotUpdated {
            get {
                return ResourceManager.GetString("UserNotUpdated", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 无法找到指定条件的用户列表。 的本地化字符串。
        /// </summary>
        internal static string UsersNotFound {
            get {
                return ResourceManager.GetString("UsersNotFound", resourceCulture);
            }
        }
    }
}
