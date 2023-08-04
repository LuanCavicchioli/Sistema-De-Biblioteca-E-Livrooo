using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SistemaBiblioteca
{
    public partial class FrmCadastroLivro : Form
    {
        public string conexaoString;
        private SqlConnection conexaoDB;
        DataGridViewRow LinhaSelecionada;
        public FrmCadastroLivro()
        {
            InitializeComponent();

            //String de conexão

            conexaoString = "Data Source=MAR0625646W10-1;Initial Catalog=Biblioteca;Integrated Security=True";

            //Inicializando a conexão com o Banco de dados
            conexaoDB = new SqlConnection(conexaoString);
        }
        public void carregarDadosLivros()
        {
            try
            {
                conexaoDB.Open();
                string sql = "select * from livros";

                SqlDataAdapter adapter = new SqlDataAdapter(sql, conexaoDB);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataTable.Columns["numero_de_pagina"].ColumnName = "N° Pagina";
                dataTable.Columns["preco"].ColumnName = "Preço";
                dataTable.Columns["ano_de_publicacao"].ColumnName = "Ano Publicação";
                dataTable.Columns["isbn"].ColumnName = "ISBN";

                dgvLivro.DataSource = dataTable;

                conexaoDB.Close();
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao carregar os dados: " + ex);
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if (txtISBN.Text == "" || txtAnoPublicacao.Text == "" || txtTitulo.Text == "" || txtAutor.Text == "" || txtPreco.Text == "" || txtNumeroPagina.Text == "")
            {
                MessageBox.Show("PREENCHA TODAS AS COLUNAS", "AVISO", MessageBoxButtons.OKCancel , MessageBoxIcon.Warning);
            }
            else
            {

                string sql = "INSERT INTO Livros(titulo,autor,numero_de_pagina,preco,ano_de_publicacao,isbn) VALUES (@titulo, @autor,@numero_de_pagina,@preco,@ano_de_publicacao,@isbn)";
                try
                {
                    SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);

                    sqlCmd.Parameters.AddWithValue("@titulo", txtTitulo.Text);
                    sqlCmd.Parameters.AddWithValue("@autor", txtAutor.Text);
                    sqlCmd.Parameters.AddWithValue("@numero_de_pagina", Convert.ToInt32(txtNumeroPagina.Text));
                    sqlCmd.Parameters.AddWithValue("@preco", txtPreco.Text);
                    sqlCmd.Parameters.AddWithValue("@ano_de_publicacao", Convert.ToInt32(txtAnoPublicacao.Text));
                    sqlCmd.Parameters.AddWithValue("@isbn", txtISBN.Text);

                    conexaoDB.Open();
                    sqlCmd.ExecuteNonQuery();
                    MessageBox.Show("Cadastro realizado com sucesso!");

                    conexaoDB.Close();
                    carregarDadosLivros();
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Erro ao Inserir os Dados: " + ex);
                }
            }
        }

        private void FrmCadastroLivro_Load(object sender, EventArgs e)
        {
            carregarDadosLivros();
        }

        private void dgvLivro_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.ColumnIndex >= 0)
            {
                LinhaSelecionada = dgvLivro.Rows[e.RowIndex];

                txtISBN.Text = LinhaSelecionada.Cells["isbn"].Value.ToString();

            }
        }
    }
}
