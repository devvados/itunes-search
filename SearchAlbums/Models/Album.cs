using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace SearchAlbums.Models
{
    /// <summary>
    /// Альбом
    /// </summary>
    [Serializable]
    public class Album
    {
        string albumName;
        int trackCount;
        DateTime releaseDate;

        [XmlElement("Name")]
        public string AlbumName
        {
            get
            {
                return albumName;
            }
            set
            {
                albumName = value;
            }
        }

        [XmlElement("Tracks")]
        public int TrackCount
        {
            get
            {
                return trackCount;
            }

            set
            {
                trackCount = value;
            }
        }

        [XmlElement("Release")]
        public DateTime ReleaseDate
        {
            get
            {
                return releaseDate;
            }

            set
            {
                releaseDate = value;
            }
        }

        public Album() { }

        public Album(string album, int count, DateTime release)
        {
            AlbumName = album;
            TrackCount = count;
            ReleaseDate = release;
        }

    }
}
