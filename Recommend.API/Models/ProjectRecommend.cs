using System;
using System.Collections.Generic;

namespace Recommend.API.Models
{
    public class ProjectRecommend
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserAvatar { get; set; }
        public EnumReCommendType ReCommendType { get; set; }
        public int ProjectId { get; set; }
        public string ProjectAvatar { get; set; }
        public string Company { get; set; }
        public string Introduction { get; set; }
        public string Tags { get; set; }
        /// <summary>
        /// 融资阶段
        /// </summary>
        public string FinStage { get; set; }
        public DateTime CreatedTime { get; set; }
        public DateTime ReCommendTime { get; set; }
        public List<ProjectReferenceUser> ProjectReferenceUsers { get; set; }
    }
}
