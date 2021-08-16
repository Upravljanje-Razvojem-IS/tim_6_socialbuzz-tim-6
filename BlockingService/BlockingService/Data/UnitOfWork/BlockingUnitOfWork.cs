using BlockingService.Data.AccountMock;
using BlockingService.Data.Implementation.Interface;
using BlockingService.Data.Implementation.Repository;
using BlockingService.Models;
using System;

namespace BlockingService.Data.UnitOfWork
{
    public class BlockingUnitOfWork : IUnitOfWork
    {
        private readonly BlockingContext context;
        public IRepositoryBlocking RepositoryBlocking { get; set; }
        public IRepositoryAccount RepositoryAccount { get; set; }


        public BlockingUnitOfWork(BlockingContext context, IRepositoryAccount repositoryAccount)
        {
            this.context = context;
            RepositoryBlocking= new RepositoryBlocking(context);
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
