Beacon of Hope - Bulldog Scholarship Application Management System
==========================================

A system for managing scholarship applications built on top of Entity Framework, Azure, AngularJs, Bootstrap, ASP.Net MVC and WebAPI.

In order to get the application working you must:
 - Restore the NuGet packages.
 - Change the connection strings in the config files. This unfortunately isn't DRY. Make sure you add your test connection strings in the data test projects. The application uses both Azure Table Storage and a Azure SQL Storage.
 - Use EF Migrations to update your databases with the appropriate tables. 
 - Update your deadline date in the WebAPI config file. 
 - Create a Admin account using the instructions in NotesForBootstrappingAdmin.txt
