
delete from DishComplex where dishid=1361 and companyid=8
ALTER TABLE [dbo].[DishComplex]  WITH CHECK ADD  CONSTRAINT [FK_DishComplex_Complex_ComplexId] FOREIGN KEY([ComplexId])
REFERENCES [dbo].[Complex] ([Id])
GO

ALTER TABLE [dbo].[DishComplex] CHECK CONSTRAINT [FK_DishComplex_Complex_ComplexId]
GO



ALTER TABLE [dbo].[DishComplex]  WITH CHECK ADD  CONSTRAINT [FK_DishComplex_Dishes_DishId] FOREIGN KEY([DishId])
REFERENCES [dbo].[Dishes] ([Id])
GO

ALTER TABLE [dbo].[DishComplex] CHECK CONSTRAINT [FK_DishComplex_Dishes_DishId]
GO
