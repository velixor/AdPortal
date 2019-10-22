- Документирование API: **`swagger`**
- **`ASP.NET Core 3.0`**
- база данных: **`PostgreSQL`**
- ОРМ: **`EF`**.
- валидация: **`FluentValidation`**
- мапинг: **`AutoMaper`**
- фильтрация/сортировка/пагинация: [**`Sieve`**](https://github.com/velixor/AdPortal#sieve) 

# Сущности

**Объявление**

1. Id (`guid`)
2. Номер (`int`)*
3. Пользователь (`guid`) ссылка на таблицу*
4. Текст (`string`)*
5. Картинка*
6. Рейтинг (`int`)*
7. Создано (`datetime`)*

**Пользователь**

1. Id (`guid`)
2. Имя (`string`)*

### Реализовать:

- [x]  добавление
- [x]  удаление
- [x]  редактирование
- [x]  поиск.
- [x]  сортировки
- [x]  поиск по всем полям
- [x]  фильтрацию
- [x]  постраничный просмотр
- [x]  один пользователь может опубликовать не более X(брать из настроек) объявлений

# [Sieve](https://github.com/Biarity/Sieve)

## Send a request
An example sort/filter/page query:
```curl
api/ads

?sorts=     userId,-creationDate        // sort by user, then descendingly by creation date 
&filters=   rating>10, Content@=продам  // filter to ads with more than 10 rating, and a content that contains the phrase "продам"
&page=      1                           // get the first page...
&pageSize=  10                          // ...which contains 10 ads

```
More formally:
* `sorts` is a comma-delimited ordered list of property names to sort by. Adding a `-` before the name switches to sorting descendingly.
* `filters` is a comma-delimited list of `{Name}{Operator}{Value}` where
    * `{Name}` is the name of a property with the Sieve attribute or the name of a custom filter method for TEntity
        * You can also have multiple names (for OR logic) by enclosing them in brackets and using a pipe delimiter, eg. `(LikeCount|CommentCount)>10` asks if `LikeCount` or `CommentCount` is `>10`
    * `{Operator}` is one of the [Operators](#operators)
    * `{Value}` is the value to use for filtering
        * You can also have multiple values (for OR logic) by using a pipe delimiter, eg. `Title@=new|hot` will return posts with titles that contain the text "`new`" or "`hot`"
* `page` is the number of page to return
* `pageSize` is the number of items returned per page 

Notes:
* You can use backslashes to escape commas and pipes within value fields
* You can have spaces anywhere except *within* `{Name}` or `{Operator}` fields

## Operators
| Operator   | Meaning                  |
|------------|--------------------------|
| `==`       | Equals                   |
| `!=`       | Not equals               |
| `>`        | Greater than             |
| `<`        | Less than                |
| `>=`       | Greater than or equal to |
| `<=`       | Less than or equal to    |
| `@=`       | Contains                 |
| `_=`       | Starts with              |
| `!@=`      | Does not Contains        |
| `!_=`      | Does not Starts with     |
| `@=*`      | Case-insensitive string Contains |
| `_=*`      | Case-insensitive string Starts with |
| `==*`      | Case-insensitive string Equals |
| `!@=*`     | Case-insensitive string does not Contains |
| `!_=*`     | Case-insensitive string does not Starts with |
