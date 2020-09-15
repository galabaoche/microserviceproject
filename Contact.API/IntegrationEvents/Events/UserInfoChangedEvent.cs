using System;

namespace Contact.API.IntegrationEvents.Events
{
    public class UserInfoChangedEvent : AbstractEvent
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int UserId { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get; set; }
        /// <summary>
        /// 公司职位
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 头像地址
        /// </summary>
        public string Avatar { get; set; }
    }
}
