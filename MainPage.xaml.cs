using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

//Разработать ПО для моделирования простых комбинационных логических схем.

//Комбинационные схемы задаются в виде набора входных и выходных контактов, логических элементов
//(OR, AND, NOR, NOT, NAND, NOR, XNOR) и связей между ними.

//Необходимо реализовать алгоритм трассировки линий межэлементных соединений с возможностью дальнейшего редактирования
//(удаления линии, перемещения сегментов линий).

//Желательно реализовать возможность подключения линии к выходу другого элемента, входному/выходному контакту и другой линии
//(по аналогии с Electronics Workbench).

//Моделирование заключается в изменении значений на входах (по щелчку мышкой или нажатию на определенную клавишу,
//которая задается в свойствах входного порта).

//При этом выполняется распространение сигнала по всем линиям межсоединений с изменением отображения линии и
//вычисление значений внутренних сигнальных соединений.

//Количество входных и выходных контактов – произвольное,
//количество информационных входов логических элементов должно быть конфигурируемым через страницу свойств(от 2 до 8).

//Состав библиотеки логических элементов может быть расширен другими функциональными компонентами
//(мультиплексоры, дешифраторы, компараторы, триггеры) по желанию разработчиков.

namespace SchemeCreator {
    public sealed partial class MainPage : Page {

        Data.Scheme scheme = new Data.Scheme();
        public MainPage() {

            InitializeComponent();

            var test = new SchemeCreator.Test.Test();

            Content = scheme.frameManager.Grid;
            SizeChanged += MainPageSizeChanged;
        }

        private void MainPageSizeChanged(object sender, SizeChangedEventArgs e) {
            
            scheme.frameManager.Grid.Height = ActualHeight;
            scheme.frameManager.Grid.Width = ActualWidth;
            
            scheme.frameManager.SizeChanged(ActualWidth, ActualHeight);
        }
    }
}