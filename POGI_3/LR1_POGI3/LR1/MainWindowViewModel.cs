using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
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
        public ShapesCreator shapeCreator;

        // Свойства (и переменные к ним) для биндинга оконных элементов
        public ObservableCollection<Line> Lines { get; set; }
        private Shape _selectedShape;
        public Shape SelectedShape
        {
            get
            {
                return _selectedShape;
            }
            private set
            {
                _selectedShape = value;
                OnPropertyChanged(nameof(SelectedShape));
            }
        }
        private double _xSlider;
        public double XSlider
        {
            get { return _xSlider; }
            set
            {
                if (SelectedShape != null)
                {
                    _xSlider = value;
                    SelectedShape.shiftSlider(value, 0);
                    OnPropertyChanged(nameof(XSlider));
                }
            }
        }
        private double _ySlider;
        public double YSlider
        {
            get { return _ySlider; }
            set
            {
                if (SelectedShape != null)
                {
                    _ySlider = value;
                    SelectedShape.shiftSlider(0, value);
                    OnPropertyChanged(nameof(YSlider));
                }
            }
        }
        private double _lineSlider;
        public double LineSlider
        {
            get { return _lineSlider; }
            set
            {
                if (SelectedShape != null)
                {
                    _lineSlider = value;
                    SelectedShape.LineThickness = _lineSlider;
                    OnPropertyChanged(nameof(LineSlider));
                }
                else
                {
                    _lineSlider = 3;
                    OnPropertyChanged(nameof(LineSlider));
                }
            }
        }

        // Команды для кнопок
        public RelayCommand randomTriangleClick {  get; private set; }
        public RelayCommand randomRectangleClick { get; private set; }
        public RelayCommand squareClick { get; private set; }

        // Конструктор
        public MainWindowViewModel()
        {
            randomTriangleClick = new RelayCommand(param => addLines(subscribeShape(shapeCreator.createTriangle())));
            randomRectangleClick = new RelayCommand(param => addLines(subscribeShape(shapeCreator.createRectangle())));
            squareClick = new RelayCommand(param => addLines(subscribeShape(shapeCreator.createSquare())));

            shapeCreator = new ShapesCreator();
            Lines = new ObservableCollection<Line>();
            resetSliders(0, 0, 3);
        }

        /// <summary>
        /// Подписка на событие нажатия на фигуру.
        /// </summary>
        /// <param name="shape">Фигура для подписки</param>
        /// <returns>Возвращает фигуру на которую была сделана подписка</returns>
        private Shape subscribeShape(Shape shape)
        {
            shape.ShapeClicked += newSelectedShape;
            return shape;
        }

        /// <summary>
        /// Добавление линий в коллекцию.
        /// </summary>
        /// <param name="shape">Источник линий</param>
        private void addLines(Shape shape)
        {
            foreach (DecoratedLine dLine in shape.Lines)
            {
                Lines.Add(dLine.Line);
            }
        }

        /// <summary>
        /// Обновление выбранной фигуры.
        /// Вызывается при нажатии на фигуру.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newSelectedShape(object sender, EventArgs e)
        {
            if (SelectedShape != null) SelectedShape.Color = Brushes.Red;
            SelectedShape = (Shape)sender;
            SelectedShape.Color = Brushes.Blue;

            resetSliders(0, 0, SelectedShape.LineThickness);
        }

        /// <summary>
        /// Обновление слайдеров.
        /// </summary>
        /// <param name="xValue">Новое значение для X слайдера</param>
        /// <param name="yValue">Новое значение для Y слайдера</param>
        /// <param name="lineValue">Новое значение для слайдера линии</param>
        private void resetSliders(int xValue, int yValue, double lineValue)
        {
            YSlider = yValue;
            XSlider = xValue;
            LineSlider = lineValue;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}