using LinqManager.Attributes;
using LinqManager.Enums;
using LinqManager.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using LinqManager.Constants;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LinqManager.EFCoreExtensions, PublicKey=002400000480000094000000060200000024000052534131000400000100010095871e91babb4eeda97c9d1ab742dcfefbcccd4bf494171aefb664ebed3eaea278bde0101da0fced3de80efe591bb4b592f4476e8d39ba700c2327382a50a87ee4a76c40dc7cb2f162d73a5da487e3ec82aa8a4a5d12cccd0d031ca375fbba658f4f1fbd0ca9642cc1cc9d294b51b918b0d35e1f281368ae25aabd66085923f4")]
namespace LinqManager
{    
    public sealed class LinqManager
    {
        private readonly MappingCache _cache;

        public LinqManager()
        {
            _cache = new MappingCache();
        }

        public IQueryable<DbType> Process<DbType, DtoType>(IQueryable<DbType> source, LinqProccesRequest request)
        {
            if (source == null)
                throw new LinqManagerException(ErrorMessages.RequestValidationError, new ArgumentNullException(nameof(source)));

            Validate<DtoType>(request);

            source = Filter<DbType, DtoType>(source, request.FilterBy);
            source = Sort<DbType, DtoType>(source, request.SortBy);
            return source;
        }

        public IQueryable<DbType> Process<DbType, DtoType>(IQueryable<DbType> source, LinqProccesRequest request, out int count)
        {
            if (source == null)
                throw new LinqManagerException(ErrorMessages.RequestValidationError, new ArgumentNullException(nameof(source)));

            if (request.Pagination == null)
                throw new LinqManagerException(ErrorMessages.RequestValidationError, new ArgumentNullException(nameof(request.Pagination)));

            Validate<DtoType>(request);

            source = Filter<DbType, DtoType>(source, request.FilterBy);

            count = source.Count();

            source = Sort<DbType, DtoType>(source, request.SortBy);

            source = source.Skip((request.Pagination.Page - 1) * request.Pagination.PageSize).Take(request.Pagination.PageSize);
            return source;
        }

        internal IQueryable<DbType> Filter<DbType, DtoType>(IQueryable<DbType> source, List<Filter<string>> filterBy)
        {
            if (!filterBy.Any())
                return source;

            try
            {
                foreach(var filterProperty in filterBy)
                {
                    var mapping = _cache.GetMapping<DtoType>()
                        .FirstOrDefault(p => string.Equals(p.DtoPropertyName, filterProperty.PropertyName, StringComparison.OrdinalIgnoreCase));

                    if (mapping == null)
                        throw new ArgumentNullException(nameof(mapping));

                    if (mapping.FilterMethod == FilterMethods.OnlySortable)
                        continue;

                    var filterValues = filterProperty.PropertyValue.Split(new[] { SpecialCharacters.RangeSplitter }, StringSplitOptions.None);
                    short index = 0;

                    foreach (var filterValue in filterValues)
                    {
                        var param = CreateParam(typeof(DbType), "item");
                        var property = CreateProperty(param, mapping.DbPropertyName ?? filterProperty.PropertyName);
                        var value = CreateValue(property.Type, filterValue);

                        var method = mapping.FilterMethod == FilterMethods.Range ? 
                                index == 0 ? FilterMethods.GreaterThanOrEqual : FilterMethods.LessThanOrEqual 
                            : mapping.FilterMethod;

                        var lambda = CreateLambda<DbType, DtoType>(param, property, value, method);

                        source = source.Where(lambda);
                        index++;
                    }
                }

                return source;
            }
            catch(Exception ex)
            {
                throw new LinqManagerException(ErrorMessages.ProcessError, ex);
            }
        }

        internal IQueryable<DbType> Sort<DbType, DtoType>(IQueryable<DbType> source, List<Filter<bool>> sortBy)
        {
            if (!sortBy.Any())
                return source;

            try
            {
                source = source.OrderBy(o => 0);
                foreach(var sortProperty in sortBy)
                {
                    var mapping = _cache.GetMapping<DtoType>()
                        .FirstOrDefault(p => string.Equals(p.DtoPropertyName, sortProperty.PropertyName, StringComparison.OrdinalIgnoreCase));

                    if (mapping == null)
                        throw new ArgumentNullException(nameof(mapping));

                    var param = CreateParam(typeof(DbType), "item");
                    var property = CreateProperty(param, mapping.DbPropertyName ?? sortProperty.PropertyName);
                    var lambda = Expression.Lambda<Func<DbType, object>>(Expression.Convert(property, typeof(object)), param);

                    if (sortProperty.PropertyValue)
                        source = ((IOrderedQueryable<DbType>)source).ThenByDescending(lambda);
                    else
                        source = ((IOrderedQueryable<DbType>)source).ThenBy(lambda);
                }

                return source;
            }
            catch(Exception ex)
            {
                throw new LinqManagerException(ErrorMessages.ProcessError, ex);
            }
        }

