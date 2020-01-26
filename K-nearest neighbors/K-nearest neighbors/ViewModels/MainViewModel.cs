using Caliburn.Micro;
using K_nearest_neighbors.Common.Models;
using K_nearest_neighbors.Data_Access;
using K_nearest_neighbors.Data_Access.Repositories;
using K_nearest_neighbors.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
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

        private ObservableCollection<DataPointLine> _dataPointLines;

        public ObservableCollection<DataPointLine> DataPointLines
        {
            get { return _dataPointLines; }
            set 
            { 
                _dataPointLines = value;
                NotifyOfPropertyChange(() => DataPointLines);
            }
        }


        private Dictionary<int, List<ColoredDataPoint>> _classifiedDataPoints;

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

        private int _maxPoints;

        public int MaxPoints
        {
            get { return _maxPoints; }
            set 
            {
                if (Points == null || Points.Count() == null)
                    return;

                _maxPoints = Points.Count() <= 10 ? Points.Count() : 10;
                NotifyOfPropertyChange(() => MaxPoints);
            }
        }

        public int DifferentTypes { get; set; }

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

        private float _panelX;

        public float PanelX
        {
            get { return _panelX; }
            set 
            { 
                _panelX = value;
                NotifyOfPropertyChange(() => PanelX);
            }
        }

        private float _panelY;

        public float PanelY
        {
            get { return _panelY; }
            set 
            { 
                _panelY = value;
                NotifyOfPropertyChange(() => PanelY);
            }
        }

        public float WIDTH => 600;
        public float HEIGHT => 300;

        public MainViewModel()
        {
            NewPoint = new DataPointDto() { X = 1, Y = 1};
            CurrentKValue = 1;
            PrepareWindow();
        }

        #region Event
        public async void AddDataPointFromCanvas(Canvas canvas)
        {
            if (PointLimits == null)
                PointLimits = new Point(WIDTH, HEIGHT);

            var mousePosition = Mouse.GetPosition(canvas);
            DataPointDto pointDto = new DataPointDto();
            pointDto.X = (float)(PanelX / ((WIDTH - 20) + 10)) * PointLimits.X;
            pointDto.Y = (float)(PanelY / ((HEIGHT - 20) + 10)) * PointLimits.Y;

            await SavePoint(pointDto);
            PrepareWindow();
        }

        public void RefreshColors()
        {
            PrepareWindow();
        }

        public void DataPointClicked(ColoredDataPoint coloredDataPoint)
        {
            System.Windows.MessageBox.Show($"ID: {coloredDataPoint.Id}", "Data point id");
        }

        public void LoadFromFile()
        {
            try
            {
                var dataPoints = FileReader.ReadFromFileDto();
                var pointRepository = new DataPointRepository(new ClassificationContext());
                foreach (var pnt in dataPoints)
                {
                    SavePoint(pnt);
                }
                PrepareWindow();
            }
            catch
            {
                System.Windows.MessageBox.Show("Invalid data format in file", "Invalid", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        #endregion

        private void PrepareWindow()
        {
            InitializeBaseVariables();
            GetPoints();
            SetPointClassification(Points);
            SetDataPointColors();
            if (Points != null)
                MaxPoints = 10;
        }

        private void InitializeBaseVariables()
        {
            DataPointLines = new ObservableCollection<DataPointLine>();
            Points = new BindableCollection<ColoredDataPoint>();
            _classifiedDataPoints = new Dictionary<int, List<ColoredDataPoint>>();
            CanvasMesurements = new Point(WIDTH, HEIGHT);
        }

        private void GetPoints()
        {
            var pointRepository = new DataPointRepository(new ClassificationContext());
            var dataPoints = pointRepository.GetAllDto();
            PointLimits = pointRepository.GetPointLimits();

            foreach (var point in dataPoints)
            {
                PointFitToCanvas(point);
                Points.Add(new ColoredDataPoint((DataPointDto)point));
            }
        }

        private void PointFitToCanvas(IGenericDto point)
        {
            (point as DataPointDto).X = ((point as DataPointDto).X / PointLimits.X) * (WIDTH - 20) + 10;
            (point as DataPointDto).Y = ((point as DataPointDto).Y / PointLimits.Y) * (HEIGHT - 20) + 10;
        }

        public async void AddPoint()
        {
            var pointRepository = new DataPointRepository(new ClassificationContext());
            await pointRepository.CreateNewDataPoint(NewPoint);

            PrepareWindow();
        }

        private async Task SavePoint(DataPointDto point)
        {
            var pointRepository = new DataPointRepository(new ClassificationContext());
            await pointRepository.CreateNewDataPoint(point);
        }

        public void DrawLines(ColoredDataPoint baseDataPoint)
        {
            if(!CanDrawLines(baseDataPoint))
                return;

            DataPointLines = new BindableCollection<DataPointLine>();

            CalculateDataPointDistances(baseDataPoint);
            var ClassifiedDataPoints = DataController.GetClassifiedDataPoints(Points);
            var compeatingDataPoints = DataController.GetSmallestDistances(ClassifiedDataPoints, CurrentKValue);
            foreach (var compeatingDataPoint in compeatingDataPoints)
            {
                DataPointLines.Add(new DataPointLine(baseDataPoint, compeatingDataPoint, 5));
            }
        }

        private void SetPointClassification(BindableCollection<ColoredDataPoint> points)
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
        }

        private void SetDataPointColors()
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
            if (!DoesDataExist())
                return;

            ProcessClassificationAsync();

            PrepareWindow();
        }

        private void ProcessClassificationAsync()
        {
            //getting all unclassified objects
            foreach (var unclassifiedData in _classifiedDataPoints[-1])
            {
                CalculateDataPointDistances(unclassifiedData);
                AssignClassification(unclassifiedData);
                UpdateDataPointsAsync(unclassifiedData);
            }
        }

        private static async void UpdateDataPointsAsync(ColoredDataPoint unclassifiedData)
        {
            if (unclassifiedData.AssignedClassification == null)
                throw new Exception("the assigning failed"); 
            var pointRepository = new DataPointRepository(new ClassificationContext());
            await pointRepository.SaveAssignedClassification(unclassifiedData.Id, (int)unclassifiedData.AssignedClassification);
        }

        private void CalculateDataPointDistances(ColoredDataPoint unclassifiedData)
        {
            //going trough all possible classifications
            foreach (var classificationType in _classifiedDataPoints)
            {
                //if classification not set
                if (classificationType.Key == -1)
                    continue;

                //find distance
                foreach (var classifiedData in classificationType.Value)
                {
                    classifiedData.Distance = DataController.GetDistance(classifiedData, unclassifiedData);
                }
            }
        }

        private void AssignClassification(DataPointDto unassignedDataPoint)
        {
            var ClassifiedDataPoints = DataController.GetClassifiedDataPoints(Points);
            var compeatingDataPoints = DataController.GetSmallestDistances(ClassifiedDataPoints, CurrentKValue);
            unassignedDataPoint.AssignedClassification = DataController.ExtractClassification(compeatingDataPoints);
        }

        private bool CanDrawLines(ColoredDataPoint baseDataPoint)
        {
            if (!_classifiedDataPoints.ContainsKey(-1))
                return false;
            if (_classifiedDataPoints[-1].Count() == 0)
                return false;
            if (Points.Count() == _classifiedDataPoints[-1].Count())
                return false;
            if (baseDataPoint.AssignedClassification != null)
                return false;
            return true;
        }

        private bool DoesDataExist()
        {
            if (!_classifiedDataPoints.ContainsKey(-1))
                return false;
            if (_classifiedDataPoints[-1].Count == 0)
                return false;
            return true;
        }


    }
}
