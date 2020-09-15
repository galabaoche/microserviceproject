namespace Recommend.API.Dtos
{
    public class UserIdentity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        /// <value></value>
        public int UserId { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        /// <value></value>
        public string Avatar { get; set; }
        public string Title { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
    }
}
