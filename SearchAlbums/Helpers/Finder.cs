using Newtonsoft.Json.Linq;
using SearchAlbums.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SearchAlbums.Helpers
{
    /// <summary>
    /// Поиск альбомов по названию группы/исполнителя
    /// </summary>
    class Finder
    {
        /// <summary>
        /// Поиск альбомов
        /// </summary>
        /// <param name="artist"> Исполнитель </param>
        /// <returns></returns>
        public static List<Album> FindAlbums(string artist)
        {
            List<Artist> artists;

            var requestString = BuilRequestString(artist);

            Task<string> jsonTask = Task.Factory.StartNew(() => GetResponse(requestString));

            Console.Write("Ждем ответ от сервера...");
            //пока ожидается ответ от сервера
            while (jsonTask.IsCompleted != true)
            {
                Thread.Sleep(500);
                Console.Write(".");
            }
            Console.WriteLine();

            var jsonString = jsonTask.Result;

            if (jsonString != "")
            {
                artists = Cache.LoadData();

                var jsonResult = (JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(jsonString);
                var albums = GetAlbums(jsonResult);

                try
                {
                    if (albums.Count > 0)
                    {
                        if (!artists.Any(x => x.ArtistName == artist))
                        {
                            Artist a = new Artist
                            {
                                ArtistName = artist,
                                Albums = albums
                            };
                            artists.Add(a);
                        }

                        Cache.SaveData(artists);
                    }
                    else
                        throw new Exception("Альбомов не найдено!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return albums;
            }
            else
            {
                Console.WriteLine("Отсутствует подключение к интернету! Поиск в локальном хранилище...");

                List<Album> albums = new List<Album>();
                artists = Cache.LoadData();

                try
                {
                    if (artists.Count > 0)
                    {
                        var a = artists.Find(x => x.ArtistName.ToLower() == artist.ToLower());

                        if (a != null)
                        {
                            return a.Albums;
                        }
                        else
                            throw new Exception("Альбомов не найдено!");
                    }
                    else
                    {
                        throw new Exception("В локальном хранилище данных не найдено!");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                return albums;
            }
        }

        /// <summary>
        /// Строка запроса
        /// </summary>
        /// <param name="artistName"> Исполнитель </param>
        /// <returns></returns>
        public static string BuilRequestString(string artistName)
        {
            var str = artistName.Replace(' ', '+');
            var req = "https://itunes.apple.com/search?term=" + str + "&entity=album";

            return req;
        }

        /// <summary>
        /// Получение ответа от сервера
        /// </summary>
        /// <param name="req"> Строка запроса </param>
        /// <returns></returns>
        public static string GetResponse(object req)
        {
            HttpClient client = new HttpClient();
            HttpWebRequest request = WebRequest.Create((string)req) as HttpWebRequest;
            request.Timeout = 10000;

            try
            {
                WebResponse webResponse = request.GetResponse();
                var responseText = new StreamReader(webResponse.GetResponseStream()).ReadToEnd();

                return responseText;
            }
            catch (Exception)
            {
                return "";
            }
        }

        /// <summary>
        /// Разбор JSON
        /// </summary>
        /// <param name="response"> JSON объект </param>
        /// <returns></returns>
        public static List<Album> GetAlbums(JObject response)
        {
            var token = response.Root.Last.First.Children().ToList();

            List<Album> albums = token.Select(p => new Album
            {
                AlbumName = (string)p["collectionName"],
                TrackCount = (int)p["trackCount"],
                ReleaseDate = (DateTime)p["releaseDate"]
            }).ToList();

            return albums;
        }
    }
}
