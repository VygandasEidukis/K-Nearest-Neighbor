using K_nearest_neighbors.Common.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace K_nearest_neighbors.Data_Access.Repositories
{
    public static class FileReader
    {
        private static readonly string _path = (Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase), "data.txt")).Replace("file:\\", "");
        public static List<DataPointDto> ReadFromFileDto()
        {
            if (!File.Exists(_path))
                File.Create(_path);

            try
            {
                var dataPoints = new List<DataPointDto>();

                string[] lines = File.ReadAllLines(_path);

                foreach(var line in lines)
                {
                    DataPointDto pointDto = new DataPointDto();
                    string[] numbers = line.Split(' ');
                    if(numbers.Count() > 1)
                    {
                        pointDto.X = float.Parse(numbers[0]);
                        pointDto.Y = float.Parse(numbers[1]);
                        if (numbers.Count() > 2 && numbers[2] != "")
                            pointDto.AssignedClassification = Int32.Parse(numbers[2]);
                        dataPoints.Add(pointDto);
                    }
                    
                }

                return dataPoints;
            }
            catch
            {
                throw new Exception("Invalid data");
            }
        }
    }
}
