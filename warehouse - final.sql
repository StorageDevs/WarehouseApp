-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 06, 2025 at 11:00 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `warehouse`
--
CREATE DATABASE IF NOT EXISTS `warehouse` DEFAULT CHARACTER SET utf8 COLLATE utf8_hungarian_ci;
USE `warehouse`;

-- --------------------------------------------------------

--
-- Table structure for table `inventory`
--

CREATE TABLE `inventory` (
  `InventoryID` int(11) NOT NULL,
  `MaterialID` int(11) NOT NULL,
  `LocationID` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `inventory`
--

INSERT INTO `inventory` (`InventoryID`, `MaterialID`, `LocationID`, `Quantity`) VALUES
(9, 1, 3, 20);

-- --------------------------------------------------------

--
-- Table structure for table `location`
--

CREATE TABLE `location` (
  `LocationID` int(11) NOT NULL,
  `LocationName` varchar(50) NOT NULL,
  `LocationDescription` varchar(100) NOT NULL,
  `LocationCapacity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `location`
--

INSERT INTO `location` (`LocationID`, `LocationName`, `LocationDescription`, `LocationCapacity`) VALUES
(1, 'Bevételezés', 'Anyag készletre vétele', 0),
(2, 'Kivezetés', 'Anyag kivezetése a rendszerből', 0),
(3, 'Tárhely 1 ', 'Tárhely 1 leírás', 100),
(4, 'Tárhely 2', 'Tárhely 2 leírás', 100),
(5, 'Tárhely 3', 'Tárhely 3 leírás', 100),
(6, 'Tárhely 4', 'Tárhely 4 leírás', 100),
(7, 'Tárhely 5', 'Tárhely 5 leírás', 100);

-- --------------------------------------------------------

--
-- Table structure for table `material`
--

CREATE TABLE `material` (
  `MaterialID` int(11) NOT NULL,
  `MaterialNumber` int(11) NOT NULL,
  `MaterialDescription` char(100) NOT NULL,
  `Unit` char(10) NOT NULL,
  `price/unit` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `material`
--

INSERT INTO `material` (`MaterialID`, `MaterialNumber`, `MaterialDescription`, `Unit`, `price/unit`) VALUES
(1, 1001, 'Anyag 1', 'm', 100),
(2, 1002, 'Anyag 2', 'liter', 150),
(3, 1003, 'Anyag 3', 'db', 80),
(4, 1004, 'Anyag 4', 'db', 60);

-- --------------------------------------------------------

--
-- Table structure for table `transaction`
--

CREATE TABLE `transaction` (
  `TransactionID` int(11) NOT NULL,
  `TransactionDateTime` datetime NOT NULL DEFAULT current_timestamp(),
  `MaterialID` int(11) NOT NULL,
  `TransactionFromID` int(11) NOT NULL,
  `TransactedQty` int(11) NOT NULL,
  `TransactionToID` int(11) NOT NULL,
  `UserID` varchar(265) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- Dumping data for table `transaction`
--

INSERT INTO `transaction` (`TransactionID`, `TransactionDateTime`, `MaterialID`, `TransactionFromID`, `TransactedQty`, `TransactionToID`, `UserID`) VALUES
(14, '2025-04-06 10:43:10', 1, 1, 20, 3, '');

--
-- Triggers `transaction`
--
DELIMITER $$
CREATE TRIGGER `MaterialTransfer` AFTER INSERT ON `transaction` FOR EACH ROW BEGIN
#1. Létrehoz anyag+tárhely rekorot ha még nem lenne az inventory táblába

#1.1 Betárolás előtt
IF NOT EXISTS
      ( SELECT MaterialID,LocationID
      	FROM inventory
      	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionToID) AND new.TransactionFromID=1
        
     THEN INSERT INTO inventory (LocationID,MaterialID,Quantity)
       	values(new.TransactionToID,new.MaterialID,0);         
    END IF;

#1.2 kitárolás előtt
IF NOT EXISTS
      ( SELECT MaterialID,LocationID
      	FROM inventory
      	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionFromID) AND new.TransactionToID=2
        
     THEN INSERT INTO inventory (LocationID,MaterialID,Quantity)
       	values(new.TransactionFromID,new.MaterialID,0);         
    END IF;

#1.3 mozgatás
	#1.3.1 Indító és fogadó tárhely és anyag
	IF NOT EXISTS
      ( SELECT MaterialID,LocationID
      	FROM inventory
      	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionFromID)  AND new.TransactionFromID!=1 AND new.TransactionToID!=2
        
     THEN 
     	INSERT INTO inventory (LocationID,MaterialID ,Quantity)
       	VALUES(new.TransactionFromID,new.MaterialID,0);      
    END IF;
    
    IF NOT EXISTS
      ( SELECT MaterialID,LocationID
      	FROM inventory
      	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionToID)  AND new.TransactionFromID!=1 AND new.TransactionToID!=2
        
     THEN 
     	INSERT INTO inventory (LocationID,MaterialID ,Quantity)
       	VALUES(new.TransactionToID,new.MaterialID,0);      
    END IF;
     
     
     
     
#2. Inventory tábla értékeinek módosítása az anyagmozgatásnak megfelelően:

#2.1 Bevételez
	IF new.TransactionFromID=1 THEN 
    	UPDATE inventory
        SET Quantity=Quantity+new.TransactedQty
    	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionToID;
	END IF;
#2.2 Kitárol
    IF new.TransactionToID=2 THEN
    	UPDATE inventory
        SET Quantity=Quantity-new.TransactedQty
    	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionFromID;
	END IF;  
#2.3 Mozgat tárhelyek között  
    IF new.TransactionFromID!=1 AND new.TransactionToID!=2 THEN
	#2.3.1 Indító tárhely minuszolás
   		UPDATE inventory
    	SET Quantity=Quantity-new.TransactedQty
    	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionFromID;
	#2.3.2 Fogadó tárhely pluszolás
    	UPDATE inventory
    	SET Quantity=Quantity+new.TransactedQty
    	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionToID; 
	END IF;

END
$$
DELIMITER ;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `inventory`
--
ALTER TABLE `inventory`
  ADD PRIMARY KEY (`InventoryID`),
  ADD KEY `LocationId` (`LocationID`),
  ADD KEY `MaterialNumber` (`MaterialID`);

--
-- Indexes for table `location`
--
ALTER TABLE `location`
  ADD PRIMARY KEY (`LocationID`),
  ADD UNIQUE KEY `LocationName` (`LocationName`);

--
-- Indexes for table `material`
--
ALTER TABLE `material`
  ADD PRIMARY KEY (`MaterialID`),
  ADD UNIQUE KEY `MaterialNumber` (`MaterialNumber`);

--
-- Indexes for table `transaction`
--
ALTER TABLE `transaction`
  ADD PRIMARY KEY (`TransactionID`),
  ADD KEY `TransactionFrom` (`TransactionFromID`),
  ADD KEY `TransactionTo` (`TransactionToID`),
  ADD KEY `UserID` (`UserID`),
  ADD KEY `MaterialNumberID` (`MaterialID`),
  ADD KEY `TransactionFromID` (`TransactionFromID`),
  ADD KEY `TransactionToID` (`TransactionToID`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `inventory`
--
ALTER TABLE `inventory`
  MODIFY `InventoryID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `location`
--
ALTER TABLE `location`
  MODIFY `LocationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;

--
-- AUTO_INCREMENT for table `material`
--
ALTER TABLE `material`
  MODIFY `MaterialID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT for table `transaction`
--
ALTER TABLE `transaction`
  MODIFY `TransactionID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=15;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `inventory`
--
ALTER TABLE `inventory`
  ADD CONSTRAINT `inventory_ibfk_2` FOREIGN KEY (`LocationID`) REFERENCES `location` (`LocationID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `inventory_ibfk_3` FOREIGN KEY (`MaterialID`) REFERENCES `material` (`MaterialID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `transaction`
--
ALTER TABLE `transaction`
  ADD CONSTRAINT `transaction_ibfk_4` FOREIGN KEY (`TransactionFromID`) REFERENCES `location` (`LocationID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `transaction_ibfk_5` FOREIGN KEY (`TransactionToID`) REFERENCES `location` (`LocationID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `transaction_ibfk_6` FOREIGN KEY (`MaterialID`) REFERENCES `material` (`MaterialID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
