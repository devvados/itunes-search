using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SearchAlbums.Models
{
    /// <summary>
    /// Исполнитель
    /// </summary>
    [Serializable]
    public class Artist
    {
        string artistName;
        List<Album> albums;

        [XmlElement("Artist")]
        public string ArtistName
        {
            get
            {
                return artistName;
            }

            set
            {
                artistName = value;
            }
        }

        [XmlArray("Albums")]
        public List<Album> Albums
        {
            get
            {
                return albums;
            }

            set
            {
                albums = value;
            }
        }

        public Artist() { }

        public Artist(string artist, List<Album> alb)
        {
            ArtistName = artist;
            Albums = new List<Album>(alb);
        }
    }
}
