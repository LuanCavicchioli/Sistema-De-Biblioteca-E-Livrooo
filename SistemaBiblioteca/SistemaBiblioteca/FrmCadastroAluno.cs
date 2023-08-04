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
    public partial class FrmCadastroAluno : Form
    {
        public string conexaoString;
        private SqlConnection conexaoDB;
        DataGridViewRow registroSelecionado;

        public FrmCadastroAluno()
        {
            InitializeComponent();

            //String de conexão
            
            conexaoString = "Data Source=MAR0625646W10-1;Initial Catalog=Biblioteca;Integrated Security=True";
            
            //Inicializando a conexão com o Banco de dados
            conexaoDB = new SqlConnection(conexaoString);
        }

   

        private void FrmCadastroAluno_Load(object sender, EventArgs e)
        {
            
            carregarDadosAlunos();
            btnExcluir.Enabled = false;
            btnAtualizar.Enabled = false;

            txtNome.TabIndex = 0;
            txtCPF.TabIndex = 1;
            txtEmail.TabIndex = 2;
            txtDataNascimento.TabIndex = 3;
            txtTelefone.TabIndex = 4;
            txtCelular.TabIndex = 5;    
            btnAdicionar.TabIndex = 6;
            btnAtualizar.TabIndex = 7;
            btnExcluir.TabIndex = 8;    
            txtPesquisar.TabIndex = 9;
            btnPesquisar.TabIndex =10;
            dgvAlunos.TabIndex = 11;
        }

        private void carregarDadosAlunos(int id = 0)
        {
            try
            {
                conexaoDB.Open();

                string sql;

                if (id == 0)
                {
                    sql = "SELECT * FROM alunos";
                }
                else
                {
                    sql = "SELECT * FROM alunos WHERE id="+id;
                }
               

                SqlDataAdapter adpater = new SqlDataAdapter(sql, conexaoDB);
                DataTable dataTable = new DataTable();
                adpater.Fill(dataTable);

                dataTable.Columns["id"].ColumnName = "ID";
                dataTable.Columns["nome"].ColumnName = "Nome";
                dataTable.Columns["cpf"].ColumnName = "CPF";
                dataTable.Columns["telefone"].ColumnName = "Telefone";
                dataTable.Columns["celular"].ColumnName = "Celular";
                dataTable.Columns["data_nascimento"].ColumnName = "Data de Nascimento";

                dgvAlunos.DataSource = dataTable;

                conexaoDB.Close();


            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao carregar os dados: " + ex);
            }
        }

        private void btnAdicionar_Click(object sender, EventArgs e)
        {
            if(txtNome.Text == ""||txtCPF.Text == ""||txtEmail.Text ==""||txtTelefone.Text==""||txtCelular.Text=="")
            try
            {
                string sql = "INSERT INTO alunos (nome, cpf, email, telefone, celular, data_nascimento) VALUES (@nome, @cpf, @email, @telefone, @celular, @data_nascimento)";
                
                conexaoDB.Open();

                SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);
                               
                sqlCmd.Parameters.AddWithValue("@nome", txtNome.Text);
                sqlCmd.Parameters.AddWithValue("@cpf", txtCPF.Text);
                sqlCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                sqlCmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                sqlCmd.Parameters.AddWithValue("@celular", txtCelular.Text);

                DateTime dataNascimento;
                DateTime.TryParse(txtDataNascimento.Text, out dataNascimento);

                sqlCmd.Parameters.AddWithValue("@data_nascimento", dataNascimento);

                sqlCmd.ExecuteNonQuery();

                MessageBox.Show("Cadastro Realizado com Sucesso!!!");

                conexaoDB.Close();

                carregarDadosAlunos();

            }catch (SqlException ex) {
                MessageBox.Show("Erro ao Inserir os Dados: " + ex);
            }


        }

        private void dgvAlunos_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >=0)
            {
                registroSelecionado = dgvAlunos.Rows[e.RowIndex];

                txtNome.Text = registroSelecionado.Cells["nome"].Value.ToString();
                txtCPF.Text = registroSelecionado.Cells["cpf"].Value.ToString();
                txtEmail.Text = registroSelecionado.Cells["email"].Value.ToString();
                txtDataNascimento.Text = registroSelecionado.Cells["Data de Nascimento"].Value.ToString();
                txtTelefone.Text = registroSelecionado.Cells["Telefone"].Value.ToString();
                txtCelular.Text = registroSelecionado.Cells["celular"].Value.ToString();

                btnAdicionar.Enabled = false;
                btnAtualizar.Enabled = true;
                btnExcluir.Enabled = true;
            }

          
        }

        private void btnAtualizar_Click(object sender, EventArgs e)
        {
            try
            {
                int id = Convert.ToInt32(registroSelecionado.Cells["id"].Value.ToString());

                string sql = "UPDATE alunos SET " +
                    "nome=@nome, " +
                    "cpf=@cpf, " +
                    "email=@email, " +
                    "telefone=@telefone, " +
                    "celular=@celular, " +
                    "data_nascimento=@data_nascimento " +
                    "WHERE id=@id";

                conexaoDB.Open();

                SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);

                sqlCmd.Parameters.AddWithValue("@id", id);
                sqlCmd.Parameters.AddWithValue("@nome", txtNome.Text);
                sqlCmd.Parameters.AddWithValue("@cpf", txtCPF.Text);
                sqlCmd.Parameters.AddWithValue("@email", txtEmail.Text);
                sqlCmd.Parameters.AddWithValue("@telefone", txtTelefone.Text);
                sqlCmd.Parameters.AddWithValue("@celular", txtCelular.Text);

                DateTime dataNascimento;
                DateTime.TryParse(txtDataNascimento.Text, out dataNascimento);

                sqlCmd.Parameters.AddWithValue("@data_nascimento", dataNascimento);

                sqlCmd.ExecuteNonQuery();

                MessageBox.Show("Atualização Realizada com Sucesso!!!");

                conexaoDB.Close();

                carregarDadosAlunos();

            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erro ao Atualizar os Dados: " + ex);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            if(registroSelecionado != null)
            {
                DialogResult resultado = MessageBox.Show("Tem certeza que deseja exlcuir o aluno","Atenção",MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if(resultado == DialogResult.Yes)
                {

                    try
                    {
                        int id = Convert.ToInt32(registroSelecionado.Cells["id"].Value.ToString());

                        string sql = "DELETE FROM alunos " +
                                     "WHERE id=@id";

                        conexaoDB.Open();

                        SqlCommand sqlCmd = new SqlCommand(sql, conexaoDB);

                        sqlCmd.Parameters.AddWithValue("@id", id);
                        sqlCmd.ExecuteNonQuery();

                        MessageBox.Show("Aluno excluido com Sucesso!!!");

                        conexaoDB.Close();

                        carregarDadosAlunos();

                    }
                    catch (SqlException ex)
                    {
                        MessageBox.Show("Erro ao Excluir os Dados: " + ex);
                    }

                }
            }
        }

        private void btnPesquisar_Click(object sender, EventArgs e)
        {
            int id;
            if(int.TryParse(txtPesquisar.Text, out id))
            {
                carregarDadosAlunos(id);
            }
            else
            {
                MessageBox.Show("Código do aluno inválido");
            }
        }

        private void txtPesquisar_KeyUp(object sender, KeyEventArgs e
        {
            carregarDadosAlunos();
        }

       
    }
}
