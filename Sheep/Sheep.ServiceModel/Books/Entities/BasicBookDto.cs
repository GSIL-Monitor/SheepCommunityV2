using System.Runtime.Serialization;
using ServiceStack.Model;

namespace Sheep.ServiceModel.Books.Entities
{
    /// <summary>
    ///     基本书籍信息。
    /// </summary>
    [DataContract]
    public class BasicBookDto : IHasStringId
    {
        /// <summary>
        ///     编号。
        /// </summary>
        [DataMember(Order = 1)]
        public string Id { get; set; }

        /// <summary>
        ///     标题。
        /// </summary>
        [DataMember(Order = 2)]
        public string Title { get; set; }
    }
}