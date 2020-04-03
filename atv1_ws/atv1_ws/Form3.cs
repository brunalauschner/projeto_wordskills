using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atv1_ws
{
    public partial class Form3 : Form
    {
        projeto_wsEntities bd = new projeto_wsEntities();
        usuario logada = new usuario();
        usuario usSenha = new usuario();
        usuario pesquisado = new usuario();
        public Form3()
        {
            InitializeComponent();

            label1.Visible = false;
            logada = Form1.logado;
            usSenha.senha = Form1.logado2.senha;
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

            btnSalvar.Click += salvarAlteracao;
            button1.Click += addFoto;

            monthCalendar1.Visible = false;
            pictureBox4.Click += aparecerCalendario;

            txtSenha.UseSystemPasswordChar = true;
            txtConfirmacao.UseSystemPasswordChar = true;
            pictureBox5.Click += aparecerSenha;

            txtConfirmacao.Visible = true;
            label26.Visible = true;
            label27.Visible = true;
            txtSenha.LostFocus += saiu;
            

            txtData.TextChanged += txtData_TextChanged;
            txtTelefone.TextChanged += txtTelefone_TextChanged;
            txtCEP.TextChanged += txtCEP_TextChanged;
            comboBox1.TextChanged += comboBox1_TextChanged;
            comboBox2.TextChanged += comboBox2_TextChanged;
            comboBox3.TextChanged += comboBox3_TextChanged;
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
            // pesquisa por nome

            comboBox3.Items.Clear();

            estados tes = new estados();
            bd.estados.ToList().ForEach(p =>
            {
                if (p.nome == comboBox2.Text)
                {
                    tes = p;
                }
            });

            bd.cidade.ToList().ForEach(s =>
            {
                if (s.id_estadoss == tes.id)
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
            if (imgErroNome.Visible == false && imgErroData.Visible == false && imgErroTelefone.Visible == false &&
                imgErroEmail.Visible == false && imgErroSenha.Visible == false && imgErroCEP.Visible == false &&
                imgErroPais.Visible == false && imgErroEstado.Visible == false && imgErroCidade.Visible == false &&
                imgErroBairro.Visible == false && imgErroEndereco.Visible == false) //todos os campos foram preenchidos corretamente
            {
                if (txtConfirmacao.Text == txtSenha.Text)
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

                    string passwords = encryption(novoValorSenha);

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
                            f.senha = passwords;
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

                    label1.Visible = true;
                    label1.Text = "Perfil salvo com sucesso!";
                }
                else
                {
                    label1.Visible = true;
                    label1.Text = "Confira o campo Confirmação de Senha";
                }
            }
            else
            {
                label1.Visible = true;
                label1.Text = "Preencha os campos obrigatórios!";
            }
        }

        public string encryption(String password)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] encrypt;
            UTF8Encoding encode = new UTF8Encoding();
            //encrypt the given password string into Encrypted data  
            encrypt = md5.ComputeHash(encode.GetBytes(password));
            StringBuilder encryptdata = new StringBuilder();
            //Create a new string by using the encrypted data  
            for (int i = 0; i < encrypt.Length; i++)
            {
                encryptdata.Append(encrypt[i].ToString());
            }
            return encryptdata.ToString();
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
            txtSenha.Text = usSenha.senha;
            txtCEP.Text = logada.cep;
            comboBox1.Text = aaa.nome;
            comboBox2.Text = bbb.nome;
            comboBox3.Text = ccc.nome;
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

          
        private void Form3_Load(object sender, EventArgs e)
        {
        }

        private void textBox7_TextChanged(object sender, EventArgs e) //bairro
        {
            int nChar = txtBairro.Text.Length;
            if (!string.IsNullOrEmpty(txtBairro.Text) && nChar >= 2)
            {
                imgCertoBairro.Visible = true;
                imgErroBairro.Visible = false;
            }
            else
            {
                imgCertoBairro.Visible = false;
                imgErroBairro.Visible = true;
            }
        }

        private void txtNome_TextChanged(object sender, EventArgs e)
        {
            int nChar = txtNome.Text.Length;
            if (!string.IsNullOrEmpty(txtNome.Text) && nChar >= 2)
            {
                imgCertoNome.Visible = true;
                imgErroNome.Visible = false;
            }
            else
            {
                imgCertoNome.Visible = false;
                imgErroNome.Visible = true;
            }
        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {
            Regex validar = new Regex("^([a-zA_]+)([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}" +
                "\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\" + ".)+))([a-zA-Z]{2,4}|[0,9]{1,3})(\\]?)$");

            if (validar.IsMatch(txtEmail.Text))
            {
                int nChar = txtEmail.Text.Length;
                if (!string.IsNullOrEmpty(txtEmail.Text) && nChar >= 2)
                {
                    imgCertoEmail.Visible = true;
                    imgErroEmail.Visible = false;
                }
                else
                {
                    imgCertoEmail.Visible = false;
                    imgErroEmail.Visible = true;
                }
            }
            else
            {
                imgCertoEmail.Visible = false;
                imgErroEmail.Visible = true;
            }
        }

        private void txtSenha_TextChanged(object sender, EventArgs e)
        {
            int nChar = txtSenha.Text.Length;
            if (!string.IsNullOrEmpty(txtSenha.Text) && nChar >= 2)
            {
                imgCertoSenha.Visible = true;
                imgErroSenha.Visible = false;
            }
            else
            {
                imgCertoSenha.Visible = false;
                imgErroSenha.Visible = true;
            }
        }

        private void txtConfirmacao_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtEndereco_TextChanged(object sender, EventArgs e)
        {
            int nChar = txtEndereco.Text.Length;
            if (!string.IsNullOrEmpty(txtEndereco.Text) && nChar >= 2)
            {
                imgCertoEndereco.Visible = true;
                imgErroEndereco.Visible = false;
            }
            else
            {
                imgCertoEndereco.Visible = false;
                imgErroEndereco.Visible = true;
            }
        }

        private void txtData_TextChanged(object sender, EventArgs e)
        {
            int nChar = txtData.Text.Length;
            if (!string.IsNullOrEmpty(txtData.Text) && nChar == 10)
            {
                imgCertoData.Visible = true;
                imgErroData.Visible = false;
            }
            else
            {
                imgCertoData.Visible = false;
                imgErroData.Visible = true;
            }
        }

        private void txtTelefone_TextChanged(object sender, EventArgs e)
        {
            int nChar = txtTelefone.Text.Length;
            if (!string.IsNullOrEmpty(txtTelefone.Text) && nChar == 15)
            {
                imgCertoTelefone.Visible = true;
                imgErroTelefone.Visible = false;
            }
            else
            {
                imgCertoTelefone.Visible = false;
                imgErroTelefone.Visible = true;
            }
        }

        private void txtCEP_TextChanged(object sender, EventArgs e)
        {
            int nChar = txtCEP.Text.Length;
            if (!string.IsNullOrEmpty(txtCEP.Text) && nChar == 9)
            {
                imgCertoCEP.Visible = true;
                imgErroCEP.Visible = false;
            }
            else
            {
                imgCertoCEP.Visible = false;
                imgErroCEP.Visible = true;
            }
        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox1.Text))
            {
                imgCertoPais.Visible = true;
                imgErroPais.Visible = false;
            }
            else
            {
                imgCertoPais.Visible = false;
                imgErroPais.Visible = true;
            }
        }

        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox2.Text))
            {
                imgCertoEstado.Visible = true;
                imgErroEstado.Visible = false;
            }
            else
            {
                imgCertoEstado.Visible = false;
                imgErroEstado.Visible = true;
            }
        }

        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(comboBox3.Text))
            {
                imgCertoCidade.Visible = true;
                imgErroCidade.Visible = false;
            }
            else
            {
                imgCertoCidade.Visible = false;
                imgErroCidade.Visible = true;
            }
        }


        private void txtData_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }
    }
}
