using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WordleBackend.Services;

namespace WordleBackend.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class WordController : ControllerBase
    {
        private readonly IWordService _wordService;

        public WordController(IWordService wordService)
        {
            _wordService = wordService;
        }

        [HttpPost]
        public async Task<IActionResult> AddWord([FromBody] WordRequest request)
        {
            try
            {
                var word = await _wordService.AddWordAsync(request.Text, request.CategoryId);
                return Ok(word);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("today")]
        public async Task<IActionResult> SetTodaysWord([FromBody] WordRequest request)
        {
            try
            {
                var word = await _wordService.SetTodaysWordAsync(request.Text);
                return Ok(word);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetWords([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var words = await _wordService.GetWordsAsync(page, pageSize);
                return Ok(words);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetWord(int id)
        {
            try
            {
                var word = await _wordService.GetWordByIdAsync(id);
                if (word == null)
                {
                    return NotFound(new { message = "کلمه یافت نشد" });
                }
                return Ok(word);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWord(int id)
        {
            try
            {
                var result = await _wordService.DeleteWordAsync(id);
                if (!result)
                {
                    return NotFound(new { message = "کلمه یافت نشد" });
                }
                return Ok(new { message = "کلمه با موفقیت حذف شد" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("validate")]
        public async Task<IActionResult> ValidateWord([FromQuery] string text)
        {
            try
            {
                var isValid = await _wordService.IsValidWordAsync(text);
                return Ok(new { isValid });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    public class WordRequest
    {
        public string Text { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public string Language { get; set; } = "fa";
        public string Difficulty { get; set; } = "medium";
    }
} 