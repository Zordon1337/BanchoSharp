-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: localhost
-- Generation Time: 19. Feb, 2024 00:03 AM
-- Tjener-versjon: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `test`
--

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `scores`
--

CREATE TABLE `scores` (
  `ReplayId` text DEFAULT NULL,
  `BeatmapHash` text DEFAULT NULL,
  `username` text DEFAULT NULL,
  `score` text DEFAULT NULL,
  `combo` text DEFAULT NULL,
  `fc` text DEFAULT NULL,
  `mods` text DEFAULT NULL,
  `count300` text DEFAULT NULL,
  `count100` text DEFAULT NULL,
  `count50` text DEFAULT NULL,
  `countGeki` text DEFAULT NULL,
  `countKatu` text DEFAULT NULL,
  `countMiss` text DEFAULT NULL,
  `time` text DEFAULT NULL,
  `mode` text DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dataark for tabell `scores`
--

INSERT INTO `scores` (`ReplayId`, `BeatmapHash`, `username`, `score`, `combo`, `fc`, `mods`, `count300`, `count100`, `count50`, `countGeki`, `countKatu`, `countMiss`, `time`, `mode`) VALUES
('0', '12a4873fbada3b8a3724c2c3a246b066', 'ppy', '10900', '49', 'True', '208', '22', '0', '0', '12', '0', '0', '240218225725', '0');

-- --------------------------------------------------------

--
-- Tabellstruktur for tabell `users`
--

CREATE TABLE `users` (
  `userid` text NOT NULL,
  `username` text NOT NULL,
  `password` text NOT NULL,
  `totalscore` text NOT NULL,
  `accuracy` text NOT NULL,
  `PP` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=latin1 COLLATE=latin1_swedish_ci;

--
-- Dataark for tabell `users`
--

INSERT INTO `users` (`userid`, `username`, `password`, `totalscore`, `accuracy`, `PP`) VALUES
('1', 'ppy', 'e373a9be7afbfa19aa17eaa54f19af88', '10900', '1', '0');
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
