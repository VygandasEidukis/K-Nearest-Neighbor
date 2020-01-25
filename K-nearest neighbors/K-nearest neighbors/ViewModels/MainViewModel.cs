using Caliburn.Micro;
using K_nearest_neighbors.Common.Models;
using K_nearest_neighbors.Data_Access;
using K_nearest_neighbors.Data_Access.Repositories;
using K_nearest_neighbors.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media;

namespace K_nearest_neighbors.ViewModels
{
    public class MainViewModel : Screen
    {
        private DataPointDto _newPoint;

        public DataPointDto NewPoint
        {
            get { return _newPoint; }
            set 
            { 
                _newPoint = value;
                NotifyOfPropertyChange(() => NewPoint);
            }
        }


        private Point _canvasMesurements;

        public Point CanvasMesurements
        {
            get { return _canvasMesurements; }
            set
            {
                _canvasMesurements = value;
                NotifyOfPropertyChange(() => CanvasMesurements);
            }
        }

        private BindableCollection<ColoredDataPoint> _points;

        public BindableCollection<ColoredDataPoint> Points
        {
            get => _points;
            set
            {
                _points = value;
                NotifyOfPropertyChange(() => Points);
            }
        }


        private Dictionary<int, List<ColoredDataPoint>> _classifiedDataPoints;

        public int DifferentTypes { get; set; }

        public int MaxPoints => Points.Count();

        private Point _pointLimits;

        public Point PointLimits
        {
            get { return _pointLimits; }
            set
            {
                _pointLimits = value;
                NotifyOfPropertyChange(() => PointLimits);
            }
        }

        private int _currentKValue;

        public int CurrentKValue
        {
            get { return _currentKValue; }
            set
            {
                _currentKValue = value;
                NotifyOfPropertyChange(() => CurrentKValue);
            }
        }

        public float WIDTH => 600;
        public float HEIGHT => 300;


        public MainViewModel()
        {
            NewPoint = new DataPointDto() { X = 1, Y = 1
        };
            CurrentKValue = 1;
            GetPoints();
        }

        public void AddPoint()
        {
            var pointRepository = new DataPointRepository(new ClassificationContext());
            pointRepository.CreateNewDataPoint(NewPoint);
            GetPoints();
        }

        private void GetPoints()
        {
            Thread.Sleep(100);
            Points = new BindableCollection<ColoredDataPoint>();
            _classifiedDataPoints = new Dictionary<int, List<ColoredDataPoint>>();
            CanvasMesurements = new Point(WIDTH, HEIGHT);
            var pointRepository = new DataPointRepository(new ClassificationContext());

            PointLimits = pointRepository.GetPointLimits();
            var t = pointRepository.GetAllDto();

            foreach (var tt in t)
            {
                (tt as DataPointDto).X = ((tt as DataPointDto).X / PointLimits.X) * (WIDTH - 20) + 10;
                (tt as DataPointDto).Y = ((tt as DataPointDto).Y / PointLimits.Y) * (HEIGHT - 20) + 10;
                Points.Add(new ColoredDataPoint((DataPointDto)tt));
            }

            ClasifyPoints(Points);

        }

        private void ClasifyPoints(BindableCollection<ColoredDataPoint> points)
        {
            _classifiedDataPoints = new Dictionary<int, List<ColoredDataPoint>>();
            foreach (var point in points)
            {
                //if no classification is set
                if (point.AssignedClassification == null)
                {
                    if (!_classifiedDataPoints.ContainsKey(-1))
                    {
                        _classifiedDataPoints[-1] = new List<ColoredDataPoint>();
                    }
                    _classifiedDataPoints[-1].Add(point);
                    continue;
                }

                if (!_classifiedDataPoints.ContainsKey((int)point.AssignedClassification))
                {
                    _classifiedDataPoints.Add((int)point.AssignedClassification, new List<ColoredDataPoint>());
                }
                _classifiedDataPoints[(int)point.AssignedClassification].Add(point);
            }
            DifferentTypes = _classifiedDataPoints.Count();
            ColorPoints();
        }

        private void ColorPoints()
        {
            Random random = new Random();
            foreach (var dicItem in _classifiedDataPoints)
            {
                Brush brush = new SolidColorBrush(Color.FromArgb(100, (byte)random.Next(10, 250), (byte)random.Next(10, 250), (byte)random.Next(10, 250)));
                foreach (var item in dicItem.Value)
                {
                    item.brush = brush;
                    if (item.AssignedClassification == null)
                    {
                        item.brush = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));
                    }
                }
            }
        }

        public void ExecuteCalculation()
        {
            if (!_classifiedDataPoints.ContainsKey(-1))
                return;
            if(_classifiedDataPoints[-1].Count == 0 )
                return;

            //getting all unclassified objects
            foreach(var unclassifiedData in _classifiedDataPoints[-1])
            {
                //going trough all possible classifications
                foreach(var classificationType in _classifiedDataPoints)
                {
                    //if classification not set
                    if (classificationType.Key == -1)
                        continue;

                    //find distance
                    foreach(var classifiedData in classificationType.Value)
                    {
                        classifiedData.Distance = DataController.GetDistance(classifiedData, unclassifiedData);
                    }
                }

                AssignClassification(unclassifiedData);
                var pointRepository = new DataPointRepository(new ClassificationContext());
                if (unclassifiedData.AssignedClassification == null)
                    throw new Exception("the assigning failed");
                pointRepository.SaveAssignedClassification(unclassifiedData.Id, (int)unclassifiedData.AssignedClassification);
            }

            GetPoints();
        }

        private void AssignClassification(DataPointDto unassignedDataPoint)
        {
            var ClassifiedDataPoints = DataController.GetClassifiedDataPoints(Points);
            var compeatingDataPoints = DataController.GetSmallestDistances(ClassifiedDataPoints, CurrentKValue);
            unassignedDataPoint.AssignedClassification = DataController.ExtractClassification(compeatingDataPoints);
        }
    }
}
