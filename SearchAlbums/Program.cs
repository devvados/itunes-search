using SearchAlbums.Helpers;
using SearchAlbums.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SearchAlbums
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.Clear();
                Console.Write("Введите название группы/исполнителя: ");
                var artistName = Console.ReadLine();

                var albums = Finder.FindAlbums(artistName);

                PrintResult(albums);

                Console.WriteLine("\nХотите продолжить? Д/Н");
            }
            while (Console.ReadLine().ToLower() == "д");
        }

        public static void PrintResult(List<Album> a)
        {
            for (int i = 0; i < a.Count; i++)
            {
                Console.WriteLine(string.Format("{0}. {1}", i + 1, a[i].AlbumName));
            }
        }
    }
}
