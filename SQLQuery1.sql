CREATE TABLE [InfoFiles]
(
IdFile INT PRIMARY KEY IDENTITY (1,1) NOT NULL,
[FileName] NVARCHAR(max) NOT NULL,
[FilePath] NVARCHAR(max) NOT NULL,
[FileSize] NVARCHAR(max) NOT NULL,
)


select *from [InfoFiles]