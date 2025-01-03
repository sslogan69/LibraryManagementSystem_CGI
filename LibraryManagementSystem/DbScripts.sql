USE [LibraryDB]
GO

-- Create Books table
CREATE TABLE [dbo].[Books](
    [BookId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [Title] NVARCHAR(255) NOT NULL,
    [Author] NVARCHAR(255) NOT NULL,
    [AvailableCopies] INT NOT NULL
);
GO

-- Create Users table
CREATE TABLE [dbo].[Users](
    [UserId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [UserName] NVARCHAR(255) NOT NULL
);
GO

-- Create BorrowRecords table
CREATE TABLE [dbo].[BorrowRecords](
    [BorrowId] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    [BookId] INT NULL,
    [UserId] INT NULL,
    [BorrowDate] DATETIME NOT NULL,
    [ReturnDate] DATETIME NULL,
    FOREIGN KEY ([BookId]) REFERENCES [dbo].[Books]([BookId]),
    FOREIGN KEY ([UserId]) REFERENCES [dbo].[Users]([UserId])
);
GO

-- Insert sample data into Books table
INSERT INTO [dbo].[Books] ([Title], [Author], [AvailableCopies]) VALUES
('The Great Gatsby', 'F. Scott Fitzgerald', 5),
('To Kill a Mockingbird', 'Harper Lee', 3),
('1984', 'George Orwell', 4),
('Pride and Prejudice', 'Jane Austen', 2),
('The Catcher in the Rye', 'J.D. Salinger', 6);
GO

-- Insert sample data into Users table
INSERT INTO [dbo].[Users] ([UserName]) VALUES
('John Doe'),
('Jane Smith'),
('Alice Johnson'),
('Bob Brown'),
('Charlie Davis');
GO

-- Insert sample data into BorrowRecords table
INSERT INTO [dbo].[BorrowRecords] ([BookId], [UserId], [BorrowDate], [ReturnDate]) VALUES
(1, 1, '2024-01-01', NULL),
(2, 2, '2024-01-02', '2024-01-10'),
(3, 3, '2024-01-03', NULL),
(4, 4, '2024-01-04', '2024-01-12'),
(5, 5, '2024-01-05', NULL);
GO
