using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;

namespace Controlador
{
    public partial class Form2 : Form
    {
        DataTable dt = new DataTable();
        DataTable dtB = new DataTable();
        DataTable dtC = new DataTable();
        DataTable dtD = new DataTable();

        // Conexión con Firebase
        IFirebaseClient client;
        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "HcsVQiUAq9CYf2ufjVDfofQabHDmyeqEizAXM93V",
            BasePath = "https://proyectocompdistribuida.firebaseio.com/"
        };

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            // Firebase
            client = new FireSharp.FirebaseClient(config);
            if (client != null)
            {
                MessageBox.Show("Conexión Establecida con el Controlador");
            }

            // DataGridView1
            dt.Columns.Add("ID");
            dt.Columns.Add("Tiempo");
            dt.Columns.Add("Condición");
            dataGridView1.DataSource = dt;
            // DataGridView2
            dtB.Columns.Add("ID");
            dtB.Columns.Add("Tiempo");
            dtB.Columns.Add("Condición");
            dataGridView2.DataSource = dtB;
            // DataGridView3
            dtC.Columns.Add("ID");
            dtC.Columns.Add("Tiempo");
            dtC.Columns.Add("Condición");
            dataGridView3.DataSource = dtC;
            // DataGridView4
            dtD.Columns.Add("ID");
            dtD.Columns.Add("Tiempo");
            dtD.Columns.Add("Condición");
            dataGridView4.DataSource = dtD;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void import()
        {
            dt.Rows.Clear();
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("Counter/node");
            Counter_class obj1 = resp1.ResultAs<Counter_class>();
            int cnt = Convert.ToInt32(obj1.cnt);

            while (true)
            {
                if(i == cnt)
                {
                    break;
                }

                i++;

                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("SensorA/" + i);
                    Data obj2 = resp2.ResultAs<Data>();

                    DataRow row = dt.NewRow();
                    row["ID"] = obj2.Id;
                    row["Tiempo"] = obj2.Time;
                    row["Condición"] = obj2.Condition;

                    dt.Rows.Add(row);

                }
                catch
                {
                    MessageBox.Show("Error al conectar a la base de datos");
                }
            }
            // MessageBox.Show("Actualizado");
        }

        private async void importB()
        {
            dtB.Rows.Clear();
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("CounterB/node");
            Counter_class obj1 = resp1.ResultAs<Counter_class>();
            int cnt = Convert.ToInt32(obj1.cnt);

            while (true)
            {
                if (i == cnt)
                {
                    break;
                }

                i++;

                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("SensorB/" + i);
                    Data obj2 = resp2.ResultAs<Data>();

                    DataRow row = dtB.NewRow();
                    row["ID"] = obj2.Id;
                    row["Tiempo"] = obj2.Time;
                    row["Condición"] = obj2.Condition;

                    dtB.Rows.Add(row);

                }
                catch
                {
                    MessageBox.Show("Error al conectar a la base de datos");
                }
            }
            // MessageBox.Show("Actualizado");
        }

        private async void importC()
        {
            dtC.Rows.Clear();
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("CounterC/node");
            Counter_class obj1 = resp1.ResultAs<Counter_class>();
            int cnt = Convert.ToInt32(obj1.cnt);

            while (true)
            {
                if (i == cnt)
                {
                    break;
                }

                i++;

                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("SensorC/" + i);
                    Data obj2 = resp2.ResultAs<Data>();

                    DataRow row = dtC.NewRow();
                    row["ID"] = obj2.Id;
                    row["Tiempo"] = obj2.Time;
                    row["Condición"] = obj2.Condition;

                    dtC.Rows.Add(row);

                }
                catch
                {
                    MessageBox.Show("Error al conectar a la base de datos");
                }
            }
            // MessageBox.Show("Actualizado");
        }

        private async void importD()
        {
            dtD.Rows.Clear();
            int i = 0;
            FirebaseResponse resp1 = await client.GetTaskAsync("CounterD/node");
            Counter_class obj1 = resp1.ResultAs<Counter_class>();
            int cnt = Convert.ToInt32(obj1.cnt);

            while (true)
            {
                if (i == cnt)
                {
                    break;
                }

                i++;

                try
                {
                    FirebaseResponse resp2 = await client.GetTaskAsync("SensorD/" + i);
                    Data obj2 = resp2.ResultAs<Data>();

                    DataRow row = dtD.NewRow();
                    row["ID"] = obj2.Id;
                    row["Tiempo"] = obj2.Time;
                    row["Condición"] = obj2.Condition;

                    dtD.Rows.Add(row);

                }
                catch
                {
                    MessageBox.Show("Error al conectar a la base de datos");
                }
            }
            // MessageBox.Show("Actualizado");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            import();
            importB();
            importC();
            importD();
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            var fire = new Fire
            {
                Active = "Yes"
            };

            FirebaseResponse response = await client.SetTaskAsync("Fire/", fire);
            Data result = response.ResultAs<Data>();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var fire = new Fire
            {
                Active = "No"
            };

            FirebaseResponse response = await client.SetTaskAsync("Fire/", fire);
            Data result = response.ResultAs<Data>();
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            FirebaseResponse response = await client.DeleteTaskAsync("SensorA/");
            response = await client.DeleteTaskAsync("SensorB/");
            response = await client.DeleteTaskAsync("SensorC/");
            response = await client.DeleteTaskAsync("SensorD/");
            var obj = new Counter_class
            {
                cnt = "0"
            };

            SetResponse response1 = await client.SetTaskAsync("Counter/node", obj);
            response1 = await client.SetTaskAsync("CounterB/node", obj);
            response1 = await client.SetTaskAsync("CounterC/node", obj);
            response1 = await client.SetTaskAsync("CounterD/node", obj);
            MessageBox.Show("La base de datos ha regresado a sus valores por defecto");
        }
    }
}
