/* 
	Archivo para ingresar las categorías del docente en la base de datos.
	Tipos de categorías 
	1 - usuario administrador
	2 - usuario revisor
	3 - usuario docente
*/
USE [Atestados]
GO

INSERT INTO [dbo].[TipoCategoria]
           ([Nombre])
     VALUES
           ('Primera'),
		   ('Segunda'),
		   ('Tercera'),
		   ('Cuarta')
GO