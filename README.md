# SpecificationTranslator
SpecificationTranslator是Specification模式的.NET实现，它基于Lambda表达式树设计，并允许你转换为相应的SQL语句。

## 使用Specification查询数据

在设计数据层类型时，增加**FindBy(ISpecification specification)**方法，以避免查询接口膨胀。目前仅支持单表查询，如果涉及多表连接查询，请创建新的查询接口方法。
以下为常用接口方法：
```
IList<TEntity> FindBy(ISpecification<TEntity> specification);
IPagedList<TEntity> FindBy(ISpecification<TEntity> specification, int skipCount, int takeCount, params Sort[] sort);
```

假设我们需要查询账户为“ShadowTracker”的数据，可以这么写：

```
var specification = new AnonymousSpecification<User>(v => v.Account == "ShadowTracker");
var result = _userRepository.FindBy(specification);
```

你可以在你的Repository实现里，引入下面的代码实现翻译：

```
public IList<T> FindBy<T>(ISpecification<T> specification)
{
    var expression = specification.AsExpression();

    string sql = new OracleWhereSqlGenerator(expression).Generate();

    // 使用翻译后的**sql**语句作为**where**条件，从数据库查询并返回
    // ...
    // ...
}
```

上面的**specification**将会被翻译为：

```
Account = 'ShadowTracker'
```

### 组合查询
简单的组合查询，可以在一个**AnonymousSpecification**构造器内完成，构造器的参数是一个Lambda表达式树。假设我们需要查询账户为“ShadowTracker”或者Email为“ShadowTracker.1989@gmail.com”的数据：

```
var specification = new AnonymousSpecification<User>(v => v.Account == "ShadowTracker" || v.Email == "ShadowTracker.1989@gmail.com");
```

也可以通过构造多个Specification连接来组合：

```
var specification = new AnonymousSpecification<User>(v => v.Account == "ShadowTracker").Or(v => v.Email == "ShadowTracker.1989@gmail.com");
```

复杂的组合查询，也可以通过构造多个Specification连接来完成。例如：

```
var specification = new AnonymousSpecification<User>(v => v.Account == "ShadowTracker" && v.Status == Status.Active)
                                    .And(v => v.Inputter != null || v.Modifier == "admin").And(v => string.IsNullOrEmpty(v.Description));
```

支持外部捕获变量的查询，例如：

```
string Account = "ShadowTracker";
var specification = new AnonymousSpecification<User>(v => v.Account == Account);
```

### 条件查询
如果需要在某些情况满足时才加上查询条件，可以使用**AndIf/OrIf/NotIf**组合。例如：

```
var specification = new AnySpecification<User>()
                .AndIf(!string.IsNullOrWhiteSpace(input.Account),
                    v => v.Account.Contains(input.Account))
                .AndIf(!string.IsNullOrWhiteSpace(input.Description),
                    v => v.Description.Contains(input.Description))
                .AndIf(input.UserStatus.HasValue,
                    v => v.Status == input.Status.Value);
```

上面的**AnySpecification**不用传入构造器参数，在这里使用**AnySpecification**是为了避免出现所有条件都不满足的情况。如果下面的所有**AndIf**的条件都不满足时，最终的SQL语句将表示为 **Where 1 = 1**

### 支持列表

下面的表格列举了SQL操作符以及常见类型的用法（其他基元类型如long/double/float/short等的用法类似）：

.NET类型  |   SQL操作符	|	Lambda示例										|	SQL语句
--------  |   -------	|	-----------------------------------------------	|	-------------------
String    |   LIKE      |	v => v.Account.Contains("Tracker") 				|	Account LIKE '%Tracker%'
          |   		    |	v => v.Account.StartsWith("Tracker")				|	Account LIKE 'Tracker%'
          |   		    |	v => v.Account.EndsWith("Tracker")				|	Account LIKE '%Tracker'
          |   =		    |	v => v.Account.Equals("Tracker")					|	Account = 'Tracker'
          |   		    |	v => v.Account == "Tracker"						|	Account = 'Tracker'
          |             |   v => v.Account == null                      |   Account IS NULL
          |   !=        |   v => !v.Account.Equals("Tracker")                |   NOT (Account = 'Tracker')
          |             |   v => v.Account != "Tracker"                      |   Account <> 'Tracker'
          |             |   v => v.Account != null                      |   Account IS NOT NULL
Boolean   |             |   v => v.Enabled == true                          |   Enabled = 1
          |             |   v => v.Enabled == false                         |   Enabled = 0
          |             |   v => v.Enabled != true                          |   Enabled <> 1
          |             |   v => v.Enabled != false                         |   Enabled <> 0
enum      |             |   v => v.Sex == SexStub.Male                      |   Sex = 1
          |             |   v => v.Sex != SexStub.Male                      |   Sex <> 1
