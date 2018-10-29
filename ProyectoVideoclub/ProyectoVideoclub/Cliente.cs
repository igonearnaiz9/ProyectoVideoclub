using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ProyectoVideoclub
{
    class Cliente
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVIDEOCLUB_CHUCK"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;

        private string dni, nombre, apellido, email, password;
        private DateTime fechaNac;
        private int idCliente;


        //Constructores
        public Cliente()
        {

        }
        public Cliente(int idCliente, string nombre, string apellido, string email, string password, DateTime fechaNac)
        {
            this.idCliente = idCliente;
            this.nombre = nombre;
            this.apellido = apellido;
            this.email = email;
            this.password = password;
            this.fechaNac = fechaNac;
        }
        //Getters y Setters. TODO:
        public string GetNombreCliente()
        {
            return nombre;
        }
        public void SetNombreCliente(string nombre)
        {
            this.nombre = nombre;
        }

        public string GetApellidoCliente()
        {
            return apellido;
        }
        public void SetApellidoCliente(string apellido)
        {
            this.apellido = apellido;
        }

        public string GetEmailCliente()
        {
            return email;
        }
        public void SetEmailCliente(string email)
        {
            this.email = email;
        }


        public string GetPasswordCliente()
        {
            return password;
        }
        public void SetPasswordCliente(string password)
        {
            this.password = password;
        }

        public string GetDNICliente()
        {
            return dni;
        }
        public void SetDNICliente(string dni)
        {
            this.dni = dni;
        }

        public DateTime GetFechaNacCliente()
        {
            return fechaNac;
        }
        public void SetFechaNacCliente(DateTime fechaNac)
        {
            this.fechaNac = fechaNac;
        }
        // Recibo una direccion de correo y compruebo si existe en la tabla de clientes      
        // entra Direccion de correo a comprobar 
        // devuelve el ID_CLIENTE si existe, si no devuelve 0
        public int CheckLogin(string emailLogin)
        {
            cadena = "SELECT ID_CLIENTE FROM CLIENTES WHERE EMAIL ='" + emailLogin + "'";
            try
            {
                conexion.Open();
                comando = new SqlCommand(cadena, conexion);
                SqlDataReader clienteExistente = comando.ExecuteReader();
                if (clienteExistente.Read())
                {
                    return Convert.ToInt32(clienteExistente["ID_CLIENTE"]);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
            finally
            {
                conexion.Close();
            }
        }
        //Quiero una función que me muestre el nombre del cliente logueado en el saludo
        public string MostrarNombreCliente(int idCliente)
        {
            cadena = "SELECT NOMBRE FROM CLIENTES WHERE ID_CLIENTE ='" + idCliente + "'";
            try
            {
                conexion.Open();
                comando = new SqlCommand(cadena, conexion);
                SqlDataReader clienteExistente = comando.ExecuteReader();
                if (clienteExistente.Read())
                {
                    return (clienteExistente["NOMBRE"].ToString());
                }
                else
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
            finally
            {
                conexion.Close();
            }
        }
        
        // recibo una direccion de correo y una contraseña y compruebo si existen en la tabla de clientes.      
        // entra Direccion de correo a comprobar entra password a comprobar
        // devuelve Boolean indicando si existe o no
        public bool CheckPassword(string emailLogin,string password)
        {
            cadena = "SELECT EMAIL FROM CLIENTES WHERE EMAIL ='" + emailLogin + "' AND PASS ='" + password + "'";
            try
            {
                conexion.Open();
                comando = new SqlCommand(cadena, conexion);
                SqlDataReader clienteExistente = comando.ExecuteReader();
                return clienteExistente.Read();
            }
            catch
            {
                return false;
            }
            finally
            {
                conexion.Close();
            }
        }

        //public bool InsertarClienteStrings(string nombre, string apellido, string dni, string email, string password, )
        //{
        //    conexion.Open();
        //    cadena = "INSERT INTO CLIENTES VALUES( '" + nombre + "','" + apellido + "','" + dni + "'," +
        //        "'" + email + "','" + password + "','" + fechaNac + "')";
        //    comando = new SqlCommand(cadena, conexion);
        //    comando.ExecuteNonQuery();
        //    conexion.Close();
        //}

        public bool InsertarCliente()
        {
            cadena = "INSERT INTO CLIENTES VALUES( '" + nombre + "','" + apellido + "','" + dni + "'," +
                "'" + email + "','" + password + "','" + fechaNac.ToString("yyyyMMdd") + "')";
            try
            {
                conexion.Open();
                comando = new SqlCommand(cadena, conexion);
                comando.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                conexion.Close();
            }
        }


    }
}
