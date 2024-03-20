using Microsoft.EntityFrameworkCore;

namespace DEPLOY.Cachorro.Infra.Repository.Repositories.Base
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly CachorroDbContext _context;

        public UnitOfWork(CachorroDbContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<bool> CommitAndSaveChangesAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
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
