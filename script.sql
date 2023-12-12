USE MASTER
GO

CREATE DATABASE BDMatricula
GO

USE BDMatricula
GO

CREATE TABLE Carrera(
	cod_carrera INT IDENTITY PRIMARY KEY,
	nombre_carrera VARCHAR(100)
)
GO

CREATE TABLE Curso(
	cod_curso INT IDENTITY PRIMARY KEY,
	nombre_curso VARCHAR(100),
	creditos int,
	horas int
)
GO



CREATE TABLE Rol(
	cod_rol INT IDENTITY PRIMARY KEY,
	nombre_rol VARCHAR(50)
)
GO



CREATE TABLE Usuario(
	cod_usuario INT IDENTITY PRIMARY KEY,
	nombres VARCHAR(100),
	apellidos VARCHAR(100),
	correo  VARCHAR(100) UNIQUE,
	contrasennia VARCHAR(200),
	fecha_registro DATETIME,
	cod_rol INT,
	FOREIGN KEY(cod_rol) REFERENCES Rol(cod_rol)
)
GO

CREATE TABLE Estudiante(
	cod_estudiante INT IDENTITY PRIMARY KEY,
	cod_usuario INT,
	cod_carrera INT,
	apoderado VARCHAR(100),
	foreign key(cod_carrera) references Carrera(cod_carrera),
	foreign key(cod_usuario) references Usuario(cod_usuario)
)
GO

CREATE TABLE Profesor(
	cod_profesor INT IDENTITY PRIMARY KEY,
	cod_usuario INT,
	direccion VARCHAR(400),
	telefono CHAR(15),
	foreign key(cod_usuario) references Usuario(cod_usuario)
)
GO

CREATE TABLE Seccion(
	num_seccion INT IDENTITY(1000,1) PRIMARY KEY,
	cod_profesor INT,
	cod_curso INT,
	dia_semana varchar(40),
	hora_inicio time,
	hora_fin time,
	capacidad int,
	vacantes int,
	aula varchar(10),
	foreign key(cod_profesor) references Profesor(cod_profesor),
	foreign key(cod_curso) references Curso(cod_curso)
)
GO

CREATE TABLE Matricula(
	num_matricula INT IDENTITY(1000,1) PRIMARY KEY,
	cod_estudiante INT,
	fecha datetime,
	total_horas int,
	costo decimal(8,2),
	estado varchar(100),
	foreign key(cod_estudiante) references Estudiante(cod_estudiante)
)
GO

CREATE TABLE DetalleMatricula(
	cod_detMatricula INT IDENTITY PRIMARY KEY,
	num_matricula INT,
	num_seccion INT,
	foreign key(num_matricula) references Matricula(num_matricula),
	foreign key(num_seccion) references Seccion(num_seccion)
)
GO

