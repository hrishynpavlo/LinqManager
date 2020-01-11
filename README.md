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
