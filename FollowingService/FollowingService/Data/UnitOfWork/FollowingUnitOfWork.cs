using FollowingService.Data.AccountMock;
using FollowingService.Data.Implementation.Interface;
using FollowingService.Data.Implementation.Repository;
using FollowingService.Model;
using System;

namespace FollowingService.Data.UnitOfWork
{
    public class FollowingUnitOfWork : IUnitOfWork
    {
        private readonly FollowingContext context;
        public IRepositoryFollowing RepositoryFollowing { get; set; }
        public IRepositoryAccount RepositoryAccount { get; set; }

        public FollowingUnitOfWork(FollowingContext context, IRepositoryAccount repositoryAccount)
        {
            this.context = context;
            RepositoryFollowing = new RepositoryFollowing(context);
            RepositoryAccount = repositoryAccount;
        }
        public void Commit()
        {
            context.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
