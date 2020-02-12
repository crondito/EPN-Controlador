using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Controlador
{
    public partial class Form6 : Form
    {
        System.Timers.Timer timer;
        bool fireOn = false;

        // Conexión con Firebase
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "HcsVQiUAq9CYf2ufjVDfofQabHDmyeqEizAXM93V",
            BasePath = "https://proyectocompdistribuida.firebaseio.com/"
        };

        public Form6()
        {
            InitializeComponent();
            
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                MessageBox.Show("Conexión Establecida con Servidor D");
            }

            comboBox1.SelectedItem = "5";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button1.Enabled = false;

            int time = Convert.ToInt32(comboBox1.SelectedItem);
            timer = new System.Timers.Timer(TimeSpan.FromSeconds(time).TotalMilliseconds);
            timer.AutoReset = true;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(CheckFire);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(Exportar);
            timer.Start();

            comboBox1.Enabled = false;
        }

        private void Exportar(object sender, ElapsedEventArgs e)
        {
            if (fireOn)
            {
                timer.Stop();
                MessageBox.Show("Fuego detectado, apagando Sensor D");
            } else if (!fireOn)
            {
                Export();
            }
            
        }

        private async void Export()
        {
            Random r = new Random();
            int rInt = r.Next(1, 101);
            string info = "OK";
            if (rInt <= 10)
            {
                info = "Error en producción";
            }
            DateTime aDate = DateTime.Now;

            FirebaseResponse resp = await client.GetTaskAsync("CounterD/node");
            Counter_class get = resp.ResultAs<Counter_class>();

            var data = new Data
            {
                Id = (Convert.ToInt32(get.cnt) + 1).ToString(),
                Time = aDate.ToString("HH:mm:ss"),
                Condition = info
            };

            SetResponse response = await client.SetTaskAsync("SensorD/" + data.Id, data);
            Data result = response.ResultAs<Data>();

            var obj = new Counter_class
            {
                cnt = data.Id
            };

            SetResponse response1 = await client.SetTaskAsync("CounterD/node", obj);

        }

        private async void CheckFire(object sender, ElapsedEventArgs e)
        {
            FirebaseResponse response2 = await client.GetTaskAsync("Fire/");
            Fire obj1 = response2.ResultAs<Fire>();
            if (obj1.Active == "No")
            {
                fireOn = false;
            }
            else if (obj1.Active == "Yes")
            {
                fireOn = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Desactivar();
            timer.Stop();
        }

        private void Desactivar()
        {
            button1.Enabled = true;
            button2.Enabled = false;
            comboBox1.Enabled = true;
        }
    }
}
