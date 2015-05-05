
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF OBJECT_ID (N'zeiterfassung.fkt_TaetigkeitDauer ') IS NOT NULL
    DROP FUNCTION zeiterfassung.fkt_TaetigkeitDauer 
GO
-- =============================================
-- Author:        $Author: dirk.lietz $
-- Date:          $Date: 2015-04-14 13:49:03 +0200 (Di, 14. Apr 2015) $
-- Revision:      $Revision: 378 $
-- SVN-URL:       $HeadURL: http://hb-hq-svn-001.stute.loc/koe-dlz-001/Programmierung/VisualStudio/Projects/Zeiterfassung/SQLServer/fkt_TaetigkeitDauer.sql $
-- Description:   summierte Dauer der Tätigkeiten
-- =============================================
CREATE FUNCTION zeiterfassung.fkt_TaetigkeitDauer 
(	
	@id_Taetigkeit int
)
RETURNS varchar(50)
AS
BEGIN
	DECLARE @dauer int,
			@Result varchar(50)

	select @dauer=sum(dauer)/60
	from (
		select datediff(ss,start,ISNULL(ende,GETDATE())) dauer 
		from [zeiterfassung].[zeitmessung] 
		where ID_Taetigkeit=@id_Taetigkeit
	) a


	if @dauer < 60
		SET @Result = CAST(@dauer as varchar)+' min'
	else
		SET @Result=CAST((@dauer/60) as varchar)+' h '+CAST( (@dauer%60) as varchar) +' min'

	-- Return the result of the function
	RETURN @Result

END
GO

