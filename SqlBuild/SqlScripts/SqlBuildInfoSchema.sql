create schema SqlBuild;

go

create type DBVersion as table (
    Major int,
    Minor int,
    Patch int
);

go

create table SqlBuildInfo (
    Id bigint Identity,
    ProjectName nvarchar(1024),
    Version DBVersion,
    InstallTime datetime2
);

go