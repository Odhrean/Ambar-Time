SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID (N'zeiterfassung.prc_Start') IS NOT NULL
    DROP PROCEDURE zeiterfassung.prc_Start
GO
-- =============================================
-- Author:        $Author: dirk.lietz $
-- Date:          $Date: 2015-04-14 13:49:03 +0200 (Di, 14. Apr 2015) $
-- Revision:      $Revision: 378 $
-- SVN-URL:       $HeadURL: http://hb-hq-svn-001.stute.loc/koe-dlz-001/Programmierung/VisualStudio/Projects/Zeiterfassung/SQLServer/prc_Start.sql $
-- Description:   Starte Tätigkeit
-- =============================================

CREATE PROCEDURE zeiterfassung.prc_Start 
	@p_Anwender nvarchar(100), 
	@p_Taetigkeit nvarchar(2000)=NULL,			-- Entweder Name der Tätigkeit
	@p_ID_Taetigkeit int = NULL,				-- oder ID einer bereits zuvor erfassten Tätigkeit
	@p_Kategorie nvarchar(50) = NULL,
	@p_LINK nvarchar(500) = NULL,				-- Optionaler Link zur Tätigkeit (Web oder Folder)
	@p_ID int OUTPUT,							-- ID der gestarteten Tätigkeit
	@p_DEBUG int = 0	-- 0: Kein Debug
						-- 1: Ausgabe in Konsole
