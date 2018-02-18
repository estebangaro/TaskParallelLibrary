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
            ParallelLoopIterate();
            Console.WriteLine("Presione enter para finalizar");
            Console.ReadLine();
            // Continuar con tarea 4, ejercicio 1 Utilizando parallel LINQ.
        }

        // Tarea 3
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

        // Tarea 2
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
        // Tarea 2
        private static void WriteToConsole(string message, int mseconds = 5000)
        {
            Console.WriteLine($"{message} - Thread:{Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(mseconds);
            Console.WriteLine($"Fin de la tarea - Thread:{Thread.CurrentThread.ManagedThreadId}");
        }
    }
}
