/*
-- GetInventory
CREATE PROCEDURE GetInventory
AS
BEGIN
    SELECT ProductId, ProductName, Quantity FROM Inventory;
END

-- UpdateInventory
CREATE PROCEDURE UpdateInventory
    @UserName NVARCHAR(50),
    @ProductId INT,
    @QuantityChanged INT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT;

    -- Check if user exists, if not, insert
    IF NOT EXISTS (SELECT 1 FROM Users WHERE UserName = @UserName)
    BEGIN
        INSERT INTO Users (UserName) VALUES (@UserName);
    END

    SELECT @UserId = UserId FROM Users WHERE UserName = @UserName;

    -- Update inventory
    UPDATE Inventory
    SET Quantity = Quantity + @QuantityChanged
    WHERE ProductId = @ProductId;

    -- Insert into Transactions
    INSERT INTO Transactions (UserId, ProductId, QuantityChanged)
    VALUES (@UserId, @ProductId, @QuantityChanged);
END

-- GetTransactions
CREATE PROCEDURE GetTransactions
AS
BEGIN
    SELECT 
        T.TransactionId,
        U.UserName,
        I.ProductName,
        T.QuantityChanged,
        T.TransactionDate
    FROM Transactions T
    INNER JOIN Users U ON T.UserId = U.UserId
    INNER JOIN Inventory I ON T.ProductId = I.ProductId
    ORDER BY T.TransactionDate DESC;
END

-- GetLeaderboard
CREATE PROCEDURE GetLeaderboard
AS
BEGIN
    SELECT
        U.UserName,
        SUM(CASE WHEN T.QuantityChanged > 0 THEN T.QuantityChanged ELSE 0 END) AS TotalAdded,
        SUM(CASE WHEN T.QuantityChanged < 0 THEN T.QuantityChanged ELSE 0 END) AS TotalRemoved
    FROM Transactions T
    INNER JOIN Users U ON T.UserId = U.UserId
    GROUP BY U.UserName
    ORDER BY TotalAdded DESC, TotalRemoved ASC;
END

--ResetInventory
CREATE PROCEDURE ResetInventory
AS
BEGIN
    SET NOCOUNT ON;

    -- Disable foreign key constraints
    ALTER TABLE Transactions NOCHECK CONSTRAINT ALL;
    ALTER TABLE Users NOCHECK CONSTRAINT ALL;

    -- Delete data from tables
    DELETE FROM Transactions;
    DELETE FROM Users;
    DELETE FROM Inventory;

    -- Reset identity columns
    DBCC CHECKIDENT ('Inventory', RESEED, 0);
    DBCC CHECKIDENT ('Users', RESEED, 0);
    DBCC CHECKIDENT ('Transactions', RESEED, 0);

    -- Re-enable foreign key constraints
    ALTER TABLE Users CHECK CONSTRAINT ALL;
    ALTER TABLE Transactions CHECK CONSTRAINT ALL;

    -- Re-insert initial data into Inventory table
    INSERT INTO Inventory (ProductName, Quantity) VALUES ('Cakes', 0);
    INSERT INTO Inventory (ProductName, Quantity) VALUES ('Cookies', 0);
    INSERT INTO Inventory (ProductName, Quantity) VALUES ('Milkshakes', 0);
    INSERT INTO Inventory (ProductName, Quantity) VALUES ('Croissants', 0);
    INSERT INTO Inventory (ProductName, Quantity) VALUES ('Bread', 0);
END
GO

*/