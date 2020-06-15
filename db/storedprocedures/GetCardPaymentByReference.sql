CREATE PROCEDURE [dbo].[GetCardPaymentByReference]
    @Reference NVARCHAR(1000)
AS
    SET NOCOUNT ON;  
    SELECT TOP (1) [Reference]
	  ,[ThirdPartyReference]
	  ,[PaymentStatus]
      ,[CustomerAddress]
      ,[CustomerName]
      ,[CardNumber]
      ,[Cvv]
      ,[ExpiryMonth]
      ,[ExpiryYear]
      ,[Currency]
      ,[Amount]
      ,[CreatedDate]
      ,[ModifiedDate]
  FROM [Payments].[dbo].[CardPayments]
  WHERE Reference = @Reference
RETURN 

GO