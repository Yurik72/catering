ALTER TRIGGER [dbo].[DocLines_Trigger]
   ON  [dbo].[DocLines]
   AFTER INSERT,DELETE,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;