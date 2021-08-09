using FollowingService.Data.AccountMock;
using FollowingService.Data.Implementation.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FollowingService.Data.UnitOfWork
{
    public interface IUnitOfWork :IDisposable
    {
        public IRepositoryFollowing RepositoryFollowing { get; set; }
        public IRepositoryAccount RepositoryAccount { get; set; }
        void Commit();
    }
}
