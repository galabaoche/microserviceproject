using System;
using System.Collections.Generic;
using System.Linq;
using Project.Domain.Events;
using Project.Domain.SeedWork;

namespace Project.Domain.AggregatesModel
{
    public class Project : Entity, IAggregateRoot
    {
        public Project()
        {
            this.Contributors = new List<ProjectContributor>();
            this.Viewers = new List<ProjectViewer>();
            this.AddDomainEvent(new ProjectCreatedEvent { Project = this });
        }
        #region Properties
        /// <summary>
        /// 用户Id
        /// </summary>
        /// <value></value>
        public int UserId { get; set; }

        /// <summary>
        /// 项目logo
        /// </summary>
        /// <value></value>
        public string Avatar { get; set; }

        /// <summary>
        /// 公司名称
        /// </summary>
        /// <value></value>
        public string Company { get; set; }

        /// <summary>
        /// 原BP文件地址
        /// </summary>
        /// <value></value>
        public string OriginBPFile { get; set; }

        /// <summary>
        /// 转换后的BP文件地址
        /// </summary>
        /// <value></value>
        public string FormatBPFile { get; set; }

        /// <summary>
        /// 是否显示敏感信息
        /// </summary>
        /// <value></value>
        public bool ShowSecurityInfo { get; set; }

        /// <summary>
        /// 公司所在省Id
        /// </summary>
        /// <value></value>
        public int ProvinceId { get; set; }

        /// <summary>
        /// 公司所在省名称
        /// </summary>
        /// <value></value>
        public string Province { get; set; }

        /// <summary>
        /// 公司所在城市Id
        /// </summary>
        /// <value></value>
        public int CityId { get; set; }

        /// <summary>
        /// 公司所在城市名称
        /// </summary>
        /// <value></value>
        public string City { get; set; }

        /// <summary>
        /// 区域Id
        /// </summary>
        /// <value></value>
        public int AreaId { get; set; }

        /// <summary>
        /// 区域名称
        /// </summary>
        /// <value></value>
        public string AreaName { get; set; }

        /// <summary>
        /// 公司成立时间
        /// </summary>
        /// <value></value>
        public DateTime RegisterTime { get; set; }

        /// <summary>
        /// 项目基本信息
        /// </summary>
        /// <value></value>
        public string Introduction { get; set; }

        /// <summary>
        /// 出让股份比例
        /// </summary>
        /// <value></value>
        public string FinPercentage { get; set; }

        /// <summary>
        /// 融资阶段
        /// </summary>
        /// <value></value>
        public string FinStage { get; set; }

        /// <summary>
        /// 融资金额  单位(万)
        /// </summary>
        /// <value></value>
        public int FinMoney { get; set; }

        /// <summary>
        /// 收入  单位(万)
        /// </summary>
        /// <value></value>
        public int Income { get; set; }

        /// <summary>
        /// 利润  单位(万)
        /// </summary>
        /// <value></value>
        public int Revenue { get; set; }

        /// <summary>
        /// 估值  单位(万)
        /// </summary>
        /// <value></value>
        public int Valuation { get; set; }

        /// <summary>
        /// 佣金分配方式
        /// </summary>
        /// <value></value>
        public int BrokerageOptions { get; set; }

        /// <summary>
        /// 是否委托给finbook
        /// </summary>
        /// <value></value>
        public bool OnPlatform { get; set; }

        /// <summary>
        /// 可见范围设置
        /// </summary>
        /// <value></value>
        public ProjectVisibleRule VisibleRule { get; set; }

        /// <summary>
        /// 根引用项目Id
        /// </summary>
        /// <value></value>
        public int SourceId { get; set; }

        /// <summary>
        /// 上级引用项目Id
        /// </summary>
        /// <value></value>
        public int ReferenceId { get; set; }

        /// <summary>
        /// 项目标签
        /// </summary>
        /// <value></value>
        public string Tags { get; set; }

        /// <summary>
        /// 项目属性：行业领域、融资币种
        /// </summary>
        /// <value></value>
        public List<ProjectProperty> Properties { get; set; }

        /// <summary>
        /// 贡献者列表
        /// </summary>
        /// <value></value>
        public List<ProjectContributor> Contributors { get; set; }

        /// <summary>
        /// 查看者
        /// </summary>
        /// <value></value>
        public List<ProjectViewer> Viewers { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        /// <value></value>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        /// <value></value>
        public DateTime CreatedTime { get; private set; }
        #endregion

        private Project CloneProject(Project source = null)
        {
            if (source == null)
            {
                source = this;
            }
            var newProject = new Project
            {
                AreaId = source.AreaId,
                AreaName = source.AreaName,
                Avatar = source.Avatar,
                BrokerageOptions = source.BrokerageOptions,
                Company = source.Company,
                CityId = source.CityId,
                City = source.City,
                CreatedTime = source.CreatedTime,
                Contributors = new List<ProjectContributor>(),
                FinMoney = source.FinMoney,
                FinPercentage = source.FinPercentage,
                FinStage = source.FinStage,
                FormatBPFile = source.FormatBPFile,
                OriginBPFile = source.OriginBPFile,
                Income = source.Income,
                Introduction = source.Introduction,
                OnPlatform = source.OnPlatform,
                Properties = new List<ProjectProperty>(),
                Viewers = new List<ProjectViewer>(),
                ProvinceId = source.ProvinceId,
                Province = source.Province,
                ReferenceId = source.ReferenceId,
                RegisterTime = source.RegisterTime,
                Revenue = source.Revenue,
                ShowSecurityInfo = source.ShowSecurityInfo,
                SourceId = source.SourceId,
                Tags = source.Tags,
                UpdateTime = source.UpdateTime,
                UserId = source.UserId,
                Valuation = source.Valuation,
                VisibleRule = source.VisibleRule == null ? null : new ProjectVisibleRule
                {
                    Visible = source.VisibleRule.Visible,
                    Tags = source.VisibleRule.Tags
                },
            };

            newProject.Properties = new List<ProjectProperty>();
            foreach (var item in source.Properties)
            {
                newProject.Properties.Add(new ProjectProperty
                (
                    item.Key,
                    item.Value,
                    item.Text
                ));
            }
            return newProject;
        }

        /// <summary>
        /// 参与者得到项目拷贝
        /// </summary>
        /// <param name="contrbutorId"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public Project ContrbutorFork(int contrbutorId, Project source = null)
        {
            if (source == null) source = this;
            var newProject = CloneProject(source);
            newProject.UserId = contrbutorId;
            newProject.SourceId = source.SourceId == 0 ? source.Id : source.SourceId;
            newProject.ReferenceId = source.ReferenceId == 0 ? source.Id : source.ReferenceId;
            newProject.UpdateTime = DateTime.Now;
            return newProject;
        }

        public void AddViewer(int userId, string userName, string avatar)
        {
            var viewer = new ProjectViewer
            {
                UserId = userId,
                UserName = userName,
                Avatar = avatar
            };
            if (!this.Viewers.Any(v => v.UserId == userId))
            {
                Viewers.Add(viewer);
                AddDomainEvent(new ProjectViewedEvent
                {
                    Viewer = viewer,
                    Company = this.Company,
                    Introduction = this.Introduction
                });
            }
        }
        public void AddContributor(ProjectContributor contributor)
        {
            if (!this.Contributors.Any(c => c.UserId == contributor.UserId))
            {
                this.Contributors.Add(contributor);
                AddDomainEvent(new ProjectJoinedEvent
                {
                    Contributor = contributor,
                    Company = this.Company,
                    Introduction = this.Introduction
                });
            }
        }
    }
}
