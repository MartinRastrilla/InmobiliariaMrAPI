-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: 127.0.0.1
-- Tiempo de generación: 04-11-2025 a las 22:27:31
-- Versión del servidor: 10.4.32-MariaDB
-- Versión de PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `inmobiliaria_mr_db`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `archivoinmuebles`
--

CREATE TABLE `archivoinmuebles` (
  `Id` int(11) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `ArchivoId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `archivoinmuebles`
--

INSERT INTO `archivoinmuebles` (`Id`, `InmuebleId`, `ArchivoId`) VALUES
(1, 17, 1),
(2, 17, 2),
(3, 17, 3),
(4, 18, 4),
(5, 18, 5),
(6, 18, 6),
(7, 19, 7),
(8, 19, 8),
(9, 19, 9),
(10, 20, 10),
(11, 20, 11),
(12, 20, 12);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `archivos`
--

CREATE TABLE `archivos` (
  `Id` int(11) NOT NULL,
  `Nombre` longtext NOT NULL,
  `Ruta` longtext NOT NULL,
  `Fecha` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `archivos`
--

INSERT INTO `archivos` (`Id`, `Nombre`, `Ruta`, `Fecha`) VALUES
(1, 'prueba hotel', '/inmuebles/a81a00796e1e41bf.jpg', '2025-11-02 16:45:32.984184'),
(2, 'prueba hotel 2', '/inmuebles/c6cb63ec656f4097.jpg', '2025-11-02 16:45:33.090390'),
(3, 'prueba hotel 3', '/inmuebles/eae86b01e46c433f.jpg', '2025-11-02 16:45:33.391418'),
(4, 'image_4551337993297723517', '/inmuebles/ef8eb8f148c8499b.jpg', '2025-11-02 19:08:27.915881'),
(5, 'image_194761325882514738', '/inmuebles/311a4ca406994f26.jpg', '2025-11-02 19:08:28.107660'),
(6, 'image_2992605189302267867', '/inmuebles/0ad496c051f04473.jpg', '2025-11-02 19:08:28.178686'),
(7, 'image_4001701644193474924', '/inmuebles/b4724dc6b3d0405f.jpg', '2025-11-02 19:48:06.905975'),
(8, 'image_2648895038090497869', '/inmuebles/1858926af97b4c02.jpg', '2025-11-02 19:48:06.993040'),
(9, 'image_8432809953584222076', '/inmuebles/b06bab93d9fe4054.jpg', '2025-11-02 19:48:07.167410'),
(10, 'image_960555268448605335', '/inmuebles/794e355e793d48ea.jpg', '2025-11-02 20:30:28.156981'),
(11, 'image_3233218511401256245', '/inmuebles/5b633e58a71e4f1e.jpg', '2025-11-02 20:30:28.367608'),
(12, 'image_6371928928757725482', '/inmuebles/7f1fe72d232c49f6.jpg', '2025-11-02 20:30:28.496394');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratoinquilinos`
--

CREATE TABLE `contratoinquilinos` (
  `Id` int(11) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `IsPaymentResponsible` tinyint(1) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratoinquilinos`
--

INSERT INTO `contratoinquilinos` (`Id`, `ContratoId`, `InquilinoId`, `IsPaymentResponsible`) VALUES
(1, 1, 1, 1),
(2, 1, 2, 0),
(3, 2, 2, 1);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `contratos`
--

CREATE TABLE `contratos` (
  `Id` int(11) NOT NULL,
  `StartDate` datetime(6) NOT NULL,
  `EndDate` datetime(6) NOT NULL,
  `TotalPrice` decimal(65,30) NOT NULL,
  `MonthlyPrice` decimal(65,30) DEFAULT NULL,
  `Status` int(11) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `InmuebleId` int(11) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `InquilinoId` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `contratos`
--

INSERT INTO `contratos` (`Id`, `StartDate`, `EndDate`, `TotalPrice`, `MonthlyPrice`, `Status`, `CreatedAt`, `UpdatedAt`, `InmuebleId`, `PropietarioId`, `InquilinoId`) VALUES
(1, '2025-11-03 22:53:02.000000', '2025-11-24 22:53:02.000000', 35000.000000000000000000000000000000, NULL, 0, '2025-11-03 00:00:00.000000', '2025-11-03 00:00:00.000000', 2, 1, 1),
(2, '2025-11-04 23:01:24.000000', '2025-11-08 23:01:24.000000', 500000.000000000000000000000000000000, NULL, 1, '2025-11-03 00:00:00.000000', '2025-11-03 00:00:00.000000', 3, 1, 2);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inmuebles`
--

CREATE TABLE `inmuebles` (
  `Id` int(11) NOT NULL,
  `Title` varchar(100) NOT NULL,
  `Address` varchar(200) NOT NULL,
  `Rooms` int(11) NOT NULL,
  `Price` decimal(65,30) NOT NULL,
  `MaxGuests` int(11) DEFAULT NULL,
  `Available` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `PropietarioId` int(11) NOT NULL,
  `Latitude` varchar(100) DEFAULT NULL,
  `Longitude` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inmuebles`
--

INSERT INTO `inmuebles` (`Id`, `Title`, `Address`, `Rooms`, `Price`, `MaxGuests`, `Available`, `CreatedAt`, `UpdatedAt`, `PropietarioId`, `Latitude`, `Longitude`) VALUES
(1, 'Magnolia', 'Av. Cerranías Vivas 124', 3, 150000.000000000000000000000000000000, 7, 0, '2025-10-21 04:18:14.933202', '2025-10-21 21:32:53.702899', 2, '', ''),
(2, 'Hotel del Monte Dorado', 'Escondido por el Salto de la Moneda', 120, 900000.000000000000000000000000000000, 1200, 1, '2025-10-21 23:00:02.299831', '2025-10-21 23:00:02.299875', 1, NULL, NULL),
(3, 'Hotel del Monte Libre', 'Escondido por el Salto de la Negra Libre', 45, 600000.000000000000000000000000000000, 220, 1, '2025-10-21 23:00:48.212352', '2025-10-21 23:00:48.212353', 1, NULL, NULL),
(4, 'Valle Lindo', 'Escondido por el Valle del Conlara', 36, 220000.000000000000000000000000000000, 95, 0, '2025-11-01 03:54:07.287256', '2025-11-01 03:54:07.287268', 1, NULL, NULL),
(9, 'Cuidado', 'Fortuna', 2, 5.000000000000000000000000000000, 10, 1, '2025-11-01 04:11:56.956138', '2025-11-01 04:11:56.956138', 1, NULL, NULL),
(10, 'Mengui House', 'Las Menguitas 123', 2, 55000.000000000000000000000000000000, 6, 0, '2025-11-02 14:30:38.447420', '2025-11-02 14:30:38.447431', 3, NULL, NULL),
(17, 'Prueba con Fotos', 'Av. Prueba con Fotos 123', 12, 15000.000000000000000000000000000000, 10, 0, '2025-11-02 16:45:32.559764', '2025-11-02 16:45:32.559783', 1, NULL, NULL),
(18, 'El Escondrujo', 'Debajo del Puente Blanco', 1, 500.000000000000000000000000000000, 2, 1, '2025-11-02 19:08:27.200526', '2025-11-02 19:08:27.200576', 1, NULL, NULL),
(19, 'Mengui Hostel', 'Las Rositas 155', 6, 350000.000000000000000000000000000000, 38, 1, '2025-11-02 19:48:06.570306', '2025-11-02 19:48:06.570306', 3, NULL, NULL),
(20, 'Inmueble de prueba', 'Prueba 123', 5, 300000.000000000000000000000000000000, 25, 1, '2025-11-02 20:30:27.817691', '2025-11-02 20:30:27.817691', 6, NULL, NULL);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `inquilinos`
--

CREATE TABLE `inquilinos` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `DocumentNumber` varchar(15) NOT NULL,
  `Phone` varchar(15) DEFAULT NULL,
  `Email` varchar(100) DEFAULT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `inquilinos`
--

INSERT INTO `inquilinos` (`Id`, `Name`, `LastName`, `DocumentNumber`, `Phone`, `Email`, `IsActive`, `CreatedAt`, `UpdatedAt`) VALUES
(1, 'Pepe', 'Inquilino', '22333444', '2664718256', 'pinquilino@gmail.com', 1, '2025-11-03 22:21:17.000000', '2025-11-03 22:21:22.000000'),
(2, 'Maria', 'Inquilina', '33222111', NULL, NULL, 1, '2025-11-03 22:25:42.000000', '2025-11-03 22:25:46.000000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `pagos`
--

CREATE TABLE `pagos` (
  `Id` int(11) NOT NULL,
  `ContratoId` int(11) NOT NULL,
  `InquilinoId` int(11) NOT NULL,
  `Amount` decimal(65,30) NOT NULL,
  `PaymentDate` datetime(6) NOT NULL,
  `PaymentMethod` int(11) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `pagos`
--

INSERT INTO `pagos` (`Id`, `ContratoId`, `InquilinoId`, `Amount`, `PaymentDate`, `PaymentMethod`, `CreatedAt`, `UpdatedAt`) VALUES
(1, 1, 1, 35000.000000000000000000000000000000, '2025-11-03 00:00:00.000000', 0, '2025-11-03 00:00:00.000000', '2025-11-03 00:00:00.000000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `propietarios`
--

CREATE TABLE `propietarios` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `LastName` varchar(100) NOT NULL,
  `DocumentNumber` varchar(15) NOT NULL,
  `Phone` varchar(15) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `UserId` int(11) NOT NULL DEFAULT 0
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `propietarios`
--

INSERT INTO `propietarios` (`Id`, `Name`, `LastName`, `DocumentNumber`, `Phone`, `Email`, `UserId`) VALUES
(1, 'Pablo', 'Propietario', '12345678', '2664718256', 'pp@gmail.com', 2),
(2, 'Admin', 'Admin', '43490178', '2664718256', 'admin@admin.com', 3),
(3, 'Menguita', 'Rastrilla', '43490179', '2664718256', 'menguirastrilla@gmail.com', 4),
(4, 'Martin', 'Rastrilla', '43490170', '2664718256', 'rastrillamartin2@gmail.com', 5),
(5, 'algo', 'algo', '12345679', '2465656', 'qlgo@a.co', 6),
(6, 'Prueba Otro', 'Prueba', '12345670', '2664718256', 'prueba@gmail.com', 7);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `roles`
--

CREATE TABLE `roles` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Description` varchar(500) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `roles`
--

INSERT INTO `roles` (`Id`, `Name`, `Description`, `CreatedAt`, `UpdatedAt`) VALUES
(1, 'Admin', 'Realiza todas las funciones', '0000-00-00 00:00:00.000000', '0000-00-00 00:00:00.000000'),
(2, 'Operador', 'Realiza funciones de administración del sistema', '0000-00-00 00:00:00.000000', '0000-00-00 00:00:00.000000'),
(3, 'Propietario', 'Administra su perfil junto con sus inmuebles y contratos', '0000-00-00 00:00:00.000000', '0000-00-00 00:00:00.000000');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `userroles`
--

CREATE TABLE `userroles` (
  `Id` int(11) NOT NULL,
  `UserId` int(11) NOT NULL,
  `RoleId` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `userroles`
--

INSERT INTO `userroles` (`Id`, `UserId`, `RoleId`) VALUES
(1, 1, 1),
(2, 2, 3),
(3, 3, 1),
(4, 3, 2),
(5, 3, 3),
(6, 4, 3),
(7, 5, 3),
(8, 6, 3),
(9, 7, 3);

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `users`
--

CREATE TABLE `users` (
  `Id` int(11) NOT NULL,
  `Name` varchar(100) NOT NULL,
  `Email` varchar(100) NOT NULL,
  `Password` varchar(300) NOT NULL,
  `IsActive` tinyint(1) NOT NULL,
  `UpdatedAt` datetime(6) NOT NULL,
  `CreatedAt` datetime(6) NOT NULL,
  `LastName` varchar(100) NOT NULL DEFAULT '',
  `ProfilePicRoute` longtext DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `users`
--

INSERT INTO `users` (`Id`, `Name`, `Email`, `Password`, `IsActive`, `UpdatedAt`, `CreatedAt`, `LastName`, `ProfilePicRoute`) VALUES
(1, 'Martin', 'rastrillamartin@gmail.com', '$2a$11$OHxmmBghTh9NA4r/BRi2w.3iKBXGSvTp2mvQ.40YL/T4GNe0C/vVK', 1, '2025-10-19 05:02:16.818338', '2025-10-19 05:02:16.818338', 'Rastrilla', NULL),
(2, 'Pablo', 'pp@gmail.com', '$2a$11$.BORycE08KVEgyTmCwXDquQqIJ2nsWQUIHRjciLW8Ma2K9B7lx/3G', 1, '2025-11-02 14:31:25.563237', '2025-10-19 05:04:00.202369', 'Propietario', '/uploads/profiles/0eb5b6d3f81443c5.png'),
(3, 'Admin', 'admin@admin.com', '$2a$11$p4XMfRFU04A7s3xbYar9qekWDMGo9t9GlPSlMjtblenH4qpfnO.wm', 1, '2025-10-20 22:15:20.172883', '2025-10-20 21:28:28.614021', 'Admin', 'https://ui-avatars.com/api/?name=Admin+Admin'),
(4, 'Menguita', 'menguirastrilla@gmail.com', '$2a$11$oHFBiaV3imfB05ISqEwyt.neivjgg73idr.fpSy8Nb/A2hlfYpu72', 1, '2025-10-30 23:48:07.001803', '2025-10-28 22:12:12.409292', 'Rastrilla', '/uploads/profiles/ea8a212bafba465a.jpg'),
(5, 'Martin', 'rastrillamartin2@gmail.com', '$2a$11$0CpkDoYClQq7hFVkGcKX3.SBG1/wxSgrX/DNb9LtbHmIF0RGs32eW', 1, '2025-11-02 20:19:10.997792', '2025-11-02 20:19:10.997793', 'Rastrilla', 'https://ui-avatars.com/api/?name=Martin+Rastrilla&background=8E02AA&color=fff&size=256'),
(6, 'algo', 'qlgo@a.co', '$2a$11$raVCl.EpI/a4CNZbAxlo/.AYrBjTgxCPuexFhMY2S7orQrbrMXTYu', 1, '2025-11-02 20:26:10.611094', '2025-11-02 20:26:10.611094', 'algo', 'https://ui-avatars.com/api/?name=algo+algo&background=8E02AA&color=fff&size=256'),
(7, 'Prueba Otro', 'prueba@gmail.com', '$2a$11$BNTxcbE58Mee6m6LdNVkQuUjDE8m51fvyFoEDGusi0huEaz9X85li', 1, '2025-11-02 20:29:07.270963', '2025-11-02 20:28:30.499905', 'Prueba', '/uploads/profiles/487a69b752c74ae1.jpg');

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `__efmigrationshistory`
--

CREATE TABLE `__efmigrationshistory` (
  `MigrationId` varchar(150) NOT NULL,
  `ProductVersion` varchar(32) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Volcado de datos para la tabla `__efmigrationshistory`
--

INSERT INTO `__efmigrationshistory` (`MigrationId`, `ProductVersion`) VALUES
('20251017032301_InitialCreate', '9.0.10'),
('20251019043022_RefactorUserPropietarioModel', '9.0.10'),
('20251019045652_FixPropietarioIdOnUser', '9.0.10'),
('20251020113752_Archivos', '9.0.10');

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `archivoinmuebles`
--
ALTER TABLE `archivoinmuebles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_ArchivoInmuebles_ArchivoId` (`ArchivoId`),
  ADD KEY `IX_ArchivoInmuebles_InmuebleId` (`InmuebleId`);

--
-- Indices de la tabla `archivos`
--
ALTER TABLE `archivos`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `contratoinquilinos`
--
ALTER TABLE `contratoinquilinos`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_ContratoInquilinos_ContratoId_InquilinoId` (`ContratoId`,`InquilinoId`),
  ADD KEY `IX_ContratoInquilinos_InquilinoId` (`InquilinoId`);

--
-- Indices de la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Contratos_InmuebleId` (`InmuebleId`),
  ADD KEY `IX_Contratos_InquilinoId` (`InquilinoId`),
  ADD KEY `IX_Contratos_PropietarioId` (`PropietarioId`);

--
-- Indices de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Inmuebles_PropietarioId` (`PropietarioId`);

--
-- Indices de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_Inquilinos_DocumentNumber` (`DocumentNumber`);

--
-- Indices de la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD PRIMARY KEY (`Id`),
  ADD KEY `IX_Pagos_ContratoId` (`ContratoId`),
  ADD KEY `IX_Pagos_InquilinoId` (`InquilinoId`);

--
-- Indices de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_Propietarios_DocumentNumber` (`DocumentNumber`),
  ADD KEY `IX_Propietarios_UserId` (`UserId`);

--
-- Indices de la tabla `roles`
--
ALTER TABLE `roles`
  ADD PRIMARY KEY (`Id`);

--
-- Indices de la tabla `userroles`
--
ALTER TABLE `userroles`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_UserRoles_UserId_RoleId` (`UserId`,`RoleId`),
  ADD KEY `IX_UserRoles_RoleId` (`RoleId`);

--
-- Indices de la tabla `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`Id`),
  ADD UNIQUE KEY `IX_Users_Email` (`Email`);

--
-- Indices de la tabla `__efmigrationshistory`
--
ALTER TABLE `__efmigrationshistory`
  ADD PRIMARY KEY (`MigrationId`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `archivoinmuebles`
--
ALTER TABLE `archivoinmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `archivos`
--
ALTER TABLE `archivos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;

--
-- AUTO_INCREMENT de la tabla `contratoinquilinos`
--
ALTER TABLE `contratoinquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `contratos`
--
ALTER TABLE `contratos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=21;

--
-- AUTO_INCREMENT de la tabla `inquilinos`
--
ALTER TABLE `inquilinos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=3;

--
-- AUTO_INCREMENT de la tabla `pagos`
--
ALTER TABLE `pagos`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=2;

--
-- AUTO_INCREMENT de la tabla `propietarios`
--
ALTER TABLE `propietarios`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=7;

--
-- AUTO_INCREMENT de la tabla `roles`
--
ALTER TABLE `roles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=4;

--
-- AUTO_INCREMENT de la tabla `userroles`
--
ALTER TABLE `userroles`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=10;

--
-- AUTO_INCREMENT de la tabla `users`
--
ALTER TABLE `users`
  MODIFY `Id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `archivoinmuebles`
--
ALTER TABLE `archivoinmuebles`
  ADD CONSTRAINT `FK_ArchivoInmuebles_Archivos_ArchivoId` FOREIGN KEY (`ArchivoId`) REFERENCES `archivos` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_ArchivoInmuebles_Inmuebles_InmuebleId` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `contratoinquilinos`
--
ALTER TABLE `contratoinquilinos`
  ADD CONSTRAINT `FK_ContratoInquilinos_Contratos_ContratoId` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_ContratoInquilinos_Inquilinos_InquilinoId` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `contratos`
--
ALTER TABLE `contratos`
  ADD CONSTRAINT `FK_Contratos_Inmuebles_InmuebleId` FOREIGN KEY (`InmuebleId`) REFERENCES `inmuebles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Contratos_Inquilinos_InquilinoId` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`Id`),
  ADD CONSTRAINT `FK_Contratos_Propietarios_PropietarioId` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `inmuebles`
--
ALTER TABLE `inmuebles`
  ADD CONSTRAINT `FK_Inmuebles_Propietarios_PropietarioId` FOREIGN KEY (`PropietarioId`) REFERENCES `propietarios` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `pagos`
--
ALTER TABLE `pagos`
  ADD CONSTRAINT `FK_Pagos_Contratos_ContratoId` FOREIGN KEY (`ContratoId`) REFERENCES `contratos` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_Pagos_Inquilinos_InquilinoId` FOREIGN KEY (`InquilinoId`) REFERENCES `inquilinos` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `propietarios`
--
ALTER TABLE `propietarios`
  ADD CONSTRAINT `FK_Propietarios_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;

--
-- Filtros para la tabla `userroles`
--
ALTER TABLE `userroles`
  ADD CONSTRAINT `FK_UserRoles_Roles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `roles` (`Id`) ON DELETE CASCADE,
  ADD CONSTRAINT `FK_UserRoles_Users_UserId` FOREIGN KEY (`UserId`) REFERENCES `users` (`Id`) ON DELETE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
