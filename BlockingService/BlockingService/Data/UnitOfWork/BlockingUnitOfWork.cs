using BlockingService.Data.AccountMock;
using BlockingService.Data.Implementation.Interface;
using BlockingService.Data.Implementation.Repository;
using BlockingService.Models;
using System;

namespace BlockingService.Data.UnitOfWork
{
    public class BlockingUnitOfWork : IUnitOfWork, IDisposable
    {
        private BlockingContext context;
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

        public void Dispose()
        {
            context.Dispose();
        }
    }
}
