using System;
using System.Collections.Generic;
using System.Linq;

namespace User.API.Models
{
    public class AppUser
    {
        public int Id { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        /// <value></value>
        public string Company { get; set; }

        /// <summary>
        /// 职位
        /// </summary>
        /// <value></value>
        public string Title { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        /// <value></value>
        public string Phone { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>
        /// <value></value>
        public string Avatar { get; set; }

        /// <summary>
        /// 性别 1:男 0:女
        /// </summary>
        /// <value></value>
        public byte Gender { get; set; }

        /// <summary>
        /// 详细地址
        /// </summary>
        /// <value></value>
        public string Address { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        /// <value></value>
        public string Email { get; set; }

        /// <summary>
        /// 电话
        /// </summary>
        /// <value></value>
        public string Tel { get; set; }

        /// <summary>
        /// 省Id
        /// </summary>
        /// <value></value>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 省名称
        /// </summary>
        /// <value></value>
        public string Province { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        /// <value></value>
        public int CityId { get; set; }

        /// <summary>
        /// 城市名称
        /// </summary>
        /// <value></value>
        public string City { get; set; }

        /// <summary>
        /// 名片地址
        /// </summary>
        /// <value></value>
        public string NameCard { get; set; }

        /// <summary>
        /// 用户属性列表
        /// </summary>
        /// <value></value>
        public List<UserProperty> Properties { get; set; } = new List<UserProperty>();
    }
}
