using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

namespace ProyectoVideoclub
{
    class MenuPrincipal
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVIDEOCLUB_CHUCK"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        //static string cadena;
        //static SqlCommand comando;
        static int idCliente;


        public  /*static*/void Iniciar(int clienteLogueado) //he tenido que quitar el static al meter el 
                                        //Método PeliculasporEdad
        
        //Una vez logueado me aparece el Menu Principal
        {
            idCliente = clienteLogueado;
            const int PELICULASDISPONIBLESPOREDAD = 1, ALQUILARPELICULA = 2,MISALQUILERES=3,
               LOGOUT=4;
            int option;
            Cliente cliente = new Cliente();//necesito esto para poder acceder a los métodos de la clase
            do
            {
                Console.WriteLine("Bienvenido al Videoclub Chuck, " + cliente.MostrarNombreCliente(idCliente).ToUpper());
                Console.WriteLine();
                Console.WriteLine(Program.FraseRandom());
                Console.WriteLine();
                Console.WriteLine("*************************************************************************");
                Console.WriteLine();
                Console.WriteLine("Si quieres ver las películas disponibles, elige la opción 1");
                Console.WriteLine();
                Console.WriteLine("Si quieres alquilar una película, elige la opción 2");
                Console.WriteLine();
                Console.WriteLine("Si quieres ver tus alquileres, elige la opción 3");
                Console.WriteLine();
                //Console.WriteLine("Si quieres editar tus datos de cliente, elige la opción 4");
                //Console.WriteLine();
                Console.WriteLine("Si quieres salir, elige la opción 4");
                option = Int32.Parse(Console.ReadLine());
                


                switch (option)
                {
                    case PELICULASDISPONIBLESPOREDAD:
                        ListarPeliculasPorEdad();
                        break;
                    case ALQUILARPELICULA:
                        PeticionAlquiler();
                        break;
                    case MISALQUILERES:
                        MostrarMisAlquileres();
                        PeticionDevolucion();
                        break;
                    
                }
            } while (option <=0 || option>3);

        }

        public void ListarPeliculasPorEdad()
        {
            Pelicula peliculas = new Pelicula();
            List<Pelicula> listaPeliculas = new List<Pelicula>();
            listaPeliculas = peliculas.PeliculasPorEdad(idCliente);
            foreach (Pelicula peli in listaPeliculas)
            {
                Console.WriteLine("Id Pelicula: "+ peli.idPelicula + "\t Título: " + peli.titulo + "\t Duración: " 
                    + peli.duracionMinutos + "\t PEGI: " + peli.pegi);
            }
            Console.WriteLine();
            Iniciar(idCliente);
        }

        public void PeticionAlquiler()
        {
            string idpeliculaelegida;
            bool primeraVuelta = true;
            string TituloPeli;
            Pelicula peliculas = new Pelicula();
            do
            {
                if (!primeraVuelta)
                {
                    Console.WriteLine("Tu elección no está disponible");
                }
                Console.WriteLine("Introduce el id de película");
                idpeliculaelegida = Console.ReadLine();
                primeraVuelta = false;
                TituloPeli = peliculas.CheckPelicula(idpeliculaelegida, idCliente);
            } while (TituloPeli=="");

            Alquiler alquilo = new Alquiler();
            if (alquilo.AlquilerPelicula(idpeliculaelegida, idCliente))
            {
                Console.WriteLine("La pelicula " + TituloPeli + " ha sido alquilada");
                Console.WriteLine();
                Iniciar(idCliente);
            }
            else
            {
                Console.WriteLine("La pelicula " + TituloPeli + " NO ha podido ser alquilada");
                Console.WriteLine();
                PeticionAlquiler();
            }
        }

        public void MostrarMisAlquileres()
        {
            Alquiler alquila = new Alquiler();
            List<Alquiler> listaAlquiler = new List<Alquiler>();
            listaAlquiler = alquila.MisAlquileres(idCliente);
            ConsoleColor colorActual = Console.ForegroundColor;
            foreach (Alquiler alqui in listaAlquiler)
            {
                if((DateTime.Today - alqui.FechaAlquiler).TotalDays > 3)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                }
                else
                {
                    Console.ForegroundColor = colorActual;
                }
                Console.WriteLine("Id Pelicula: " + alqui.idPelicula + " Título: " + alqui.Titulo +
                                    " Fecha Alquiler: " + alqui.FechaAlquiler.ToString("dd/MM/yyyy"));
            }
            Console.ForegroundColor = colorActual;
            Console.WriteLine();
        }

        public void PeticionDevolucion()
        {
            string idpeliculaelegida;
            bool primeraVuelta = true;
            string TituloPeli;
            Alquiler alquilo = new Alquiler();
            List<Alquiler> listaAlquiler = new List<Alquiler>();
            listaAlquiler = alquilo.MisAlquileres(idCliente);
            if (listaAlquiler.Count==0)
            {
                Console.WriteLine("No tienes películas alquiladas");
                Console.WriteLine();
                Iniciar(idCliente);
                return;//al poner este return y ser un método void ya no me hace lo que sigue debajo
            }

            do
            {
                if (!primeraVuelta)
                {
                    Console.WriteLine("Tu no tienes esta pelicula alquilada");
                }
                Console.WriteLine("Introduce el id de película a devolver");
                idpeliculaelegida = Console.ReadLine();
                primeraVuelta = false;
                TituloPeli = alquilo.CheckAlquiler(idpeliculaelegida, idCliente);
            } while (TituloPeli == "");

            if (alquilo.DevolverPelicula(idpeliculaelegida, idCliente))
            {
                Console.WriteLine("La pelicula " + TituloPeli + " ha sido devuelta");
                Console.WriteLine();
                Iniciar(idCliente);
            }
            else
            {
                Console.WriteLine("La pelicula " + TituloPeli + " NO ha podido ser devuelta");
                Console.WriteLine();
                PeticionAlquiler();
            }
        }
    }
}
