## N.Pag
### Library to handle pagination pattern between frontend & backend side.


#### How it work?

N.Pag enables easy pagination mechanism by evaulate LINQ string to IQueryable interface, which can You use on Entity Framework Level to filter, sort, skip & take data! Parameters are deliver to web api by QueryParams.

#### How to use? 

1. Fetch nuget package.
2. Create of implementation Your PaginationQuery class from base classes: ```EncodedPaginationQueryBase``` or ```PaginationQueryBase```
    1. ```EncodedPaginationQueryBase``` - parameters Where & OrderBy _must be_ encoded by base64,
    2. ```PaginationQueryBase``` - parameters are normal.
3. Create controler method with argument of Your query.
4. Invoke method ```FilterBy<TModel>(myAwesomeQuery)``` on the IQuerable (DbContext or other collection).
5. Return Your awsome filtered data 🤣!

#### Example

##### Endpoints 
```users?pageSize=20&page=2&where=c3RhdHVzID09ICJOIg==``` <=> where ```status == "N"``` 


```users?pageSize=20&where=c3RhdHVzID09ICJOIg==&orderBy=bmFtZS1kZXNj``` <=> order by ```name-desc``` 

##### Queries 

```
public class GetUsersQuery : EncodedPaginationQueryBase
{
  // Some additional fields
}

```

##### Api Methods

```
[HttpGet]
[ProducesResponseType(typeof(PaginationResult<User>), StatusCodes.Status200OK)]
public async Task<IActionResult> Get(GetUsersQuery query)
{
    var count = await _context.Users.Where(paginationQuery).CountAsync();
    var listed = await _context.Users.FilterBy(paginationQuery).ToListAsync();
    
    // pagination wrapper
    var result = new PaginationResult<User>
    {
        List = itemsDto,
        TotalCount = count
    };
    
    return Json(result);
}
```

#### Nuget 
https://www.nuget.org/packages/N.Pag/
