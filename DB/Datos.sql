-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Servidor: localhost:3306
-- Tiempo de generación: 29-07-2025 a las 19:58:51
-- Versión del servidor: 10.6.20-MariaDB-cll-lve-log
-- Versión de PHP: 8.1.33

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de datos: `clini234_cerene`
--

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exp_adjuntos_evaluacion`
--

CREATE TABLE `exp_adjuntos_evaluacion` (
  `id_adjunto` int(11) NOT NULL,
  `id_evaluacion` int(11) NOT NULL,
  `tipo_archivo` varchar(20) DEFAULT NULL,
  `nombre_archivo` varchar(255) DEFAULT NULL,
  `ruta_archivo` varchar(500) DEFAULT NULL,
  `fecha_subida` datetime DEFAULT current_timestamp()
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exp_areas_evaluacion`
--

CREATE TABLE `exp_areas_evaluacion` (
  `id_area` int(11) NOT NULL,
  `nombre_area` varchar(100) NOT NULL,
  `descripcion` text DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exp_evaluaciones`
--

CREATE TABLE `exp_evaluaciones` (
  `id_evaluacion` int(11) NOT NULL,
  `id_nino` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `id_area` int(11) NOT NULL,
  `fecha` datetime DEFAULT current_timestamp(),
  `observaciones` text DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exp_opciones_pregunta`
--

CREATE TABLE `exp_opciones_pregunta` (
  `id_opcion` int(11) NOT NULL,
  `id_pregunta` int(11) NOT NULL,
  `texto_opcion` varchar(255) NOT NULL,
  `es_correcta` tinyint(1) DEFAULT 0
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exp_preguntas_evaluacion`
--

CREATE TABLE `exp_preguntas_evaluacion` (
  `id_pregunta` int(11) NOT NULL,
  `id_area` int(11) NOT NULL,
  `texto_pregunta` text NOT NULL,
  `tipo_respuesta` varchar(50) DEFAULT 'texto',
  `es_multiple` tinyint(1) DEFAULT 0
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exp_progreso_general`
--

CREATE TABLE `exp_progreso_general` (
  `id_progreso` int(11) NOT NULL,
  `id_nino` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `lenguaje` tinyint(4) DEFAULT NULL,
  `motricidad` tinyint(4) DEFAULT NULL,
  `atencion` tinyint(4) DEFAULT NULL,
  `memoria` tinyint(4) DEFAULT NULL,
  `social` tinyint(4) DEFAULT NULL,
  `observaciones` text DEFAULT NULL,
  `fecha_registro` datetime DEFAULT current_timestamp()
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exp_respuestas_evaluacion`
--

CREATE TABLE `exp_respuestas_evaluacion` (
  `id_respuesta` int(11) NOT NULL,
  `id_evaluacion` int(11) NOT NULL,
  `id_pregunta` int(11) NOT NULL,
  `id_opcion` int(11) DEFAULT NULL,
  `respuesta` text DEFAULT NULL
) ENGINE=MyISAM DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_general_ci;

-- --------------------------------------------------------

--
-- Estructura de tabla para la tabla `exp_valoraciones_sesion`
--

CREATE TABLE `exp_valoraciones_sesion` (
  `id_valoracion` int(11) NOT NULL,
  `id_nino` int(11) NOT NULL,
  `id_usuario` int(11) NOT NULL,
  `participacion` tinyint(4) DEFAULT NULL,
  `atencion` tinyint(4) DEFAULT NULL,
  `tarea_casa` tinyint(4) DEFAULT NULL,
  `observaciones` text DEFAULT NULL,
  `fecha_valoracion` datetime DEFAULT current_timestamp()
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Índices para tablas volcadas
--

--
-- Indices de la tabla `exp_adjuntos_evaluacion`
--
ALTER TABLE `exp_adjuntos_evaluacion`
  ADD PRIMARY KEY (`id_adjunto`),
  ADD KEY `id_evaluacion` (`id_evaluacion`);

--
-- Indices de la tabla `exp_areas_evaluacion`
--
ALTER TABLE `exp_areas_evaluacion`
  ADD PRIMARY KEY (`id_area`);

--
-- Indices de la tabla `exp_evaluaciones`
--
ALTER TABLE `exp_evaluaciones`
  ADD PRIMARY KEY (`id_evaluacion`),
  ADD KEY `id_nino` (`id_nino`),
  ADD KEY `id_usuario` (`id_usuario`),
  ADD KEY `id_area` (`id_area`);

--
-- Indices de la tabla `exp_opciones_pregunta`
--
ALTER TABLE `exp_opciones_pregunta`
  ADD PRIMARY KEY (`id_opcion`),
  ADD KEY `id_pregunta` (`id_pregunta`);

--
-- Indices de la tabla `exp_preguntas_evaluacion`
--
ALTER TABLE `exp_preguntas_evaluacion`
  ADD PRIMARY KEY (`id_pregunta`),
  ADD KEY `id_area` (`id_area`);

--
-- Indices de la tabla `exp_progreso_general`
--
ALTER TABLE `exp_progreso_general`
  ADD PRIMARY KEY (`id_progreso`),
  ADD KEY `id_nino` (`id_nino`),
  ADD KEY `id_usuario` (`id_usuario`);

--
-- Indices de la tabla `exp_respuestas_evaluacion`
--
ALTER TABLE `exp_respuestas_evaluacion`
  ADD PRIMARY KEY (`id_respuesta`),
  ADD KEY `id_evaluacion` (`id_evaluacion`),
  ADD KEY `id_pregunta` (`id_pregunta`),
  ADD KEY `id_opcion` (`id_opcion`);

--
-- Indices de la tabla `exp_valoraciones_sesion`
--
ALTER TABLE `exp_valoraciones_sesion`
  ADD PRIMARY KEY (`id_valoracion`),
  ADD KEY `id_nino` (`id_nino`),
  ADD KEY `id_usuario` (`id_usuario`);

--
-- AUTO_INCREMENT de las tablas volcadas
--

--
-- AUTO_INCREMENT de la tabla `exp_adjuntos_evaluacion`
--
ALTER TABLE `exp_adjuntos_evaluacion`
  MODIFY `id_adjunto` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `exp_areas_evaluacion`
--
ALTER TABLE `exp_areas_evaluacion`
  MODIFY `id_area` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `exp_evaluaciones`
--
ALTER TABLE `exp_evaluaciones`
  MODIFY `id_evaluacion` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `exp_opciones_pregunta`
--
ALTER TABLE `exp_opciones_pregunta`
  MODIFY `id_opcion` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `exp_preguntas_evaluacion`
--
ALTER TABLE `exp_preguntas_evaluacion`
  MODIFY `id_pregunta` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `exp_progreso_general`
--
ALTER TABLE `exp_progreso_general`
  MODIFY `id_progreso` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `exp_respuestas_evaluacion`
--
ALTER TABLE `exp_respuestas_evaluacion`
  MODIFY `id_respuesta` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT de la tabla `exp_valoraciones_sesion`
--
ALTER TABLE `exp_valoraciones_sesion`
  MODIFY `id_valoracion` int(11) NOT NULL AUTO_INCREMENT;

--
-- Restricciones para tablas volcadas
--

--
-- Filtros para la tabla `exp_valoraciones_sesion`
--
ALTER TABLE `exp_valoraciones_sesion`
  ADD CONSTRAINT `exp_valoraciones_sesion_ibfk_1` FOREIGN KEY (`id_nino`) REFERENCES `nino` (`id`),
  ADD CONSTRAINT `exp_valoraciones_sesion_ibfk_2` FOREIGN KEY (`id_usuario`) REFERENCES `Usuarios` (`id`);
COMMIT;
