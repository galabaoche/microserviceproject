using System;
using MongoDB.Bson.Serialization.Attributes;

namespace Contact.API.Models
{
    [BsonIgnoreExtraElements]
    public class ContactApplyRequest
    {
        /// <summary>
        /// 被申请人Id
        /// </summary>
        /// <value></value>
        public int UserId { get; set; }
        /// <summary>
        /// 申请人名称
        /// </summary>
        /// <value></value>
        public string Name { get; set; }
        /// <summary>
        /// 申请人公司
        /// </summary>
        /// <value></value>
        public string Company { get; set; }
        /// <summary>
        /// 申请人工作岗位
        /// </summary>
        /// <value></value>
        public string Title { get; set; }
        /// <summary>
        /// 申请人用户头像
        /// </summary>
        /// <value></value>
        public string Avatar { get; set; }
        /// <summary>
        /// 申请人Id
        /// </summary>
        /// <value></value>
        public int ApplierId { get; set; }

        /// <summary>
        /// 是否通过 0 未通过， 1 已通过
        /// </summary>
        /// <value></value>
        public int Approvaled { get; set; }

        /// <summary>
        /// 处理时间
        /// </summary>
        /// <value></value>
        public DateTime HandledTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime ApplyTime { get; set; }
    }
}