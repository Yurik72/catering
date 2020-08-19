-- prepare
Declare @companyid int
set @companyid=8
begin tran

DECLARE curs CURSOR FORWARD_ONLY  STATIC 
 FOR  
SELECT id,cat FROM import order by id

DECLARE @cat nvarchar(50) 
DECLARE @id int
DECLARE @prevcat  nvarchar(50) 
  
OPEN curs;  
  
-- Perform the first fetch.  
FETCH NEXT FROM curs INTO @id,@cat;  
  
-- Check @@FETCH_STATUS to see if there are any more rows to fetch.  
WHILE @@FETCH_STATUS = 0  
BEGIN  
   if (@cat IS NULL)
   begin
	update import set cat=@prevcat where id=@id
   end
   else
   begin
   set @prevcat=@cat
   end

   -- This is executed as long as the previous fetch succeeds.  
   FETCH NEXT FROM curs INTO @id,@cat;  
END  
  
CLOSE curs;  
DEALLOCATE curs;  
delete from import where name is null
select * from import

insert into IngredientCategories(code,name,companyid)
select distinct '' as code,cat,@companyid from import

select * from IngredientCategories where companyid=@companyid
 
insert into Ingredients(CompanyId,Name,MeasureUnit,IngredientCategoriesId)
select @companyid,im.name,im.meas,ic.id
from import im,
(select max(id) as id,name from IngredientCategories where CompanyId=@companyid group by name ) ic
where ic.name=im.cat
select * from Ingredients where companyid=@companyid
rollback tran