This project uses Entity Framework. This framework allows users to implement a "Code First" approach to the data structure of a project. 
To add an entity, use the following:
    1. Add the entity (class) you would like to add to the database to DataContext.cs
    2. Run the following command to create a new migration: 
        a. dotnet ef migrations add <MessageInPascalCase>
    3. Run the following command to apply the migration to the database (actually update the database with the changes/additions)
        b. dotnet ef database update
