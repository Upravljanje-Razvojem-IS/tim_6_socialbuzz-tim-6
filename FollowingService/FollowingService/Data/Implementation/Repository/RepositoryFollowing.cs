using FollowingService.Data.Implementation.Interface;
using FollowingService.Model;
using FollowingService.Model.Entity;
using FollowingService.Models.Mocks;
using System.Collections.Generic;
using System.Linq;

namespace FollowingService.Data.Implementation.Repository
{
    public class RepositoryFollowing : IRepositoryFollowing
    {
        private FollowingContext context;

        public RepositoryFollowing(FollowingContext context)
        {
            this.context = context;
        }

        public bool Find(Account follower, Account following)
        {
            if(context.Follows.Where(f=>f.FollowingId== following.Account_id  && f.FollowerId == follower.Account_id).Count()>0)
            {
                return true;
            }
            return false;
        }

        public Following Follow(Account follower, Account following)
        {
          return  context.Follows.Add(new Following { FollowerId = follower.Account_id, FollowingId = following.Account_id}).Entity;
        }

        public List<Following> GetAll()
        {
            return context.Follows.ToList();
        }

        public List<Following> GetAllFollowers(Account account)
        {
            return context.Follows.Where(f => f.FollowingId == account.Account_id).ToList();
        }

        public List<Following> GetAllFollowing(Account account)
        {
            return context.Follows.Where(f => f.FollowerId == account.Account_id).ToList();
        }

        public Following Unfollow(Account follower, Account following)
        {
            Following f = context.Follows.FirstOrDefault(f => f.FollowerId == follower.Account_id && f.FollowingId == following.Account_id);
            return context.Follows.Remove(f).Entity;
        }
    }
}
