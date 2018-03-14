using System.Runtime.Serialization;

namespace JPush.Push
{
    /// <summary>
    ///     推送响应。
    /// </summary>
    [DataContract]
    public class PushResponse
    {
        #region 属性

        /// <summary>
        ///     错误信息。
        /// </summary>
        [DataMember(Order = 1, Name = "error")]
        public Error Error { get; set; }

        #endregion
    }
}