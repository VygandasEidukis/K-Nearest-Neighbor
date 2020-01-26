using K_nearest_neighbors.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace K_nearest_neighbors.Models
{
    public static class DataController
    {
        public static float GetDistance(IMesurable mesurableClassified, IMesurable mesurableUnclassified)
        {
            float distance = (float)Math.Sqrt(Math.Pow(mesurableClassified.X - mesurableUnclassified.X, 2) + Math.Pow(mesurableClassified.Y - mesurableUnclassified.Y, 2));
            return distance;
        }

        public static List<ColoredDataPoint> GetClassifiedDataPoints(IEnumerable<ColoredDataPoint> dataPoints)
        {
            var classifiedDataPoints = new List<ColoredDataPoint>();
            foreach (var dataPoint in dataPoints)
            {
                if (dataPoint.AssignedClassification != null)
                {
                    classifiedDataPoints.Add(dataPoint);
                }
            }

            return classifiedDataPoints;
        }

        public static IEnumerable<ColoredDataPoint> GetSmallestDistances(IEnumerable<ColoredDataPoint> distancedObjects, int amount)
        {
            var leastDistant = new List<ColoredDataPoint>();

            distancedObjects = distancedObjects.OrderBy(s => s.Distance);

            int i = 0;

            foreach (var dataObject in distancedObjects)
            {
                leastDistant.Add(dataObject);
                i++;
                if (i >= amount)
                    break;
            }

            return leastDistant;
        }

        public static int ExtractClassificationDefault(IEnumerable<DataPointDto> shortestDistances)
        {
            Dictionary<int, List<DataPointDto>> possibleClassifications = ExtractDataToDictionary(shortestDistances);

            possibleClassifications = possibleClassifications.OrderByDescending(x => x.Value.Count()).ToDictionary(x => x.Key, x => x.Value);
            int repeats = GetRepeatedDataPointDistanceCount(possibleClassifications);

            if (repeats > 1)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, repeats);
                int i = 0;
                foreach (var classification in possibleClassifications)
                {
                    if (i == randomNumber)
                        return classification.Key;
                    i++;
                }
            }

            return possibleClassifications.FirstOrDefault().Key;
        }

        public static int ExtractClassification(IEnumerable<DataPointDto> shortestDistances)
        {
            Dictionary<int, List<DataPointDto>> possibleClassifications = ExtractDataToDictionary(shortestDistances);

            List<int> availableClassifications = new List<int>();
            int maxCount = 0;
            foreach(var inListClassifications in possibleClassifications.OrderByDescending(x => x.Value.Count))
            {
                if (inListClassifications.Value.Count < maxCount)
                    break;
                maxCount = inListClassifications.Value.Count;
                availableClassifications.Add(inListClassifications.Key);
            }

            if (availableClassifications.Count == 1)
                return availableClassifications[0];

            Random random = new Random();
            return availableClassifications[random.Next(0, availableClassifications.Count - 1)];
        }

        private static int GetRepeatedDataPointDistanceCount(Dictionary<int, List<DataPointDto>> possibleClassifications)
        {
            int value = possibleClassifications.FirstOrDefault().Value.Count();
            int repeats = 0;
            foreach (var classification in possibleClassifications)
            {
                if (value == classification.Value.Count())
                {
                    repeats++;
                }
            }

            return repeats;
        }

        private static Dictionary<int, List<DataPointDto>> ExtractDataToDictionary(IEnumerable<DataPointDto> shortestDistances)
        {
            Dictionary<int, List<DataPointDto>> possibleClassifications = new Dictionary<int, List<DataPointDto>>();

            foreach (var data in shortestDistances)
            {
                if (!possibleClassifications.ContainsKey((int)data.AssignedClassification))
                    possibleClassifications.Add((int)data.AssignedClassification, new List<DataPointDto>());
                possibleClassifications[(int)data.AssignedClassification].Add(data);
            }

            return possibleClassifications;
        }
    }
}
