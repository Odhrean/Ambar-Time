using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zeiterfassung
{
    public class Taetigkeit
    {
        int ID;
        String infos;
        String Titel;
        String Link="";
        String Kategorie;
        int dauer;
        DateTime dbSyncTime;

        bool aktiv;

        public Taetigkeit()
        {
        }
        public Taetigkeit(int ID, String Titel)
        {
            this.ID = ID;
            this.Titel = Titel;    
        }
        public Taetigkeit(int ID, String Titel, String Link)
        {
            this.ID = ID;
            this.Titel = Titel;
            setLink(Link);
        }
        public Taetigkeit(int ID, String Titel, String Link,String Kategorie)
        {
            this.ID = ID;
            this.Titel = Titel;
            setLink(Link);
            this.Kategorie = Kategorie;
        }

        public Taetigkeit(int ID, String Titel, String Link, String info, String Kategorie, int d, bool a)
        {
            this.ID = ID;
            this.Titel = Titel;
            infos = info;
            setLink(Link);
            this.Kategorie = Kategorie;
            dauer = d;
            aktiv = a;
            dbSyncTime = DateTime.Now;
        }

        public void setId(int ID)
        {
            this.ID = ID;
        }
        public int getID()
        {
            return ID;
        }
        public void setTitel(String titel)
        {
            this.Titel = titel;
        }
        public String getTitel()
        {
            return Titel;
        }
        public String getTitelKurz(int laenge)
        {
            String titel_kurz;
            if (Titel.Length > laenge)
            {
                titel_kurz = Titel.Substring(0, laenge) + "...";
            }
            else
            {
                titel_kurz = Titel;
            }

            return titel_kurz;
        }
        public double getDauer()
        {
            if(!aktiv)
                return dauer;

            // Dauer aus der DB + Dauer seit dem letzten Sync wenn es die aktive Tätigkeit ist
            return dauer + DateTime.Now.Subtract(dbSyncTime).Minutes;
        }
        public void setDauer(int dauer)
        {
            this.dauer = dauer;
        }
        public bool isAktiv()
        {
            return aktiv;
        }
        public String getInfos()
        {
            return infos;
        }
        public void setInfos(String info)
        {
            infos = info;
        }
        public void setLink(String Link)
        {
            this.Link = Link;           
        }
        public String getLink()
        {
            if (Link == null)
            {
                return "";
            }
            else
            {
                return Link;
            }
        }
        public void setKategorie(String kat)
        {
            Kategorie = kat;
        }
        public String getKategorie()
        {
            return Kategorie;
        }
        public String getLinkKurz(int laenge)
        {
            String link_kurz;
            if (getLink().Length > laenge)
            {
                link_kurz = getLink().Substring(0, laenge) + "...";
            }
            else
            {
                link_kurz = getLink();
            }

            return link_kurz;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            if(obj.GetType() != typeof(Taetigkeit))
                return false;

            Taetigkeit test = obj as Taetigkeit;

            return this.ID == test.getID();
        }
    }
}
