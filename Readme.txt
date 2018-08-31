Bank
- a simple bank system using dotnet core (currently 2.1)
- can create user, deposit, withdraw, transfer

Installation
1. checkout the code from
https://github.com/une6/bank
2. open SQL Server and execute the scripts under the folder DBScripts/
3. Run the application by either opening using visual studio and press F5

Disclaimer
1. application is made simple, and lots of improvements can still be done e.g. added security, code cleanup, more checking and unit tests
2. concurrency handled by avoiding UPDATE queries from the DB to avoid deadlock and etc.