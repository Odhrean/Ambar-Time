SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID (N'zeiterfassung.prc_saveTaetigkeit') IS NOT NULL
    DROP PROCEDURE zeiterfassung.prc_saveTaetigkeit
GO
-- =============================================
-- Author:        $Author: dirk.lietz $
-- Date:          $Date: 2015-04-14 13:49:03 +0200 (Di, 14. Apr 2015) $
-- Revision:      $Revision: 378 $
-- SVN-URL:       $HeadURL: http://hb-hq-svn-001.stute.loc/koe-dlz-001/Programmierung/VisualStudio/Projects/Zeiterfassung/SQLServer/prc_saveTaetigkeit.sql $
-- Description:   Tätigkeit updaten
-- =============================================

CREATE PROCEDURE zeiterfassung.prc_saveTaetigkeit 
	@p_Anwender nvarchar(100),
	@p_ID_Taetigkeit int,
	@p_Taetigkeit nvarchar(2000),
	@p_Kategorie nvarchar(50) = NULL,
	@p_LINK nvarchar(500) = NULL,
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
	SET @procedure = 'prc_saveTaetigkeit'

	IF @p_DEBUG = 1			
		print msdb.dbo.debugOut('Start Ausfuehrung Procedure '+@procedure+' AS USER_NAME:'+USER_NAME()+' SUSER_SNAME:'+SUSER_SNAME()+' ORIGINAL_LOGIN:'+ORIGINAL_LOGIN()+' (ID: '+@mark+')',@procedure)

	DECLARE @id_Anwender int,
			@id_Kategorie int;

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


		SELECT @id_Anwender = ID FROM [zeiterfassung].[Anwender] 
		WHERE Anwender = @p_Anwender;


		SET @id_Kategorie = NULL;

		IF @p_Kategorie is not NULL 
		BEGIN
			SELECT @id_Kategorie = ID 
			FROM [zeiterfassung].[Kategorie]
			WHERE ID_Anwender=@id_Anwender AND [Kategorie]=@p_Kategorie;

			IF @id_Kategorie is NULL
			BEGIN
				INSERT INTO [zeiterfassung].[Kategorie] ([Kategorie],[ID_Anwender])
				VALUES(@p_Kategorie,@id_Anwender);

				SET @id_Kategorie = SCOPE_IDENTITY();
			END
		END

		UPDATE [zeiterfassung].[Taetigkeit]
			SET [Taetigkeit] = @p_Taetigkeit,
				[ID_Kategorie] = @id_Kategorie,
				Link = @p_LINK
			WHERE ID = @p_ID_Taetigkeit and [ID_Anwender]=@id_Anwender;

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
