using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using todos.Clases;  //para llamar de la clase conexion


namespace todos
{
    
    public partial class Form1 : Form
    {
        
        private Conexion mConexion; //objeto de clase conexion
        public Form1()
        {
            InitializeComponent();
            mConexion = new Conexion();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //declarar variables tipo string
            String usuario = txtUsuario.Text;
            String contraseña = txtContraseña.Text;
            MySqlDataReader reader = null;
        
            //comando para bd
            string sql = "SELECT * FROM usuario where nombre='" + usuario + "'and contraseña= '" + contraseña + "'";

            MySqlConnection connection = mConexion.getConexionP();   //conexion a la bd

            connection.Open();

            try
            {
                MySqlCommand comando = new MySqlCommand(sql, connection);   //objeto con comando y conexion
                comando.ExecuteNonQuery();

                comando.Parameters.AddWithValue("@usuario", usuario); //espacio listo para introducir lo que se escriba en la variable usuario
                comando.Parameters.AddWithValue("@contraseña", contraseña);  //se compara con los datos de la bd y si no marca error

                reader = comando.ExecuteReader(); 

                if (reader.HasRows)  
                {
                    while (reader.Read())
                    {
                        txtId.Text = reader.GetInt32(0).ToString();
                        txtUsuario.Text = reader.GetString(1);
                        txtContraseña.Text = reader.GetString(2);
                  

                        if(usuario=="Salvador" && contraseña=="pass"){
                            MessageBox.Show("Bienvenido " + reader.GetValue(3) + " " + reader.GetValue(1));  //obtiene valor de columna 3 y 1 en la bd

                            Form2 form3 = new Form2();  //llamar el siguiente form
                            form3.Show();  //mostrar siguiente interfaz
                            this.Hide();   //se esconde una interfaz y aparece otra

                        }else if(usuario == "Eduardo" && contraseña == "caja")
                        {
                            MessageBox.Show("Bienvenido " + reader.GetValue(3) + " " + reader.GetValue(1));

                            Form4 form4 = new Form4();
                            form4.Show();
                            this.Hide();

                        }
                    }
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos, verifique de nuevo");
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Error"+ex.Message);
            }
            finally
            {
                reader.Close();   //cerrar lector
                connection.Close();  //cerrar conexion
            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
