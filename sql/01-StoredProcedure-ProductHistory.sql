USE PTAnalitic

GO

CREATE PROCEDURE dbo.sp_InsertProductHistories
@productHistories ProductHistoryType READONLY
AS 
SET NOCOUNT ON

	INSERT INTO dbo.ProductHistory (PointOfSale, ProductId, Date, Stock)
	SELECT PointOfSale, ProductId, Date, Stock 
	FROM @productHistories;

GO