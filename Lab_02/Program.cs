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
            Console.WriteLine($"Ejecutando consulta LINQ To Objects y PLINQ. {Thread.CurrentThread.ManagedThreadId}");
            // ParallelLoopIterate();
            RunLINQ();
            RunPLINQ();
            Console.WriteLine("Finalizando ejecución de consulta LINQ To Objects y PLINQ...");
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
            }catch(AggregateException ex)
            {
                Console.WriteLine("Por procesar " + ex.InnerExceptions.Count + " exepciones.");
                foreach(var exe in ex.InnerExceptions)
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
            System.Diagnostics.Stopwatch S = new System.Diagnostics.Stopwatch();
            S.Start();
            List<ProductDTO> Products = NorthWind.Repository.Products.Select(
                    product => new ProductDTO
                    {
                        ProductId = product.ProductID,
                        ProductName = product.ProductName,
                        UnitPrice = product.UnitPrice,
                        UnitsInStock = product.UnitsInStock
                    }
                ).ToList();
            S.Stop();
            WriteToConsole($"Tiempo de ejecución con LINQ: {S.ElapsedTicks} ticks... para un total" +
                $" de {Products.Count} productos recuperados.");
        }

        static void RunPLINQ()
        {
            System.Diagnostics.Stopwatch S = new System.Diagnostics.Stopwatch();
            S.Start();
            List<ProductDTO> Products = NorthWind.Repository.Products.AsParallel()
                .Select(
                    product => new ProductDTO
                    {
                        ProductId = product.ProductID,
                        ProductName = product.ProductName,
                        UnitPrice = product.UnitPrice,
                        UnitsInStock = product.UnitsInStock
                    }
                ).ToList();
            S.Stop();
            WriteToConsole($"Tiempo de ejecución con PLINQ: {S.ElapsedTicks} ticks... para un total" +
                $" de {Products.Count} productos recuperados.");
        }

        static void WriteToConsole(string message)
        {
            if (message == "Tarea 2.")
                throw new NotImplementedException();
            Console.WriteLine($"{message}. {Thread.CurrentThread.ManagedThreadId}");
            // Thread.Sleep(5000);
            // Console.WriteLine($"Fin de la tarea. {Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
