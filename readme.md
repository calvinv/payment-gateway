Prequisities:
- dotnet core 3.1
- MS Sql server

Installation:
Run the CreateDatabase.sql script on a new database called "payments", this will build your db objects
Configure the connection string in "\src\Payment.Gateway\Payment.Gateway.Api\appsettings.json" with the correct details, it's currently using windows auth

Run:
- run the simulator by running the "runsimulator.bat" file
- run the payment gateway by running the "runpaymentgateway.bat" file
- navigate to https://localhost:5001/swagger/index.html
- create a payment using the sample request 

{
  "amount": 1000,
  "currency": "GBP",
  "reference": "12345",
  "cardNumber": "5555 3412 4444 1115",
  "cvv": "123",
  "expiryMonth": 4,
  "expiryYear": 25,
  "customerAddress": "123 Lane rd",
  "customerName": "Bob Roberts"
}

- get the payment using it's reference "12345"


Assumptions:
- The bank endpoint would have it's own authentication endpoint for logging in and fetching a token.
- One bank supports all the card payments, there would need to be changes depending on how many banks are integrated with
- If there are other types of payments (bank transfer), there would be new endpoints. Everything could be under a payment controller if that was the choice

Areas to improve:
- it currently allows duplicate references, this needs to be a bad request on creation if it is a duplicate
- secrets are implemented in a basic way, there should be a more robust way to get an auth token and then validate it.
- There should be integration tests to run at build time, most of what this does is integrate to different sources and that needs testing. There is currently only a small suite of unit tests, that could be expanded.
- logging is implemented but there is no source to log to at the moment, typically there would be logging to some external service or to file, it just needs to be configured in Program.cs