CREATE PROCEDURE SP_MANT_Profesor
(
@cod_profesor INT = 0,
@nombres VARCHAR(100) = '',
@apellidos VARCHAR(100) = '',
@correo  VARCHAR(100) = '',
@contrasennia VARCHAR(200) = '',
@direccion VARCHAR(100) = '',
@telefono VARCHAR(100) = '',
@accion INT = 0
)
AS
BEGIN
	DECLARE @msg VARCHAR(MAX) = NULL
	DECLARE @cod_usuario INT = 0
	DECLARE @cod_rol INT = 1

	BEGIN TRY
		BEGIN TRANSACTION

		-- ACCION GUARDAR UN REGISTRO
		IF @accion = 1 
			BEGIN
				IF EXISTS (SELECT * FROM Usuario WHERE correo = @correo)
				    SET @msg = 'El correo ' +  @correo + ' ya se encuentra registrado!'
				ELSE  
				    BEGIN
						INSERT INTO Usuario(nombres,apellidos,correo,contrasennia,fecha_registro,cod_rol) VALUES(@nombres,@apellidos,@correo,@contrasennia, GETDATE(),@cod_rol)
						SET @cod_usuario = @@IDENTITY
						INSERT INTO Profesor(cod_usuario,direccion,telefono) VALUES(@cod_usuario,@direccion,@telefono)

						SET @msg = 'OK'
					END
			END

		 -- ACCION EDITAR UN REGISTRO
		IF @accion = 2 
			BEGIN
				SET @cod_usuario = (SELECT cod_usuario FROM Profesor WHERE cod_profesor = @cod_profesor)

				IF @cod_usuario IS NOT NULL
				   BEGIN
					   IF EXISTS(SELECT * FROM Usuario WHERE correo = @correo AND cod_usuario != @cod_usuario)
						SET @msg = 'El correo ' +  @correo + ' ya se encuentra registrado!'
						ELSE  
						  BEGIN
							UPDATE Usuario SET nombres = @nombres , apellidos =  @apellidos ,correo =  @correo,
							contrasennia = @contrasennia
							WHERE cod_usuario = @cod_usuario
								
							UPDATE Profesor SET direccion = @direccion , telefono = @telefono
							WHERE cod_profesor = @cod_profesor
							
							SET @msg = 'OK'
						  END
				   END
				ELSE
				  SET @msg = 'No se encontró Profesor con el codigo ' + CAST(@cod_profesor AS VARCHAR(10))
			END

	   -- ACCION ELIMINAR UN REGISTRO
		IF @accion = 3 
			BEGIN
				SET @cod_usuario = (SELECT cod_usuario FROM Profesor WHERE cod_profesor = @cod_profesor)
				IF @cod_usuario IS NOT NULL
				    BEGIN
						DELETE FROM Profesor WHERE cod_profesor = @cod_profesor
						DELETE FROM Usuario WHERE cod_usuario = @cod_usuario

						SET @msg = 'OK'
					END
					
				ELSE  
					SET @msg = 'No se encontró Profesor con el codigo ' + CAST(@cod_profesor AS VARCHAR(10))

			END

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	    PRINT 'Error Number:'+CAST(ERROR_NUMBER() AS VARCHAR(10))
		PRINT 'Error Message:'+ERROR_MESSAGE()
		PRINT 'Error Severity:'+CAST(ERROR_SEVERITY() AS VARCHAR(10))
		PRINT 'Error State:'+CAST(ERROR_STATE() AS VARCHAR(10))
		PRINT 'Error Line:'+CAST(ERROR_LINE() AS VARCHAR(10))
		PRINT 'Error Proc:'+COALESCE(ERROR_PROCEDURE(), 'Not vithin procedure')

		SET @msg =  ERROR_MESSAGE()

		ROLLBACK TRANSACTION
	END CATCH

	SELECT @msg as 'result'
END
GO


--  '<Tabla><Item><numSeccion>1002</numSeccion></Item><Item><numSeccion>1004</numSeccion></Item></Tabla>'
CREATE PROCEDURE SP_GUARDAR_MATRICULA
(
@cod_estudiante INT = 0,
@strSecciones XML = ''
)
AS
BEGIN
	DECLARE @numMatricula INT
	DECLARE @numSeccion INT
	DECLARE @horas INT
	DECLARE @totalHoras INT
	DECLARE @costo DECIMAL(8,2)
	DECLARE @msg VARCHAR(MAX) = NULL
	DECLARE @hDoc INT
	DECLARE @nCont INT
	DECLARE @pagoHora DECIMAL(8,2) = 40
	
	BEGIN TRY
		declare @tabla as table(  
			Row int,  
			numSeccion int
		);  

		SET @totalHoras = 0

		BEGIN TRANSACTION

		INSERT INTO @tabla(Row,numSeccion)
	    SELECT ROW_NUMBER() OVER(ORDER BY nodo.item.value('numSeccion[1]','int') ) AS Row, 
		nodo.item.value('numSeccion[1]','int') 
	    FROM @strSecciones.nodes('Tabla/Item') nodo(item)
	   
		IF (SELECT COUNT(1) FROM @tabla) > 0
		   BEGIN
		    
		    INSERT INTO Matricula(cod_estudiante,fecha,estado) VALUES(@cod_estudiante,GETDATE(),'Pendiente')

			SET @numMatricula = @@IDENTITY
			
			SET @nCont = 1

			WHILE (@nCont <=(SELECT COUNT(1) FROM @tabla))
			   BEGIN
				  SET @numSeccion = (SELECT numSeccion FROM @tabla WHERE Row = @nCont)
				  SET @horas = (SELECT horas FROM Seccion s INNER JOIN Curso c ON c.cod_curso = s.cod_curso WHERE num_seccion = @numSeccion)
				  
				  INSERT INTO DetalleMatricula(num_matricula,num_seccion) VALUES(@numMatricula,@numSeccion)

				  UPDATE Seccion SET vacantes = vacantes - 1
				  WHERE num_seccion = @numSeccion

				  SET @totalHoras = @totalHoras + @horas
				  SET @nCont = @nCont + 1
			   END
		   END
		ELSE
		  BEGIN
			 SET @msg = 'No se encontraron seccion seleccionadas!'
		  END
	
		IF @totalHoras > 0 AND @msg IS NULL
			BEGIN
				UPDATE Matricula SET total_horas = @totalHoras , costo = (@pagoHora * @totalHoras)
				WHERE num_matricula = @numMatricula
				SET @msg = 'OK'
				COMMIT TRANSACTION
			END
		ELSE
			BEGIN
				SET @msg = 'No se encontraron secciones! La matricula no se pudo procesar!'
				ROLLBACK TRANSACTION
			END

	END TRY
	BEGIN CATCH
	    PRINT 'Error Number:'+CAST(ERROR_NUMBER() AS VARCHAR(10))
		PRINT 'Error Message:'+ERROR_MESSAGE()
		PRINT 'Error Severity:'+CAST(ERROR_SEVERITY() AS VARCHAR(10))
		PRINT 'Error State:'+CAST(ERROR_STATE() AS VARCHAR(10))
		PRINT 'Error Line:'+CAST(ERROR_LINE() AS VARCHAR(10))
		PRINT 'Error Proc:'+COALESCE(ERROR_PROCEDURE(), 'Not vithin procedure')

		SET @msg =  ERROR_MESSAGE()

		ROLLBACK TRANSACTION
	END CATCH

	SELECT @msg as 'result'
