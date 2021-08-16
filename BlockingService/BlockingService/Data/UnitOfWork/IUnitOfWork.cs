using BlockingService.Data.AccountMock;
using BlockingService.Data.Implementation.Interface;
using System;

namespace BlockingService.Data.UnitOfWork
{
    public interface IUnitOfWork :IDisposable
    {
        public IRepositoryBlocking RepositoryBlocking { get; set; }
        public IRepositoryAccount RepositoryAccount { get; set; }
        void Commit();
    }
}
