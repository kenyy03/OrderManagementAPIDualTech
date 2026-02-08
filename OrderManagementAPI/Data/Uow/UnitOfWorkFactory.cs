using Microsoft.EntityFrameworkCore;
using OrderManagementAPI.Data.Uow.Enums;
using OrderManagementAPI.Data.Uow.Interfaces;

namespace OrderManagementAPI.Data.Uow
{
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private Dictionary<UnitOfWorkType, IUnitOfWork> _unitOfWorks;
        private Dictionary<UnitOfWorkType, DbContext> _contexts;

        public UnitOfWorkFactory()
        {
            _unitOfWorks = [];
            _contexts = [];
        }

        public IUnitOfWork CreateUnitOfWork(UnitOfWorkType unitOfWorkType)
        {
            if (_unitOfWorks.TryGetValue(unitOfWorkType, out IUnitOfWork? value))
            {
                return value;
            }

            DbContext context = _contexts[unitOfWorkType];
            UnitOfWork unitOfWork = new(context);
            _unitOfWorks.Add(unitOfWorkType, unitOfWork);
            return unitOfWork;
        }

        public void RegisterUnitOfWork<TDbContext>(UnitOfWorkType unitOfWorkType, TDbContext context)
        {
            if (context is not DbContext dbContext)
            {
                throw new InvalidOperationException($"El contexto debe ser un DbContext, pero se recibió {context!.GetType().Name}");
            }
            _contexts.Add(unitOfWorkType, dbContext);
        }
    }
}
