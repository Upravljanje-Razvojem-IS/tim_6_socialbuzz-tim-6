using FollowingService.Data.AccountMock;
using FollowingService.Data.Implementation.Interface;
using FollowingService.Model;
using FollowingService.Model.Entity;
using FollowingService.Models.Mocks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            if(context.Followings.Where(f=>f.FollowingId== following.Account_id  && f.FollowerId == follower.Account_id).Count()>0)
            {
                return true;
            }
            return false;
        }

        public Following Follow(Account follower, Account following)
        {
          return  context.Followings.Add(new Following { FollowerId = follower.Account_id, FollowingId = following.Account_id}).Entity;
        }

        public List<Following> GetAll()
        {
            return context.Followings.ToList();
        }

        public List<Following> GetAllFollowers(Account account)
        {
            return context.Followings.Where(f => f.FollowingId == account.Account_id).ToList();
        }

        public List<Following> GetAllFollowing(Account account)
        {
            return context.Followings.Where(f => f.FollowerId == account.Account_id).ToList();
        }

        public Following Unfollow(Account follower, Account following)
        {
            Following f = context.Followings.FirstOrDefault(f => f.FollowerId == follower.Account_id && f.FollowingId == following.Account_id);
            return context.Followings.Remove(f).Entity;
        }
    }
}
