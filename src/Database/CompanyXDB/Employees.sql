CREATE SEQUENCE employee_code_seq
  AS BIGINT
  START WITH 1
  INCREMENT BY 1
  MINVALUE 1
  NO MAXVALUE
  NO CYCLE
  NO CACHE;

GO

CREATE FUNCTION generateNewEmployeeCode(@nextSeqValInt bigint)
RETURNS Char(8)
AS
	BEGIN		
		declare @codeEmp  char(8)
		declare @nextSeqValString varchar(7) = CAST(@nextSeqValInt as varchar(7))		
		declare @firstLetter char(1) = 'X';

		SET @codeEmp = CASE 
			WHEN  @nextSeqValInt > 0 and @nextSeqValInt < 10 THEN @firstLetter+'000000'+@nextSeqValString
			WHEN  @nextSeqValInt >= 10 and @nextSeqValInt < 100 THEN @firstLetter+'00000'+@nextSeqValString
			WHEN  @nextSeqValInt >= 100 and @nextSeqValInt < 1000 THEN @firstLetter+'0000'+@nextSeqValString
			WHEN  @nextSeqValInt >= 1000 and @nextSeqValInt < 10000 THEN @firstLetter+'000'+@nextSeqValString
			WHEN  @nextSeqValInt >= 10000 and @nextSeqValInt < 100000 THEN @firstLetter+'00'+@nextSeqValString
			WHEN  @nextSeqValInt >= 100000 and @nextSeqValInt < 1000000 THEN @firstLetter+'0'+@nextSeqValString
			WHEN  @nextSeqValInt >= 1000000 and @nextSeqValInt < 10000000 THEN @firstLetter+@nextSeqValString
		END		
	RETURN @codeEmp
	END
GO

CREATE TABLE [dbo].[Employees]
(
	[empId] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT newid(), 
    [empLastName] VARCHAR(50) NOT NULL, 
    [empFirstName] VARCHAR(50) NULL, 
    [empMiddleName] VARCHAR(50) NULL, 
    [empSalary] DECIMAL(8, 2) NOT NULL, 
    [empManager] UNIQUEIDENTIFIER NULL, 
    [empTitle] VARCHAR(50) NOT NULL, 
    [empCode] CHAR(8) NOT NULL DEFAULT ([dbo].[generateNewEmployeeCode](CONVERT([bigint],NEXT VALUE FOR [dbo].[employee_code_seq]))), 
    [UTCRegisteredOn] BIGINT NOT NULL, 
    [empImage] VARCHAR(13) NULL, 
    [UTCStartDate] BIGINT NOT NULL, 
    CONSTRAINT [FK_Employees_Manager] FOREIGN KEY ([empManager]) REFERENCES [Employees]([empId]), 
    CONSTRAINT [CK_Employees_empCode] CHECK (empSalary > 0)
)

GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'This is the primary key for identifying an employee',
    @level0type = N'SCHEMA',
    @level0name = N'dbo',
    @level1type = N'TABLE',
    @level1name = N'Employees',
    @level2type = N'COLUMN',
    @level2name = N'empId'
GO

CREATE INDEX [IX_Employees_LastName] ON [dbo].[Employees] ([empLastName])

GO

CREATE INDEX [IX_Employees_FirstName] ON [dbo].[Employees] ([empFirstName])

GO

CREATE INDEX [IX_Employees_MiddleName] ON [dbo].[Employees] ([empMiddleName])

GO

CREATE UNIQUE INDEX [IX_Employees_empCode] ON [dbo].[Employees] ([empCode])
GO

