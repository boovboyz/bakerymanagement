using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace bakerymanagement.Hubs
{
    public class InventoryHub : Hub
    {
        // Method to send updated inventory to clients
        public async Task UpdateInventory(int productId, int quantity)
        {
            await Clients.All.SendAsync("UpdateInventory", productId, quantity);
        }
    }
}
