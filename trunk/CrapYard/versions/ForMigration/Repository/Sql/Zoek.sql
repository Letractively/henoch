CREATE Procedure dbo.Zoek
	 @term varchar(50)
as                                       	

Select prio, custname, Voornaam, tussenvoegsel ,AchterNaam
From
(

Select  '1' as prio, c.custname, firstname as Voornaam, midlename as tussenvoegsel ,lastname as AchterNaam
from Customer_CP cp inner join  Customer c  on cp.custcode = c.custcode
where lastname like '%' + @term + '%' 

UNION

Select  '2' as prio, c.custname, firstname as Voornaam, midlename as tussenvoegsel ,lastname as AchterNaam
from Customer_CP cp inner join  Customer c  on cp.custcode = c.custcode
where firstname like '%' + @term + '%' 


UNION

Select  distinct  '0' as prio, c.custname, '' as Voornaam, '' as tussenvoegsel,'' as AchterNaam
from Customer_CP cp inner join  Customer c  on cp.custcode = c.custcode
where c.custname like '%' + @term + '%'  
 )  alles
order by prio,  AchterNaam
RETURN