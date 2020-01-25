﻿using K_nearest_neighbors.Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            foreach(var dataPoint in dataPoints)
            {
                if(dataPoint.AssignedClassification != null)
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

            foreach(var dataObject in distancedObjects)
            {
                leastDistant.Add(dataObject);
                i++;
                if (i > amount)
                    break;
            }

            return leastDistant;
        }

        public static int ExtractClassification(IEnumerable<DataPointDto> shortestDistances)
        {
            Dictionary<int, List<DataPointDto>> possibleClassifications = new Dictionary<int, List<DataPointDto>>();

            foreach(var data in shortestDistances)
            {
                if(!possibleClassifications.ContainsKey((int)data.AssignedClassification))
                    possibleClassifications.Add((int)data.AssignedClassification, new List<DataPointDto>());
                possibleClassifications[(int)data.AssignedClassification].Add(data);
            }

            possibleClassifications = possibleClassifications.OrderByDescending(x => x.Value.Count()).ToDictionary(x => x.Key, x=> x.Value);

            int value = possibleClassifications.FirstOrDefault().Value.Count();
            int repeats = 0;
            foreach(var classification in possibleClassifications)
            {
                if(value == classification.Value.Count())
                {
                    repeats++;
                }
            }

            if(repeats > 1)
            {
                Random random = new Random();
                int randomNumber = random.Next(0, repeats);
                int i = 0;
                foreach(var classification in possibleClassifications)
                {
                    if (i == randomNumber)
                        return classification.Key;
                    i++;
                }
            }

            return possibleClassifications.FirstOrDefault().Key;
        }
    }
}
