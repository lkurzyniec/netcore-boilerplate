namespace HappyCode.NetCoreBoilerplate.Core.Repositories
{
    public interface IRepository<TEntity>
        where TEntity : class
    {

    }

    public abstract class RepositoryBase<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        protected EmployeesContext DbContext { get; }

        public RepositoryBase(EmployeesContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
