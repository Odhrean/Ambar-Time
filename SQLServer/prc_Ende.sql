SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID (N'zeiterfassung.prc_Ende') IS NOT NULL
    DROP PROCEDURE zeiterfassung.prc_Ende
GO
-- =============================================
-- Author:        $Author: dirk.lietz $
-- Date:          $Date: 2015-04-13 13:25:40 +0200 (Mo, 13. Apr 2015) $
-- Revision:      $Revision: 371 $
-- SVN-URL:       $HeadURL: http://hb-hq-svn-001.stute.loc/koe-dlz-001/Programmierung/VisualStudio/Projects/Zeiterfassung/SQLServer/prc_Ende.sql $
-- Description:   Starte Tätigkeit
-- =============================================

CREATE PROCEDURE zeiterfassung.prc_Ende 
	@p_ID_Taetigkeit int = NULL,		-- Entweder Tätigkeit 
	@p_Anwender nvarchar(50)=NULL,		-- oder die letzte Tätigkeit des Anwenders stoppen
	@p_Stop bit = 0,	-- 0: temporär stoppen
						-- 1: Tätigkeit beenden

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
	SET @procedure = 'prc_Ende'

	IF @p_DEBUG = 1			
		print msdb.dbo.debugOut('Start Ausfuehrung Procedure '+@procedure+' AS USER_NAME:'+USER_NAME()+' SUSER_SNAME:'+SUSER_SNAME()+' ORIGINAL_LOGIN:'+ORIGINAL_LOGIN()+' (ID: '+@mark+')',@procedure)

	DECLARE @id_Anwender int;

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


		DECLARE @stat int
		IF @p_Stop=0 
			SET @stat=1
		ELSE
			SET @stat=2


		IF @p_Anwender is not NULL AND @p_ID_Taetigkeit is NULL
		BEGIN
			SELECT @id_Anwender = ID FROM [zeiterfassung].[Anwender] 
			WHERE Anwender = @p_Anwender;

			SELECT @p_ID_Taetigkeit = ID FROM [zeiterfassung].[Taetigkeit]
			WHERE ID_Anwender = @id_Anwender AND [Status] = 0;
		END


		UPDATE [zeiterfassung].[Taetigkeit]
			SET [Status] = @stat,	
				TIME_AEN =getdate()
			WHERE ID = @p_ID_Taetigkeit;

		UPDATE [zeiterfassung].[zeitmessung]
			SET Ende=getdate()
			WHERE [ID_Taetigkeit]= @p_ID_Taetigkeit
			AND Ende is NULL;


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
