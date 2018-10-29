using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;

namespace ProyectoVideoclub
{
    class Program
    {
        static String connectionString = ConfigurationManager.ConnectionStrings["conexionVIDEOCLUB_CHUCK"].ConnectionString;
        static SqlConnection conexion = new SqlConnection(connectionString);
        static string cadena;
        static SqlCommand comando;
        static int ClienteLogueado;
        static List<string> listaFrases = new List<string>();

        static void Main(string[] args)
        {
            //Compruebo que funciona la conexion
            //conexion.Open();
            //cadena = "SELECT * FROM PELICULAS";
            //comando = new SqlCommand(cadena, conexion);
            //SqlDataReader registros = comando.ExecuteReader();
            //while (registros.Read())
            //{
            //    Console.WriteLine(registros["TITULO"].ToString());
            //    Console.WriteLine();
            //}
            //conexion.Close();
            //Console.ReadLine();

            //Como se trata de un string no hace falta hacer lo de Futbolista f1 = new Futbolista()
            listaFrases.Add("M.C. Hammer learned the hard way that Chuck Norris can touch this.");
            listaFrases.Add("When the Boogeyman goes to sleep every night he checks his closet for Chuck Norris.");
            listaFrases.Add("Chuck Norris doesn't cheat death. He wins fair and square.");
            listaFrases.Add("There is no theory of evolution, just a list of creatures Chuck Norris allows to live.");
            listaFrases.Add("When a zombie apocalypse starts, Chuck Norris doesn't try to survive. The zombies do.");
            listaFrases.Add("Chuck Norris makes onions cry.");
            listaFrases.Add("When Bruce Banner gets mad he turns into the Hulk. When the Hulk gets mad he turns into Chuck Norris. When Chuck Norris gets mad, run.");
            listaFrases.Add("Chuck Norris sleeps with a pillow under his gun.");
            listaFrases.Add("Chuck Norris puts the 'laughter' in 'manslaughter'.");

            MenuInicial();
        }
        //Aparece un primer Menu Inicial en el que sólo tengo la opción de Loguearme, Registrarme o Salir
        public static void MenuInicial()
        {
            const int LOGIN = 1, REGISTRO = 2, SALIR=3;
            int option;
            do
            {
                Console.WriteLine("Bienvenido al Videoclub Chuck");
                Console.WriteLine();
                Console.WriteLine(FraseRandom());
                Console.WriteLine();
                Console.WriteLine("*************************************************************************");
                Console.WriteLine();
                Console.WriteLine("Si ya eres cliente, elige la opción 1");
                Console.WriteLine();
                Console.WriteLine("Si no eres cliente, elige la opción 2");
                Console.WriteLine();
                Console.WriteLine("Si deseas salir, elige la opción 3");
                Console.WriteLine();
                option = Int32.Parse(Console.ReadLine());
               
                switch (option)
                {
                    case LOGIN:                        
                        MetodoLogin();
                        break;
                    case REGISTRO:
                        RegistrarCliente();               
                        break;               
                }
            } while (option != SALIR);
            Console.WriteLine("When Chuck Norris enters a room, he doesn't turn the lights on, he turns the dark off");
            Console.WriteLine();
            Console.WriteLine("Hasta pronto!!");
            Console.ReadLine();
        }

        //Para Loguearme porque ya soy cliente
        //Primero: compruebo que el email(que voy a utilizar como identificador)está en la BBDD
        public static void MetodoLogin()
        {           
            Cliente cliente = new Cliente();// instancio la clase para poder acceder a sus procesos
            string emailLogin;
            bool primeraVuelta = true;
            do
            {
                if (!primeraVuelta)
                {
                    Console.WriteLine("No eres un cliente registrado, regístrate por favor");
                }

                Console.WriteLine("Introduce tu Email");//Va debajo del if porque si no aparece 2 veces
                emailLogin = Console.ReadLine();
                primeraVuelta = false;
                ClienteLogueado = cliente.CheckLogin(emailLogin);
            } while (ClienteLogueado==0);

            //Segundo: compruebo también que la contraseña intorducida es la correcta
            string passwordLogin;
            primeraVuelta = true;
            do
            {
                if (!primeraVuelta)
                {
                    Console.WriteLine("Tu contraseña no es correcta, introdúcela de nuevo");
                }
                Console.WriteLine("Introduce tu Contraseña");
                passwordLogin = Console.ReadLine();
                primeraVuelta = false;
            } while (!cliente.CheckPassword(emailLogin,passwordLogin));

            // Si los datos son correctos quiero que se me abra el menu principal
            Console.WriteLine();
            MenuPrincipal menu = new MenuPrincipal();
            menu.Iniciar(ClienteLogueado);
        }
        //Para registrarme porque no soy cliente
        public static void RegistrarCliente()
        {
            string dni, nombre, apellido, email, password;
            DateTime fechaNac;
            

            Console.WriteLine("Introduce los siguientes datos:");
            Console.WriteLine("DNI");//TODO:q no se pueda quedar vacío, comprobar metododni
            dni = Console.ReadLine();
            Console.WriteLine("Nombre");
            nombre = Console.ReadLine();
            Console.WriteLine("Apellido");
            apellido = Console.ReadLine();
            Console.WriteLine("Email");//TODO:q no se pueda quedar vacío, comprobar metodoemail
            email = Console.ReadLine();
            Console.WriteLine("Fecha de Nacimiento");
            fechaNac = Convert.ToDateTime(Console.ReadLine());
            Console.WriteLine("Contraseña");
            password = Console.ReadLine();

            //Para poder insertar los datos que recogido dentro de la BBDD
            Cliente cliente = new Cliente();
            cliente.SetNombreCliente(nombre);
            cliente.SetApellidoCliente(apellido);
            cliente.SetEmailCliente(email);
            cliente.SetDNICliente(dni);
            cliente.SetPasswordCliente(password);
            cliente.SetFechaNacCliente(fechaNac);

            //Si se ha producido algún fallo al insertar los datos en la BBDD, nos vuelve a pedir los datos
            if (!cliente.InsertarCliente())
            {
                Console.WriteLine("No se ha podido insertar el registro, vuelve a introducir tus datos");
                Console.WriteLine();
                RegistrarCliente();
            }
            else
            {
                Console.WriteLine("Te has registrado correctamente.");
                Console.WriteLine();
                MenuInicial();
            }
            
        }

        public static string FraseRandom()
        {
            var rnd = new Random();
            return listaFrases[rnd.Next(listaFrases.Count)];
        }
    }
}
