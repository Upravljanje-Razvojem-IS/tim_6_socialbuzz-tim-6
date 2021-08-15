using FollowingService.Data.AccountMock;
using FollowingService.Data.Implementation.Interface;
using System;

namespace FollowingService.Data.UnitOfWork
{
    public interface IUnitOfWork :IDisposable
    {
        public IRepositoryFollowing RepositoryFollowing { get; set; }
        public IRepositoryAccount RepositoryAccount { get; set; }
        void Commit();
    }
}
