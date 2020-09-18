declare @ingid int
set @ingid=760

select * from Consignment where IngredientsId=@ingid

select * from ConsignmentMove where IngredientsId=@ingid

select * from DocLines dl, where IngredientsId=@ingid