Database auditing 

There are two ways of database auditing
the tables that are expected to get updated will have a shadow table
we will insert a record to the shodow table each time the main table is updated.
The tables that are not expected to get updated, will have only a 'CreatedBy'
column and their values shouldn't change.

Add migration

1.Go to 'Package Manager Console'
2.Select 'LL.Data'
3.Type 'Add-Migration Name'
4.Type 'Update-Database'