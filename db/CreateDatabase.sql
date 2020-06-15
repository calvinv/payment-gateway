CREATE TABLE [dbo].[CardPayments]
(
    [CardPaymentId] INT IDENTITY NOT NULL PRIMARY KEY,
    [Reference] NVARCHAR(1000) NOT NULL,
    [ThirdPartyReference] NVARCHAR(1000) NULL,
    [PaymentStatus] NVARCHAR(500) NULL,
    [CustomerAddress] NVARCHAR(2000) NOT NULL,
    [CustomerName] NVARCHAR(500) NOT NULL,
    [CardNumber] NVARCHAR(25) NOT NULL,
    [Cvv] NVARCHAR(6) NOT NULL,
    [ExpiryMonth] INT NOT NULL,
    [ExpiryYear] INT NOT NULL,
	[Currency] NVARCHAR(3) NOT NULL,
    [Amount] DECIMAL(18, 2) NOT NULL,
    [CreatedDate] DATETIME2 NOT NULL, 
    [ModifiedDate] DATETIME2 NOT NULL
)

GO

CREATE PROCEDURE [dbo].[UpdateCardPayment]
    @Reference NVARCHAR(1000),
    @PaymentStatus NVARCHAR(1000),
    @ThirdPartyReference NVARCHAR(1000)
AS
    SET NOCOUNT ON;
	
	UPDATE CardPayments 
	SET [ThirdPartyReference] = @ThirdPartyReference,
		[PaymentStatus] = @PaymentStatus,
	    [ModifiedDate] = GETUTCDATE()
	WHERE [Reference] = @Reference
    
RETURN 

GO

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

CREATE PROCEDURE [dbo].[CreateCardPayment]
    @Reference NVARCHAR(1000),
    @ThirdPartyReference NVARCHAR(1000),
    @PaymentStatus NVARCHAR(500),
    @CustomerAddress NVARCHAR(2000),
    @CustomerName NVARCHAR(500),
    @CardNumber NVARCHAR(25),
    @Cvv NVARCHAR(6),
    @ExpiryMonth INT,
    @ExpiryYear INT,
	@Currency NVARCHAR(3),
    @Amount DECIMAL(18, 2)
AS	
    SET NOCOUNT ON;
	INSERT INTO CardPayments (        
    [Reference],
    [ThirdPartyReference],
    [PaymentStatus],
    [CustomerAddress],
    [CustomerName],
    [CardNumber],
    [Cvv],
    [ExpiryMonth],
    [ExpiryYear],
	[Currency],
    [Amount],
    [CreatedDate], 
    [ModifiedDate])
    VALUES
    (   
		@Reference,
		@ThirdPartyReference,
		@PaymentStatus,
		@CustomerAddress,
		@CustomerName,
		@CardNumber,
		@Cvv,
		@ExpiryMonth,
		@ExpiryYear,
		@Currency,
		@Amount,
        GETUTCDATE(),
        GETUTCDATE()
     )
RETURN 


GO

