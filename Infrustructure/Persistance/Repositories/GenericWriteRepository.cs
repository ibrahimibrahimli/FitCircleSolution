using Application.Abstracts.Repositories;
using Domain.Common;
using Persistance.Context;

namespace Persistance.Repositories
{
    public class GenericWriteRepository<T> : IWriteRepository<T> where T : BaseAuditableEntity
    {
        readonly FitCircleDbContext _context;

        public GenericWriteRepository(FitCircleDbContext context)
        {
            _context = context;
        }

        public Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Remove(T entity)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Update(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
