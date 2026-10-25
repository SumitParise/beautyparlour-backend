using BeautyParlourAPIs.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautyParlourAPIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FeedbackController : ControllerBase	
	{
		private readonly IFeedback _feedbackService;

		public FeedbackController(IFeedback feedbackService)
		{
			_feedbackService = feedbackService;
		}

		[HttpPost]
		public async Task<IActionResult> AddFeedback(string customerName, int rating, string comments)
		{
			try
			{
				await _feedbackService.Add(customerName, rating, comments);
				return Ok("Feedback added successfully.");
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while adding feedback.");
			}
		}

		[HttpGet("list")]
		public async Task<IActionResult> GetList()
		{
			try
			{
				var feedbacks = await _feedbackService.GetListAsync();
				return Ok(feedbacks);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while retrieving feedbacks.");
			}
		}

		[HttpGet("admin/list")]
		public async Task<IActionResult> GetAdminList()
		{
			try
			{
				var feedbacks = await _feedbackService.GetListForAdminAsync();
				return Ok(feedbacks);
			}
			catch (Exception ex)
			{
				return StatusCode(500, "An error occurred while retrieving feedbacks.");
			}
		}

		[HttpPost("getbyid")]
		public async Task<IActionResult> GetByID([FromBody] int id)
		{
			try
			{
				var feedback = await _feedbackService.GetByID(id);
				if (feedback == null)
				{
					return NotFound("Feedback not found.");
				}
				return Ok(feedback);
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while retrieving the feedback.");
			}

		}

		[HttpPost("update")]
		public async Task<IActionResult> UpdateFeedback(int feedbackID,string customerName, int rating, string comments,bool isActive)
		{
			try
			{
				await _feedbackService.Update(feedbackID, customerName, rating, comments, isActive);
				return Ok("Feedback updated successfully.");
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while updating feedback.");
			}
		}

		[HttpPost("delete")]
		public async Task<IActionResult> Delete([FromQuery] int id)
		{
			try
			{
				var feedback = await _feedbackService.GetByID(id);

				if (feedback == null)
				{
					return NotFound("Feedback not found.");
				}

				await _feedbackService.Delete(id);

				return Ok("Feedback Deleted Successfully..!");
			}
			catch (Exception) { 
			return StatusCode(500, "An error occurred while deleting the feedback.");	
			}
		}

		[HttpPost("updateisactive")]
		public async Task<IActionResult> UpdateIsActive([FromQuery] int feedbackID)
		{
			try
			{
				var result = await _feedbackService.UpdateIsActive(feedbackID);
				if (!result)
				{
					return BadRequest("Failed to update IsActive status.");
				}
				return Ok("IsActive status updated successfully.");
			}
			catch (ArgumentException ex)
			{
				return BadRequest(ex.Message);
			}
			catch (Exception)
			{
				return StatusCode(500, "An error occurred while updating IsActive status.");
			}
		}



	}
}
