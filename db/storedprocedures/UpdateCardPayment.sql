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