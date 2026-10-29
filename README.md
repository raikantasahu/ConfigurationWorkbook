# ConfigWorkbook

ConfigWorkbook writes the configuration workbook of one Aras Innovator ItemType to an Excel file.
The program logs in to an Innovator server, runs SQL queries through ApplySQL, and writes the
results to 12 worksheets.

## Requirements

- The .NET 10 SDK.
- An Aras Innovator server. The program uses the IOM (Aras.IOM 15.1.2).
- A user that can run ApplySQL. By default, only members of Administrators can run ApplySQL.

## Build

```bash
dotnet build ConfigWorkbook.csproj
```

## Run

```bash
dotnet run --project ConfigWorkbook.csproj -- --url http://<server>/<web-alias> --database <db> --user <name> --itemtype Part
```

| Option | Required | Description |
|---|---|---|
| `-u`, `--url <url>` | Yes | The Innovator server URL. |
| `-d`, `--database <name>` | Yes | The database name. |
| `-n`, `--user <name>` | Yes | The Innovator login name. |
| `-t`, `--itemtype <name>` | Yes | The name of the ItemType, for example `Part`. |
| `-p`, `--password <pw>` | No | The password, or its MD5. If you do not give it, the program asks for it. |
| `-o`, `--output <file>` | No | The workbook to write. The default is `<itemtype> Configuration Workbook.xlsx`. The name must end in `.xlsx`. |
| `-h`, `--help` | No | Show the help. |

### Exit codes

| Code | Meaning |
|---|---|
| 0 | The program wrote the workbook. |
| 1 | A query failed, or the program could not write the workbook. |
| 2 | Bad usage, the login failed, or the ItemType does not exist. |

### run.bat

`run-template.bat` is a wrapper for the program. It keeps the connection settings in one place.

1. Copy `run-template.bat` to `run.bat`.
2. In `run.bat`, set `INNOVATOR_URL`, `INNOVATOR_DB` and `INNOVATOR_USER`.
3. Set `INNOVATOR_PASSWORD` too, or leave it empty to get a password prompt.
4. Run `run.bat <itemtype> [extra options]`, for example `run.bat Part`.

Git ignores `run.bat`, so your settings and credentials stay out of the repository.
You can also set the `INNOVATOR_*` variables in the environment. They override the values in the file.

## The workbook

| Sheet | Contents |
|---|---|
| Packaging | The package that contains the ItemType. |
| Properties | The properties of the ItemType. |
| Relationship Types | The RelationshipTypes. Each null RelationshipType also gets the properties of its relationship ItemType. |
| Views | The views. Each form gets its fields, field events and form events. |
| Server Events | The server events. |
| Actions | The actions. |
| Life Cycle Maps | The life cycle maps. Each map gets its states. |
| Permissions | The permissions, grouped by permission. |
| Can Add | The identities that can add the ItemType. |
| Client Events | The client events. |
| Reports | The reports. |
| Client Style (CUI) | The presentation configurations, command bar sections and command bar items. |

> **Note:** The program does not make screenshots. Make the screenshots of the Forms and the Life Cycle
> Maps manually, and add them to the workbook.

## Project structure

| Path | Contents |
|---|---|
| `Program.cs` | The entry point: it reads the options, logs in, finds the ItemType and builds the workbook. |
| `Options.cs` | The command-line options and the help text. |
| `SqlRunner.cs` | Runs a query from `Sql/` through ApplySQL and reads the rows. |
| `SheetWriter.cs` | Writes titles and tables to a worksheet. |
| `WorkbookBuilder.cs` | Writes each sheet that `SheetFactory` makes, then saves the workbook. |
| `Sheets/Sheet.cs` | The `Sheet` base class and the `Column` record. |
| `Sheets/SheetFactory.cs` | Makes the sheets in workbook order. |
| `Sheets/*Sheet.cs` | One class for each sheet, with its columns and its code. |
| `Sql/*.sql` | The queries. The build puts them into the program as resources. |

### Add a sheet

1. Add a query to `Sql/`, for example `Sql/MyQuery.sql`.
2. Add a class to `Sheets/` that derives from `Sheet`. Give it a `Name`, its columns, and a `Write` method.
3. Add the class to the list in `SheetFactory.CreateAll`, at the position of the sheet in the workbook.

### Write a query

- Use a snake_case alias for each column. The `Column` keys use these aliases.
- Use `@ItemTypeId`, `@SourceId`, `@FormId` or `@MapId` for an id. ApplySQL has no bind parameters, so
  the program checks that each id has 32 hex characters, then puts it into the SQL text.
- Put comments only on lines that start with `--`. The program removes these lines, because ApplySQL
  joins the lines of a query and a line comment would hide the rest of the SQL.
- A NULL column is missing from the ApplySQL response, so the program reads it as an empty value.

## License

This project is published under the MIT License. See [LICENSE](LICENSE).
