# PipeDB
Simple CRUD Database utilizing pipe separated value text files.

## Usage
(When in the REPL)

### Create
**{ ~ }  » create** {column1Value, column2Value, ..., lastColumnValue}

This creates a new row in the database with the specified structure. Note that there are no default values - you must specify a value for each column defined in the header.

### Read
**{ ~ }  » read** ( * )

This reads out all of the rows in the current database ( * is optional).

**{ ~ }  » read** columnName
This reads out all of the values under the specified column name.

**{ ~ }  » read** * where columnName = value

This reads out all of the rows which satisfy the provided query.

**{ ~ }  » read** columnName where columnName* = value

This reads out all of the values under the specified column name which satisfy the provided query. Note that the two column names in this command do not have to be the same column name.

### Update
**{ ~ }  » update** columnName = newValue where columnName* = currentValue

This updates all the rows which satisfy the provided query to reflect the specified change. Note again that the two column names need not be identical.

### Delete
**{ ~ }  » delete** where columnName = value

This removes all of the rows which satisfy the provided query (be careful).
