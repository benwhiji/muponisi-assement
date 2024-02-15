﻿using System.Security.Cryptography;

namespace ClosestVehicles;

internal static class Program
{

    
    
    public static void Main(string[] args)
    {
        MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes("VehiclePositions.dat"));
        using (BinaryReader fileReader = new BinaryReader(memoryStream))
        {



            int ID;
            string Regnumber;
            float Langitude;
            float Latitude;
            long Recordedtime;

            var vehicles = new List<Vehicle>();
            //loop through the data and create a vehicle object for each row
            while (fileReader.BaseStream.Position != fileReader.BaseStream.Length)
            {
                ID = fileReader.ReadInt32();
                Regnumber = ReadNullTerminatedString(fileReader);
                Langitude = fileReader.ReadSingle();
                Latitude = fileReader.ReadSingle();
                Recordedtime = fileReader.ReadInt64();
                vehicles.Add(new Vehicle(ID, Regnumber, Langitude, Latitude, Recordedtime));

            }

            var positions = new List<Position>();

            positions.Add(new Position("Pos 1", 34.544909f, -102.100843f));
            positions.Add(new Position("Pos 2", 32.345544f, -99.123124f));
            positions.Add(new Position("Pos 3", 33.234235f, -100.214124f));
            positions.Add(new Position("Pos 4", 35.195739f, -95.348899f));
            positions.Add(new Position("Pos 5", 31.895839f, -97.789573f));
            positions.Add(new Position("Pos 6", 32.895839f, -101.789573f));
            positions.Add(new Position("Pos 7", 34.115839f, -100.225732f));
            positions.Add(new Position("Pos 8", 32.335839f, -99.992232f));
            positions.Add(new Position("Pos 9", 33.535339f, -94.792232f));
            positions.Add(new Position("Pos 10", 32.234235f, -100.222222f));
            DateTime now = DateTime.Now;
            
            foreach (var position in positions)
            {
                foreach (var vehicle in vehicles)
                {
                    vehicle.SetDistance(position.Langitude, position.Latitude);
                }
                var closestVehicles = vehicles.OrderBy(v => v.Distance).Take(10);
                Console.WriteLine("Closest vehicles to " + position.Name );
                  Console.WriteLine("REGNUMBER Distance recordtime ");
                foreach (var vehicle in closestVehicles)
                {
                    Console.WriteLine(vehicle.Regnumber + " " + vehicle.Distance  + " " + vehicle.Recordedtime);
                }
                Console.WriteLine();
            }
            
             
                Console.WriteLine($"Time taken: {DateTime.Now - now}");
            }
        
    }

    public static string ReadNullTerminatedString(this System.IO.BinaryReader stream)
    {
        string str = "";
        char ch;
        while ((int)(ch = stream.ReadChar()) != 0)
            str = str + ch;
        return str;
    }

}