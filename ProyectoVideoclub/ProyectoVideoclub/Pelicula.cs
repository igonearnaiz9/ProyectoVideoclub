using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ProyectoVideoclub
{
    class Pelicula
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVIDEOCLUB_CHUCK"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;

        public string titulo { get; set; }
        public string protagonista { get; set; }
        public string director { get; set; }
        public string sinopsis { get; set; }
        public int idPelicula { get; set; }
        public int duracionMinutos { get; set; }
        public int year { get; set; }
        public int pegi { get; set; }
        public bool estado { get; set; }

     
        
        //Constructores
    public Pelicula()
        {

        }
        public Pelicula(int idPelicula, string titulo, string protagonista, string director, string sinopsis,
            int  duracionMinutos, int year,int pegi, bool estado)
        {
            this.idPelicula = idPelicula ;
            this.titulo = titulo;
            this.protagonista = protagonista;
            this.director = director;
            this.sinopsis = sinopsis;
            this.duracionMinutos = duracionMinutos;
            this.year = year;
            this.pegi = pegi;
            this.estado = estado;

        }

        // devuelvo un Listado  de peliculas que cumplan los requisitos de edad del usuario
        // devuelve SqlDataReader con el listado
        public List<Pelicula> PeliculasPorEdad(int IdCliente)
        {
            cadena = "SELECT ID_PELICULA, TITULO, DURACION_MINUTOS, PEGI, SINOPSIS ";
            cadena += "FROM PELICULAS ";
            cadena += "WHERE PEGI <= (SELECT DATEDIFF(year, FECHA_NACIMIENTO, GETDATE()) ";
            cadena += "               FROM CLIENTES ";
            cadena += "               WHERE ID_CLIENTE = " + IdCliente + ")";
            try
            {
                conexion.Open();
                comando = new SqlCommand(cadena, conexion);
                SqlDataReader PeliculasPorEdad = comando.ExecuteReader();
                List<Pelicula> listaPeliculas = new List<Pelicula>();

                while (PeliculasPorEdad.Read())
                {
                    Pelicula peli = new Pelicula();//creo elemento de tipo pelicula para añadir a la lista
                    peli.idPelicula= Convert.ToInt32(PeliculasPorEdad["ID_PELICULA"]);
                    peli.titulo = PeliculasPorEdad["TITULO"].ToString();
                    peli.duracionMinutos = Convert.ToInt32(PeliculasPorEdad["DURACION_MINUTOS"]);
                    peli.pegi= Convert.ToInt32(PeliculasPorEdad["PEGI"]);
                    peli.sinopsis= PeliculasPorEdad["SINOPSIS"].ToString();
                    listaPeliculas.Add(peli);
                }
                return listaPeliculas;
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
        public string CheckPelicula(string idPelicula, int idCliente)
        {
            cadena = "SELECT TITULO ";
            cadena += "FROM PELICULAS ";
            cadena += "WHERE ID_PELICULA = " + idPelicula + " AND ";
            cadena += "      ESTADO = 0 AND ";
            cadena += "      PEGI <= (SELECT DATEDIFF(year, FECHA_NACIMIENTO, GETDATE()) ";
            cadena += "               FROM CLIENTES ";
            cadena += "               WHERE ID_CLIENTE = " + idCliente + ")";
            try 
            {
                conexion.Open();
                comando = new SqlCommand(cadena, conexion);
                SqlDataReader peliculadisponible = comando.ExecuteReader();
                if (peliculadisponible.Read())
                {
                    return peliculadisponible["TITULO"].ToString();
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
