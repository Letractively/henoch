CREATE PROCEDURE GetCandidatesTrotters
(@orderNo varChar(50) )
AS
	Select od.OrdNo, od.Ordln, od.PriceType, od.PriceSold, od.Unit ,od.PriceSell, od.PriceRent FROM 
	[sygniondb].dbo.Order_Detail od INNER JOIN

	(SELECT distinct proj.* FROM
 	[sygniondb_tro].dbo.troProject proj INNER JOIN
	( 
     SELECT pjtcd
     FROM [sygniondb_tro].dbo.troProject_Act
     WHERE  st = 'D' AND DATEDIFF(day, datefr, GETDATE())  > 30   AND NOT (DATEDIFF(day, dateTo, GETDATE())  > 0)
  	) pact   
	ON proj.pjtcd = pact.pjtcd   
	) trotters

	ON od.OrdNo = trotters.ordNo
	WHERE od.ordNo =@orderNo
RETURN    