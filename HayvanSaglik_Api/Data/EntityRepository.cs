using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HayvanSaglik_Api.Data
{
    public class EntityRepository : IEntityRepository
    {
        private ApplicationDbContext _applicationDbContext;

        public EntityRepository(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public void Add<T>(T entity) where T : class
        {
            _applicationDbContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _applicationDbContext.Remove(entity);
        }

        public bool SaveAll()
        {
            return _applicationDbContext.SaveChanges() > 0;
        }

        public void Update<T>(T entity) where T : class
        {
            // TODO:
        }
    }
}
