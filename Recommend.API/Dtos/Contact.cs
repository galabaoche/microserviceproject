using System;

namespace Recommend.API.Dtos
{
    public class Contact
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        /// <value></value>
        public int UserId { get; set; }
        /// <summary>
        /// 好友Id
        /// </summary>
        /// <value></value>
        public int ContactId { get; set; }
    }
}
