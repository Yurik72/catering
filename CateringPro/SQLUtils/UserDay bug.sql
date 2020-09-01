select u.Quantity,a.cnt from UserDay u,

(
select count(*)as cnt,userid,companyid,date from UserDayComplex group by userid,companyid,date
) a

where a.userid=u.userid and a.companyid=u.companyid and a.Date=u.Date and a.cnt!=u.Quantity