END
GO

CREATE PROCEDURE SP_MANT_Estudiante
(
@cod_estudiante INT = 0,
@nombres VARCHAR(100) = '',
@apellidos VARCHAR(100) = '',
@correo  VARCHAR(100) = '',
@contrasennia VARCHAR(200) = '',
@cod_carrera INT = 0,
@apoderado VARCHAR(100) = '',
@accion INT = 0
)
AS
BEGIN
	DECLARE @msg VARCHAR(MAX) = NULL
	DECLARE @cod_usuario INT = 0
	DECLARE @cod_rol INT = 2

	BEGIN TRY
		BEGIN TRANSACTION

		-- ACCION GUARDAR UN REGISTRO
		IF @accion = 1 
			BEGIN
				IF EXISTS (SELECT * FROM Usuario WHERE correo = @correo)
				    SET @msg = 'El correo ' +  @correo + ' ya se encuentra registrado!'
				ELSE  
				   IF NOT EXISTS (SELECT * FROM Carrera WHERE cod_carrera = @cod_carrera)
				     SET @msg = 'No se encontro carrera con el codigo '+ CAST(@cod_carrera AS VARCHAR(10))
				   ELSE
					  BEGIN
						INSERT INTO Usuario(nombres,apellidos,correo,contrasennia,fecha_registro,cod_rol) VALUES(@nombres,@apellidos,@correo,@contrasennia, GETDATE(),@cod_rol)
						SET @cod_usuario = @@IDENTITY
						INSERT INTO Estudiante(cod_usuario,cod_carrera,apoderado) VALUES(@cod_usuario,@cod_carrera,@apoderado)

						SET @msg = 'OK'
					  END
			END

		 -- ACCION EDITAR UN REGISTRO
		IF @accion = 2 
			BEGIN
				SET @cod_usuario = (SELECT cod_usuario FROM Estudiante WHERE cod_estudiante = @cod_estudiante)

				IF @cod_usuario IS NOT NULL
				   BEGIN
					   IF EXISTS(SELECT * FROM Usuario WHERE correo = @correo AND cod_usuario != @cod_usuario)
						SET @msg = 'El correo ' +  @correo + ' ya se encuentra registrado!'
						ELSE  
						   IF NOT EXISTS (SELECT * FROM Carrera WHERE cod_carrera = @cod_carrera)
							 SET @msg = 'No se encontro carrera con el codigo '+ CAST(@cod_carrera AS VARCHAR(10))
						   ELSE
							  BEGIN
								UPDATE Usuario SET nombres = @nombres , apellidos =  @apellidos ,correo =  @correo,
								contrasennia = @contrasennia
								WHERE cod_usuario = @cod_usuario
								
								UPDATE Estudiante SET cod_carrera = @cod_carrera , apoderado = @apoderado
								WHERE cod_estudiante = @cod_estudiante
							
								SET @msg = 'OK'
							  END
				   END
				ELSE
				  SET @msg = 'No se encontró Estudiante con el codigo ' + CAST(@cod_estudiante AS VARCHAR(10))
			END

	   -- ACCION ELIMINAR UN REGISTRO
		IF @accion = 3 
			BEGIN
				SET @cod_usuario = (SELECT cod_usuario FROM Estudiante WHERE cod_estudiante = @cod_estudiante)
				IF @cod_usuario IS NOT NULL
				    BEGIN
						DELETE FROM Estudiante WHERE cod_estudiante = @cod_estudiante
						DELETE FROM Usuario WHERE cod_usuario = @cod_usuario

						SET @msg = 'OK'
					END
					
				ELSE  
					SET @msg = 'No se encontró Estudiante con el codigo ' + CAST(@cod_estudiante AS VARCHAR(10))

			END

		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	    PRINT 'Error Number:'+CAST(ERROR_NUMBER() AS VARCHAR(10))
		PRINT 'Error Message:'+ERROR_MESSAGE()
		PRINT 'Error Severity:'+CAST(ERROR_SEVERITY() AS VARCHAR(10))
		PRINT 'Error State:'+CAST(ERROR_STATE() AS VARCHAR(10))
		PRINT 'Error Line:'+CAST(ERROR_LINE() AS VARCHAR(10))
		PRINT 'Error Proc:'+COALESCE(ERROR_PROCEDURE(), 'Not vithin procedure')

		SET @msg =  ERROR_MESSAGE()

		ROLLBACK TRANSACTION
	END CATCH

	SELECT @msg as 'result'
