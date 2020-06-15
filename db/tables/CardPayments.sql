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