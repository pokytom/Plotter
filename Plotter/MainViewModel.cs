using OxyPlot.Axes;

namespace Plotter
{
    using OxyPlot;
    using OxyPlot.Series;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows.Input;

    public class MainViewModel : INotifyPropertyChanged
    {
        private double width;
        private double height;
        private double uShapeCutoutWidth;
        private double uShapeCutoutHeight;
        private PlotModel model;

        public MainViewModel()
        {
            Width = 10; // Default width
            Height = 10; // Default height
            UShapeCutoutWidth = 2; // Default U-shape cutout width
            UShapeCutoutHeight = 2; // Default U-shape cutout height
            UpdateCommand = new RelayCommand(UpdatePlot);
            DrawSquareCommand = new RelayCommand(DrawSquare);
            DrawUShapeCommand = new RelayCommand(DrawUShape);
            UpdatePlot();
        }

        public double Width
        {
            get => width;
            set
            {
                if (width != value)
                {
                    width = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Height
        {
            get => height;
            set
            {
                if (height != value)
                {
                    height = value;
                    OnPropertyChanged();
                }
            }
        }

        public double UShapeCutoutWidth
        {
            get => uShapeCutoutWidth;
            set
            {
                if (uShapeCutoutWidth != value)
                {
                    uShapeCutoutWidth = value;
                    OnPropertyChanged();
                }
            }
        }

        public double UShapeCutoutHeight
        {
            get => uShapeCutoutHeight;
            set
            {
                if (uShapeCutoutHeight != value)
                {
                    uShapeCutoutHeight = value;
                    OnPropertyChanged();
                }
            }
        }

        public PlotModel Model
        {
            get => model;
            private set
            {
                model = value;
                OnPropertyChanged();
            }
        }

        public ICommand UpdateCommand { get; }
        public ICommand DrawSquareCommand { get; }
        public ICommand DrawUShapeCommand { get; }

        // Method to update plot based on current dimensions (for both square and U-shape)
        private void UpdatePlot()
        {
            var tmp = new PlotModel { Title = "Geometry Plot" };

            // Přidejte osy s přizpůsobenými barvami
            tmp.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Bottom, // X-os
                Minimum = -10,
                Maximum = Width,
                AbsoluteMinimum = -10,
                AbsoluteMaximum = Width,
                TextColor = OxyColors.LightGray, // Barva textu popisků osy X
                MajorGridlineColor = OxyColors.Gray, // Barva hlavní mřížky
                MinorGridlineColor = OxyColors.Gray, // Barva vedlejší mřížky
                AxislineColor = OxyColors.White, // Barva samotné osy
            });

            tmp.Axes.Add(new LinearAxis
            {
                Position = AxisPosition.Left, // Y-os
                Minimum = -10,
                Maximum = Height,
                AbsoluteMinimum = -10,
                AbsoluteMaximum = Height,
                TextColor = OxyColors.LightGray, // Barva textu popisků osy Y
                MajorGridlineColor = OxyColors.Gray,
                MinorGridlineColor = OxyColors.Gray,
                AxislineColor = OxyColors.White,
            });

            // Pokračujte s definováním série
            var rectangle = new LineSeries { Title = "Shape", MarkerType = MarkerType.None };
            if (isUShape)
                DrawUShape(rectangle);
            else
                DrawSquare(rectangle);

            tmp.Series.Add(rectangle);
            Model = tmp;
        }


        // Method to draw square
        private void DrawSquare(LineSeries series)
        {
            double halfWidth = Width / 2;
            double halfHeight = Height / 2;

            // Points for square
            series.Points.Clear();
            series.Points.Add(new DataPoint(-halfWidth, -halfHeight));
            series.Points.Add(new DataPoint(halfWidth, -halfHeight));
            series.Points.Add(new DataPoint(halfWidth, halfHeight));
            series.Points.Add(new DataPoint(-halfWidth, halfHeight));
            series.Points.Add(new DataPoint(-halfWidth, -halfHeight)); // Close the square
        }

        // Method to draw U-shape with cutout dimensions
        private void DrawUShape(LineSeries series)
        {
            double halfWidth = Width / 2;
            double halfHeight = Height / 2;
            double cutoutWidth = UShapeCutoutWidth / 2; // Half the width for symmetry
            double cutoutHeight = UShapeCutoutHeight / 2; // Half the height for symmetry

            // Points for U-shape with cutout
            series.Points.Clear();

            // Left vertical line
            series.Points.Add(new DataPoint(-halfWidth, -halfHeight));
            series.Points.Add(new DataPoint(-halfWidth, halfHeight - cutoutHeight));

            // Right vertical line
            series.Points.Add(new DataPoint(halfWidth, halfHeight - cutoutHeight));
            series.Points.Add(new DataPoint(halfWidth, -halfHeight));

            // Closing the shape: bottom line
            series.Points.Add(new DataPoint(-halfWidth + cutoutWidth, -halfHeight));
            series.Points.Add(new DataPoint(halfWidth - cutoutWidth, -halfHeight));

            // Closing the top part of the U-shape
            series.Points.Add(new DataPoint(-halfWidth, halfHeight - cutoutHeight));
            series.Points.Add(new DataPoint(halfWidth, halfHeight - cutoutHeight));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private bool isUShape = false;
        public void DrawSquare() => isUShape = false; // set flag to false for square
        public void DrawUShape() => isUShape = true; // set flag to true for U-shape

        // For plot resizing
        public double PlotHeight => Height + 50;  // Adjusting the height
        public double PlotWidth => Width + 50;    // Adjusting the width
    }

    public class RelayCommand : ICommand
    {
        private readonly Action execute;
        private readonly Func<bool> canExecute;

        public RelayCommand(Action execute, Func<bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object parameter) => canExecute == null || canExecute();
        public void Execute(object parameter) => execute();
        public event EventHandler CanExecuteChanged;
    }
}
