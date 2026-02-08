using OrderManagementAPI.Data.Uow.Enums;

namespace OrderManagementAPI.Data.Uow.Interfaces
{
    public interface IUnitOfWorkFactory
    {
        void RegisterUnitOfWork<TDbContext>(UnitOfWorkType unitOfWorkType, TDbContext context);
        IUnitOfWork CreateUnitOfWork(UnitOfWorkType unitOfWorkType);
    }
}
