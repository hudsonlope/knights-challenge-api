using Knights.Challenge.Domain.DTOs.Request;
using Knights.Challenge.Domain.DTOs.Response;
using Knights.Challenge.Domain.Enums;
using Knights.Challenge.Domain.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Knights.Challenge.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KnightsController : ControllerBase
    {
        private readonly IKnightService _knightService;

        public KnightsController(IKnightService knightService)
        {
            _knightService = knightService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<KnightResponseDTO>>> GetAllKnights()
        {
            var knights = await _knightService.GetAllKnights();

            if (!knights.Any())
                return NoContent();

            return Ok(knights);
        }

        [HttpGet("filter={filter}")]
        public async Task<ActionResult<IEnumerable<KnightResponseDTO>>> GetHeroKnights(CategoryEnum filter)
        {
            if (filter == CategoryEnum.Hero)
            {
                var heroKnights = await _knightService.GetHeroKnights();

                if (!heroKnights.Any())
                    return NoContent();

                return Ok(heroKnights);
            }
            else
            {
                return BadRequest("Filtro inválido. Utilize 'Hero' para obter apenas os heróis.");
            }
        }

        [HttpPost]
        public async Task<ActionResult<KnightResponseDTO>> CreateKnight(KnightRequestDTO knight)
        {
            var createdKnight = await _knightService.CreateKnight(knight);
            return CreatedAtAction(nameof(GetKnightById), new { id = createdKnight.Id }, createdKnight);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<KnightResponseDTO>> GetKnightById(string id)
        {
            var knight = await _knightService.GetKnightById(id);
            if (knight == null)
            {
                return NotFound();
            }
            return Ok(knight);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteKnight(string id)
        {
            var found = await _knightService.UpdateKnightDeleted(id);

            if (!found)
                return NotFound();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateKnight(string id, [FromBody] NewNicknameRequestDTO newNicknameRequestDTO)
        {
            var updatedKnight = await _knightService.UpdateKnightNickname(id, newNicknameRequestDTO);

            if (updatedKnight == null)
                return NotFound();

            return Ok(updatedKnight);
        }
    }
}
