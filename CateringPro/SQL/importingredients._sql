begin tran
Declare @companyid int
SET @companyid=1


delete from Ingredients where CompanyId=@companyid
and (select count(*)  from Bgnorep$ where Bgnorep$.F3=Ingredients.Name)>0


insert into Ingredients(CompanyId,Name,MeasureUnit,Avgprice)

select @companyid ,F3,Isnull(F4,'') ,Isnull(f6,0) from Bgnorep$ where not  F3  IS NULL 





delete from IngredientCategories where CompanyId=@companyid
and (select count(*)  from Bgnorep$ where Bgnorep$.F2=IngredientCategories.Name)>0


insert into IngredientCategories(CompanyId,Name)

select @companyid ,Isnull(max(F2),'') from Bgnorep$ where not  F2  IS NULL group by F2

Select * from IngredientCategories


Update Ingredients
set IngredientCategoriesid=ic.id
from Ingredients i,IngredientCategories ic,Bgnorep$ b

where i.CompanyId=@companyid 
and ic.CompanyId=@companyid 
and ic.Name=b.F2
and i.Name =b.f3


Select * from Ingredients
commit tran 

