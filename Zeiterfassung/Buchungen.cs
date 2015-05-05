using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Zeiterfassung
{
    class Buchungen
    {

        public List<Taetigkeit> GetTaetigkeiten()
        {
            List<Taetigkeit> ret = new List<Taetigkeit>();

            String sql = @"select t.ID,t.Taetigkeit,t.Link, 
                       LTRIM('Gestartet: '+convert(varchar,t.time_neu,120)+', Dauer: '+zeiterfassung.fkt_TaetigkeitDauer(t.ID)), 
                       k.[Kategorie], 
                       zeiterfassung.fkt_TaetigkeitDauer_Min(t.ID), 
                       CASE WHEN [status]=0 THEN 1 ELSE 0 END
                       from [zeiterfassung].[Taetigkeit] t WITH(NOLOCK)
                       join [zeiterfassung].[Anwender] a WITH(NOLOCK) on t.ID_Anwender=a.ID 
                       left outer join [zeiterfassung].[Kategorie] k on k.ID=t.[ID_Kategorie] 
                       where a.Anwender='" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "' "
                       +"and [status] in (0,1) "
                       +"order by t.time_aen ";
    
            clsDatabase db = new clsDatabase(sql, "Buchungen->GetOffeneTaetigkeiten");

            while (db.getDataReader().Read())
            {
                Taetigkeit taetigkeit = new Taetigkeit(
                     db.getDataReader().GetInt32(0),    // ID
                     db.getDataReader().GetString(1),   // Titel
                     !db.getDataReader().IsDBNull(2) ? db.getDataReader().GetString(2):"",         // Link
                     !db.getDataReader().IsDBNull(3) ? db.getDataReader().GetString(3):"",         // Infos  
                     !db.getDataReader().IsDBNull(4) ? db.getDataReader().GetString(4):"",         // Kategorie
                     !db.getDataReader().IsDBNull(5) ? db.getDataReader().GetInt32(5) : 0,         // Dauer Min
                     db.getDataReader().GetInt32(6)==1 ? true : false
                 );
                
                ret.Add(taetigkeit);
            }

            db.close();

            return ret;
        }

        public List<Kategorie> GetKategorien()
        {
            List<Kategorie> ret = new List<Kategorie>();

            String sql = @"select k.ID,Kategorie from [zeiterfassung].[Kategorie] k 
                           join  [zeiterfassung].[Anwender] a on k.ID_Anwender=a.ID
                           where a.Anwender='" + System.Security.Principal.WindowsIdentity.GetCurrent().Name + "' "
               +          "order by Kategorie";

            clsDatabase db = new clsDatabase(sql, "Buchungen->GetKategorien");

            while (db.getDataReader().Read())
            {
                Kategorie kategorie = new Kategorie();
                kategorie.ID=((int)db.getDataReader()[0]);
                kategorie.Name=((string)db.getDataReader()[1]);
                ret.Add(kategorie);
            }

            ret.Add(new Kategorie());
            db.close();

            return ret;
        }
        public void saveTaetigkeit(Taetigkeit save)
        {
            if(save != null && save.getID() > 0)
            {
                clsDatabase db = new clsDatabase("zeiterfassung.prc_saveTaetigkeit", "Buchungen->saveTaetigkeit", clsDatabase.PROC);
                SqlParameter p_Anwender = db.getCommand().Parameters.Add("@p_Anwender", SqlDbType.VarChar);
                p_Anwender.Direction = ParameterDirection.Input;
                SqlParameter p_Taetigkeit = db.getCommand().Parameters.Add("@p_Taetigkeit", SqlDbType.VarChar);
                p_Taetigkeit.Direction = ParameterDirection.Input;
                SqlParameter p_ID_Taetigkeit = db.getCommand().Parameters.Add("@p_ID_Taetigkeit", SqlDbType.Int);
                p_ID_Taetigkeit.Direction = ParameterDirection.Input;
                SqlParameter p_Kategorie = db.getCommand().Parameters.Add("@p_Kategorie", SqlDbType.VarChar);
                p_Kategorie.Direction = ParameterDirection.Input;
                SqlParameter p_LINK = db.getCommand().Parameters.Add("@p_LINK", SqlDbType.VarChar);
                p_Kategorie.Direction = ParameterDirection.Input;


                p_Anwender.Value = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                p_ID_Taetigkeit.Value = save.getID();;
                p_Taetigkeit.Value = save.getTitel();

                if (save.getKategorie() != "")
                {
                    p_Kategorie.Value = save.getKategorie();
                }
                if (save.getLink() != "")
                {
                    p_LINK.Value = save.getLink();
                }
                db.execute(false);

                db.close();
            }
        }
        public int start(int ID_Taetigkeit)
        {
            return start("", "", ID_Taetigkeit,"");
        }

        public int start(String Taetigkeit)
        {
            return start(Taetigkeit, "", 0,"");
        }

        public int start(String Taetigkeit, String Kategorie,String Link)
        {
            return start(Taetigkeit, Kategorie, 0,Link);
        }

        public int start(String Taetigkeit, String Kategorie, int ID_Taetigkeit,String Link) 
        {
            int id = 0;

            
            clsDatabase db = new clsDatabase("zeiterfassung.prc_Start", "Buchungen->start", clsDatabase.PROC);
            SqlParameter p_Anwender = db.getCommand().Parameters.Add("@p_Anwender", SqlDbType.VarChar);
            p_Anwender.Direction = ParameterDirection.Input;
            SqlParameter p_Taetigkeit = db.getCommand().Parameters.Add("@p_Taetigkeit", SqlDbType.VarChar);
            p_Taetigkeit.Direction = ParameterDirection.Input;
            SqlParameter p_ID_Taetigkeit = db.getCommand().Parameters.Add("@p_ID_Taetigkeit", SqlDbType.Int);
            p_ID_Taetigkeit.Direction = ParameterDirection.Input;
            SqlParameter p_Kategorie = db.getCommand().Parameters.Add("@p_Kategorie", SqlDbType.VarChar);
            p_Kategorie.Direction = ParameterDirection.Input;
            SqlParameter p_LINK = db.getCommand().Parameters.Add("@p_LINK", SqlDbType.VarChar);
            p_Kategorie.Direction = ParameterDirection.Input;
            SqlParameter p_ID = db.getCommand().Parameters.Add("@p_ID", SqlDbType.Int);
            p_ID.Direction = ParameterDirection.Output;


            p_Anwender.Value = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            if (Taetigkeit != "")
            {
                p_Taetigkeit.Value = Taetigkeit;
            }

            if (ID_Taetigkeit != 0)
            {
                p_ID_Taetigkeit.Value = ID_Taetigkeit;
            }

            if (Kategorie != "")
            {
                p_Kategorie.Value = Kategorie;
            }
            if (Link != "")
            {
                p_LINK.Value = Link;
            }
            db.execute(false);
            id = Convert.ToInt32(p_ID.Value); 
            db.close();
            
            return id;
        }


        // ############################################################################################


        public void stop()
        {
            stop(0, false);
        }

        public void stop(bool bStop)
        {
            stop(0, bStop);
        }

        public void stop(int ID_Taetigkeit)
        {
            stop(ID_Taetigkeit, false);
        }

        public void stop(int ID_Taetigkeit, bool bStop)
        {
            
            clsDatabase db = new clsDatabase("zeiterfassung.prc_Ende", "Buchungen->stop", clsDatabase.PROC);

            SqlParameter p_Anwender = db.getCommand().Parameters.Add("@p_Anwender", SqlDbType.VarChar);
            p_Anwender.Direction = ParameterDirection.Input;
            SqlParameter p_ID_Taetigkeit = db.getCommand().Parameters.Add("@p_ID_Taetigkeit", SqlDbType.Int);
            p_ID_Taetigkeit.Direction = ParameterDirection.Input;
            SqlParameter p_Stop = db.getCommand().Parameters.Add("@p_Stop", SqlDbType.Bit);
            p_Stop.Direction = ParameterDirection.Input;

            p_Anwender.Value = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

            if (bStop)
            {
                p_Stop.Value = 1;
            }
            else 
            {
               p_Stop.Value=0;
            }

            if (ID_Taetigkeit != 0)
            {
                p_ID_Taetigkeit.Value = ID_Taetigkeit;
            }

            db.execute(false);

            db.close();
             
        }

    }
}

