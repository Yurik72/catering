declare @userid nvarchar(100)
set @userid='cc2c3c17-7b8b-4a93-8d7b-49eaaab54fd2'
select * from UserFinances where id=@userid
select * from UserFinIncomes where id=@userid