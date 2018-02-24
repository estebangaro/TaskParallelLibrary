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

            // RunContinuationTask();
            //RunNestedTasks(); 

            //NestedTaskExcercise2();

            HandleTaskExceptions();
            Console.WriteLine("Presione enter para finalizar");
            Console.ReadLine();
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

        // Tarea 1, Ejercicio 2
        private static List<string> GetProductNames()
        {
            Thread.Sleep(3000);
            return NorthWind.Repository.Products.Select(product => product.ProductName).ToList();
        }

        // Tarea 1, Ejercicio 2
        private static void RunContinuationTask()
        {
            Task<List<string>> FirstTask = new Task<List<string>>(GetProductNames);
            Task<int> SecondTask = 
                FirstTask.ContinueWith<int>(antecedentTask => ProcessData(antecedentTask.Result));

            FirstTask.Start();
            Console.WriteLine($"Número de productos procesados: {SecondTask.Result}");
        }

        // Tarea 1, Ejercicio 2
        private static int ProcessData(List<string> productNames)
        {
            foreach(var nombre in productNames)
            {
                Console.WriteLine(nombre);
            }
            return productNames.Count;
        }

        // Tarea 2, Ejercicio 2
        private static void RunNestedTasks()
        {
            var OuterTask = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Iniciando la tarea externa...");
                var InnerTask = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Iniciando tarea anidada");
                    Thread.Sleep(3000);
                    Console.WriteLine("Finalizando tarea anidada");
                }, TaskCreationOptions.AttachedToParent);
            });

            OuterTask.Wait();
            Console.WriteLine("Finalizando tarea externa");
        }

        // Tarea 2, Ejercicio Propuesto
        private static void NestedTaskExcercise()
        {
            Task task = Task.Factory.StartNew(() =>
            {
                Console.WriteLine("Iniciando tarea externa");
                Task childT = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Iniciando tarea hija");
                    Thread.Sleep(2000);
                    throw new Exception("Excepción en tarea HIJA");
                    Console.WriteLine("Finalizando tarea hija");
                }, TaskCreationOptions.AttachedToParent);
                throw new Exception("Excepción en tarea padre");
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Excepción controlada");
                foreach(var excepcion in ae.InnerExceptions)
                {
                    Console.WriteLine(excepcion.InnerException != null ?
                        excepcion.InnerException.Message : excepcion.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepción desconocida");
            }
            Console.WriteLine($"La tarea externa ha finalizado, estatus {task.Status}");
        }

        // Tarea 2, Ejercicio Propuesto
        private static void NestedTaskExcercise2()
        {
            Task task = Task.Run(() =>
            {
                Console.WriteLine("Iniciando tarea externa");
                Task childT = Task.Run(() =>
                {
                    Console.WriteLine("Iniciando tarea hija");
                    Thread.Sleep(2000);
                    Console.WriteLine("Finalizando tarea hija");
                });
            });

            try
            {
                task.Wait();
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("Excepción controlada");
                foreach (var excepcion in ae.InnerExceptions)
                {
                    Console.WriteLine(excepcion.InnerException != null ?
                        excepcion.InnerException.Message : excepcion.Message);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Excepción desconocida");
            }
            Console.WriteLine($"La tarea externa ha finalizado, estatus {task.Status}");
        }

        // Ejercicio 3, Tarea 1.
        private static void RunLongTask(CancellationToken ct)
        {
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(2000);
                ct.ThrowIfCancellationRequested();
            }
        }
        // Ejercicio 3, Tarea 1.
        private static void HandleTaskExceptions()
        {
            CancellationTokenSource cts = new CancellationTokenSource();
            CancellationToken ct = cts.Token;

            Task LongTask = Task.Run(delegate ()
            {
                RunLongTask(ct);
            }, ct);
            cts.Cancel();
            try
            {
                LongTask.Wait();
            }
            catch (AggregateException ae)
            {
                foreach (var excepcion in ae.InnerExceptions)
                {
                    Console.WriteLine($"Excepción controlada: " +
                        $"{(excepcion.InnerException != null ? excepcion.InnerException.Message : excepcion.Message)}" +
                        $", Tipo: {excepcion.GetType().Name}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Manejo de excepción no esperada...");
            }
        }
    }
}
