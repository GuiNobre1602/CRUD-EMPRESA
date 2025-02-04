﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Office.Interop.Excel;

namespace CRUD_EMPRESAS
{
    public partial class Registro : Form
    {

        //Conexão via string
        string path = "Data Source=DESKTOP-DTKBJB1;Initial Catalog=registration;Integrated Security=True";
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter adpt;
        System.Data.DataTable dt;
        int ID;
        
        public Registro()
        {
            InitializeComponent();
            con = new SqlConnection(path);
            display();
            button2.Enabled = false;
            button3.Enabled = false;
        }

        // Botões e Grid que executam as Querrys
        private void button1_Click(object sender, EventArgs e)
        {
            if (txtName.Text=="" || txtFName.Text == "" || txtFunction.Text == "" || txtID.Text == "" || txtEmail.Text == "" || txtID.Text == "" || txtAddress.Text == "")
            {
                MessageBox.Show("Por favor, preencha todos os campos");
            }
            else
            {
                try
                {
                    con.Open();

                    string gender;
                    if (rbtnMale.Checked)
                    {
                        gender = "Masculino";
                    }
                    else
                    {
                        gender = "Feminino";
                    }


                    cmd = new SqlCommand("insert into Employee (Employee_Name, Employee_FName ,Employee_Designation ,Employee_Email ,Emp_ID ,Gender, Addrss) " +
                        "values ('" + txtName.Text + "' , '" + txtFName.Text + "', '" + txtFunction.Text + "' , '" + txtEmail.Text + "', '" + txtID.Text + "', '" + gender + "', '" + txtAddress.Text + "')", con);
                    cmd.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show(" Dados enviados para o banco de dados ");
                    clear();
                    display();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ID = int.Parse(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            txtName.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtFName.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
            txtFunction.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtEmail.Text = dataGridView1.Rows[e.RowIndex].Cells[4].Value.ToString();
            txtID.Text = dataGridView1.Rows[e.RowIndex].Cells[5].Value.ToString();

            rbtnMale.Checked = true;
            rbtnFemale.Checked = false;

            if (dataGridView1.Rows[e.RowIndex].Cells[6].Value.ToString() == "Feminino")
            {
                rbtnMale.Checked = false;
                rbtnFemale.Checked = true;
            }

            txtAddress.Text = dataGridView1.Rows[e.RowIndex].Cells[7].Value.ToString();
            button2.Enabled = true;
            button3.Enabled = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                string gender;
                if (rbtnMale.Checked)
                {
                    gender = "Masculino";
                }
                else
                {
                    gender = "Feminino";
                }

                con.Open();
                cmd = new SqlCommand("update employee set Employee_Name='" + txtName.Text + "', Employee_FName= '" + txtFName.Text + "',  Employee_Designation= '" + txtFunction.Text + "', Employee_Email='" + txtEmail.Text + "', Emp_ID='" + txtID.Text + "', Gender='" + gender + "', Addrss='" + txtAddress.Text + "' where Employee_Id='" + ID + "' ", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show(" Dados atualizados ");
                display();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                con.Open();
                cmd = new SqlCommand("delete from Employee where Employee_Id='"  + ID + "' ", con);
                cmd.ExecuteNonQuery();
                con.Close();
                MessageBox.Show(" Dado deletado da lista");
                display();

            }
            catch(Exception ex)
            {

            }
        }

        private void searchBar_TextChanged(object sender, EventArgs e)
        {
            con.Open();
            adpt = new SqlDataAdapter("select * from employee where Employee_Name like '%" + searchBar.Text + "%' ", con);
            dt = new System.Data.DataTable();
            adpt.Fill(dt);
            dataGridView1.DataSource = dt;
            con.Close();
        }

        private void button4_Click(object sender, EventArgs e) // Excell Export 
        {
            try
            {
                Microsoft.Office.Interop.Excel.Application Excell = new Microsoft.Office.Interop.Excel.Application();
                Workbook wb = Excell.Workbooks.Add(XlSheetType.xlWorksheet);
                Worksheet ws = (Worksheet)Excell.ActiveSheet;
                Excell.Visible = true;

                for (int j = 2; j <= dataGridView1.Rows.Count; j++)
                {
                    for (int i = 1; i <= 1; i++)
                    {
                        ws.Cells[j, i] = dataGridView1.Rows[j - 2].Cells[i - 1].Value;
                    }
                }

                for (int i = 1; i < dataGridView1.Columns.Count + 1; i++)
                {
                    ws.Cells[1, i] = dataGridView1.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < dataGridView1.Columns.Count - 1; i++)
                {
                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        ws.Cells[i + 2, j + 1] = dataGridView1.Rows[i].Cells[j].Value.ToString();
                    }
                }
            }
            catch (Exception)
            { 

            }
        }


        //Metodos criados pelo usario
        public void clear()
        {
            txtFName.Text = "";
            txtFunction.Text = "";
            txtEmail.Text = "";
            txtAddress.Text = "";
            txtID.Text = "";
            txtName.Text = "";

        }

        public void display()
        {
            try
            {
                dt = new System.Data.DataTable();
                con.Open();
                adpt = new SqlDataAdapter("select * from employee", con);
                adpt.Fill(dt);
                dataGridView1.DataSource = dt;
                con.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }





        //metodos não utilizados criados automaticos pelo Windows Form
        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged_1(object sender, EventArgs e)
        {

        }



        private void Registro_Load(object sender, EventArgs e)
        {

        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
    }
}