AS
BEGIN
	SET NOCOUNT ON;
	-- Unique ID for Procedure-Call
	DECLARE @mark CHAR(32) = replace(newid(), '-', '');

    DECLARE @TranCounter INT;
    SET @TranCounter = @@TRANCOUNT;


	DECLARE @procedure nvarchar(50)
	SET @procedure = 'prc_Start'

	IF @p_DEBUG = 1			
		print msdb.dbo.debugOut('Start Ausfuehrung Procedure '+@procedure+' AS USER_NAME:'+USER_NAME()+' SUSER_SNAME:'+SUSER_SNAME()+' ORIGINAL_LOGIN:'+ORIGINAL_LOGIN()+' (ID: '+@mark+')',@procedure)


	DECLARE @id_taetigkeit int,
			@id_Kategorie int,
			@id_Anwender int;

	BEGIN TRY
			IF @TranCounter > 0
			BEGIN
				SAVE TRANSACTION @mark
				IF @p_DEBUG = 1			
					print msdb.dbo.debugOut('SAVE TRANSACTION '+@mark , @procedure)
				
			END
			ELSE
			BEGIN
				BEGIN TRANSACTION;
				IF @p_DEBUG = 1			
					print msdb.dbo.debugOut('BEGIN TRANSACTION', @procedure)
			END
		-- Begin Code

		-- Anwender ID ermitteln oder neu anlegen
		SET @id_Anwender = NULL;
		select @id_Anwender = ID from [zeiterfassung].[Anwender] where [Anwender]=@p_Anwender;

		IF @id_Anwender is NULL
		BEGIN
			INSERT INTO [zeiterfassung].[Anwender]([Anwender])
			VALUES(@p_Anwender);

			SET @id_Anwender = SCOPE_IDENTITY();

		END
		
		-- Wenn eine Erfassung für den Anwender läuft diese vorher stoppen

		-- 0: Erfassung läuft
		-- 1: Erfassung ist gestoppt
		-- 2: Taetigkeit ist beendet
		SET @id_taetigkeit = NULL;
		select @id_taetigkeit=ID from [zeiterfassung].[Taetigkeit] where ID_Anwender=@id_Anwender and [Status]=0;

		-- Wenn die zu startende Taetigkeit bereits läuft nix machen und raus hier
		IF @p_ID_Taetigkeit = @id_taetigkeit
		BEGIN
			IF @TranCounter = 0
			BEGIN
				IF @p_DEBUG = 1			
					print msdb.dbo.debugOut('Läuft bereits.... COMMIT TRANSACTION', @procedure)

				COMMIT TRANSACTION;
			END
			SET @p_ID = @id_taetigkeit;
			RETURN 0;
		END

		IF @id_taetigkeit is not NULL
		BEGIN
			UPDATE [zeiterfassung].[Taetigkeit]
				SET [Status] = 1,		-- Taetigkeit stoppen
					TIME_AEN =getdate()
				WHERE ID = @id_taetigkeit;

			UPDATE [zeiterfassung].[zeitmessung]
				SET Ende=getdate()
			WHERE [ID_Taetigkeit]= @id_taetigkeit
			and Ende is NULL;
		END

		-- War diese Taetigkeit bereits im Gang dann diese wieder starten
		IF @p_ID_Taetigkeit is not NULL
		BEGIN
			UPDATE [zeiterfassung].[Taetigkeit]
				SET [Status] = 0,
					TIME_AEN =getdate()
			WHERE [ID]= @p_ID_Taetigkeit;
			
			INSERT INTO [zeiterfassung].[zeitmessung]
					   ([ID_Taetigkeit] ,[Start])
				 VALUES (@p_ID_Taetigkeit ,getdate());
			
			SET @p_ID = @p_ID_Taetigkeit;
		END
		ELSE IF @p_Taetigkeit is not NULL
		BEGIN
			SET @id_Kategorie = NULL;

			SELECT @id_Kategorie = ID 
			FROM [zeiterfassung].[Kategorie]
			WHERE ID_Anwender=@id_Anwender AND [Kategorie]=@p_Kategorie;

			IF @id_Kategorie is NULL and @p_Kategorie is not NULL
			BEGIN
				INSERT INTO [zeiterfassung].[Kategorie] ([Kategorie],[ID_Anwender])
				VALUES(@p_Kategorie,@id_Anwender);

				SET @id_Kategorie = SCOPE_IDENTITY();
			END

			INSERT INTO [zeiterfassung].[Taetigkeit] ([ID_Anwender],[Taetigkeit],[Status],[TIME_AEN],ID_Kategorie,Link)
			VALUES(@id_Anwender,@p_Taetigkeit,0,getdate(),@id_Kategorie,@p_LINK);

			SELECT @id_taetigkeit = SCOPE_IDENTITY();

			INSERT INTO [zeiterfassung].[zeitmessung]
					   ([ID_Taetigkeit] ,[Start])
				 VALUES (@id_taetigkeit ,getdate());

			SET @p_ID = @id_taetigkeit;
		END

		-- End Code
			IF @TranCounter = 0
			BEGIN
				IF @p_DEBUG = 1			
					print msdb.dbo.debugOut('COMMIT TRANSACTION', @procedure)

				COMMIT TRANSACTION;
			END
	END TRY
	BEGIN CATCH
		declare @error int, @message varchar(4000), @line int,@state int,@severity int, @xstate int;
		select @error = ERROR_NUMBER(), @message = ERROR_MESSAGE(),@line=ERROR_LINE(),@state= ERROR_STATE(),@severity=ERROR_SEVERITY(),@xstate = XACT_STATE();	
		
		IF @p_DEBUG = 1			
				print msdb.dbo.debugOut('ERROR: '+CAST(@error as varchar)+': '+@message+' (Line: '+CAST(@line as varchar)+')',@procedure)
		
		IF @xstate = -1
		BEGIN
			IF @p_DEBUG = 1			
				print msdb.dbo.debugOut('ROLLBACK TRANSACTION -> XACT_STATE = -1',@procedure)
			ROLLBACK TRANSACTION;
		END
		IF @xstate = 1 and @TranCounter = 0
		BEGIN
			IF @p_DEBUG = 1			
				print msdb.dbo.debugOut('ROLLBACK TRANSACTION',@procedure)
			ROLLBACK TRANSACTION;
		END
		IF @xstate = 1 and @TranCounter > 0
		BEGIN
			IF @p_DEBUG = 1			
				print msdb.dbo.debugOut('ROLLBACK TRANSACTION '+@mark+' (SAFEPOINT)',@procedure)
			ROLLBACK TRANSACTION @mark;
		END
						
		RAISERROR ('%s: %d: %s (Line: %d)', @severity, @state, @procedure,@error, @message,@line) ;
	
	END CATCH
END
GO
