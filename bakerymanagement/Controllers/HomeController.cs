using bakerymanagement.Data;
using bakerymanagement.Hubs;
using bakerymanagement.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace bakerymanagement.Controllers
{
    public class HomeController : Controller
    {
        private readonly BakeryContext _context;
        private readonly IHubContext<InventoryHub> _hubContext;

        public HomeController(BakeryContext context, IHubContext<InventoryHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Displays the main inventory page.
        /// </summary>
        /// <returns>The inventory view.</returns>
        public IActionResult Index()
        {
            var inventory = _context.GetInventory();
            return View(inventory);
        }

        /// <summary>
        /// Displays the update inventory page.
        /// </summary>
        /// <returns>The update inventory view.</returns>
        public IActionResult UpdateInventory()
        {
            var products = _context.GetInventory();

            var model = new UpdateInventoryViewModel
            {
                ProductUpdates = products.Select(p => new ProductUpdateModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CurrentQuantity = p.Quantity,
                    QuantityChanged = 0
                }).ToList()
            };

            return View(model);
        }

        /// <summary>
        /// Handles the submission of inventory updates.
        /// </summary>
        /// <param name="model">The update inventory view model containing user input.</param>
        /// <returns>A redirect to the main inventory page.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateInventory(UpdateInventoryViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserName))
            {
                ModelState.AddModelError("UserName", "User name is required.");
            }

            if (!ModelState.IsValid)
            {
                // Re-fetch current quantities in case of validation errors
                var products = _context.GetInventory();
                model.ProductUpdates = products.Select(p => new ProductUpdateModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.ProductName,
                    CurrentQuantity = p.Quantity,
                    QuantityChanged = model.ProductUpdates.FirstOrDefault(u => u.ProductId == p.ProductId)?.QuantityChanged ?? 0
                }).ToList();

                return View(model);
            }

            foreach (var update in model.ProductUpdates)
            {
                if (update.QuantityChanged != 0)
                {
                    _context.UpdateInventory(model.UserName, update.ProductId, update.QuantityChanged);

                    // Get updated quantity
                    var updatedProduct = _context.GetInventory().FirstOrDefault(p => p.ProductId == update.ProductId);

                    // Notify clients about the update
                    await _hubContext.Clients.All.SendAsync("UpdateInventory", update.ProductId, updatedProduct.Quantity);
                }
            }

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Displays the transactions and leaderboard page.
        /// </summary>
        /// <returns>The transactions view.</returns>
        public IActionResult Transactions()
        {
            var transactions = _context.GetTransactions();
            var leaderboard = _context.GetLeaderboard();
            var model = new TransactionsViewModel
            {
                Transactions = transactions,
                Leaderboard = leaderboard
            };
            return View(model);
        }

        /// <summary>
        /// Handles the reset of inventory and transaction history.
        /// </summary>
        /// <returns>A redirect to the main inventory page.</returns>
        [HttpPost]
        public async Task<IActionResult> ResetInventory()
        {
            _context.ResetInventory();

            // Notify clients to refresh their inventory data
            var inventory = _context.GetInventory();
            foreach (var product in inventory)
            {
                await _hubContext.Clients.All.SendAsync("UpdateInventory", product.ProductId, product.Quantity);
            }

            // Redirect to the Inventory page
            return RedirectToAction("Index");
        }
    }
}
