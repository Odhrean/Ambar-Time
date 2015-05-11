using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using wyDay.Controls;

namespace Zeiterfassung
{
    class ZEContextMenu
    {
        ContextMenu cm;
        Buchungen buch;
        Taetigkeit akt;
        List<Taetigkeit> offeneTaetigkeiten;

        public void Dispose()
        {
            if ( (cm != null))
            {
                cm.Dispose();
            }
            
        }

        public ZEContextMenu()
        {
            cm = new ContextMenu();
            buch = new Buchungen();            
            cm.Popup += new System.EventHandler(ZeiterfassungNotifyApp.MyPopupEventHandler);
            offeneTaetigkeiten = new List<Taetigkeit>();

            refresh();            

        }

        public ContextMenu getContextMenu()
        {
            cm.MenuItems.Clear();
            
            VistaMenu vistaMenu = new VistaMenu();

            foreach (var tatigkeit in offeneTaetigkeiten) {
                addMenuItem(tatigkeit, vistaMenu);    
            }            

            cm.MenuItems.Add("-");
            MenuItem weitere = new MenuItem("mehr...");
            weitere.MenuItems.Add(MiInfo(vistaMenu));
            MenuItem refresh = new MenuItem("aktualisieren");
            refresh.Click += (o, i) =>
            {
                this.refresh();
            };
            weitere.MenuItems.Add(refresh);
            MenuItem alleTaetigkeiten = new MenuItem("Historie");
            alleTaetigkeiten.Click += (o, i) =>
            {
                new HistorieTaetigkeiten().Show();
            };
            weitere.MenuItems.Add(alleTaetigkeiten);

            weitere.MenuItems.Add(MiEnde(vistaMenu));
            cm.MenuItems.Add(weitere);
            cm.MenuItems.Add("-");

            addAktiveTaetigkeit(vistaMenu);
            cm.MenuItems.Add("-");
            cm.MenuItems.Add(MiNeu(vistaMenu));
            
            ((System.ComponentModel.ISupportInitialize)(vistaMenu)).EndInit();

            return cm;

        }

        public void addNewTaetigkeit(Taetigkeit taetigkeit)
        {
            refresh();
        }

        private void addMenuItem(Taetigkeit taetigkeit,VistaMenu vistaMenu)
        {

            // Aktive Tätigkeit hier nicht anzeigen
            if (taetigkeit.Equals(akt))
                return;

            MenuItem mi = new MenuItem(taetigkeit.getTitel());
            MenuItem infos = new MenuItem(taetigkeit.getInfos());
            infos.Enabled = false;
            mi.MenuItems.Add(infos);
            MenuItem weiter = new MenuItem("fortsetzen");
            vistaMenu.SetImage(weiter, Properties.Resources.start_icon);
            weiter.Click += (o, i) =>
            {
                this.resume(taetigkeit);
            };
            mi.MenuItems.Add(weiter);

            if (akt != null && !taetigkeit.Equals(akt))
            {
                MenuItem weiterUndStop = new MenuItem("fortsetzen und \"" + akt.getTitelKurz(30)+"\" beenden");
                vistaMenu.SetImage(weiterUndStop, Properties.Resources.start_icon);
                weiterUndStop.Click += (o, i) =>
                {
                    this.stop(akt);
                    this.resume(taetigkeit);
                };
                mi.MenuItems.Add(weiterUndStop);
            }

            if (taetigkeit.getLink() != "")
            {
                MenuItem link = new MenuItem("fortsetzen (" + taetigkeit.getLinkKurz(30) + ")");
                vistaMenu.SetImage(link, Properties.Resources.start_icon);
                link.Click += (o, i) =>
                {
                    this.resume(taetigkeit);
                    System.Diagnostics.Process.Start(taetigkeit.getLink());
                };
                mi.MenuItems.Add(link);
            }

            MenuItem miStop = new MenuItem("beenden");
            vistaMenu.SetImage(miStop, Properties.Resources.fertig_icon);
            miStop.Click += (o, i) =>
            {
                this.stop(taetigkeit);
            };
            mi.MenuItems.Add(miStop);

            MenuItem miBearbeiten = new MenuItem("Tätigkeit editieren");
            vistaMenu.SetImage(miBearbeiten, Properties.Resources.edit_icon);
            miBearbeiten.Click += (o, i) =>
            {
                new EditTaetigkeit(taetigkeit).Show();                
            };
            mi.MenuItems.Add(miBearbeiten);

            cm.MenuItems.Add(mi);


        }
        
