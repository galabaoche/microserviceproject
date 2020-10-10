using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project.API.Application.Commands;
using ProjectEntity = Project.Domain.AggregatesModel.Project;
using MediatR;
using Project.Domain.AggregatesModel;
using Project.API.Application.Service;
using Project.API.Application.Queries;

namespace Project.API.Controllers
{
    [ApiController]
    [Route("/api/projects")]
    public class ProjectController : BaseController
    {
        private readonly ILogger<ProjectController> _logger;
        private readonly IMediator _mediator;
        private readonly IRecommendService _recommendService;
        private readonly IProjectQueries _projectQueries;


        public ProjectController(ILogger<ProjectController> logger, IMediator mediator, IRecommendService recommendService, IProjectQueries projectQueries)
        {
            _logger = logger;
            _mediator = mediator;
            _recommendService = recommendService;
            _projectQueries = projectQueries;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ProjectEntity>> CreateProject([FromBody] ProjectEntity project)
        {
            if (project == null)
            {
                throw new ArgumentException(nameof(Project));
            }
            project.UserId = UserIdentity.UserId;
            var command = new CreateProjectCommand { Project = project };
            var result = await _mediator.Send(command);
            return result;
        }

        [HttpPut]
        [Route("view/{projectId}")]
        public async Task<ActionResult> ViewProject(int projectId)
        {
            if (!await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                return BadRequest("没有查看此项目的权限");
            }
            var command = new ViewProjectCommand
            {
                UserId = UserIdentity.UserId,
                UserName = UserIdentity.Name,
                Avatar = UserIdentity.Avatar,
            };
            await _mediator.Send(command);
            return Ok();
        }


        [HttpPut]
        [Route("join/{projectId}")]
        public async Task<ActionResult> JoinProject([FromBody] ProjectContributor contributor)
        {
            if (!await _recommendService.IsProjectInRecommend(contributor.ProjectId, UserIdentity.UserId))
            {
                return BadRequest("没有查看此项目的权限");
            }
            var command = new JoinProjectCommand { Contributor = contributor };
            await _mediator.Send(command);
            return Ok();
        }

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetProjects()
        {
            var result = await _projectQueries.GetProjectByUserIdAsync(UserIdentity.UserId);
            return Ok(result);
        }

        [HttpGet]
        [Route("my/{projectId}")]
        public async Task<IActionResult> GetMyProjectDetail(int projectId)
        {
            var result = await _projectQueries.GetProjectDetailAsync(projectId);
            if (result.UserId == UserIdentity.UserId)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("无权查看此项目");
            }
        }

        [HttpGet]
        [Route("recommends/{projectId}")]
        public async Task<IActionResult> GetRecommendProjectDetail(int projectId)
        {
            if (await _recommendService.IsProjectInRecommend(projectId, UserIdentity.UserId))
            {
                var result = await _projectQueries.GetProjectDetailAsync(projectId);
                return Ok(result);
            }
            else
            {
                return BadRequest("无权查看此项目");
            }
        }
    }
}
