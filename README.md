# LinqManager
The open source code library for building dynamic Linq Expressions

## How to start?

Firstly you need to download nuget package [LinqManager](https://www.nuget.org/packages/LinqManager/).


```cs
PM> Install-Package LinqManager
```

The main goal of this library is to collate db and dto models and dynamically build linq expression. This example will be demonstrated on a book store. Let's create asp.net core web api and install two linq manager extension packages. It's only supported for entity framework core 3.0 just now. 

```cs
PM> Install-Package LinqManager.WebExtensions
PM> Install-Package LinqManager.EFCoreExtensions 
```

Add Linq Manager as services in **Startup.cs**

```cs
services.AddLinqManager();
```

Create db and dto models. We will use **DtoMap** attribute to define methods and property name of db model. The list of supported methods you can find [here](https://github.com/hrishynpavlo/LinqManager/blob/master/LinqManager/Enums/FilterMethods.cs). The second parameter (db property name) is optional for case when dto property name equals db property name. 

Characters "`.`" and "`:`" are using for calling inner properties. `.` is for inner properties and `:` is for inner lambda parameter.

**Db Author Model**
```cs
public class Author
    {
        public Author()
        {
            Books = new HashSet<Book>();
        }

        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CountryId { get; set; }

        public Country Country { get; set; }
        public ICollection<Book> Books { get; set; }
    }
```

**Dto Author Model**
```cs
public class AuthorDto
    {
        public int Id { get; set; }
        
        [DtoMap(FilterMethods.StartWith)]
        public string FirstName { get; set; }

        [DtoMap(FilterMethods.StringContains)]
        public string LastName { get; set; }

        [DtoMap(FilterMethods.Range)]
        public DateTime DateOfBirth { get; set; }

        [DtoMap(FilterMethods.Equals, "Country.Id")]
        public DropDown<int> Country { get; set; }
        
        [DtoMap(FilterMethods.Any, "Books:Name")]
        public List<DropDown<int>> Books { get; set; } = new List<DropDown<int>>();
    }
```

Linq Manager gets **LinqProccesRequest** parameter that contains data for filtering, sorting, pagination. [Here](https://github.com/hrishynpavlo/LinqManager/tree/master/LinqManager/Factories) provided factories class and default it's implementation.

```cs
private readonly LinqManager.LinqManager _linqManager;
...
public async Task<QueryPagedModel<Author>> GetAllAsync(LinqProccesRequest request)
        {
            var items = await _linqManager.ProcessAsync<Author, AuthorDto>(_db.Authors, request);
            var result = new QueryPagedModel<Author>(items.Query, items.Count);

            return result;
        }
```

Look at service which calls **GetAllAsync(...)** method.

```cs
public async Task<ObjectPagedModel<AuthorDto>> GetAllAsync(string filterBy, string sortBy)
        {
            var request = new DefaultRequestFactory(filterBy, sortBy, 1, 10).CreateRequest();
            var query = await _unitOfWork.Authors.GetAllAsync(request);
            return new ObjectPagedModel<AuthorDto>(await query.Items.ProjectTo<AuthorDto>(_mapper.ConfigurationProvider).ToListAsync(), query.Count);
        }
```

Parameters `filterBy` and `sortBy` has format ***property1Name=property1Value,...,propertyNName=propertyNValue*** (in case sort it's =true or =false for descending).

Run the [following project](https://github.com/hrishynpavlo/LinqManager/tree/master/BookStore), create couple of authors and books. Try to do GET request  with filterting and sorting parameters. You should get something like this.

![swagger example](https://github.com/hrishynpavlo/LinqManager/blob/master/img/swagger_example_1.jpg)
