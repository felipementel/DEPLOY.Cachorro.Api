﻿using Asp.Versioning;
using DEPLOY.Cachorro.Application.Dtos;
using DEPLOY.Cachorro.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace DEPLOY.Cachorro.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class TutoresController : ControllerBase
    {
        private readonly ITutorAppServices _tutorAppServices;

        public TutoresController(ITutorAppServices tutorAppServices)
        {
            _tutorAppServices = tutorAppServices;
        }

        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<TutorDto>), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Listar Tutor",
            Tags = new[] { "Tutores" },
            Description = "Operação para listar tutores")]
        public async Task<IActionResult> ListAllAsync(
            CancellationToken cancellationToken = default)
        {
            var items = await _tutorAppServices.GetAllAsync(cancellationToken);

            return Ok(items);
        }

        [HttpGet("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TutorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Obter Tutor",
            Tags = new[] { "Tutores" },
            Description = "Operação para obter tutor por id")]
        public async Task<IActionResult> GetByIdAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var items = await _tutorAppServices.GetByIdAsync(id, cancellationToken);

            if (items == null)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpPost]
        [Consumes("application/json")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(TutorDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Cadastrar Tutor",
            Tags = new[] { "Tutores" },
            Description = "Operação para cadastrar tutor")]
        public async Task<IActionResult> CreateAsync(
            [FromBody] TutorDto tutorDto,
            CancellationToken cancellationToken = default)
        {
            var item = await _tutorAppServices.InsertAsync(
                tutorDto, 
                cancellationToken);

            if (item.Erros.Any())
                return UnprocessableEntity(item.Erros);

            return CreatedAtAction("GetById",
                new { 
                    id = item.Id,
                    version = new ApiVersion(
                        1,
                        0)
                    .ToString() },
                item);
        }

        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar Tutor",
            Tags = new[] { "Tutores" },
            Description = "Operação para atualizar tutor")]
        public async Task<IActionResult> UpdateAsync(
            long id,
            [FromBody] TutorDto tutorDto,
            CancellationToken cancellationToken = default)
        {
            if (id != tutorDto.Id)
            {
                return UnprocessableEntity();
            }

            var retorned = await _tutorAppServices.UpdateAsync(
                id,
                tutorDto, 
                cancellationToken);

               return !retorned.Any() ? NoContent() 
                : UnprocessableEntity(retorned);
        }

        [HttpDelete("{id}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [SwaggerOperation(
            Summary = "Excluir Tutor",
            Tags = new[] { "Tutores" },
            Description = "Operação para excluir tutor por id")]
        public async Task<IActionResult> DeleteAsync(
            long id,
            CancellationToken cancellationToken = default)
        {
            var item = await _tutorAppServices.DeleteAsync(id, cancellationToken);

            if (!item)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}
