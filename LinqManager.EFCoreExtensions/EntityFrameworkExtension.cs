using LinqManager.Constants;
using LinqManager.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

[assembly: AssemblyKeyFile("LinqManager.EfCoreExtensions.snk")]
namespace LinqManager.EFCoreExtensions
{   
    public static class EntityFrameworkExtension
    {
        public async static Task<LinqAsyncResponse<DbType>> ProcessAsync<DbType, DtoType>(this LinqManager linqManager, 
            IQueryable<DbType> source, LinqProccesRequest request) where DbType: class
        {
            return await linqManager.InternalProcessAsync<DbType, DtoType>(source, request);
        }

        public async static Task<LinqAsyncResponse<DbType>> ProcessAsync<DbType, DtoType>(this LinqManager linqManager,
            DbSet<DbType> dbSet, LinqProccesRequest request) where DbType : class
        {
            if (dbSet == null)
                throw new LinqManagerException(ErrorMessages.ProcessError, new ArgumentNullException(nameof(dbSet)));

            return await linqManager.InternalProcessAsync<DbType, DtoType>(dbSet.AsQueryable(), request);
        }

        private async static Task<LinqAsyncResponse<DbType>> InternalProcessAsync<DbType, DtoType>(this LinqManager linqManager,
            IQueryable<DbType> source, LinqProccesRequest request) where DbType : class
        {
            if (source == null)
                throw new LinqManagerException(ErrorMessages.RequestValidationError, new ArgumentNullException(nameof(source)));

            if (request.Pagination == null)
                throw new LinqManagerException(ErrorMessages.RequestValidationError, new ArgumentNullException(nameof(request.Pagination)));

            linqManager.Validate<DtoType>(request);

            source = linqManager.Filter<DbType, DtoType>(source, request.FilterBy);

            var count = await source.CountAsync();

            source = linqManager.Sort<DbType, DtoType>(source, request.SortBy);

            source = source.Skip((request.Pagination.Page - 1) * request.Pagination.PageSize).Take(request.Pagination.PageSize);
            return new LinqAsyncResponse<DbType> { Query = source, Count = count };
        }
    }
}