DELETE FROM UserFinOutComes;
DELETE FROM UserFinIncomes;
DISABLE TRIGGER UserDay.UserDayGuard_Trigger
ON UserDay;
DELETE FROM UserDay;
DELETE FROM UserDayComplex;
DELETE FROM UserDayDish;
ENABLE TRIGGER UserDay.UserDayGuard_Trigger
ON UserDay;
--to clear all avaible complexes per day
DELETE FROM DayComplex;
DELETE FROM DayDish;