CREATE PROCEDURE GetInstalledTrotters
AS


	SELECT distinct proj.* FROM
 	troProject proj INNER JOIN
	( 
     SELECT pjtcd
     FROM troProject_Act
     WHERE  st = 'D' AND DATEDIFF(day, datefr, GETDATE())  > 30  AND NOT (DATEDIFF(day, dateTo, GETDATE())  > 0)
  	) pact   

ON proj.pjtcd = pact.pjtcd   
 
ORDER BY dateFR desc
RETURN 
