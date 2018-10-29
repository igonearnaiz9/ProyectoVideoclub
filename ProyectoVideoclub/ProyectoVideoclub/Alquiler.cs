using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ProyectoVideoclub
{
    class Alquiler
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVIDEOCLUB_CHUCK"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;

        public int idAlquiler { get; set; }
        public int idCliente { get; set; }
        public int idPelicula { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaAlquiler { get; set; }
        public DateTime FechaDevolucion { get; set; }



        public bool AlquilerPelicula(string idPelicula, int idCliente)
        {
            cadena = "INSERT INTO ALQUILER VALUES( " + idCliente + "," + idPelicula + ", ";
            cadena += "                            DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0), NULL);";
            cadena += "UPDATE PELICULAS SET ESTADO=1 WHERE ID_PELICULA= " + idPelicula  ;
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

        public bool DevolverPelicula(string idPelicula, int idCliente)
        {
            cadena =  "UPDATE ALQUILER SET FECHA_DEVOLUCION=DATEADD(dd, DATEDIFF(dd, 0, getdate()), 0) ";
            cadena += "WHERE ID_PELICULA= " + idPelicula + " AND FECHA_DEVOLUCION IS NULL AND ID_CLIENTE = " + idCliente  + ";";
            cadena += "UPDATE PELICULAS SET ESTADO=0 WHERE ID_PELICULA= " + idPelicula;
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

        // Este proceso devuelve un Listado      
        // de peliculas que cumplan los 
        // requisitos de edad del usuario
        // devuelve SqlDataReader con el listado
        public List<Alquiler> MisAlquileres(int idCliente)
        {
            cadena = "SELECT              ";
            cadena += "	 ALQUILER.ID_PELICULA, 				";
            cadena += "	 PELICULAS.TITULO, 				";
            cadena += "	 ALQUILER.FECHA_ALQUILER                ";
            cadena += "FROM                ";
            cadena += "	 ALQUILER, PELICULAS                ";
            cadena += "WHERE               ";
            cadena += "	 ALQUILER.ID_CLIENTE = " + idCliente + " AND                 ";
            cadena += "	 ALQUILER.FECHA_DEVOLUCION IS NULL AND 				";
            cadena += "	 PELICULAS.ID_PELICULA = ALQUILER.ID_PELICULA             ";

            try
            {
                conexion.Open();
                comando = new SqlCommand(cadena, conexion);
                SqlDataReader Alquileres = comando.ExecuteReader();
                List<Alquiler> listaAlquileres = new List<Alquiler>();

                while (Alquileres.Read())
                {
                    Alquiler alquila = new Alquiler();
                    alquila.idPelicula = Convert.ToInt32(Alquileres["ID_PELICULA"]);
                    alquila.Titulo = Alquileres["TITULO"].ToString();
                    alquila.FechaAlquiler = Convert.ToDateTime(Alquileres["FECHA_ALQUILER"]);
                    listaAlquileres.Add(alquila);
                }
                return listaAlquileres;
            }
            catch
            {
                return null;
            }
            finally
            {
                conexion.Close();
            }
        }

        // recibo un idcliente y un id pelicula compruebo si existe en la tabla de peliculas y si está disponible      
        // entra id pelicula, idcliente
        // devuelve si la pelicula está disponible o no
        public string CheckAlquiler(string idPelicula, int idCliente)
        {
            cadena = "	SELECT 				";
            cadena += "	 ALQUILER.ID_PELICULA, 				";
            cadena += "	 PELICULAS.TITULO				";
            cadena += "	FROM 				";
            cadena += "	 ALQUILER, PELICULAS 				";
            cadena += "	WHERE 				";
            cadena += "	 ALQUILER.ID_CLIENTE=" + idCliente + " AND 				";
            cadena += "	 ALQUILER.ID_PELICULA = " + idPelicula + " AND 				";
            cadena += "	 ALQUILER.FECHA_DEVOLUCION IS NULL AND 				";
            cadena += "	 PELICULAS.ID_PELICULA=ALQUILER.ID_PELICULA				";

            try
            {
                conexion.Open();
                comando = new SqlCommand(cadena, conexion);
                SqlDataReader peliculaDevuelta = comando.ExecuteReader();
                if (peliculaDevuelta.Read())
                {
                    return peliculaDevuelta["TITULO"].ToString();
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
    }
}
