using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atv1_ws
{
    public partial class Form2 : Form
    {
        private int ticks = 0;

        public Form2()
        {
            InitializeComponent();
            inicializarProgress();
        }

        private void inicializarProgress()
        {
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 110;  //12*10
            timer1.Start();
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            progressBar1.Increment(1);
            ticks++;
            if(ticks == 0 || ticks == 1)
            {
                label2.Text = "Carregando componentes...";
            }
            else if (ticks == 30)
            {
                label2.Text = "Carregando dados...";
            }
            else if (ticks == 60)
            {
                label2.Text = "Carregando imagens...";
            }
            else if (ticks == 90)
            {
                label2.Text = "Iniciando Dashboard...";
            }
            else if(ticks == 120)
            {
                timer1.Stop();
                new Form3().Show();
                this.Hide();
            }
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }
    }
}
