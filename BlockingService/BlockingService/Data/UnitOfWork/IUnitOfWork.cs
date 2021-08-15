using BlockingService.Data.AccountMock;
using BlockingService.Data.Implementation.Interface;

namespace BlockingService.Data.UnitOfWork
{
    public interface IUnitOfWork
    {
        public IRepositoryBlocking RepositoryBlocking { get; set; }
        public IRepositoryAccount RepositoryAccount { get; set; }
        void Commit();
    }
}
