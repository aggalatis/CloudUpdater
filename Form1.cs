using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using CloudUpdater.Classes;


namespace CloudUpdater
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            Thread trd = new Thread(new ThreadStart(startProcess));
            trd.IsBackground = true;
            trd.Start();

        }

        private void startProcess() {

            Helpers Helpers = new Helpers();
            Server Server = new Server(Helpers);
            while (Server.authecticated(Helpers) == false)
            {
                Helpers.writeLog("Authentications Failed! Retrying after 2 minute...");
                Thread.Sleep(120000);
            }
            Helpers.writeLog("Authentication Passed!!");
            Database DB = new Database(Server.dbhost, Server.dbport, Server.dbname, Server.dbuser, Server.dbpass);
            int lastServerID = Server.retriveLastID(Helpers);

            while (true) {
                if (lastServerID == 0) {
                    Helpers.writeLog("Unable to obtain last order number trying again in 1 minute");
                    Thread.Sleep(60000);
                    lastServerID = Server.retriveLastID(Helpers);
                    continue;
                }
                int dbindex = DB.getLastIndex(Helpers);
                if (dbindex != lastServerID) {
                    //Here I am going to collect the orders and send them to server..
                }
                Thread.Sleep(60000);
            }
            


        }
    }
}
