using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MySqlX.XDevAPI.Common;
using WarehouseBackend.Models;
using static WarehouseBackend.Models.TransactionDTO;

namespace WarehouseBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionsController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public TransactionsController(WarehouseContext context)
        {
            _context = context;
        }


        #region// GET: api/Transactions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Transaction>>> GetTransactions()
        {
            try
            {
                var result = await
                            (
                                from transaction in _context.Transactions

                                join material in _context.Materials
                                on transaction.MaterialId equals material.MaterialId

                                join locationFrom in _context.Locations
                                on transaction.TransactionFromId equals locationFrom.LocationId

                                join locationTo in _context.Locations
                                on transaction.TransactionToId equals locationTo.LocationId

                                join transferby in _context.Users
                                on transaction.UserId equals transferby.UserId
                                select new GetAllTransaction
                                {
                                    TransactionId = transaction.TransactionId,
                                    MaterialNumber = material.MaterialNumber,
                                    MaterialDescription = material.MaterialDescription,
                                    TransferFrom=locationFrom.LocationName,
                                    TransferTo=locationTo.LocationName,
                                    TransferedQuantity = transaction.TransactedQty,
                                    TransferBy=transferby.UserName,
                                    TransferDate=transaction.TransactionDateTime,
                                }
                            )
                            .ToListAsync();

                return Ok(new { Result = result, message = "Successfull request" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Result = "", Message = ex.Message });
            }
        }
        #endregion

        #region// GET: api/Transactions/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Transaction>> GetTransaction(int id)
        {
            var transaction = await _context.Transactions.FindAsync(id);

            if (transaction == null)
            {
                return NotFound();
            }

            return Ok(new { Result = transaction, message = "Successfull request" });
        }
        #endregion


        #region// POST: api/Transactions

        [HttpPost]
        public async Task<ActionResult> AddTransaction(CreateTransaction CreateTransaction)
        {
            
            try
            {
                var materialNumberToId = await _context.Materials.FirstOrDefaultAsync(m => m.MaterialNumber == CreateTransaction.MaterialNumber);
                var transactionFromLocationNameToId = await _context.Locations.FirstOrDefaultAsync(m => m.LocationName == CreateTransaction.TransactionFromLocationName);
                var transactionToLocationNameToId = await _context.Locations.FirstOrDefaultAsync(m => m.LocationName == CreateTransaction.TransactionToLocationName);
                var transactionUserNameToId = await _context.Users.FirstOrDefaultAsync(m => m.UserName == CreateTransaction.UserName);

                if (materialNumberToId ==null)
                {
                    return NotFound("No such material");

                }

                if (transactionFromLocationNameToId == null)
                {
                    return NotFound("Invalid transfer location from");

                }

                if (transactionToLocationNameToId == null)
                {
                    return NotFound("Invalid transfer location to");

                }

                if (transactionUserNameToId == null)
                {
                    return NotFound("No such user");

                }



                var transaction= new Transaction
                {
                 MaterialId = materialNumberToId.MaterialId,
                 TransactionFromId = transactionFromLocationNameToId.LocationId,
                 TransactionToId = transactionToLocationNameToId.LocationId,
                 TransactedQty = CreateTransaction.TransactedQty,
                 UserId= transactionUserNameToId.UserId
                };

                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetTransaction", new { id = transaction.TransactionId }, transaction);


            }
            catch (Exception ex)
            {
              return StatusCode(500, new { Result = "", Message = ex.Message });
            }
        }
        #endregion

        #region// DELETE: api/Transactions/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteTransaction(int id)
        //{
        //    var transaction = await _context.Transactions.FindAsync(id);
        //    if (transaction == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Transactions.Remove(transaction);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}
        #endregion

        //private bool TransactionExists(int id)
        //{
        //    return _context.Transactions.Any(e => e.TransactionId == id);
        //}
    }
}
