using System;
using System.Threading.Tasks;
using LoanApp.Entities.Loan.Dto;
using LoanApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace LoanApp.Controllers
{
    [Route("api/loans")]
    public class LoanController : Controller
    {
        private readonly ILoanService _loanService;

        public LoanController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateLoanDto createLoanDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var loan = await _loanService.Create(createLoanDto);
                return StatusCode(201, loan);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllUserLoans(int userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                return new OkObjectResult(await _loanService.GetUserLoans(userId));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferCash([FromBody] CreateLoanCashTransferDto createLoanCashTransferDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var transfer = await _loanService.TransferCash(createLoanCashTransferDto);
                return StatusCode(201, transfer);
            }
            catch (Exception ex) when (ex is ArgumentException || ex is InvalidOperationException)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}