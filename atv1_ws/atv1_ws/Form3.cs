using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atv1_ws
{
    public partial class Form3 : Form
    {
        projeto_wsEntities bd = new projeto_wsEntities();
        usuario logada = new usuario();
        usuario pesquisado = new usuario();
        public Form3()
        {
            InitializeComponent();

            label1.Visible = false;
            logada = Form1.logado;
            carregarFoto();
            preencheInfo();
            preencheCombos();

            comboBox1.SelectedIndexChanged += mudaPais;
            comboBox2.SelectedIndexChanged += mudaEstado;

            panel1.BackColor = Color.FromArgb(1, 115, 56);
            panel2.BackColor = Color.FromArgb(17, 92, 53);
            panel3.BackColor = Color.White;
            panel3.Visible = false;

            linkLabel5.Location = new System.Drawing.Point(0, 56);

            linkLabel2.Click += expandirCadastros;
            linkLabel3.Visible = false;
            linkLabel4.Visible = false;

            linkLabel5.Click += expandirRelatorios;
            linkLabel6.Visible = false;
            linkLabel7.Visible = false;

            if (logada.administrador == true) 
            {
                checkBox1.Enabled = true;
            }
            else
            {
                checkBox1.Enabled = false;
            }
            
            this.roundShapePB1.ContextMenuStrip = this.contextMenuStrip1;
            roundShapePB1.MouseHover += mostrarNome;
            //roundShapePB1.MouseLeave += tirarNome;
            //roundShapePB1.Click += mostrarMenu;

            btnSalvar.Click += salvarAlteracao;
            button1.Click += addFoto;

            monthCalendar1.Visible = false;
            pictureBox4.Click += aparecerCalendario;

            txtSenha.UseSystemPasswordChar = true;
            txtConfirmacao.UseSystemPasswordChar = true;
            pictureBox5.Click += aparecerSenha;
            txtSenha.LostFocus += saiu;

        }
            
        private void saiu(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSenha.Text))
            {
                txtConfirmacao.Visible = false;
                label26.Visible = false;
                label27.Visible = false;
            }
            else
            {
                txtConfirmacao.Visible = true;
                label26.Visible = true;
                label27.Visible = true;
            }
        }
        private void aparecerSenha(object sender, EventArgs e)
        {
            if (txtSenha.UseSystemPasswordChar == true)
            {
                txtSenha.UseSystemPasswordChar = false;
                txtConfirmacao.UseSystemPasswordChar = false;
            }
            else
            {
                txtSenha.UseSystemPasswordChar = true;
                txtConfirmacao.UseSystemPasswordChar = true;
            }
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {
            txtData.Text = monthCalendar1.SelectionStart.ToString();
        }

        private void aparecerCalendario(object sender, EventArgs e)
        {
            if (monthCalendar1.Visible == false)
            {
                monthCalendar1.Visible = true;
                txtTelefone.Visible = false;
                label16.Visible = false;
                label17.Visible = false;
            }
            else
            {
                monthCalendar1.Visible = false;
                txtTelefone.Visible = true;
                label16.Visible = true;
                label17.Visible = true;
            }
        }

        private void mudaEstado(object sender, EventArgs e)
        {
            comboBox3.Items.Clear();

            bd.cidade.ToList().ForEach(s =>
            {
                if (s.id_estadoss == comboBox2.SelectedIndex + 1)
                {
                    comboBox3.Items.Add(s.nome);
                }
            });
        }

        private void mudaPais(object sender, EventArgs e)
        {
            comboBox2.Items.Clear();
            bd.estados.ToList().ForEach(s =>
            {
                if (s.id_paiss == comboBox1.SelectedIndex + 1)
                {
                    comboBox2.Items.Add(s.nome);

                    comboBox3.Items.Clear();
                    bd.cidade.ToList().ForEach(a =>
                    {
                        if (a.id_estadoss == comboBox2.SelectedIndex + 1)
                        {
                            comboBox3.Items.Add(a.nome);
                        }
                    });
                }
            });
        }

        private void addFoto(object sender, EventArgs e)
        {
            OpenFileDialog opnfd = new OpenFileDialog();
            opnfd.Filter = "Image Files (*.jpg;*.jpeg;.*.gif;)|*.jpg;*.jpeg;.*.gif";
            if (opnfd.ShowDialog() == DialogResult.OK)
            {
                roundShapePB2.Image = new Bitmap(opnfd.FileName);
            }

            //MessageBox.Show(comboBox1.SelectedIndex.ToString());
        }

        private void preencheCombos()
        {
            //país
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            comboBox3.Items.Clear();
            bd.paises.ToList().ForEach(m =>
            {
                comboBox1.Items.Add(m.nome);
            });

            bd.estados.ToList().ForEach(s =>
            {
                comboBox2.Items.Add(s.nome);
            });

            bd.cidade.ToList().ForEach(c =>
            {
                comboBox3.Items.Add(c.nome);
            });
        }

        private void salvarAlteracao(object sender, EventArgs e)
        {
            if (txtSenha == txtConfirmacao)
            {
                if (comboBox1.SelectedItem == null || comboBox2.SelectedItem == null || comboBox3.SelectedItem == null)
                {
                    
                    usuario alterar = new usuario();
                    alterar = logada;
                    string novoValorNome = txtNome.Text;
                    string novoValorData = txtData.Text;
                    string novoValorTelefone = txtTelefone.Text;
                    string novoValorCelular = txtCelular.Text;
                    string novoValorEmail = txtEmail.Text;
                    string novoValorSenha = txtSenha.Text;
                    string novoValorCEP = txtCEP.Text;
                    string novoValorBairro = txtBairro.Text;
                    string novoValorEndereco = txtEndereco.Text;

                    paises tes = new paises();
                    tes = bd.paises.Where(u => u.nome.Equals(comboBox1.Text)).FirstOrDefault();

                    estados tec = new estados();
                    tec = bd.estados.Where(d => d.nome.Equals(comboBox2.Text)).FirstOrDefault();

                    cidade tep = new cidade();
                    tep = bd.cidade.Where(g => g.nome.Equals(comboBox3.Text)).FirstOrDefault();


                    Image img = roundShapePB2.Image;
                    byte[] arr;
                    ImageConverter converter = new ImageConverter();
                    arr = (byte[])converter.ConvertTo(img, typeof(byte[]));


                    bool validAdm = false;
                    if (checkBox1.Checked == true)
                    {
                        validAdm = true;
                    }
                    else
                    {
                        validAdm = false;
                    }


                    //usuario novin = new usuario();
                    bd.usuario.ToList().ForEach(f =>
                    {
                        if (f.id == alterar.id)
                        {
                            f.nome = novoValorNome;
                            f.foto = arr;
                            f.dataDeNascimento = novoValorData;
                            f.telefone = novoValorTelefone;
                            f.celular = novoValorCelular;
                            f.email = novoValorEmail;
                            f.senha = novoValorSenha;
                            f.cep = novoValorCEP;

                            f.id_pais = tes.id;
                            f.id_estado = tec.id;
                            f.id_cidade = tep.id;

                            f.bairro = novoValorBairro;
                            f.endereco = novoValorEndereco;
                            f.administrador = validAdm;

                            bd.SaveChanges();
                        }
                    });
                    //bd.usuario.Add(novin);
                    bd.SaveChanges();

                    label1.Text = "Perfil salvo com sucesso!";
                }
                else
                {
                    label1.Visible = true;
                    label1.Text = "Atenção: País/Estado/Cidade não selecionado.";
                }
            }
            else
            {
                label1.Visible = true;
                label1.Text = "Atenção: As senhas não são iguais.";
            }
        }

        private void sairToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new Form1().Show();
            this.Hide();
        }

        private void perfilToolStripMenuItem_Click(object sender, EventArgs e)
        {
            panel3.Visible = true;
        }

        private void preencheInfo()
        {
            paises aaa = new paises();
            aaa = bd.paises.Where(u => u.id.Equals(logada.id_pais)).FirstOrDefault();

            estados bbb = new estados();
            bbb = bd.estados.Where(d => d.id.Equals(logada.id_estado)).FirstOrDefault();

            cidade ccc = new cidade();
            ccc = bd.cidade.Where(g => g.id.Equals(logada.id_cidade)).FirstOrDefault();

            txtNome.Text = logada.nome;
            txtData.Text = logada.dataDeNascimento;
            txtTelefone.Text = logada.telefone;
            txtCelular.Text = logada.celular;
            txtEmail.Text = logada.email;
            txtSenha.Text = logada.senha;
            txtCEP.Text = logada.cep;
            comboBox1.Text = aaa.nome;
            comboBox2.Text = bbb.nome;
            comboBox3.Text = ccc.nome;
            //comboBox1.SelectedIndex = logada.id_pais;
            //comboBox2.SelectedIndex = logada.id_estado;
            //comboBox3.SelectedIndex = logada.id_cidade;
            txtBairro.Text = logada.bairro;
            txtEndereco.Text = logada.endereco;
            if(logada.administrador == true)
            {
                checkBox1.Checked = true;
            }
            else
            {
                checkBox1.Checked = false;
            }

            usuario obj;
            obj = bd.usuario.FirstOrDefault(a => a.id == logada.id);
            if (obj == null) return;
            var stream = new MemoryStream(obj.foto);
            roundShapePB2.Image = Image.FromStream(stream);
        }

        //private void mostrarMenu(object sender, EventArgs e)
        //{
        //contextMenuStrip1.Visible = true;
        //}


        private void mostrarNome(object sender, EventArgs e)
        {
            logada = Form1.logado;
            ToolTip tt = new ToolTip();
            tt.SetToolTip(this.roundShapePB1, logada.nome);
        }

        private void carregarFoto()
        {
            pesquisado = Form1.logado;
            usuario obj;
            obj = bd.usuario.FirstOrDefault(a => a.id == pesquisado.id);
            if (obj == null) return;
            var stream = new MemoryStream(obj.foto);
            roundShapePB1.Image = Image.FromStream(stream);
        }

        private void expandirRelatorios(object sender, EventArgs e)
        {
            if (linkLabel3.Visible == false && linkLabel4.Visible == false)
            {
                if (linkLabel6.Visible == false && linkLabel7.Visible == false)
                {
                    linkLabel6.Visible = true;
                    linkLabel7.Visible = true;
                    linkLabel6.Location = new System.Drawing.Point(0, 83);
                    linkLabel7.Location = new System.Drawing.Point(0, 110);
                }
                else
                {
                    linkLabel6.Visible = false;
                    linkLabel7.Visible = false;
                }
            }
            else
            {
                if (linkLabel6.Visible == false && linkLabel7.Visible == false)
                {
                    linkLabel6.Visible = true;
                    linkLabel7.Visible = true;
                    linkLabel6.Location = new System.Drawing.Point(0, 138);
                    linkLabel7.Location = new System.Drawing.Point(0, 165);
                }
                else
                {
                    linkLabel6.Visible = false;
                    linkLabel7.Visible = false;
                }
            }
                
        }

        private void expandirCadastros(object sender, EventArgs e)
        {
            if (linkLabel3.Visible == false && linkLabel4.Visible == false)
            {
                linkLabel3.Visible = true;
                linkLabel4.Visible = true;
                linkLabel5.Location = new System.Drawing.Point(0, 110);
                linkLabel6.Location = new System.Drawing.Point(0, 138);
                linkLabel7.Location = new System.Drawing.Point(0, 165);
            }
            else
            {
                linkLabel3.Visible = false;
                linkLabel4.Visible = false;
                linkLabel5.Location = new System.Drawing.Point(0, 56);
                linkLabel6.Location = new System.Drawing.Point(0, 83);
                linkLabel7.Location = new System.Drawing.Point(0, 110);
            }
        }

        private void configuraçõesToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
