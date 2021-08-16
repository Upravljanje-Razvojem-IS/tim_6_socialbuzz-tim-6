using BlockingService.Data.Implementation.Interface;
using BlockingService.Model.Entity;
using BlockingService.Models;
using BlockingService.Models.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace BlockingService.Data.Implementation.Repository
{
    public class RepositoryBlocking : IRepositoryBlocking
    {
        private readonly BlockingContext context;

        public RepositoryBlocking(BlockingContext context)
        {
            this.context = context;
        }
        public Blocking Block(Account blocker, Account blocked)
        {
            return context.Blocks.Add(new Blocking { BlockerId = blocker.Account_id, BlockedId = blocked.Account_id }).Entity;
        }

        public bool Find(Account blocker, Account blocked)
        {
            if (context.Blocks.Any(f => f.BlockerId == blocker.Account_id && f.BlockedId == blocked.Account_id))
            {
                return true;
            }
            return false;
        }

        public List<Blocking> GetAll()
        {
            return context.Blocks.ToList();
        }

        public List<Blocking> GetAllBlockedAccounts(Account account)
        {
            return context.Blocks.Where(f => f.BlockerId == account.Account_id).ToList();
        }

        public Blocking Unblock(Account blocker, Account blocked)
        {
            Blocking f = context.Blocks.FirstOrDefault(f => f.BlockerId == blocker.Account_id && f.BlockedId == blocked.Account_id);
            return context.Blocks.Remove(f).Entity;
        }
    }
}
