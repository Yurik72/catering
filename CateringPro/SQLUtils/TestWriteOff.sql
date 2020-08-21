Declare @iid int
SET @iid=11
select * from Ingredients where Id=@iid
select * from Consignment where IngredientsId=@iid
select * from ConsignmentMove where IngredientsId=@iid

begin tran

update DocLines set quantity=100, IngredientsId=@iid where id=3531
select * from Ingredients where Id=@iid
select * from Consignment where IngredientsId=@iid
select * from ConsignmentMove where IngredientsId=@iid
rollback tran 