END
GO


CREATE PROCEDURE SP_GUARDAR_SECCION
(
@cod_profesor INT = 0,
@cod_curso INT = 0,
@dia  VARCHAR(100) = '',
@hora_inicio  TIME,
@capacidad INT = 0,
@aula VARCHAR(10) = ''
)
AS
BEGIN
	DECLARE @msg VARCHAR(MAX) = NULL
	DECLARE @horas INT
	DECLARE @hora_fin  TIME
	DECLARE @minutosAcademicos INT = 45

	BEGIN TRY
		BEGIN TRANSACTION

		IF NOT EXISTS (SELECT * FROM Profesor WHERE cod_profesor = @cod_profesor)
			SET @msg = 'No se encontró Profesor con el codigo ' + CAST(@cod_profesor AS VARCHAR(10))
		ELSE IF NOT EXISTS (SELECT * FROM Curso WHERE cod_curso = @cod_curso)
			SET @msg = 'No se encontró Curso con el codigo ' + CAST(@cod_curso AS VARCHAR(10))
		ELSE
		   BEGIN
			 SET @horas = (SELECT horas FROM Curso WHERE cod_curso = @cod_curso)
			 SET @hora_fin = (DATEADD(MINUTE, (@horas * @minutosAcademicos),@hora_inicio))

			 INSERT INTO Seccion(cod_profesor,cod_curso,dia_semana,hora_inicio,hora_fin,capacidad,vacantes,aula) 
			 VALUES(@cod_profesor,@cod_curso,@dia,@hora_inicio,@hora_fin, @capacidad,@capacidad,@aula)

			  SET @msg = 'OK'
		   END
		  
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
	    PRINT 'Error Number:'+CAST(ERROR_NUMBER() AS VARCHAR(10))
		PRINT 'Error Message:'+ERROR_MESSAGE()
		PRINT 'Error Severity:'+CAST(ERROR_SEVERITY() AS VARCHAR(10))
		PRINT 'Error State:'+CAST(ERROR_STATE() AS VARCHAR(10))
		PRINT 'Error Line:'+CAST(ERROR_LINE() AS VARCHAR(10))
		PRINT 'Error Proc:'+COALESCE(ERROR_PROCEDURE(), 'Not vithin procedure')

		SET @msg =  ERROR_MESSAGE()

		ROLLBACK TRANSACTION
	END CATCH

	SELECT @msg as 'result'
END
GO


INSERT INTO Carrera VALUES('Ing. de Sistemas')
INSERT INTO Carrera VALUES('Ing. de Redes')
INSERT INTO Carrera VALUES('Ing. Aeronautica')
INSERT INTO Carrera VALUES('Contabilidad')
INSERT INTO Carrera VALUES('Administracción')
INSERT INTO Carrera VALUES('Derecho')

INSERT INTO Curso VALUES('Quimica General' , 4 , 6)
INSERT INTO Curso VALUES('Calculo Aplicada a la Fisica 1' ,5 , 6)
INSERT INTO Curso VALUES('Introducción a las TIC' , 3 , 2)
INSERT INTO Curso VALUES('Ingles I' , 4 , 3)
INSERT INTO Curso VALUES('Base de Datos' , 4 , 4)
INSERT INTO Curso VALUES('Taller de Programación' , 3 , 4)
INSERT INTO Curso VALUES('Estadistica Inferencial' , 3 , 4)
INSERT INTO Curso VALUES('Investigación Academica' , 4 , 6)

