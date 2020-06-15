
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