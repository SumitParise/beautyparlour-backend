using BeautyParlourAPIs.Models.DTOs;
using BeautyParlourAPIs.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BeautyParlourAPIs.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize]
	public class AccountController : ControllerBase
	{

		private readonly IAccountService _accountService;

		public AccountController(IAccountService accountService)
		{
			_accountService= accountService;
		}


		[HttpPost]
		public async Task<IActionResult> AddAccount([FromBody] CustomerAccountRequest request)
		{
			try
			{
				var result = await _accountService.AddAccountAsync(request);

				if (!result)
				{
					
					return BadRequest(new ResponseDto
					{
						Message = "Account isn't added",
						IsSuccess = false
					});
				}

				

				return Ok(new ResponseDto
				{
					Message = "Account added successfully..!",
					IsSuccess = true
				});

			}
			catch (Exception ex)
			{
				var response = new ResponseDto
				{
					Message = ex.Message,
					IsSuccess = false
				};

				return StatusCode(500, response);
			}

		}


		[HttpGet("list")]
		public async Task<IActionResult> GetAccounts()
		{
			try
			{
				var result = await _accountService.GetAllAccountsAsync();

				if (result == null)
					return NotFound();

				return Ok(result);

			}
			catch (Exception ex)
			{
				var response = new ResponseDto
				{
					Message = ex.Message,
					IsSuccess = false
				};

				return StatusCode(500,response);
			}

		}
		[HttpPost("update")]
		public async Task<IActionResult> UpdateAccount([FromBody] CustomerAccountRequest request)
		{
			try
			{
				var result = await _accountService.UpdateAccountAsync(request);
				if (!result)
				{
					return BadRequest(new ResponseDto
					{
						Message = "Account isn't updated",
						IsSuccess = false
					});
				}
				return Ok(new ResponseDto
				{
					Message = "Account updated successfully..!",
					IsSuccess = true
				});
			}
			catch (Exception ex)
			{
				var response = new ResponseDto
				{
					Message = ex.Message,
					IsSuccess = false
				};
				return StatusCode(500, response);
			}

		}

		[HttpPost("delete")]
		public async Task<IActionResult> DeleteAccount(int id)
		{
			try
			{
				var result = await _accountService.DeleteAccountAsync(id);

				if (!result)
					return BadRequest(new ResponseDto
					{
						Message = "Account Not Deleted",
						IsSuccess = false
					});

				return Ok(new ResponseDto()
				{
					Message = "Deleted Successfully",
					IsSuccess = true
				});

			}catch(Exception ex)
			{
				return StatusCode(500, new ResponseDto()
				{
					Message = ex.Message,
					IsSuccess = false
				});
			}
		}

	}
}
