using System;
using System.Collections.Generic;

namespace Contact.API.Models
{
    public class Contact
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
        /// 公司
        /// </summary>
        /// <value></value>
        public string Company { get; set; }
        /// <summary>
        /// 工作岗位
        /// </summary>
        /// <value></value>
        public string Title { get; set; }
        /// <summary>
        /// 用户头像
        /// </summary>
        /// <value></value>
        public string Avatar { get; set; }

        /// <summary>
        /// 用户标签
        /// </summary>
        /// <value></value>
        public List<string> Tags { get; set; } = new List<string>();
    }
}
