using FollowingService.Model.Entity;
using FollowingService.Models.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowingService.Data.Implementation.Interface
{
    public interface IRepositoryFollowing
    {
        Following Follow(Account follower, Account following);
        Following Unfollow(Account follower, Account following);
        List<Following> GetAllFollowers(Account account);
        List<Following> GetAllFollowing(Account account);
        List<Following> GetAll();
        bool Find(Account follower, Account following);
    }
}
