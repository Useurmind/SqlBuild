create procedure procedure1
as
begin
    select * from table1;
end

go

create procedure procedure2
as
begin
    select * from table2;
end

go

create procedure procedure3
as
begin
    exec procedure1;
end