        internal void Validate<DtoType>(LinqProccesRequest request)
        {
            var mapping = _cache.GetMapping<DtoType>();

            var filterByProps = new List<string>();
            var sortByProps = new List<string>();

            foreach (var filterProperty in request.FilterBy.Select(s => s.PropertyName))
            {
                if(!mapping.Any(m => m.FilterMethod != FilterMethods.OnlySortable 
                    && m.DtoPropertyName.Equals(filterProperty, StringComparison.OrdinalIgnoreCase)))
                    filterByProps.Add(filterProperty);
            }

            foreach(var sortProperty in request.SortBy.Select(s => s.PropertyName))
            {
                if (!mapping.Any(m => m.DtoPropertyName.Equals(sortProperty, StringComparison.OrdinalIgnoreCase)))
                    sortByProps.Add(sortProperty);
            }

            if (filterByProps.Any() || sortByProps.Any())
                throw new LinqManagerException(ErrorMessages.MismatchProperty, new ArgumentException(nameof(request)),
                    new Dictionary<string, IReadOnlyList<string>> { { nameof(request.FilterBy), filterByProps.AsReadOnly() },
                        { nameof(request.SortBy), sortByProps.AsReadOnly() } });
        }

        #region Expressions
        private ParameterExpression CreateParam(Type type, string name)
        {
            return Expression.Parameter(type, name);
        }

        private MemberExpression CreateProperty(ParameterExpression parameter, string name)
        {
            var names = name.Split('.');
            MemberExpression CreatePropertyRecoursive(Expression p, string n) => Expression.Property(p, n);

            var prop = CreatePropertyRecoursive(parameter, names[0]);
            foreach (var n in names.Skip(1))
                prop = CreatePropertyRecoursive(prop, n);

            return prop;
        }

        private UnaryExpression CreateValue(Type type, object value)
        {
            return Expression.Convert(Expression.Constant(value), type, type.GetMethod("Parse", new[] { typeof(string) }));
        }

        private Expression<Func<DbType, bool>> CreateLambda<DbType, DtoType>(ParameterExpression parameter, MemberExpression property, UnaryExpression value, FilterMethods method)
        {
            Expression<Func<DbType, bool>> lambda = null;
           
            switch (method)
            {
                case FilterMethods.Equals: {
                        lambda = Expression.Lambda<Func<DbType, bool>>(Expression.Equal(property, value), parameter);
                        break; }
                case FilterMethods.StringContains: {
                        lambda = Expression.Lambda<Func<DbType, bool>>(Expression.Call(property, 
                            typeof(string).GetMethod(nameof(string.Contains), new Type[] { typeof(string) }), value), parameter);
                        break; }
                case FilterMethods.StartWith: {
                        lambda = Expression.Lambda<Func<DbType, bool>>(Expression.Call(property, 
                            typeof(string).GetMethod(nameof(string.StartsWith), new Type[] { typeof(string) }), value), parameter);
                        break; }
                case FilterMethods.Any: {
                        var anyMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public)
                            .FirstOrDefault(m => m.Name == nameof(Enumerable.Any) && m.GetParameters().Count() == 2);

                        if (anyMethod == null)
                            throw new LinqManagerException(ErrorMessages.ProcessError, new ArgumentNullException(nameof(anyMethod)));

                        var collectionType = property.Type.GetInterfaces()
                            .FirstOrDefault(f => f.IsGenericType && f.GetGenericTypeDefinition().Equals(typeof(IEnumerable<>)))
                            .GetGenericArguments()[0];
                        var genericMethod = anyMethod.MakeGenericMethod(collectionType);

                        var anyParam = CreateParam(collectionType, "anyItem");
                        var anyProp = CreateProperty(anyParam, _cache.GetMapping<DtoType>()
                            .FirstOrDefault(p => p.DbPropertyName == property.Member.Name).DbCollectionPropertyName);
                        var anyConst = CreateValue(anyProp.Type, ((ConstantExpression)value.Operand).Value.ToString());

                        lambda = Expression.Lambda<Func<DbType, bool>>(Expression.Call(genericMethod, property, 
                            Expression.Lambda(Expression.Equal(anyProp, anyConst), anyParam)), parameter);

                        break; }
                case FilterMethods.LessThan: {
                        lambda = Expression.Lambda<Func<DbType, bool>>(Expression.LessThan(property, value), parameter);
                        break; }
                case FilterMethods.LessThanOrEqual: {
                        lambda = Expression.Lambda<Func<DbType, bool>>(Expression.LessThanOrEqual(property, value), parameter);
                        break;
                    }
                case FilterMethods.GreaterThan: {
                        lambda = Expression.Lambda<Func<DbType, bool>>(Expression.GreaterThan(property, value), parameter);
                        break;
                    }
                case FilterMethods.GreaterThanOrEqual: {
                        lambda = Expression.Lambda<Func<DbType, bool>>(Expression.GreaterThanOrEqual(property, value), parameter);
                        break;
                    }
                default: 
                    throw new LinqManagerException($"{method.ToString()} {ErrorMessages.UnsupportedMethod}",
                        new ArgumentException(nameof(method)));
            }

            return lambda;
        }
        #endregion
    }
}