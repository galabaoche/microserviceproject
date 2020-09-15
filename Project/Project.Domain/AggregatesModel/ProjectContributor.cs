using System;
using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public class ProjectContributor : Entity
    {
        public int ProjectId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Avatar { get; set; }
        public DateTime CreatedTime { get; set; }

        /// <summary>
        /// 关闭者
        /// </summary>
        /// <value></value>
        public bool IsCloser { get; set; }

        /// <summary>
        /// 1:财务顾问  2:投资机构
        /// </summary>
        /// <value></value>
        public int ContributorType { get; set; }
    }
}
