# Setup Instructions

1. Clone the repo in Visual Studio

2. Open the Package Manager Console by going into 'Tools -> NuGet Package Manager -> Package Manager Console' located on the top of the IDE
   
3. Run the following command in the Package Manager Console to re-create the database with sample data. It should say 'There is already an object named 'Pallets' in the database' when it is done running, meaning that it has been successful
```
update-database
```

4. Launch the code. A swagger page with all the available endpoints should appear in your browser. Select any endpoint and follow the instructions on how to use it

5. Run the following command if you wish to drop the database 
```
drop-database
```

6. Re-run the following command if you wish to re-instate the database with all of its default values
```
update-database
```
