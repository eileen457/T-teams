using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using todos.Clases;

namespace todos
{
    public partial class Form2 : Form
    {
        private Conexion mConexion;
        public Form2()
        {
            InitializeComponent();
            mConexion = new Conexion();
            btnActualizar.Enabled = false;
            btnEliminar.Enabled = false;
        }

       
        //verificar si un string contiene algún número
        private bool ContainsNum(string text)
        {
            return text.Any(char.IsDigit); //revisa en toda la cadena de texto si hay algun numero
        }                                  //si hay numero devuelve true

        //verifica si el registro ya existe antes de guardar
        private bool RegistroEsta(MySqlConnection connection, string codigo) //private no todos lo pueden usar     conectado a la base de datos busca en fila codigo
        {
            string query = "SELECT COUNT(*) FROM productos WHERE codigo='" + codigo + "'";
            MySqlCommand cmd = new MySqlCommand(query, connection);  //objeto utiliza conexion y comando en bd
            int count = Convert.ToInt32(cmd.ExecuteScalar()); //execute devuelve en numeros lo que el objeto cmd encuentre
            return count > 0; //si es uno entonces ya existe si es cero no existe
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {

            String id = txtId.Text;

            String nombre = txtNombre.Text;
            String descripcion = txtDescripcion.Text;
            String categoria = txtCategoria.Text;

            double precio;
            int existencia;

            int codigo;

            if (string.IsNullOrEmpty(txtCodigo.Text.ToString()))
            {
                MessageBox.Show("Por favor, ingrese un codigo");
                return;
            }

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(descripcion) || string.IsNullOrEmpty(categoria) || string.IsNullOrEmpty(txtPrecio.Text) || string.IsNullOrEmpty(txtExistencias.Text))
            {
                MessageBox.Show("Ups! Al parecer tienes recuadros vacio por llenar");
                return;
            }

            //si en precio hay letras     los decimales se ponen con puntos
            try
            {
                precio = double.Parse(txtPrecio.Text);
                if (double.Parse(txtPrecio.Text) > 3000)
                {
                    MessageBox.Show("Error: El precio debe ser menor que $3,000");
                    return;
                }

            }
            catch (FormatException)  //exepcion cuando la cadena de entrada no es valido
            {
                MessageBox.Show("Error: El precio debe ser un número");
                return;
            }


            //si en existencia hay letras 
            try
            {
                existencia = int.Parse(txtExistencias.Text);

                if (int.Parse(txtExistencias.Text) > 50)
                {
                    MessageBox.Show("Error: Tiene que haber menos de 50 existencias");
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: La existencia debe ser un número entero");
                return;
            }

            //si en codigo hay letras marca error
            try
            {
                codigo = int.Parse(txtCodigo.Text);

            }
            catch (FormatException)
            {
                MessageBox.Show("Error: El codigo tiene que ser un número entero");
                return;
            }


            //comando para trabajar dentro de la bd
            string sql = "INSERT INTO productos (codigo, nombre, descripcion, precio, existencias, categoria) VALUES ('" + codigo + "','" + nombre + "','" + descripcion + "', '" + precio + "', '" + existencia + "', '" + categoria + "')";


            // Conexion a la bd
            MySqlConnection connection = mConexion.getConexionT();


            connection.Open();


            try
            {

                if ( ContainsNum(nombre) || ContainsNum(descripcion) || ContainsNum(categoria))
                {
                    MessageBox.Show("Ups! Al parecer tiene un error de valores en nombre, descripcion o categoria");

                }
                else if ( nombre.Length > 57 || descripcion.Length > 57 || categoria.Length > 24)
                {

                    MessageBox.Show("Ups! Al parecer los valores en nombre, descripcion o categoria son mayores a lo aceptado");

                }
                else if (RegistroEsta(connection, codigo.ToString()))
                {
                    MessageBox.Show("Ya existe un registro con el mismo código");
                    limpiar();
                    return;

                }
                else
                {
                    MySqlCommand comando = new MySqlCommand(sql, connection);
                    comando.ExecuteNonQuery();
                    MessageBox.Show("Registro guardado");
                    limpiar();

                }


            }
            catch (MySqlException ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }


        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {

            String id = txtId.Text;
          //  String codigo = txtCodigo.Text;
            String nombre = txtNombre.Text;
            String descripcion = txtDescripcion.Text;
            String categoria = txtCategoria.Text;

            double precio;
            int existencia;
            int codigo;

            if (string.IsNullOrEmpty(txtCodigo.Text.ToString()))
            {
                MessageBox.Show("Por favor, ingrese un codigo");
                return;
            }

            if (string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(descripcion) || string.IsNullOrEmpty(categoria) || string.IsNullOrEmpty(txtPrecio.Text) || string.IsNullOrEmpty(txtExistencias.Text))
            {
                MessageBox.Show("Ups! Al parecer tiene recuadros vacio por llenar");
                return;
            }


            //si en precio hay letras marca error
            try
            {
                precio = double.Parse(txtPrecio.Text);

                if (double.Parse(txtPrecio.Text) > 3000)
                {
                    MessageBox.Show("Error: El precio debe ser menor que $3000");
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: El precio tiene que ser un número");
                return;
            }


            //si en existencia hay letras marca error
            try
            {
                existencia = int.Parse(txtExistencias.Text);

                if (int.Parse(txtExistencias.Text) > 50)
                {
                    MessageBox.Show("Error: La existencia debe ser un número menor que 50 existencias");
                    return;
                }
            }
            catch (FormatException)
            {
                MessageBox.Show("Error: La existencia tiene que ser un número entero");
                return;
            }

            //si en codigo hay letras marca error
            try
            {
                codigo = int.Parse(txtCodigo.Text);

            }
            catch (FormatException)
            {
                MessageBox.Show("Error: El codigo tiene que ser un número entero");
                return;
            }


            //consulta a la base de datos para actualizar al registro
            string sql =
            "UPDATE productos SET codigo='" + codigo + "', nombre='" + nombre + "', descripcion='" + descripcion + "', precio='" + precio + "', existencias='" + existencia + "', categoria='" + categoria + "' WHERE id='" + id + "' ";


            MySqlConnection connection = mConexion.getConexionT();
         //   MySqlCommand comando = new MySqlCommand(sql, connection);  //para poder ejecutar comandos en bd

            connection.Open();


            try
            {

                //verifica si existe el registro antes de actualizarlo
               // int datonuevo = comando.ExecuteNonQuery();

                //verifica que los campos sean correctos antes de actualizarlo
                if (ContainsNum(nombre) || ContainsNum(descripcion) || ContainsNum(categoria))
                {
                    MessageBox.Show("Ups! Al parecer tiene un error de valores de nombre, descripcion o categoria");
                    //si hay un numero en la cadena, sale este mensaje
                }
                else if( nombre.Length > 57 || descripcion.Length > 57 || categoria.Length > 24)
                {
                    MessageBox.Show("Ups! Al parecer los valores en codigo, nombre, descripcion o categoria son mayores a lo aceptado");
                    //si se pasan de ese largo, sale un mensaje de advertencia
                }


              //   if (datonuevo > 0)  //si hay aunque sea un dato diferente entonces sale el mensaje
               // {
                    DialogResult opcion1 = MessageBox.Show("¿Desea actualizar este registro?", "Advertencia de actualizacion", MessageBoxButtons.YesNo);
                    if (opcion1 == DialogResult.Yes)
                    {

                    MySqlCommand comando = new MySqlCommand(sql, connection);
                    comando.ExecuteNonQuery();
                        MessageBox.Show("Actualizacion exitosa");
                        limpiar();
                    btnActualizar.Enabled = false;
                    btnEliminar.Enabled = false;
                }
                    else if (opcion1 == DialogResult.No)
                    {
                        MessageBox.Show("Actualizacion cancelada");
                    }
               


                /*     }
                     else
                     {
                         MessageBox.Show("No existe este registro para actualizarlo");   //si el registro no se hizo antes entonces no existe
                     }*/


            }
            catch (MySqlException ex) 
            {

                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
           
            String codigo = txtCodigo.Text;


            //no permite borrar hasta que ingrese un id existente los campos ingresados manualmente sean correctos
            if (string.IsNullOrEmpty(codigo))
            {
                MessageBox.Show("Por favor, ingrese un codigo");
                return;
            }

            //comando para trabajar dentro de la bd
            string sql = "DELETE FROM productos WHERE codigo ='" + codigo + "'";

            MySqlConnection connection = mConexion.getConexionT();  //conexion a la bd


            connection.Open();

            //si no hay errores de caracter, elimina registro
            try
            {

              DialogResult opcion=  MessageBox.Show("¿Desea eliminar este registro?","Advertencia de eliminar",MessageBoxButtons.YesNo);
                if (opcion == DialogResult.Yes)
                {
                    MySqlCommand comando = new MySqlCommand(sql, connection);
                    comando.ExecuteNonQuery(); //devuelve el numero de filas afectadas
                    MessageBox.Show("Registro eliminado");
                    limpiar();
                    btnEliminar.Enabled = false;
                    btnActualizar.Enabled = false;
                }
                else if (opcion == DialogResult.No)
                {
                    MessageBox.Show("Eliminacion cancelada");
                }



            }
            catch (MySqlException ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {

            if (txtCodigo.Text.Length == 0 &&   //si ya se encuentran vacios entonces manda mensaje de lo contrario vacia los textbox
              txtNombre.Text.Length == 0 &&
              txtDescripcion.Text.Length == 0 &&
              txtPrecio.Text.Length == 0 &&
              txtExistencias.Text.Length == 0 &&
              txtCategoria.Text.Length == 0 &&
              txtId.Text.Length == 0
              )
            {
                MessageBox.Show("Los campos ya estan vacios");
            }
            else
            {

                limpiar();
            }
        }
        private void limpiar()
        {
            txtId.Text = "";
            txtCodigo.Text = "";
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            txtPrecio.Text = "";
            txtExistencias.Text = "";
            txtCategoria.Text = "";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            String codigo = txtCodigo.Text;
            MySqlDataReader reader = null;  //objeto lector de mysqldatareader para bd

            if (string.IsNullOrEmpty(codigo))
            {
                MessageBox.Show("Por favor, ingrese un codigo");
                return;
            }

            //comando para la bd
            string sql = "SELECT id, codigo, nombre, descripcion, precio, existencias, categoria FROM productos WHERE codigo LIKE '" + codigo + "' LIMIT 1";
            MySqlConnection connection = mConexion.getConexionT();

            connection.Open();

            try
            {
                
                MySqlCommand comando = new MySqlCommand(sql, connection);
                reader = comando.ExecuteReader();
                if (reader.HasRows)    //si hay filas en la bd entonces el lector lee el seleccionado
                {
                    while (reader.Read())
                    {
                        btnActualizar.Enabled = true;
                        btnEliminar.Enabled = true;
                        txtId.Text = reader.GetInt32(0).ToString(); //obtiene el valor de numero entero de la bd y lo convierte de texto
                        txtCodigo.Text = reader.GetInt32(1).ToString();
                        txtNombre.Text = reader.GetString(2);
                        txtDescripcion.Text = reader.GetString(3);
                        txtPrecio.Text = reader.GetDouble(4).ToString();  //obtiene el valor como entero con decimales
                        txtExistencias.Text = reader.GetInt32(5).ToString();
                        txtCategoria.Text = reader.GetString(6);
                    }
                }
                else
                {
                    MessageBox.Show("No se encontro este registro");
                    limpiar();
                }

            }
            catch (MySqlException ex)
            {

                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                reader.Close();
                connection.Close();
            }
        }

    }

}
