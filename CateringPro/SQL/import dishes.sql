begin tran
Declare @companyid int
SET @companyid=2

Declare @categoryid int
select @categoryid=max(id) from Categories where CompanyId=@companyid
--delete from amenu where f2 is null and f3 is null
--delete from amenu where f2 like'Каль%'
--delete from amenu where f2 like'Зав.%'
--delete from amenu where f2 like'№%'
--delete from amenu where f2 like'П%'
--delete from amenu where f2 like'"%'
--delete from amenu where f2 like'break%'
update amenu set 
name =rtrim(ltrim(SUBSTRING(f2,14,len(f2)-13)))
where f2 like 'Найменування:%'
update amenu set 
description =rtrim(ltrim(SUBSTRING(f2,6,len(f2)-5)))
where f2 like 'Опис:%'

insert  Dishes (CompanyId,code,price,Name,description,CategoriesId)
select @companyid,'',0,name,'',@categoryid from amenu where not name is null

update amenu 
set did=dishes.Id
from amenu,dishes
where amenu.name= dishes.name and Dishes.CompanyId=@companyid

update amenu 
set did=(select min (did) from amenu x where x.num=amenu.num)
where amenu.did is null


update Dishes
set description=amenu.description
from dishes ,amenu
where dishes.companyid=@companyid
and dishes.id=amenu.did and not amenu.description is null

update amenu 
set iid=Ingredients.id
from amenu, Ingredients
where Ingredients.companyid=@companyid
and Ingredients.Name=rtrim(ltrim(amenu.f3))

insert into DishIngredients(dishid,IngredientId,CompanyId,Proportion)
select  did,iid,@companyid,min(f9) from amenu 
where not f3 is null and not f9 is null and not iid is null
group by  did,iid
select * from amenu
select * from Dishes
select * from DishIngredients

rollback tran