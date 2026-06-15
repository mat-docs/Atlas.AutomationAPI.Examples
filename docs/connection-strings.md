# Connection Strings

When you load a session programmatically (for example with
`LoadSqlRaceSessions`), you supply a **session key** (a GUID) and a **connection
string** describing where the session lives. The connection string format
depends on the database that backs the session.

## SQLite session file (`.ssn2`)

```
DbEngine=SQLite;Data Source=C:\path\to\file.ssn2;Pooling=false;
```

## SQLite session database (`.ssndb`)

```
DbEngine=SQLite;Data Source=C:\path\to\file.ssndb;Pooling=false;
```

## SQL Server

```
server=SQLSERVER\InstanceName;Initial Catalog=databaseName;Trusted_Connection=True;
```

- Use `Trusted_Connection=True;` for Windows authentication.
- For SQL authentication, replace it with `User Id=...;Password=...;`.

## Finding the session key

The session key is the session's GUID. You can:

- Read it from a loaded session via the API
  (`CompositeSession.Id` / `Session.Key`), or
- Query the database with the SQL Race API, or
- Copy it from the ATLAS Session Browser session properties.

## Example

```csharp
const string ConnectionString = @"DbEngine=SQLite;Data Source=C:\data\run42.ssn2;Pooling=false;";
const string SessionKey = "3f2504e0-4f89-41d3-9a0c-0305e82c3301";

application.Call(client => client.LoadSqlRaceSessions(
    setId, new[] { SessionKey }, new[] { ConnectionString }));
```

See [snippets/csharp/getting-started/03-load-session-into-set.cs](../snippets/csharp/getting-started/03-load-session-into-set.cs)
for the full, runnable version.
