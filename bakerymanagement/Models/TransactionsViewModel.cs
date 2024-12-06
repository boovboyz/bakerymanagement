using System.Collections.Generic;

namespace bakerymanagement.Models
{
    public class TransactionsViewModel
    {
        public List<Transaction> Transactions { get; set; }
        public List<UserLeaderboard> Leaderboard { get; set; }
    }
}
