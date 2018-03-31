using SearchAlbums.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SearchAlbums.Helpers
{
    static class Cache
    {
        /// <summary>
        /// Загрузка данных локального хранилища
        /// </summary>
        /// <returns></returns>
        public static List<Artist> LoadData()
        {
            List<Artist> artists = new List<Artist>();
            XmlSerializer formatter = new XmlSerializer(typeof(List<Artist>));

            if (!File.Exists("Artists.xml"))
            {
                FileStream fs = new FileStream("Artists.xml", FileMode.CreateNew);
                formatter.Serialize(fs, artists);
                fs.Close();
            }

            using (FileStream fs = new FileStream("Artists.xml", FileMode.Open))
            {
                artists = (List<Artist>)formatter.Deserialize(fs);
            }

            return artists.ToList();
        }

        /// <summary>
        /// Сохранение данных в локальное хранилище
        /// </summary>
        /// <param name="artists"> Исполнитель </param>
        public static void SaveData(List<Artist> artists)
        {
            XmlSerializer formatter = new XmlSerializer(typeof(List<Artist>));

            using (FileStream fs = new FileStream("Artists.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, artists);
            }
        }
    }
}