Int32     |   =         |   v => v.Version.Equals(10)                       |   Version = 10
          |             |   v => v.Version == 10                            |   Version = 10
          |   !=        |   v => !v.Version.Equals(10)                      |   NOT (Version = 10)
          |             |   v => v.Version != 10                            |   Version <> 10
          |   >		    |	v => v.Version > 10								|	Version > 10
          |   >=		|	v => v.Version >= 10							|	Version >= 10
          |   <		    |	v => v.Version < 10								|	Version < 10
          |   <=		|	v => v.Version <= 10							|	Version <= 10
Int32?    |   =         |   v => v.Version == 10                            |   Version = 10
          |   !=        |   v => v.Version != 10                            |   Version <> 10
          |   >         |   v => v.Version > 10                             |   Version > 10
          |   >=        |   v => v.Version >= 10                            |   Version >= 10
          |   <         |   v => v.Version < 10                             |   Version < 10
          |   <=        |   v => v.Version <= 10                            |   Version <= 10
Decimal   |   =         |   v => v.Point.Equals(1.1m)                       |   Point = 1.1
          |             |   v => v.Point == 1.1m                            |   Point = 1.1
          |   !=        |   v => !v.Point.Equals(1.1m)                      |   NOT (Point = 1.1)
          |             |   v => v.Point != 1.1m                            |   Point <> 1.1
          |   >         |   v => v.Point > 1.1m                             |   Point > 1.1
          |   >=        |   v => v.Point >= 1.1m                            |   Point >= 1.1
          |   <         |   v => v.Point < 1.1m                             |   Point < 1.1
          |   <=        |   v => v.Point <= 1.1m                            |   Point <= 1.1
Decimal?  |   =         |   v => v.Point == 1.1m                            |   Point = 1.1
          |   !=        |   v => v.Point != 1.1m                            |   Point <> 1.1
          |   >         |   v => v.Point > 1.1m                             |   Point > 1.1
          |   >=        |   v => v.Point >= 1.1m                            |   Point >= 1.1
          |   <         |   v => v.Point < 1.1m                             |   Point < 1.1
          |   <=        |   v => v.Point <= 1.1m                            |   Point <= 1.1
DateTime  |   =         |   v => v.InputDate.Equals(DateTime.Now)           |   InputDate = TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |             |   v => v.InputDate == DateTime.Now                |   InputDate = TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   !=        |   v => !v.InputDate.Equals(DateTime.Now)          |   NOT (InputDate = TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS'))
          |             |   v => v.InputDate != DateTime.Now                |   InputDate <> TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   >         |   v => v.InputDate > DateTime.Now                 |   InputDate > TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   >=        |   v => v.InputDate >= DateTime.Now                |   InputDate >= TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   <         |   v => v.InputDate < DateTime.Now                 |   InputDate < TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   <=        |   v => v.InputDate <= DateTime.Now                |   InputDate <= TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
DateTime? |   =         |   v => v.InputDate == DateTime.Now                |   InputDate = TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   !=        |   v => v.InputDate != DateTime.Now                |   InputDate <> TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   >         |   v => v.InputDate > DateTime.Now                 |   InputDate > TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   >=        |   v => v.InputDate >= DateTime.Now                |   InputDate >= TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   <         |   v => v.InputDate < DateTime.Now                 |   InputDate < TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
          |   <=        |   v => v.InputDate <= DateTime.Now                |   InputDate <= TO_DATE('2017-01-01 23:11:22', 'YYYY-MM-DD HH24:MI:SS')
All       |   IN        |   v => list.Contains(v.Name)                      |   Name IN ('admin', 'Michael', 'Ivan')


注意：
1. 部分类型不支持Equals方法查询，在构造等值或者不等值条件时，应使用 **=** **!=**
2. 可为空类型不支持 **.Value** 方法构造

## 使用Specification验证

当需要验证对象是否满足某些标准，可以使用**Specification**的**IsSatisfiedBy**方法进行验证。

```
bool IsSatisfiedBy(T value);
```

例如，当我们需要验证用户状态是否为激活状态时，可以使用**AnonymousSpecification**规约对象，并调用它的**IsSatisfiedBy**方法：

```
var specification = new AnonymousSpecification<User>(v => v.Status == Status.Active);
if (specification.IsSatisfiedBy(user))
{
    // Do something...
}
```

当我们需要复用激活用户规约时，则可以自定义**ISpecification**的实现类。在下面代码中，我们从**Specification**抽象类派生：

```
public class ActiveUserSpecification : Specification<User>
{
    public override Expression<Func<User, bool>> AsExpression()
    {
        return v => v.Status == Status.Active;
    }
}
```

然后这么用：

```
var activeUserSpecification = new ActiveUserSpecification();
if (activeUserSpecification.IsSatisfiedBy(user))
{
    // Do something...
}
```





