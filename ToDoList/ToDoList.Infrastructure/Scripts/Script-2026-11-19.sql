CREATE TABLE dbo.tasks
(
    Id INT IDENTITY(1,1) PRIMARY KEY,
    nazwa NVARCHAR(200) NOT NULL,
    isCompleted BIT NULL DEFAULT 0,
    priority NVARCHAR(50) NULL
);