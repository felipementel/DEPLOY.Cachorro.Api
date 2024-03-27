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

        public async Task BeginTransactionAsync(CancellationToken cancellationToken)
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        public async Task<bool> CommitAndSaveChangesAsync(CancellationToken cancellationToken)
        {
            return await _context.SaveChangesAsync(cancellationToken) > 0;
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
