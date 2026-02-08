using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderManagementAPI.Data.Uow.Interfaces;
using System.Data;
using System.Data.Common;

namespace OrderManagementAPI.Data.Uow
{
    public class UnitOfWork(DbContext context) : IUnitOfWork
    {
        private readonly DbContext _context = context;
        private IDbContextTransaction? _transaction;

        public void BeginTransaction()
        {
            _transaction ??= _context.Database.BeginTransaction();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction ??= await _context.Database.BeginTransactionAsync();
        }

        public void Commit()
        {
            if (_transaction is not null)
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task CommitAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            if (_context is not null)
            {
                _context.Dispose();
            }
        }

        public List<T> RawSqlQuery<T>(string query, Func<DbDataReader, T> map, int timeOut = 30, params object[] parameters) where T : class
        {
            using DbConnection connection = _context.Database.GetDbConnection();
            using DbCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = query;
            command.CommandTimeout = timeOut;
            command.Parameters.AddRange(parameters);
            List<T> result = new();
            try
            {
                connection.Open();
                using DbDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

                while (reader.Read())
                {
                    result.Add(map(reader));
                }
            }
            finally
            {
                command.Cancel();
                command.Dispose();
                connection.Close();
                connection.Dispose();
            }

            return result;
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            return new Repository<TEntity>(_context);
        }

        public void RollBack()
        {
            if (_transaction is not null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
        }

        public async Task RollBackAsync()
        {
            if (_transaction is not null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Save()
        {
            try
            {
                if (_transaction is null)
                {
                    BeginTransaction();
                    _context.SaveChanges();
                    Commit();
                    return;
                }
                _context.SaveChanges();
            }
            catch (Exception)
            {
                RollBack();
            }
        }

        public async Task SaveAsync()
        {
            try
            {
                if (_transaction is null)
                {
                    await BeginTransactionAsync();
                    await _context.SaveChangesAsync();
                    await CommitAsync();
                    return;
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                RollBack();
            }
        }

        public void SetCommandTimeout(int seconds)
            => _context.Database.SetCommandTimeout(TimeSpan.FromSeconds(seconds));
    }
}
