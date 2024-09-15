Database auditing 

There are two ways of database auditing
the tables that are expected to get updated will have a shadow table
we will insert a record to the shodow table each time the main table is updated.
The tables that are not expected to get updated, will have only a 'CreatedBy'
column and their values shouldn't change.

Add migration

Package Mangager Console
	Add-Migration YourMigrationName' --project LL.Data  --startup-project LL.API'
	Update-Database' --project LL.Data  --startup-project LL.API'

Command Line
	Linux: export DOTNET_ROOT=/user/bit/dotnet
	dotnet ef migrations add YourMigrationName --project LL.Data  --startup-project LL.API
	dotnet ef database update --project LL.Data  --startup-project LL.API
