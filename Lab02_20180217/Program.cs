using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace Lab02_20180217
{
    class Program
    {
        static void Main(string[] args)
        {
            // RunParallelTasks();
            // ParallelLoopIterate();
            // RunLINQ();
            // RunPLINQ();
            //RunCustomExercise1(1, true);
            //RunCustomExercise1(6, false);
            Console.WriteLine("Presione enter para finalizar");
            Console.ReadLine();
            // Continuar con tarea 1, ejercicio 2 Enlazando Tareas.
        }

        // Tarea 3, Ejercicio 1
        private static void ParallelLoopIterate()
        {
            int[] SquareIntsArray = new int[5];
            Parallel.For(0, 5, delegate (int indice)
            {
                SquareIntsArray[indice] = indice * indice;
                //Console.WriteLine($"Calculando el cuadrado de {indice}");
                // Ejercicio propuesto
                Console.WriteLine($"Calculando el cuadrado de {indice}, en Thread: " +
                    $"{Thread.CurrentThread.ManagedThreadId}");
            });
            Console.WriteLine("Mostrando los valores calculados...");
            Parallel.ForEach(SquareIntsArray, SquareInt =>
            {
                //Console.WriteLine($"Cuadrado de {Array.IndexOf(SquareIntsArray, SquareInt)}: {SquareInt}");
                // Ejercicio propuesto.
                Console.WriteLine($"Cuadrado de {Math.Sqrt(SquareInt)} = {SquareInt}, en Thread: " +
                    $"{Thread.CurrentThread.ManagedThreadId}");
            });
        }

        // Tarea 2, Ejercicio 1
        private static void RunParallelTasks()
        {
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}, ejecutar tareas en paralelo");
            Parallel.Invoke(
                () => { WriteToConsole("Tarea 1"); },
                () => { WriteToConsole("Tarea 2", 10000); },
                () => { WriteToConsole("Tarea 3"); });
            // Ejercicio propuesto - Generación de excepciones no controladas.
            /* Console.WriteLine("Por lanzar tarea");
            Task<int> demo =
                Task<int>.Run(() => { WriteToConsole("Tarea demo"); throw new Exception("Excepción demo"); return 25; });
            int resultado = demo.Result; */

        }
        // Tarea 2, Ejercicio 1
        private static void WriteToConsole(string message, int mseconds = 5000)
        {
            Console.WriteLine($"{message} - Thread:{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(mseconds);
            Console.WriteLine($"Fin de la tarea - Thread:{Thread.CurrentThread.ManagedThreadId}");
        }
        // Tarea 4, Ejercicio 1
        private static void RunLINQ()
        {
            System.Diagnostics.Stopwatch medidorTiempo = new System.Diagnostics.Stopwatch();
            medidorTiempo.Start();
            List<ProductDTO> DTOProducts = NorthWind.Repository.Products.Select(p => new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock
            }).ToList();
            medidorTiempo.Stop();
            Console.WriteLine($"Tiempo de ejecución con LINQ: {medidorTiempo.ElapsedTicks} Ticks");
        }
        // Tarea 4, Ejercicio 1
        private static void RunPLINQ()
        {
            System.Diagnostics.Stopwatch medidorTiempo = new System.Diagnostics.Stopwatch();
            medidorTiempo.Start();
            List<ProductDTO> DTOProducts = NorthWind.Repository.Products.AsParallel().Select(p => new ProductDTO
            {
                ProductID = p.ProductID,
                ProductName = p.ProductName,
                UnitPrice = p.UnitPrice,
                UnitsInStock = p.UnitsInStock
            }).ToList();
            medidorTiempo.Stop();
            Console.WriteLine($"Tiempo de ejecución con PLINQ: {medidorTiempo.ElapsedTicks} Ticks");
        }
        // Ejercicio Propuesto - Ejercicio 1.
        private static void RegistraAutomovil(int noSerie, string marca, string modelo, int noPuertas)
        {
            // Registrar automovil en BD y actualizar fecha de modificación.
            AutomovilesModel baseDeDatosContext = new AutomovilesModel();
            baseDeDatosContext.Automoviles.Add(new Automovil
            {
                Id = noSerie,
                Marca = marca,
                Modelo = modelo,
                Puertas = noPuertas,
                FechaRegistro = DateTime.Now
            });
            baseDeDatosContext.SaveChanges();
            baseDeDatosContext.Automoviles.First(auto => auto.Id == noSerie).FechaModificacion = DateTime.Now;
            baseDeDatosContext.SaveChanges();
        }
        private static void RunCustomExercise1(int ultimoAuto, bool isSequential)
        {
            System.Diagnostics.Stopwatch medidorTiempo = new System.Diagnostics.Stopwatch();
            Console.WriteLine($"Iniciando registro {(isSequential ? "Secuencial" : "Paralelo")} de automoviles");
            List<Automovil> AutomovilesList = new List<Automovil> {
                new Automovil{ Id = ultimoAuto++, Marca = "Mazda", Modelo = "3", Puertas = 5},
                new Automovil{ Id = ultimoAuto++, Marca = "Mazda", Modelo = "3", Puertas = 5},
                new Automovil{ Id = ultimoAuto++, Marca = "Mazda", Modelo = "3", Puertas = 5},
                new Automovil{ Id = ultimoAuto++, Marca = "Mazda", Modelo = "3", Puertas = 5},
                new Automovil{ Id = ultimoAuto++, Marca = "Mazda", Modelo = "3", Puertas = 5}
            };
            medidorTiempo.Start();
            if (isSequential)
                foreach (Automovil auto in AutomovilesList)
                {
                    RegistraAutomovil(auto.Id, auto.Marca, auto.Modelo, auto.Puertas);
                }
            else
                Parallel.ForEach<Automovil>(AutomovilesList, auto =>
                {
                    RegistraAutomovil(auto.Id, auto.Marca, auto.Modelo, auto.Puertas);
                });
            medidorTiempo.Stop();
            Console.WriteLine($"Finalizando registro {(isSequential ? "Secuencial" : "Paralelo")} de automoviles, " +
                $"tiempo de ejecución {ConvierteDeTicksASegundos(medidorTiempo.ElapsedTicks)} segundos.");
        }

        private static string ConvierteDeTicksASegundos(long elapsedTicks)
        {
            // 1 tick => 100x10-9 s; 
            // elapsedTicks ticks => ¿? s
            return (elapsedTicks * .000000100).ToString();
        }
    }
}
