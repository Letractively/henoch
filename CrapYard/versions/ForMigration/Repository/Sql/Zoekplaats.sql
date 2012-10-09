
-----------------------------------------------
-- Zoek plaatsen van opdrachtgevers
CREATE Procedure dbo.ZoekPlaats
	 @term varchar(50)
as                                       	

Select distinct prio, city, custname as Opdrachtgever, address, number 
From
(

Select  '1' as prio, c.custname, pl.city, pl.address, pl.number, pl.custcode 
from Customer_Deladd pl inner join customer c on pl.custcode= c.custcode
where city like '%' + @term + '%' ESCAPE '\' 


 )  alles inner join order_detail o on alles.custcode=o.CUSTORD

order by prio, City, custname, address, number

RETURN