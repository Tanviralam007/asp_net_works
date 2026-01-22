using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ToolShare.API.DTOs.Transaction;
using ToolShare.BLL.Interfaces.Services;

namespace ToolShare.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly IToolTransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionsController(IToolTransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        // GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TransactionResponseDTO>> GetTransactionById(int id)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByIdAsync(id);
                if (transaction == null)
                    return NotFound(new { message = $"Transaction with ID {id} not found" });

                var transactionDto = _mapper.Map<TransactionResponseDTO>(transaction);
                return Ok(transactionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the transaction", error = ex.Message });
            }
        }

        // GET: api/Transactions/request/2
        [HttpGet("request/{requestId}")]
        public async Task<ActionResult<TransactionResponseDTO>> GetTransactionByRequestId(int requestId)
        {
            try
            {
                var transaction = await _transactionService.GetTransactionByRequestIdAsync(requestId);
                if (transaction == null)
                    return NotFound(new { message = $"Transaction for request ID {requestId} not found" });

                var transactionDto = _mapper.Map<TransactionResponseDTO>(transaction);
                return Ok(transactionDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving the transaction", error = ex.Message });
            }
        }

        // GET: api/Transactions/active
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDTO>>> GetActiveTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetActiveTransactionsAsync();
                var transactionDtos = _mapper.Map<IEnumerable<TransactionResponseDTO>>(transactions);
                return Ok(transactionDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving active transactions", error = ex.Message });
            }
        }

        // GET: api/Transactions/overdue
        [HttpGet("overdue")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDTO>>> GetOverdueTransactions()
        {
            try
            {
                var transactions = await _transactionService.GetOverdueTransactionsAsync();
                var transactionDtos = _mapper.Map<IEnumerable<TransactionResponseDTO>>(transactions);
                return Ok(transactionDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving overdue transactions", error = ex.Message });
            }
        }

        // GET: api/Transactions/borrower/1
        [HttpGet("borrower/{borrowerId}")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDTO>>> GetTransactionsByBorrower(int borrowerId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByBorrowerAsync(borrowerId);
                var transactionDtos = _mapper.Map<IEnumerable<TransactionResponseDTO>>(transactions);
                return Ok(transactionDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving borrower transactions", error = ex.Message });
            }
        }

        // GET: api/Transactions/owner/3
        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<IEnumerable<TransactionResponseDTO>>> GetTransactionsByOwner(int ownerId)
        {
            try
            {
                var transactions = await _transactionService.GetTransactionsByOwnerAsync(ownerId);
                var transactionDtos = _mapper.Map<IEnumerable<TransactionResponseDTO>>(transactions);
                return Ok(transactionDtos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while retrieving owner transactions", error = ex.Message });
            }
        }

        // POST: api/Transactions/handover?requestId=2&ownerId=3
        [HttpPost("handover")]
        public async Task<ActionResult<TransactionResponseDTO>> ConfirmHandover(
            [FromQuery] int requestId,
            [FromQuery] int ownerId)
        {
            try
            {
                var transaction = await _transactionService.ConfirmHandoverAsync(requestId, ownerId);
                var transactionDto = _mapper.Map<TransactionResponseDTO>(transaction);

                return CreatedAtAction(nameof(GetTransactionById), new { id = transactionDto.Id }, transactionDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while confirming handover", error = ex.Message });
            }
        }

        // PUT: api/Transactions/1/return?ownerId=3
        [HttpPut("{transactionId}/return")]
        public async Task<ActionResult<TransactionResponseDTO>> ProcessReturn(int transactionId, [FromQuery] int ownerId)
        {
            try
            {
                var transaction = await _transactionService.ProcessReturnAsync(transactionId, ownerId);
                var transactionDto = _mapper.Map<TransactionResponseDTO>(transaction);

                return Ok(transactionDto);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (UnauthorizedAccessException ex)
            {
                return StatusCode(403, new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing return", error = ex.Message });
            }
        }

        // GET: api/Transactions/calculate-fine?expectedDate=2026-01-10&actualDate=2026-01-15&dailyRate=150
        [HttpGet("calculate-fine")]
        public async Task<ActionResult<object>> CalculateFine(
            [FromQuery] DateTime expectedDate,
            [FromQuery] DateTime actualDate,
            [FromQuery] decimal dailyRate)
        {
            try
            {
                if(expectedDate.Year < 2000 || actualDate.Year < 2000)
                    return BadRequest(new { message = "Dates must be valid and after the year 2000." });

                var fine = await _transactionService.CalculateFineAsync(expectedDate, actualDate, dailyRate);
                var lateDays = (actualDate - expectedDate).Days;

                return Ok(new
                {
                    expectedReturnDate = expectedDate,
                    actualReturnDate = actualDate,
                    lateDays = lateDays > 0 ? lateDays : 0,
                    dailyRate,
                    fineAmount = fine
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while calculating fine", error = ex.Message });
            }
        }
    }
}
