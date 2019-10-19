-- phpMyAdmin SQL Dump
-- version 4.9.1
-- https://www.phpmyadmin.net/
--
-- Хост: 127.0.0.1
-- Час створення: Жов 19 2019 р., 13:56
-- Версія сервера: 10.4.8-MariaDB
-- Версія PHP: 7.3.10

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET AUTOCOMMIT = 0;
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- База даних: `currency`
--

-- --------------------------------------------------------

--
-- Структура таблиці `currency_type`
--

CREATE TABLE `currency_type` (
  `Cur_id` int(11) NOT NULL,
  `Cur` varchar(3) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп даних таблиці `currency_type`
--

INSERT INTO `currency_type` (`Cur_id`, `Cur`) VALUES
(1, 'JPY'),
(2, 'RUB'),
(3, 'GBP'),
(4, 'USD'),
(5, 'BYN'),
(6, 'EUR'),
(7, 'PLN');

-- --------------------------------------------------------

--
-- Структура таблиці `ratio`
--

CREATE TABLE `ratio` (
  `id` int(11) NOT NULL,
  `type_id` int(11) DEFAULT NULL,
  `ratio` varchar(12) DEFAULT NULL,
  `r_date` date DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Дамп даних таблиці `ratio`
--

INSERT INTO `ratio` (`id`, `type_id`, `ratio`, `r_date`) VALUES
(1, 3, '31.65215', '2019-10-19'),
(2, 2, '0.38638', '2019-10-19');

--
-- Індекси збережених таблиць
--

--
-- Індекси таблиці `currency_type`
--
ALTER TABLE `currency_type`
  ADD PRIMARY KEY (`Cur_id`);

--
-- Індекси таблиці `ratio`
--
ALTER TABLE `ratio`
  ADD PRIMARY KEY (`id`),
  ADD KEY `type_id` (`type_id`);

--
-- AUTO_INCREMENT для збережених таблиць
--

--
-- AUTO_INCREMENT для таблиці `currency_type`
--
ALTER TABLE `currency_type`
  MODIFY `Cur_id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT для таблиці `ratio`
--
ALTER TABLE `ratio`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- Обмеження зовнішнього ключа збережених таблиць
--

--
-- Обмеження зовнішнього ключа таблиці `ratio`
--
ALTER TABLE `ratio`
  ADD CONSTRAINT `ratio_ibfk_1` FOREIGN KEY (`type_id`) REFERENCES `currency_type` (`Cur_id`);
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