INSERT INTO Rol VALUES('Docente')
INSERT INTO Rol VALUES('Estudiante')


INSERT INTO Usuario VALUES('Manuel','Sanchez Carbajal','manuel.sanchez@gmail.com','123456' , GETDATE(),1)
INSERT INTO Usuario VALUES('Ruben Eugenio','La Cruz Vilchez','ruben.cruz@gmail.com','123456' , GETDATE(),1)
INSERT INTO Usuario VALUES('Martha Fabiola','Urviola Laqui','martha.urviola@gmail.com','123456' , GETDATE(),1)
INSERT INTO Usuario VALUES('Mirella Aremi','Arancibia Garcia','mirella.arancibia@gmail.com','123456' , GETDATE(),1)
INSERT INTO Usuario VALUES('Hans Mathyus','Galindo Aroni','hans.galindo@gmail.com','123456' , GETDATE(),1)
INSERT INTO Usuario VALUES('Giovanna','Maro Rodriguez','giovanna.maro@gmail.com','123456' , GETDATE(),1)
INSERT INTO Usuario VALUES('Guadalupe Katherine','Huama Loayza','guadalupe.huama@gmail.com','123456' , GETDATE(),2)
INSERT INTO Usuario VALUES('Yuly Carolina','Moreno Lopez','yuly.moreno@gmail.com','123456' , GETDATE(),2)
INSERT INTO Usuario VALUES('Maria Angelica','Altuna Bracamonte','maria.altuna@gmail.com','123456' , GETDATE(),2)
INSERT INTO Usuario VALUES('Miguel','Torres Cardenas','miguel.torres@gmail.com','123456' , GETDATE(),2)


INSERT INTO Estudiante VALUES(7,1,'Soriano Huama Fernandez')
INSERT INTO Estudiante VALUES(8,1,'Carlos Moreno Lopez')
INSERT INTO Estudiante VALUES(9,2,'Alan Altuna Quispe')
INSERT INTO Estudiante VALUES(10,3,'Sergio Torres Villanueva')

INSERT INTO Profesor VALUES(1 , 'Calle Los Negocios 280 E9 404 Surquillo', '965412365')
INSERT INTO Profesor VALUES(2 , 'Calle El Chalan 136 Urb Monterrico - Alt Cdra 7 Av', '965895652')
INSERT INTO Profesor VALUES(3 , 'Av Haya De La Torre 481 Dpto 201 La Perla', '999656623')
INSERT INTO Profesor VALUES(4 , 'Jr. Honestidad N° 8035 Urb. Pro Los Olivos', '965632366')
INSERT INTO Profesor VALUES(5 , 'Alameda Maria Reiche 329 Dpto 204-san Borja', '965410022')
INSERT INTO Profesor VALUES(6 , 'Aahh. 1 De Abril Mz. F . Lte. 8 - San Juan De Mira', '965626666')

INSERT INTO Seccion VALUES(1,1,'Lunes','08:00:00','13:00:00',30, 29 , 'A201')
INSERT INTO Seccion VALUES(2,1,'Martes','08:00:00','13:00:00',40, 38 , 'A105')
INSERT INTO Seccion VALUES(3,2,'Miercoles','09:45:00','11:15:00',40, 39 , 'C201')
INSERT INTO Seccion VALUES(4,3,'Miercoles','08:00:00','11:15:00',42, 41 , 'B103')
INSERT INTO Seccion VALUES(5,3,'Jueves','09:45:00','13:00:00',20, 19 , 'A512')
INSERT INTO Seccion VALUES(6,2,'Sabado','12:00:00','15:00:00',30, 30 , 'A410')


INSERT INTO Matricula VALUES(1,GETDATE(),18,720,'Pendiente')
INSERT INTO Matricula VALUES(2,GETDATE(),16,640,'Pendiente')

INSERT INTO DetalleMatricula VALUES(1000,1000)
INSERT INTO DetalleMatricula VALUES(1000,1001)
INSERT INTO DetalleMatricula VALUES(1000,1002)

INSERT INTO DetalleMatricula VALUES(1001,1001)
INSERT INTO DetalleMatricula VALUES(1001,1003)
INSERT INTO DetalleMatricula VALUES(1001,1004)
INSERT INTO DetalleMatricula VALUES(1001,1005)


