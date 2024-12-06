/*
-- Create Inventory table
CREATE TABLE Inventory (
    ProductId INT PRIMARY KEY IDENTITY(1,1),
    ProductName NVARCHAR(50),
    Quantity INT
);

-- Create Users table
CREATE TABLE Users (
    UserId INT PRIMARY KEY IDENTITY(1,1),
    UserName NVARCHAR(50) UNIQUE
);

-- Create Transactions table
CREATE TABLE Transactions (
    TransactionId INT PRIMARY KEY IDENTITY(1,1),
    UserId INT FOREIGN KEY REFERENCES Users(UserId),
    ProductId INT FOREIGN KEY REFERENCES Inventory(ProductId),
    QuantityChanged INT,
    TransactionDate DATETIME DEFAULT GETDATE()
);

INSERT INTO Inventory (ProductName, Quantity) VALUES ('Cakes', 0);
INSERT INTO Inventory (ProductName, Quantity) VALUES ('Cookies', 0);
INSERT INTO Inventory (ProductName, Quantity) VALUES ('Milkshakes', 0);
INSERT INTO Inventory (ProductName, Quantity) VALUES ('Croissants', 0);
INSERT INTO Inventory (ProductName, Quantity) VALUES ('Bread', 0);

*/