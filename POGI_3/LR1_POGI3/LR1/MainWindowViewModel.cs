using System;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.ObjectModel;

namespace LR1
{
    /// <summary>
    /// ViewModel часть программы
    /// </summary>
    internal class MainWindowViewModel : INotifyPropertyChanged
    {
        private ShapesCreator _shapeCreator;
        public ShapesCreator ShapeCreator
        {
            get => _shapeCreator;
            set
            {
                _shapeCreator = value;
                OnPropertyChanged();
            }
        }
        // Свойства (и переменные к ним) для биндинга оконных элементов
        public ObservableCollection<Line> Lines { get; set; }
        private Shape _selectedShape;
        /// <summary>
        /// Выделеная фигура
        /// </summary>
        public Shape SelectedShape
        {
            get => _selectedShape;
            private set
            {
                _selectedShape = value;
                OnPropertyChanged();
            }
        }
        private double _xSlider;
        /// <summary>
        /// Слайдер перемещения по X
        /// </summary>
        public double XSlider
        {
            get => _xSlider;
            set
            {
                if (SelectedShape == null) return;
                _xSlider = value;
                SelectedShape.ShiftSlider(value, 0);
                OnPropertyChanged();
            }
        }
        private double _ySlider;
        /// <summary>
        /// Слайдер перемещения по Y
        /// </summary>
        public double YSlider
        {
            get => _ySlider;
            set
            {
                if (SelectedShape == null) return;
                _ySlider = value;
                SelectedShape.ShiftSlider(0, value);
                OnPropertyChanged();
            }
        }
        private double _lineSlider;
        /// <summary>
        /// Слайдер изменения толщины линии
        /// </summary>
        public double LineSlider
        {
            get => _lineSlider;
            set
            {
                if (SelectedShape != null)
                {
                    _lineSlider = value;
                    SelectedShape.LineThickness = _lineSlider;
                    OnPropertyChanged();
                }
                else
                {
                    _lineSlider = 3;
                    OnPropertyChanged();
                }
            }
        }

        // Команды для кнопок
        public RelayCommand RandomTriangleClick {  get; private set; }
        public RelayCommand RandomRectangleClick { get; private set; }
        public RelayCommand SquareClick { get; private set; }

        // Конструктор
        public MainWindowViewModel()
        {
            ShapeCreator = new ShapesCreator();
            Lines = new ObservableCollection<Line>();

            RandomTriangleClick = new RelayCommand(param => AddLines(SubscribeShape(ShapeCreator.CreateTriangle())));
            RandomRectangleClick = new RelayCommand(param => AddLines(SubscribeShape(ShapeCreator.CreateRectangle())));
            SquareClick = new RelayCommand(param => AddLines(SubscribeShape(ShapeCreator.CreateSquare())));

            ResetSliders(0, 0, 3);
        }

        /// <summary>
        /// Подписка на событие нажатия на фигуру.
        /// </summary>
        /// <param name="shape">Фигура для подписки</param>
        /// <returns>Возвращает фигуру на которую была сделана подписка</returns>
        private Shape SubscribeShape(Shape shape)
        {
            shape.ShapeClick += NewSelectedShape;
            return shape;
        }

        /// <summary>
        /// Добавление линий в коллекцию.
        /// </summary>
        /// <param name="shape">Источник линий</param>
        private void AddLines(Shape shape)
        {
            foreach (var dLine in shape.Lines)
                Lines.Add(dLine.Line);
        }

        /// <summary>
        /// Обновление выбранной фигуры.
        /// Вызывается при нажатии на фигуру.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NewSelectedShape(object sender, EventArgs e)
        {
            if (SelectedShape != null) SelectedShape.Color = Brushes.Red;
            SelectedShape = (Shape)sender;
            SelectedShape.Color = Brushes.Blue;

            ResetSliders(0, 0, SelectedShape.LineThickness);
        }

        /// <summary>
        /// Обновление слайдеров.
        /// </summary>
        /// <param name="xValue">Новое значение для X слайдера</param>
        /// <param name="yValue">Новое значение для Y слайдера</param>
        /// <param name="lineValue">Новое значение для слайдера линии</param>
        private void ResetSliders(int xValue, int yValue, double lineValue)
        {
            YSlider = yValue;
            XSlider = xValue;
            LineSlider = lineValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}