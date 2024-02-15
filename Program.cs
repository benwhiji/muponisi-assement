namespace TechTest1
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    internal class Program
    {
        private static void Main(string[] args)
        {
            var records = new List<Record>();
            
            List<CordinateModel> fixedCordinates = new List<CordinateModel>();
            fixedCordinates.Add(new CordinateModel { Position = 1, X = 34.544909f, Y = -102.100843f });
            fixedCordinates.Add(new CordinateModel { Position = 2, X = 32.345544f, Y = -99.123124f });
            fixedCordinates.Add(new CordinateModel { Position = 3, X = 33.234235f, Y = -100.214124f });
            fixedCordinates.Add(new CordinateModel { Position = 4, X = 35.195739f, Y = -95.348899f });
            fixedCordinates.Add(new CordinateModel { Position = 5, X = 31.895839f, Y = -97.789573f });
            fixedCordinates.Add(new CordinateModel { Position = 6, X = 32.895839f, Y = -101.789573f });
            fixedCordinates.Add(new CordinateModel { Position = 7, X = 34.115839f, Y = -100.225732f });
            fixedCordinates.Add(new CordinateModel { Position = 8, X = 32.335839f, Y = -99.992232f });
            fixedCordinates.Add(new CordinateModel { Position = 9, X = 33.535339f, Y = -94.792232f });
            fixedCordinates.Add(new CordinateModel { Position = 10, X = 32.234235f, Y = -100.222222f });

            
            using (var stream = new FileStream("VehiclePositions.dat", FileMode.Open, FileAccess.Read))
            {
                using (var reader = new BinaryReader(stream, Encoding.Default))
                {
                
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        
                        var record = new Record
                        {
                            Int32Field = reader.ReadInt32(),
                            StringField = ReadNullTerminatedString(reader),
                            X = reader.ReadSingle(),
                            Y = reader.ReadSingle(),
                            
                            DateTimeField = DateTimeOffset.FromUnixTimeSeconds((long)reader.ReadUInt64()).DateTime
                        };

                        records.Add(record);
                    }
                }
            }
            
            foreach (var item in fixedCordinates)
            {
                
                var nearestRecords = records.OrderBy(r => Distance(r, item)).Take(1).ToList();
                Console.WriteLine();

                Console.WriteLine(item.Position + " Nearest Records");
                Console.WriteLine();
            
                Console.WriteLine("{0,-10} {1,-30} {2,-15} {3,-15} {4,-30} {5,-30}", "VehicleID", "Registration", "Longitude", "Latitude", "Recorded Time UTC", "Dis from Target");
                Console.WriteLine(new string('-', 110)); 

                foreach (var record in nearestRecords)
                {
                    Console.WriteLine("{0,-10} {1,-30} {2,-15} {3,-15} {4,-30} {5,-30}",

                        record.Int32Field,
                        record.StringField,
                        record.X,
                        record.Y,
                        record.DateTimeField,
                        Math.Round(Distance(record, item), 0) + " meters");
                }
            }
        }

      
        private static string ReadNullTerminatedString(BinaryReader reader)
        {
            var bytes = new List<byte>();
            byte b;
            while ((b = reader.ReadByte()) != 0)
                bytes.Add(b);
            return Encoding.Default.GetString(bytes.ToArray());
        }

        
        private static double Distance(Record record, dynamic fixedCoordinate)
        {
            var R = 6371e3;
            var lat1 = DegreesToRadians(record.X);
            var lat2 = DegreesToRadians(fixedCoordinate.X);
            var deltaLat = DegreesToRadians(fixedCoordinate.X - record.X);
            var deltaLon = DegreesToRadians(fixedCoordinate.Y - record.Y);

            var a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                    Math.Cos(lat1) * Math.Cos(lat2) *
                    Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c; // in metres
        }

        private static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }
    }

    public class Record
    {
        public int Int32Field { get; set; }
        public string StringField { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public DateTime DateTimeField { get; set; }
        public UInt64 TimeStamp { get; set; }
    }

    public class CordinateModel
    {
        public int Position { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}
