USE [Personal]
GO

/****** Object:  UserDefinedFunction [zeiterfassung].[fkt_TaetigkeitDauer]    Script Date: 13.04.2015 13:04:22 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID (N'zeiterfassung.fkt_TaetigkeitDauer_min') IS NOT NULL
    DROP FUNCTION zeiterfassung.fkt_TaetigkeitDauer_min 
GO
-- =============================================
-- Author:        $Author: dirk.lietz $
-- Date:          $Date: 2015-04-14 13:49:03 +0200 (Di, 14. Apr 2015) $
-- Revision:      $Revision: 378 $
-- SVN-URL:       $HeadURL: http://hb-hq-svn-001.stute.loc/koe-dlz-001/Programmierung/VisualStudio/Projects/Zeiterfassung/SQLServer/fkt_TaetigkeitDauer_min.sql $
-- Description:   summierte Dauer der Tätigkeiten
-- =============================================
CREATE FUNCTION [zeiterfassung].[fkt_TaetigkeitDauer_min] 
(	
	@id_Taetigkeit int
)
RETURNS int
AS
BEGIN
	DECLARE @dauer int

	select @dauer=sum(dauer)/60
	from (
		select datediff(ss,start,ISNULL(ende,GETDATE())) dauer 
		from [zeiterfassung].[zeitmessung] 
		where ID_Taetigkeit=@id_Taetigkeit
	) a


	-- Return the result of the function
	RETURN @dauer

END

GO


