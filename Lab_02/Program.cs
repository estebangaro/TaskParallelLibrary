using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
// Tarea 1,2,3 completadas.
namespace Lab_02
{
    class Program
    {
        static void Main(string[] args)
        {
            // RunParallelTasks();
            Console.WriteLine($"Ejecutando RunContinuationTasks. {Thread.CurrentThread.ManagedThreadId}");
            // ParallelLoopIterate();
            // RunLINQ();
            // RunPLINQ();
            RunContinuationTasks();
            Console.WriteLine("Finalizando ejecución de RunContinuationTasks...");
            Console.WriteLine("Presione <enter> para finalizar");
            Console.ReadLine();
        }

        static void RunParallelTasks()
        {
            Console.WriteLine($"Ejecutando tareas en paralelo. {Thread.CurrentThread.ManagedThreadId}");
            try
            {
                Parallel.Invoke(
                    () => WriteToConsole("Tarea 1."),
                    () => WriteToConsole("Tarea 2."),
                    () => WriteToConsole("Tarea 3."));
            }
            catch (AggregateException ex)
            {
                Console.WriteLine("Por procesar " + ex.InnerExceptions.Count + " exepciones.");
                foreach (var exe in ex.InnerExceptions)
                {
                    Console.WriteLine("Exepción: " + exe.Message);
                }
            }
        }

        static void ParallelLoopIterate()
        {
            int[] ArrayOfInts = new int[5];
            ParallelLoopResult ForLoopResult = Parallel.For(0, 5, delegate (int index)
            {
                ArrayOfInts[index] = index * index;
                WriteToConsole($"Calculando el cuadrado de {index}");
            });
            WriteToConsole($"Estado de ejecución de iteración de ciclo en paralelo (For): {(ForLoopResult.IsCompleted ? "Ejecución completa" : "Ejecución incompleta")}");
            WriteToConsole("Mostrando los cuadrados calculados...");
            ParallelLoopResult ForEachLoopResult = Parallel.ForEach(ArrayOfInts, intNumber =>
                WriteToConsole($"Cuadrado de {Array.IndexOf(ArrayOfInts, intNumber)}: {intNumber}"));
            WriteToConsole($"Estado de ejecución de iteración de ciclo en paralelo (ForEach): {(ForEachLoopResult.IsCompleted ? "Ejecución completa" : "Ejecución incompleta")}");
        }

        static void RunLINQ()
        {
            //System.Diagnostics.Stopwatch S = new System.Diagnostics.Stopwatch();
            //S.Start();
            //List<ProductDTO> Products = NorthWind.Repository.Products.Select(
            //        product => new ProductDTO
            //        {
            //            ProductId = product.ProductID,
            //            ProductName = product.ProductName,
            //            UnitPrice = product.UnitPrice,
            //            UnitsInStock = product.UnitsInStock
            //        }
            //    ).ToList();
            //S.Stop();
            //WriteToConsole($"Tiempo de ejecución con LINQ: {S.ElapsedTicks} ticks... para un total" +
            //    $" de {Products.Count} productos recuperados.");
        }

        static void RunPLINQ()
        {
            //System.Diagnostics.Stopwatch S = new System.Diagnostics.Stopwatch();
            //S.Start();
            //List<ProductDTO> Products = NorthWind.Repository.Products.AsParallel()
            //    .Select(
            //        product => new ProductDTO
            //        {
            //            ProductId = product.ProductID,
            //            ProductName = product.ProductName,
            //            UnitPrice = product.UnitPrice,
            //            UnitsInStock = product.UnitsInStock
            //        }
            //    ).ToList();
            //S.Stop();
            //WriteToConsole($"Tiempo de ejecución con PLINQ: {S.ElapsedTicks} ticks... para un total" +
            //    $" de {Products.Count} productos recuperados.");
        }

        static void WriteToConsole(string message)
        {
            if (message == "Tarea 2.")
                throw new NotImplementedException();
            Console.WriteLine($"{message}. {Thread.CurrentThread.ManagedThreadId}");
            // Thread.Sleep(5000);
            // Console.WriteLine($"Fin de la tarea. {Thread.CurrentThread.ManagedThreadId}");
        }

        static List<string> GetProductNames()
        {
            Thread.Sleep(3000);
            throw new NotImplementedException();
            return new List<string> { "PlayStation 4", "Tomb Raider", "PES", "The Last Of Us", "FIFA 18" };
        }

        static void RunContinuationTasks()
        {
            Task<List<string>> GameNamesTask = new Task<List<string>>(
                    () => GetProductNames());

            Task<int> ProcessGameNames = GameNamesTask.ContinueWith(
                gameNameTask => { Console.WriteLine("Ejecutando tarea de continuación");
                    try
                    {
                        return ProcessData(gameNameTask.Result);
                    }
                    catch (AggregateException) { Console.WriteLine("Exepcion manejada"); return 0; } });
            //try
            //{
                GameNamesTask.Start();
            //}
            //catch { Console.WriteLine("Exepcion manejada"); }
            //try
            //{
            //    Console.WriteLine($"El número de nombres de juegos procesados es:");
            //Console.WriteLine($"{GameNamesTask.Result}");
                Console.WriteLine($"El número de nombres de juegos procesados es: {ProcessGameNames.Result}");
            //}
            //catch (AggregateException ex)
            //{
            //    Console.WriteLine($"Exepción controlada: {ex.Message}");
            //}
        }

        static int ProcessData(List<string> GameNames)
        {
            int i = 0;
            foreach (string name in GameNames)
            {
                Console.WriteLine($"Nombre ({++i}): {name}");
            }

            return GameNames.Count;
        }

        //static int ProcessData(Task<List<string>> GameNames)
        //{
        //    List<string> gamesnames = GameNames.Result;
        //    int i = 0;
        //    if (GameNames.Status != TaskStatus.Faulted)
        //    {
        //        foreach (string name in GameNames.Result)
        //        {
        //            Console.WriteLine($"Nombre ({++i}): {name}");
        //        }

        //        return GameNames.Result.Count;
        //    }
        //    else
        //    {
        //        Console.WriteLine($"Sin información de nombres de juego");
        //        return i;
        //    }
        //}
    }
}
