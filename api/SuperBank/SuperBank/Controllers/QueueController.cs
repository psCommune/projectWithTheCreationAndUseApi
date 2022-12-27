using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SuperBank.Domain;

namespace SuperBank.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QueueController : ControllerBase
    {
        private readonly IBankQueue queue;

        public QueueController(IBankQueue queue)
        {
            this.queue = queue;
        }

        [HttpGet("categories")]
        public IActionResult GetCategories()
        {
            return Ok(queue.Categories);
        }

        [HttpGet("questions")]
        public IActionResult GetQuestions()
        {
            return Ok(queue.Questions.Select(q => new
            {
                Code = q.QuestionCode,
                Manager = q.Manager?.Number
            }));
        }

        [HttpGet("questions/{number:int}")]
        public IActionResult GetManagerWork(int number)
        {
            if (number > 0)
            {
                return Ok(queue.CurrentQuestion(number));
            }
            else return NotFound("Invalid Manager");
        }

        [HttpPost("register/{categoryCode}")]
        public IActionResult Register(string categoryCode)        {
            var category = queue
                .Categories
                .FirstOrDefault(c => c.CategoryCode.ToUpper() == categoryCode.ToUpper());

            if (category == null)
            {
                return NotFound("Invalid category");
            }

            return Ok(queue.Register(category));
        }


        [HttpDelete("completeWork/{questionCode}")]
        public IActionResult CompleteWork(string questionCode)
        {
            var question = queue
                .Questions
                .FirstOrDefault(q => q.QuestionCode.ToUpper() == questionCode.ToUpper());

            if (question == null)
            {
                return NotFound($"Question code {questionCode} is not found!");
            }

            queue.CloseQuestion(question);
            return Ok(queue.Questions);
        }
    }

}
