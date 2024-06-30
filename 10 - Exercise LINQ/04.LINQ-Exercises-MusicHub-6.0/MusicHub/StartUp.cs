namespace MusicHub
{
    using System;
    using System.Text;
    using Data;
    using Initializer;

    public class StartUp
    {
        public static void Main()
        {
            MusicHubDbContext context =
                new MusicHubDbContext();

            DbInitializer.ResetDatabase(context);

            Console.WriteLine(ExportSongsAboveDuration(context, 4));
        }

        public static string ExportAlbumsInfo(MusicHubDbContext context, int producerId)
        {
            var albumsInfo = context
                .Producers.First(p => p.Id == producerId)
                .Albums.Select(a => new
                {
                    AlbumName = a.Name,
                    RealeaseDate = a.ReleaseDate.ToString("MM/dd/yyyy"),
                    ProducerName = a.Producer.Name,
                    Songs = a.Songs.Select(s => new
                    {
                        SongName = s.Name,
                        SongPrice = s.Price,
                        WriteName = s.Writer.Name
                    })
                    .OrderByDescending(s => s.SongName)
                    .ThenBy(s => s.WriteName),
                   AlbumPrice =  a.Price
                })
                .OrderByDescending(a => a.AlbumPrice)
                ; 
                
            StringBuilder sb = new StringBuilder();

            var abumInfoEnum = albumsInfo.ToList();
            
            foreach (var album in abumInfoEnum)
            {
                int countSong = 1;
                sb.AppendLine($"-AlbumName: {album.AlbumName}");
                sb.AppendLine($"-ReleaseDate: {album.RealeaseDate}");
                sb.AppendLine($"-ProducerName: {album.ProducerName}");
                sb.AppendLine($"-Songs:");

                foreach (var song in album.Songs)
                {
                    sb.AppendLine($"---#{countSong}");
                    sb.AppendLine($"---SongName: {song.SongName}");
                    sb.AppendLine($"---Price: {song.SongPrice:f2}");
                    sb.AppendLine($"---Writer: {song.WriteName}");
                    countSong++;
                }

                sb.AppendLine($"-AlbumPrice: {album.AlbumPrice:f2}");

            }

            return sb.ToString().TrimEnd();
        }

        public static string ExportSongsAboveDuration(MusicHubDbContext context, int duration)
        { 
            
            
          StringBuilder sb = new StringBuilder();


           return sb.ToString().TrimEnd();
        }
    }
}
