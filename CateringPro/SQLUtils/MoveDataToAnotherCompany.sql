ALTER TABLE Categories ADD  old_id int NULL ; 
go
ALTER TABLE IngredientCategories ADD  old_id int NULL ; 
go

ALTER TABLE Ingredients ADD  old_id int NULL ; 
go


ALTER TABLE Dishes ADD  old_id int NULL ; 
go

Declare @SrcCompanyId int
Declare @TargetCompanyId int

Set @SrcCompanyId =8
Set @TargetCompanyId =10


begin tran 



insert into Categories (CompanyId,Code,Name,Description,old_id)
Select @TargetCompanyId,Code,Name,Description,id from Categories where CompanyId=@SrcCompanyId

insert into IngredientCategories(CompanyId,Code,Name, old_id)
Select @TargetCompanyId,Code, Name,ID from IngredientCategories where CompanyId=@SrcCompanyId


insert into Ingredients(CompanyId,Name,MeasureUnit,IngredientCategoriesId,old_id)
Select @TargetCompanyId,i.Name,i.MeasureUnit,ic.Id,i.Id from Ingredients i,
IngredientCategories ic
where i.CompanyId=@SrcCompanyId
	  and ic.CompanyId=@TargetCompanyId
	  and i.IngredientCategoriesId=ic.old_id

insert into Dishes(CompanyId,Code,Name,Price,Description,PictureId,ReadyWeight,KKal,CookingTechnologie,CategoriesId,old_id)
Select @TargetCompanyId,d.Code,d.Name,d.Price,d.Description,
	d.PictureId,d.ReadyWeight,d.KKal,d.CookingTechnologie,c.Id,d.id   from Dishes d, 
Categories c
where d.CompanyId=@SrcCompanyId
	  and c.CompanyId=@TargetCompanyId
	  and d.CategoriesId=c.old_id

insert into DishIngredients (CompanyId,DishId,IngredientId,Proportion,ProportionNetto)
select  @TargetCompanyId,d.id,i.id,di.Proportion,di.ProportionNetto from DishIngredients di,
Ingredients i,
Dishes d

where di.CompanyId=@SrcCompanyId
	  and i.CompanyId=@TargetCompanyId
	  and d.CompanyId=@TargetCompanyId
	  and i.old_id=di.IngredientId
	  and d.old_id=di.DishId

select * from Dishes,DishIngredients where Dishes.CompanyId=@TargetCompanyId 
and DishIngredients.CompanyId=@TargetCompanyId
and DishIngredients.DishId=Dishes.id

select * from Ingredients
select * from IngredientCategories
select * from Categories
rollback tran
go

ALTER TABLE Categories   DROP COLUMN old_id ; 
go
ALTER TABLE IngredientCategories   DROP COLUMN old_id ; 
go

ALTER TABLE Ingredients   DROP COLUMN old_id ; 
go

ALTER TABLE Dishes   DROP COLUMN old_id ; 