        private MenuItem MiNeu(VistaMenu vistaMenu)
        {
            MenuItem neu = new MenuItem();
            neu.Text = "&Neu";
            vistaMenu.SetImage(neu, Properties.Resources.start);
            neu.Click += (o, i) =>
            {
                new NeuErfassen().Show();
            };
            neu.ShowShortcut = true;
            neu.Shortcut = Shortcut.CtrlN;

            return neu;
        }
        private MenuItem MiInfo(VistaMenu vistaMenu)
        {
            MenuItem info = new MenuItem();            
            info.Text = "&Info";
            vistaMenu.SetImage(info, Properties.Resources.Information_icon);
            info.Click += (o, i) =>
            {
                new About().Show();
            };
            return info;
        }
        private MenuItem MiEnde(VistaMenu vistaMenu)
        {
            MenuItem ende = new MenuItem();
            ende.Text = "&Beenden";
            vistaMenu.SetImage(ende, Properties.Resources.stop_icon);
            ende.Click += new System.EventHandler(ZeiterfassungNotifyApp.ExitClick);
            return ende;
        }

        private void addAktiveTaetigkeit(VistaMenu vistaMenu)
        {
            if (akt != null)
            {
                MenuItem miAkt = new MenuItem();
                miAkt.Text = "Aktiv: " + akt.getTitel() + ", Dauer: " + (akt.getDauer() < 60 ? Convert.ToInt32(akt.getDauer())+ " min" : Convert.ToInt32(akt.getDauer()) / 60 + " h " + akt.getDauer() % 60 + " min");
                miAkt.Enabled = false;
                cm.MenuItems.Add(miAkt);

                // Funktionen die sich auf aktive Taetigkeit beziehen aktivieren
                MenuItem stop = new MenuItem();
                stop.Text = "\""+akt.getTitel()+ "\" beenden";
                vistaMenu.SetImage(stop, Properties.Resources.fertig);
                stop.Click += (o, i) =>
                {
                    this.stop();
                };
                cm.MenuItems.Add(stop);
                MenuItem pauseLink = new MenuItem();
                pauseLink.Text = "\"" + akt.getTitel() + "\" pausieren und Link updaten";
                vistaMenu.SetImage(pauseLink, Properties.Resources.pause);
                pauseLink.Click += (o, i) =>
                {
                    new UpdateLink(akt).Show();
                };
                cm.MenuItems.Add(pauseLink);

                MenuItem pause = new MenuItem();
                pause.Text = "\"" + akt.getTitel() + "\" pausieren";
                vistaMenu.SetImage(pause, Properties.Resources.pause);
                pause.Click += (o, i) =>
                {
                    this.pause();
                };
                cm.MenuItems.Add(pause);

                MenuItem bearbeiten = new MenuItem();
                bearbeiten.Text = "\"" + akt.getTitel() + "\" editieren";
                vistaMenu.SetImage(bearbeiten, Properties.Resources.edit);
                bearbeiten.Click += (o, i) =>
                {
                    new EditTaetigkeit(akt).Show();
                };
                cm.MenuItems.Add(bearbeiten);


            }
        }

        public Taetigkeit getAktiveTaetigkeit()
        {
            return akt;
        }


        public void pause()
        {
            if (akt != null)
            {
                buch.stop(akt.getID(), false);
                refresh();
            }
        }
        public void stop()
        {
            if (akt != null)
            {
                buch.stop(akt.getID(), true);
                refresh();
            }
        }
        public void stop(Taetigkeit stopTaetigkeit)
        {
            if (stopTaetigkeit != null)
            {
                buch.stop(stopTaetigkeit.getID(), true);
                
                refresh();
            }
        }

        public void resume(Taetigkeit fortsetzen)
        {
            buch.start(fortsetzen.getID());
            refresh();
        }
        public void refresh()
        {
            offeneTaetigkeiten = buch.GetTaetigkeiten();
            akt = null;
            foreach (var tatigkeit in offeneTaetigkeiten)
            {
                if (tatigkeit.isAktiv())
                {
                    this.akt = tatigkeit;
                    ZeiterfassungNotifyApp.IconRun(akt.getTitelKurz(51));
                    break;
                }
            }            

            if (akt == null)
                ZeiterfassungNotifyApp.IconStop();

        }
    }
}
