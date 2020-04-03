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
    public partial class Form1 : Form
    {
        projeto_wsEntities bd = new projeto_wsEntities();
        public static usuario logado = new usuario();
        public static usuario logado1 = new usuario();
        public static usuario logado2 = new usuario();
        public Form1()
        {
            InitializeComponent();
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            btnEntrar.Enabled = false;
            btnEntrar.Click += entrar;
            textBox1.LostFocus += saiu1;
            textBox2.LostFocus += saiu2;
            
        }
        

        private void saiu2(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox2.Text))
            {
                label5.Visible = true;
            }
            else
            {
                label5.Visible = false;
            }
        }

        private void saiu1(object sender, EventArgs e)
        {
            Regex validar = new Regex("^([a-zA_]+)([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}" +
                "\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\" + ".)+))([a-zA-Z]{2,4}|[0,9]{1,3})(\\]?)$");

            string passwords = encryption(textBox2.Text);

            logado = bd.usuario.Where(u => u.email.Equals(textBox1.Text) && u.senha.Equals(passwords)).FirstOrDefault();
            logado1 = bd.usuario.Where(v => v.email.Equals(textBox1.Text)).FirstOrDefault();

            if (string.IsNullOrEmpty(textBox1.Text))
            {
                label4.Visible = true;
                btnEntrar.Enabled = false;
            }
            else
            {
                if (validar.IsMatch(textBox1.Text))
                {
                    label4.Visible = false;
                    btnEntrar.Enabled = true;
                    if (logado1 != null)
                    {
                        if (logado1.foto != null)
                        {
                            usuario obj;
                            obj = bd.usuario.FirstOrDefault(a => a.id == logado1.id);
                            if (obj == null) return;
                            var stream = new MemoryStream(obj.foto);
                            roundShapePB1.Image = Image.FromStream(stream);
                        }
                    }
                }
                else
                {
                    label4.Visible = true;
                    btnEntrar.Enabled = false;
                }
            }

        }

        private void entrar(object sender, EventArgs e)
        {
            logado = null;
            logado1 = null;
            string passwords = encryption(textBox2.Text);
            logado = bd.usuario.Where(u => u.email.Equals(textBox1.Text) && u.senha.Equals(passwords)).FirstOrDefault();
            logado1 = bd.usuario.Where(v => v.email.Equals(textBox1.Text)).FirstOrDefault();

            logado2.senha = textBox2.Text;

            if (logado != null)
            {
                new Form2().Show();
                this.Hide();
            }
            if (logado1 == null)
            {
                label6.Visible = true;
                label6.Text = "Atenção: Representante não cadastrado";
            }
            else if (logado1.senha != passwords)
            {
                label6.Visible = true;
                label6.Text = "Atenção: Senha incorreta";
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //Image img = roundShapePB1.Image;
        //byte[] arr;
        //ImageConverter converter = new ImageConverter();
        //arr = (byte[])converter.ConvertTo(img, typeof(byte[]));

        //usuario novo = new usuario()
        //{
        //    email = textBox1.Text,
        //    senha = textBox2.Text,
        //    nome = textBox3.Text,
        //    foto = arr
        //};

        //bd.usuario.Add(novo);
        //bd.SaveChanges();

        //OpenFileDialog opnfd = new OpenFileDialog();
        //opnfd.Filter = "Image Files (*.jpg;*.jpeg;.*.gif;)|*.jpg;*.jpeg;.*.gif";
        //    if (opnfd.ShowDialog() == DialogResult.OK)
        //    {
        //        roundShapePB1.Image = new Bitmap(opnfd.FileName);
        //    }
}
}
