namespace HappyCode.NetCoreBoilerplate.Core.Repositories
{
    public abstract class RepositoryBase<TEntity>
        where TEntity : class
    {
        protected EmployeesContext DbContext { get; }

        public RepositoryBase(EmployeesContext dbContext)
        {
            DbContext = dbContext;
        }
    }
}
