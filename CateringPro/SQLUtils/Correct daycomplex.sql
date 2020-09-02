
--delete from DishComplex where dishid=1361 and companyid=8
delete from userdaycomplex where complexid=21
delete from [DayComplex] where ComplexId=20
delete from [DayComplex] where ComplexId=22
delete from [DayComplex] where ComplexId=29
delete from [DayComplex] where ComplexId=31
delete from [DayComplex] where ComplexId=70
delete from [DayComplex] where ComplexId=21

ALTER TABLE [dbo].[DayComplex]  WITH CHECK ADD  CONSTRAINT [FK_DayComplex_Complex_ComplexId] FOREIGN KEY([ComplexId])
REFERENCES [dbo].[Complex] ([Id])
GO

ALTER TABLE [dbo].[DayComplex] CHECK CONSTRAINT [FK_DayComplex_Complex_ComplexId]
GO



