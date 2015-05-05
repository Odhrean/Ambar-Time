using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using wyDay.Controls;
using System.Reflection;
using Microsoft.Win32;
using System.Diagnostics;

namespace Zeiterfassung
{
    static class ZeiterfassungNotifyApp
    {
        private static NotifyIcon notico;
        public static ZEContextMenu cm;
        private static Taetigkeit lastActive;
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]

        //==========================================================================
        public static void Main(string[] astrArg)
        {

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            notico = new NotifyIcon();
            notico.Icon = Properties.Resources.favicon;
            notico.Text = "Zeiterfassung"; 
            notico.Visible = true;
            notico.DoubleClick += new EventHandler(NotifyIconDoubleClick);
            //notico.Click += new EventHandler(Click);

            cm = new ZEContextMenu();
            notico.ContextMenu = cm.getContextMenu();

            Application.ApplicationExit += new EventHandler(OnExit);

            // Always get the final notification when the event thread is shutting down
            // so we can unregister.
            SystemEvents.EventsThreadShutdown += new EventHandler(OnExit);
            SystemEvents.PowerModeChanged +=     new PowerModeChangedEventHandler(OnPowerModeChanged);
            SystemEvents.SessionSwitch +=        new SessionSwitchEventHandler(OnSessionSwitch);
            //SystemEvents.SessionEnding +=        new SessionEndingEventHandler(OnExit);
            SystemEvents.SessionEnded +=         new SessionEndedEventHandler(OnExit);

            Application.Run();
        }

        //==========================================================================
        public static void ExitClick(Object sender, EventArgs e)
        {
            notico.Dispose();
            cm.Dispose();
            Application.Exit();
        }

        private static void OnSessionSwitch(Object sender, SessionSwitchEventArgs e)
        {
            switch (e.Reason)
            {
                case SessionSwitchReason.ConsoleConnect:
                    break;
                case SessionSwitchReason.ConsoleDisconnect:
                    break;
                case SessionSwitchReason.RemoteConnect:
                    break;
                case SessionSwitchReason.RemoteDisconnect:
                    break;
                case SessionSwitchReason.SessionLock:
                    break;
                case SessionSwitchReason.SessionLogoff:
                    lastActive = cm.getAktiveTaetigkeit();
                    cm.pause();
                    break;
                case SessionSwitchReason.SessionLogon:
                    if (lastActive != null)
                    {
                        cm.resume(lastActive);
                    }
                    break;
                case SessionSwitchReason.SessionRemoteControl:
                    break;
                case SessionSwitchReason.SessionUnlock:
                    break;
                default:
                    break;
            }          
        }

        private static void OnPowerModeChanged(Object sender, PowerModeChangedEventArgs  e)
        {
            switch (e.Mode)
            {
                case PowerModes.Resume:
                    if (lastActive != null)
                    {
                        cm.resume(lastActive);
                    }
                    Debug.WriteLine("PowerMode: OS is resuming from suspended state");
                    break;
                case PowerModes.StatusChange:
                    Debug.WriteLine("PowerMode: There was a change relating to the power" +
                        " supply (weak battery, unplug, etc..)");
                    break;
                case PowerModes.Suspend:
                    Debug.WriteLine("PowerMode: OS is about to be suspended");
                    lastActive = cm.getAktiveTaetigkeit();
                    cm.pause();
                    break;
            }
        }

        private static void OnExit(Object sender, EventArgs e)
        {
            SystemEvents.EventsThreadShutdown -= new EventHandler(OnExit);
            SystemEvents.PowerModeChanged -=     new PowerModeChangedEventHandler(OnPowerModeChanged);
            SystemEvents.SessionSwitch -=        new SessionSwitchEventHandler(OnSessionSwitch);
            //SystemEvents.SessionEnding -=        new SessionEndingEventHandler(OnExit);
            SystemEvents.SessionEnded -=         new SessionEndedEventHandler(OnExit);

            if (cm != null && cm.getAktiveTaetigkeit() != null)
            {
                cm.pause();
            }
        }

        //==========================================================================
        private static void NotifyIconDoubleClick(Object sender, EventArgs e)
        {
            new NeuErfassen().Show();
        }
        private static void Click(Object sender, EventArgs e)
        {
            
            MethodInfo methodInfo = typeof(NotifyIcon).GetMethod("ShowContextMenu",
                BindingFlags.Instance | BindingFlags.NonPublic);

            methodInfo.Invoke(notico, null);
        }

        public static void IconRun(String taetigkeit)
        {

            //notico.Text = "Läuft: \"" + taetigkeit + "\"";
            if (notico != null)
            {
                notico.Text = "Zeiterfassung";
                if (notico.Icon != null)
                    notico.Icon = Properties.Resources.favicon_run;
            }
        }

        public static void IconStop()
        {
            if (notico != null)
            {
                notico.Text = "Zeiterfassung";
                if(notico.Icon != null )
                    notico.Icon = Properties.Resources.favicon;
            }
        }

        public static void MyPopupEventHandler(System.Object sender, System.EventArgs e)
        {

            notico.ContextMenu = cm.getContextMenu();
        }

    }
}
