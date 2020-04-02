using Newtonsoft.Json;
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
using WindowsFormsApp3.Classes;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        List<Funcionario> funcionarios = new List<Funcionario>();
        string caminhoArquivo = @"C:\Users\Aluno\Documents\teste.txt";

        public Form1()
        {
            InitializeComponent();
            btnSalvar.Click += salvarFuncionario;
            btnExcluir.Click += excluir;
            btnAlterar.Click += alterar;
            tblFuncionarios.RowHeaderMouseClick += tblFuncionarios_RowHeaderMouseClick;
            carregarTabela();
        }

        private void ClearData()
        {
            txtNome.Text = "";
            txtMatricula.Text = "";
            txtCargo.Text = "";
            txtAlterarNome.Text = "";
            txtAlterarCargo.Text = "";
            txtAlterarMatricula.Text = "";
        }
        private void tblFuncionarios_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            ClearData();
            txtAlterarNome.Text = tblFuncionarios.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtAlterarCargo.Text = tblFuncionarios.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtAlterarMatricula.Text = tblFuncionarios.Rows[e.RowIndex].Cells[2].Value.ToString();
        }


        private void alterar(object sender, EventArgs e)
        {
            string novoValorNome = txtAlterarNome.Text;
            string novoValorCargo = txtAlterarCargo.Text;
            string novoValorMatricula = txtAlterarMatricula.Text;

             string alterar = Convert.ToString(tblFuncionarios.SelectedRows[0].Cells[2].Value);

            Funcionario alterarFunc = new Funcionario();
            funcionarios.ForEach(f =>
            {
                //MessageBox.Show("primeira");
                if (f.matricula == alterar)
                {
                    f.nome = novoValorNome;
                    f.cargo = novoValorCargo;
                    f.matricula = novoValorMatricula;
                    
                    tblFuncionarios.SelectedRows[0].Cells[0].Value = f.nome;
                    tblFuncionarios.SelectedRows[0].Cells[1].Value = f.cargo;
                    tblFuncionarios.SelectedRows[0].Cells[2].Value = f.matricula;

                    string jason1 = JsonConvert.SerializeObject(funcionarios, Formatting.Indented);
                    File.WriteAllText(caminhoArquivo, jason1);
                    
                }
            });
            carregarTabela();
            ClearData();
        }



        private void excluir(object sender, EventArgs e)
        {
            string excluir = Convert.ToString(tblFuncionarios.SelectedRows[0].Cells[2].Value);
            Funcionario excluirFuncionario = new Funcionario();
            funcionarios.ForEach(f =>
            {
                if (f.matricula == excluir)
                {
                    excluirFuncionario = f;
                }

            });
            funcionarios.Remove(excluirFuncionario);
            string json = JsonConvert.SerializeObject(funcionarios, Formatting.Indented);
            File.WriteAllText(caminhoArquivo, json);
            carregarTabela();
            ClearData();

        }

        private void salvarFuncionario(object sender, EventArgs e)
        {
            Funcionario func = new Funcionario()
            {
                matricula = txtMatricula.Text,
                nome = txtNome.Text,
                cargo = txtCargo.Text
            };

            carregarTabela();

            funcionarios.Add(func);

            string json = JsonConvert.SerializeObject(funcionarios, Formatting.Indented);

            File.WriteAllText(caminhoArquivo, json);

            MessageBox.Show($"Funcionário salvo com sucesso!");
            carregarTabela();
            ClearData();
        }

        public void carregarTabela()
        {
      
            var alunosArquivo = File.ReadAllText(caminhoArquivo);

            funcionarios = JsonConvert.DeserializeObject<List<Funcionario>>(alunosArquivo);

            if (funcionarios == null)
            {
                funcionarios = new List<Funcionario>();
            }

            tblFuncionarios.Rows.Clear();
            funcionarios.ForEach(f =>
            {
                tblFuncionarios.Rows.Add(f.nome, f.cargo, f.matricula);
            });
   
        }








        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void txtExcluir_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
