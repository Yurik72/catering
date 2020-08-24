
/*
select Count(*),emailchild from importusers group by emailchild


select *,
RANK() OVER(Partition by EmailChild ORDER BY id ASC) AS LineNum
from importusers order by id
*/
Declare @CompanyId int
Set @companyid=8

begin tran
-- 0step correct data assign not existing email
Update importusers 
Set EmailChild=Email
Where EmailChild IS NULL

-- fisr assign parent
Update importusers
Set ParentId=parent.id, 
Line=p.LineNum
--select *  
from importusers im,
(
select *,
RANK() OVER(Partition by Email ORDER BY id ASC) AS LineNum
from importusers  where Email IS NOT NULL 
) p,

(
select *,
RANK() OVER(Partition by Email ORDER BY id ASC) AS LineNum
from importusers  /*where EmailChild IS NOT NULL */
) parent
where  im.Email=p.Email and 
p.Email=parent.Email and p.LineNum>1 and parent.linenum=1 and parent.id!=im.id


--select NEWID(), * from importusers


update importusers
Set Guid = LOWER(CONVERT(nvarchar(100), NEWID())), 
    SecurityStamp='LV5AXAGEH7E5CBR3GVAXIA3J3ZXNBPLE',
	PasswordHash='AQAAAAEAACcQAAAAEO9Urei/eMEZ1Ymo7OivBCpVX5OMTat8e1M54c4GWQ6ZAZq5jxZpvWgHCfC/S+KN7w=='
    
update importusers 
set ParentUserGuid=parent.Guid
from importusers im,
(select * from importusers) parent
where im.parentid IS NOT NULL and im.parentid=parent.id


select * from importusers order by id
insert into AspNetUsers(id,UserName,NormalizedUserName,
						Email, NormalizedEmail,
                        EmailConfirmed,PhoneNumberConfirmed,
						TwoFactorEnabled,LockoutEnabled,AccessFailedCount,
						PasswordHash,SecurityStamp,
						CompanyId, ConfirmedByAdmin,NameSurname,
						ChildrenCount,ChildNameSurname,ParentUserId,ext1,EmailSecond)
select _first.* from (
select  Guid,
UserName=CASE  WHEN im.Line IS  NULL THEN im.Email ELSE 'child'+CAST(im.Line as nvarchar(2))+'_'+im.Email end ,
NormalizedUserName=NULL,
Email=CASE  WHEN im.Line IS  NULL THEN im.Email ELSE 'child'+CAST(im.Line as nvarchar(2))+'_'+im.Email end,
NormalizedEmail=NULL,
EmailConfirmed=1, /*email confirmed*/
PhoneNumberConfirmed=0,
TwoFactorEnabled=0,/*TwoFactorEnabled*/
LockoutEnabled=1,
AccessFailedCount=0,/*AccessFailedCount*/
PasswordHash=im.PasswordHash,
SecurityStamp=im.SecurityStamp,
CompanyId=@CompanyId,
ConfirmedByAdmin=1,
NameSurname=Name,
ChildrenCount=(Select  count(*) +1 as chcount from importusers ch where ch.parentid=im.id),
ChildNameSurname=ChildName,
ParentUserId=ParentUserGuid,
ext1=id,
EmailSecond=im.emailchild
from importusers  im
) _first
LEFT JOIN  AspNetUsers dbl  on dbl.UserName=_first.UserName
where dbl.UserName IS NULL


Update Aspnetusers
Set NormalizedUserName=UPPER(UserName),
NormalizedEmail =UPPER(Email)
 where ext1 is not null   
 
Select *from Aspnetusers

Insert into CompanyUserCompanies (CompanyID,CompanyUserId)
Select @CompanyId,u.id from AspNetUsers u
Left Join CompanyUserCompanies uc On uc.CompanyUserId=u.id and uc.CompanyId=@CompanyId
where uc.CompanyUserId IS NULL


Insert into UserFinances(Id,CompanyId)
Select u.id,@CompanyId from AspNetUsers u
Left Join UserFinances uc On uc.id=u.id and uc.CompanyId=@CompanyId
where uc.id IS NULL

rollback tran