using System;
using System.Threading.Tasks;
using LoanApp.Entities.Base.Dto;
using LoanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            return new OkObjectResult(await _userService.GetAll());
        }

        [HttpGet("borrowers")]
        public async Task<IActionResult> GetAllBorrowersAsync()
        {
            return new OkObjectResult(await _userService.GetAllBorrowers());
        }

        [HttpGet("lenders")]
        public async Task<IActionResult> GetAllLendersAsync()
        {
            return new OkObjectResult(await _userService.GetAllLenders());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userService.Create(createUserDto);
            return StatusCode(201, user);
        }

        [HttpPost("transfer/{userId}/{amount}")]
        public async Task<IActionResult> TransferCashToUser(int userId, ulong amount)
        {
            try
            {
                await _userService.TransferCashToUser(userId, amount);
            }
            catch (Exception ex) when(ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }

            return Ok();
        }
    }
}