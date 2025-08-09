CREATE TABLE `exp_examenes` (
    `id_examen` INT AUTO_INCREMENT PRIMARY KEY,
    `id_area` INT NOT NULL,
    `id_usuario` INT NOT NULL,
    `nombre_examen` VARCHAR(255) NOT NULL,
    `fecha_creacion` DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (`id_area`) REFERENCES `exp_areas_evaluacion`(`id_area`),
    FOREIGN KEY (`id_usuario`) REFERENCES `Usuarios`(`id_usuario`)
);

CREATE TABLE `exp_secciones_examen` (
    `id_seccion` INT AUTO_INCREMENT PRIMARY KEY,
    `id_examen` INT NOT NULL,
    `nombre_seccion` VARCHAR(255) NOT NULL,
    FOREIGN KEY (`id_examen`) REFERENCES `exp_examenes`(`id_examen`)
);

CREATE TABLE `exp_preguntas_evaluacion` (
    `id_pregunta` INT AUTO_INCREMENT PRIMARY KEY,
    `id_seccion` INT NOT NULL,
    `pregunta` TEXT NOT NULL,
    FOREIGN KEY (`id_seccion`) REFERENCES `exp_secciones_examen`(`id_seccion`)
);

CREATE TABLE `exp_opciones_pregunta` (
    `id_opcion` INT AUTO_INCREMENT PRIMARY KEY,
    `texto` VARCHAR(255) NOT NULL
);

CREATE TABLE `exp_pregunta_opcion` (
    `id_pregunta` INT NOT NULL,
    `id_opcion` INT NOT NULL,
    PRIMARY KEY (`id_pregunta`, `id_opcion`),
    FOREIGN KEY (`id_pregunta`) REFERENCES `exp_preguntas_evaluacion`(`id_pregunta`),
    FOREIGN KEY (`id_opcion`) REFERENCES `exp_opciones_pregunta`(`id_opcion`)
);
