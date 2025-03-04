-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Már 04. 20:12
-- Kiszolgáló verziója: 10.4.32-MariaDB
-- PHP verzió: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Adatbázis: `warehouse`
--

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `inventory`
--

CREATE TABLE `inventory` (
  `InventoryID` int(11) NOT NULL,
  `MaterialID` int(11) NOT NULL,
  `LocationID` int(11) NOT NULL,
  `Quantity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `inventory`
--

INSERT INTO `inventory` (`InventoryID`, `MaterialID`, `LocationID`, `Quantity`) VALUES
(25, 1, 3, 25),
(26, 1, 4, 0),
(27, 2, 3, 0),
(28, 2, 4, 0),
(29, 1, 1, 0);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `location`
--

CREATE TABLE `location` (
  `LocationID` int(11) NOT NULL,
  `LocationName` varchar(50) NOT NULL,
  `LocationDescription` varchar(100) NOT NULL,
  `LocationCapacity` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `location`
--

INSERT INTO `location` (`LocationID`, `LocationName`, `LocationDescription`, `LocationCapacity`) VALUES
(1, 'Bevételezés', 'Anyag készletre vétele', 0),
(2, 'Kivezetés', 'Anyag kivezetése a rendszerből', 0),
(3, 'Rekesz 1', 'Rekesz 1 leírás', 10),
(4, 'Rekesz 2', 'Rekesz 2 leírás', 10);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `material`
--

CREATE TABLE `material` (
  `MaterialID` int(11) NOT NULL,
  `MaterialNumber` int(11) NOT NULL,
  `MaterialDescription` char(100) NOT NULL,
  `Unit` char(10) NOT NULL,
  `price/unit` decimal(10,0) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `material`
--

INSERT INTO `material` (`MaterialID`, `MaterialNumber`, `MaterialDescription`, `Unit`, `price/unit`) VALUES
(1, 100, 'Anyag 1', 'db', 10),
(2, 101, 'Anyag 2', 'm', 30);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `transaction`
--

CREATE TABLE `transaction` (
  `TransactionID` int(11) NOT NULL,
  `TransactionDateTime` datetime NOT NULL DEFAULT current_timestamp(),
  `MaterialID` int(11) NOT NULL,
  `TransactionFromID` int(11) NOT NULL,
  `TransactedQty` int(11) NOT NULL,
  `TransactionToID` int(11) NOT NULL,
  `UserID` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `transaction`
--

INSERT INTO `transaction` (`TransactionID`, `TransactionDateTime`, `MaterialID`, `TransactionFromID`, `TransactedQty`, `TransactionToID`, `UserID`) VALUES
(36, '2025-03-04 19:29:50', 1, 1, 25, 3, 1);

--
-- Eseményindítók `transaction`
--
DELIMITER $$
CREATE TRIGGER `MaterialTransfer` AFTER INSERT ON `transaction` FOR EACH ROW BEGIN
#létrehoz anyag+tárhely rekorot ha még nem lenne, még nem működik rendesen

IF NOT EXISTS
      ( SELECT MaterialID,LocationID
      	FROM inventory
      	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionFromID )
     THEN 
    	INSERT INTO inventory (LocationID,MaterialID,Quantity)
        values(new.TransactionFromID,new.MaterialID,0);
        
         
    END IF;

IF NOT EXISTS
      ( SELECT MaterialID,LocationID
      	FROM inventory
      	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionToID )
     THEN 
    	INSERT INTO inventory (LocationID,MaterialID,Quantity)
        values(new.TransactionToID,new.MaterialID,0);
        
         
    END IF;


#Bevételez
	IF new.TransactionFromID=1 THEN 
    	UPDATE inventory
        SET Quantity=Quantity+new.TransactedQty
    	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionToID;
	END IF;
#Kitárol
    IF new.TransactionToID=2 THEN
    	UPDATE inventory
        SET Quantity=Quantity-new.TransactedQty
    	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionFromID;
	END IF;  
#Mozgat tárhelyek között  
    IF new.TransactionFromID!=1 AND new.TransactionToID!=2 THEN
	#indító tárhely minuszolás
   		UPDATE inventory
    	SET Quantity=Quantity-new.TransactedQty
    	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionFromID;
	#fogadó tárhely pluszolás
    	UPDATE inventory
    	SET Quantity=Quantity+new.TransactedQty
    	WHERE MaterialID=new.MaterialID AND LocationID=new.TransactionToID; 
	END IF;

END
$$
DELIMITER ;
DELIMITER $$
CREATE TRIGGER `TransferValidate` BEFORE INSERT ON `transaction` FOR EACH ROW BEGIN
#if no material&location in inventory tabe-->no logging. 
	

END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user`
--

CREATE TABLE `user` (
  `UserID` int(11) NOT NULL,
  `UserName` varchar(30) NOT NULL,
  `Password` varchar(8) NOT NULL,
  `Role` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`UserID`, `UserName`, `Password`, `Role`) VALUES
(1, 'Operátor Józsi', 'operator', 'Operator'),
(2, 'SuperUser Kata', 'supuser', 'Superuser'),
(3, 'Admin Tibor', 'admin', 'Administrator');

--
-- Indexek a kiírt táblákhoz
--

--
-- A tábla indexei `inventory`
--
ALTER TABLE `inventory`
  ADD PRIMARY KEY (`InventoryID`),
  ADD KEY `LocationId` (`LocationID`),
  ADD KEY `MaterialNumber` (`MaterialID`);

--
-- A tábla indexei `location`
--
ALTER TABLE `location`
  ADD PRIMARY KEY (`LocationID`);

--
-- A tábla indexei `material`
--
ALTER TABLE `material`
  ADD PRIMARY KEY (`MaterialID`);

--
-- A tábla indexei `transaction`
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
-- A tábla indexei `user`
--
ALTER TABLE `user`
  ADD PRIMARY KEY (`UserID`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `inventory`
--
ALTER TABLE `inventory`
  MODIFY `InventoryID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=30;

--
-- AUTO_INCREMENT a táblához `location`
--
ALTER TABLE `location`
  MODIFY `LocationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT a táblához `material`
--
ALTER TABLE `material`
  MODIFY `MaterialID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=12;

--
-- AUTO_INCREMENT a táblához `transaction`
--
ALTER TABLE `transaction`
  MODIFY `TransactionID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=37;

--
-- AUTO_INCREMENT a táblához `user`
--
ALTER TABLE `user`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `inventory`
--
ALTER TABLE `inventory`
  ADD CONSTRAINT `inventory_ibfk_2` FOREIGN KEY (`LocationID`) REFERENCES `location` (`LocationID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `inventory_ibfk_3` FOREIGN KEY (`MaterialID`) REFERENCES `material` (`MaterialID`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Megkötések a táblához `transaction`
--
ALTER TABLE `transaction`
  ADD CONSTRAINT `transaction_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `user` (`UserID`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `transaction_ibfk_4` FOREIGN KEY (`TransactionFromID`) REFERENCES `location` (`LocationID`),
  ADD CONSTRAINT `transaction_ibfk_5` FOREIGN KEY (`TransactionToID`) REFERENCES `location` (`LocationID`),
  ADD CONSTRAINT `transaction_ibfk_6` FOREIGN KEY (`MaterialID`) REFERENCES `material` (`MaterialID`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
