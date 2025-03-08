-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Gép: 127.0.0.1
-- Létrehozás ideje: 2025. Már 08. 15:55
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
(3, 'Tárhely 1 ', 'Tárhely 1 leírás', 100),
(4, 'Tárhely 2', 'Tárhely 2 leírás', 100),
(5, 'Tárhely 3', 'Tárhely 3 leírás', 100),
(6, 'Tárhely 4', 'Tárhely 4 leírás', 100),
(7, 'Tárhely 5', 'Tárhely 5 leírás', 100);

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
(1, 1001, 'Anyag 1', 'm', 100),
(2, 1002, 'Anyag 2', 'liter', 150),
(3, 1003, 'Anyag 3', 'db', 80),
(4, 1004, 'Anyag 4', 'db', 60);

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `roletype`
--

CREATE TABLE `roletype` (
  `RoleID` int(11) NOT NULL,
  `RoleName` varchar(30) NOT NULL,
  `RoleDescription` varchar(256) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `roletype`
--

INSERT INTO `roletype` (`RoleID`, `RoleName`, `RoleDescription`) VALUES
(1, 'user', 'can only move material'),
(2, 'superuser', 'user+material manipulation+location manipulation'),
(3, 'admin', 'superuser+user manupulation');

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
-- Eseményindítók `transaction`
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

-- --------------------------------------------------------

--
-- Tábla szerkezet ehhez a táblához `user`
--

CREATE TABLE `user` (
  `UserID` int(11) NOT NULL,
  `UserName` varchar(30) NOT NULL,
  `Password` varchar(8) NOT NULL,
  `SALT` varchar(64) NOT NULL,
  `HASH` varchar(64) NOT NULL,
  `Role` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_hungarian_ci;

--
-- A tábla adatainak kiíratása `user`
--

INSERT INTO `user` (`UserID`, `UserName`, `Password`, `SALT`, `HASH`, `Role`) VALUES
(1, 'user józsi', '', '', '', 1),
(2, 'superuser béla', '', '', '', 2),
(3, 'admin klára', '', '', '', 3);

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
-- A tábla indexei `roletype`
--
ALTER TABLE `roletype`
  ADD PRIMARY KEY (`RoleID`);

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
  ADD PRIMARY KEY (`UserID`),
  ADD KEY `Role` (`Role`);

--
-- A kiírt táblák AUTO_INCREMENT értéke
--

--
-- AUTO_INCREMENT a táblához `inventory`
--
ALTER TABLE `inventory`
  MODIFY `InventoryID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `location`
--
ALTER TABLE `location`
  MODIFY `LocationID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT a táblához `material`
--
ALTER TABLE `material`
  MODIFY `MaterialID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=6;

--
-- AUTO_INCREMENT a táblához `roletype`
--
ALTER TABLE `roletype`
  MODIFY `RoleID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT a táblához `transaction`
--
ALTER TABLE `transaction`
  MODIFY `TransactionID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT a táblához `user`
--
ALTER TABLE `user`
  MODIFY `UserID` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- Megkötések a kiírt táblákhoz
--

--
-- Megkötések a táblához `inventory`
--
ALTER TABLE `inventory`
  ADD CONSTRAINT `inventory_ibfk_2` FOREIGN KEY (`LocationID`) REFERENCES `location` (`LocationID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `inventory_ibfk_3` FOREIGN KEY (`MaterialID`) REFERENCES `material` (`MaterialID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `transaction`
--
ALTER TABLE `transaction`
  ADD CONSTRAINT `transaction_ibfk_1` FOREIGN KEY (`UserID`) REFERENCES `user` (`UserID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `transaction_ibfk_4` FOREIGN KEY (`TransactionFromID`) REFERENCES `location` (`LocationID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `transaction_ibfk_5` FOREIGN KEY (`TransactionToID`) REFERENCES `location` (`LocationID`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `transaction_ibfk_6` FOREIGN KEY (`MaterialID`) REFERENCES `material` (`MaterialID`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Megkötések a táblához `user`
--
ALTER TABLE `user`
  ADD CONSTRAINT `user_ibfk_1` FOREIGN KEY (`Role`) REFERENCES `roletype` (`RoleID`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
