using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using static SchemeCreator.Data.Scheme;
using System.Linq;
using Windows.Foundation;

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

namespace SchemeCreator
{
    public sealed partial class MainPage : Page
    {
        //constructor
        public MainPage()
        {
            InitializeComponent();

            NewElementBt.Click += NewElementBt_Click;
            NewLineBt.Click += NewLineBt_Click;
            ChangeValueBt.Click += ChangeValueBt_Click;
            WorkBt.Click += WorkBt_Click;
            TraceBt.Click += TraceBt_Click;
            DeleteLineBt.Click += DeleteLineBt_Click;
            QuitBt.Click += QuitBt_Click;

            if (addGateMode)
            {
                //saving data to gateInfo
                Data.GateController.gateInfo.Add(new Data.GateInfo(userPoints[0], Data.NewGateData.id, Data.NewGateData.inputs));

                if (Data.NewGateData.id == (int)GateId.IN || Data.NewGateData.id == (int)GateId.OUT)
                {
                    //creating IN/OUT
                    Data.GateController.CreateInOut(userPoints[0], Data.NewGateData.id, false);

                    //subscribing to events
                    Data.GateController.gates[Data.GateController.gates.Count - 1].title.Tapped += GateName_Tapped;
                }
                else
                {
                    //creating logic gate
                    Data.GateController.CreateGate(userPoints[0], Data.NewGateData.id, Data.NewGateData.inputs, null);

                    //subscribing to events
                    foreach (var ellipse in from Data.Gate gate in Data.GateController.gates
                                            from Ellipse ellipse in Data.GateController.gates[Data.GateController.gates.Count - 1].inputEllipse
                                            select ellipse)
                    { ellipse.Tapped += GateInOut_Tapped; }
                }
            }

            UpdatePage();

            addGateMode = false;
        }


        //----------------------------------------------------------------------------------------------------------//
        // event handlers for buttons

        private void ChangeValueBt_Click(object sender, RoutedEventArgs e) => changeValueMode = true;
        private void NewLineBt_Click(object sender, RoutedEventArgs e) => addLineStartMode = true;
        private void NewElementBt_Click(object sender, RoutedEventArgs e) => addGateMode = true;
        private void DeleteLineBt_Click(object sender, RoutedEventArgs e) => deleteLineMode = true;
        private void TraceBt_Click(object sender, RoutedEventArgs e) => Tracing();
        private void QuitBt_Click(object sender, RoutedEventArgs e) => Application.Current.Exit();
        private void WorkBt_Click(object sender, RoutedEventArgs e)
        {
            // Clearing the elements data
            foreach (var gate in from Data.Gate gate in Data.GateController.gates
                                 where gate.id != (int)GateId.IN
                                 select gate)
            { gate.outputValue = null; }

            // Sends signals from outputs to inputs
            foreach (Line l in Data.LineController.lines)
                SendValue(Data.Scheme.GetValue(l), l);
        }

        //----------------------------------------------------------------------------------------------------------//
        // dynamic event handlers

        private void GateName_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (addLineStartMode)
            {
                if ((e.OriginalSource as TextBlock).Text != gateNames[(int)GateId.IN])
                    return;

                //saving line start X and Y
                SaveUserPointFromTextBlock(1, e.OriginalSource as TextBlock);

                addLineStartMode = false;
                addLineEndMode = true;
            }
            else if (addLineEndMode)
            {
                if ((e.OriginalSource as TextBlock).Text != gateNames[(int)GateId.OUT])
                    return;

                //saving line end X and Y
                SaveUserPointFromTextBlock(2, e.OriginalSource as TextBlock);

                if (userPoints[1] == userPoints[2])
                    return;

                for (int i = 0; i < Data.GateController.gates.Count; i++)
                {
                    if (Data.GateController.gates[i].title != e.OriginalSource as TextBlock)
                        continue;
                    if (Data.GateController.gates[i].isReserved)
                        continue;

                    //adding new line
                    Data.LineController.lineInfo.Add(new Data.LineInfo(userPoints[1], userPoints[2]));
                    Line line = Data.LineController.CreateLine(userPoints[1], userPoints[2]);
                    line.Tapped += Line_Tapped;
                    WorkSpace.Children.Add(line);

                    //reserving flags
                    Data.GateController.gates[i].isReserved = true;
                    Data.GateController.gateInfo[i].isInputsReserved[0] = true;

                    //switching off the mode
                    addLineEndMode = false;
                }
            }
            else if (changeValueMode)
            {
                foreach (var g in from Data.Gate g in Data.GateController.gates
                                  where g.title == e.OriginalSource as TextBlock
                                  where g.id == (int)GateId.IN
                                  select g)
                {
                    g.outputValue = !g.outputValue;
                    g.ColorByValue();
                    break;
                }

                changeValueMode = false;
            }
        }

        private void Dot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!addGateMode)
                return;

            //saving element X and Y
            SaveUserPointFromEllipse(0, e.OriginalSource as Ellipse);

            WorkSpace.Children.Clear();
            Frame.Navigate(typeof(UI.Dialogs.AddElement));
        }

        private void GateInOut_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //function for inputs and outputs of the element
            //has 2 modes
            if (addLineStartMode)
            {
                //saving only from outputs
                foreach (Data.Gate gate in Data.GateController.gates)
                    if (gate.outputEllipse == null) //looking for regular gates
                        continue;
                    else if ((e.OriginalSource as Ellipse).Margin == gate.outputEllipse.Margin) //looking for gate outputs
                    {
                        //saving 1st element's X and Y
                        SaveUserPointFromEllipse(1, e.OriginalSource as Ellipse);

                        //switching to next mode
                        addLineStartMode = false;
                        addLineEndMode = true;
                    }
            }
            else if (addLineEndMode)
            {
                //saving 2nd element's X and Y
                SaveUserPointFromEllipse(2, e.OriginalSource as Ellipse);

                if (userPoints[1] == userPoints[2])
                    return;

                //if choosed element is not reserved
                for (int j = 0; j < Data.GateController.gates.Count; j++)
                {
                    //for IN/OUT elements 
                    if (Data.GateController.gates[j].inputEllipse == null)
                        continue;

                    for (int i = 0; i < Data.GateController.gates[j].inputEllipse.Length; i++)
                    {
                        if ((e.OriginalSource as Ellipse) != Data.GateController.gates[j].inputEllipse[i])
                            continue;

                        if (Data.GateController.gates[j].inputReserved[i])
                            continue;

                        //creating line and saving it to the current grid
                        Data.LineController.lineInfo.Add(new Data.LineInfo(userPoints[1], userPoints[2]));
                        Line line = Data.LineController.CreateLine(userPoints[1], userPoints[2]);
                        line.Tapped += Line_Tapped;
                        WorkSpace.Children.Add(line);

                        //reserving the input
                        Data.GateController.gates[j].inputReserved[i] = true;
                        Data.GateController.gateInfo[j].isInputsReserved[i] = true;

                        //switching off the mode
                        addLineEndMode = false;
                    }
                }
            }
        }
        private void Line_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!deleteLineMode)
                return;
            
            for (int i = 0; i < Data.LineController.lineInfo.Count; i++)
            {
                if (Data.LineController.lineInfo[i].point1 == new Point((e.OriginalSource as Line).X1, (e.OriginalSource as Line).Y1))
                    if (Data.LineController.lineInfo[i].point2 == new Point((e.OriginalSource as Line).X2, (e.OriginalSource as Line).Y2))
                        Data.LineController.lineInfo.Remove(Data.LineController.lineInfo[i]);
            }

            deleteLineMode = true;
            UpdatePage();
        }

        //----------------------------------------------------------------------------------------------------------//
        // local functions

        private void UpdatePage()
        {
            WorkSpace.Children.Clear();

            Data.DotController.dots.Clear();
            Data.LineController.lines.Clear();
            Data.GateController.gates.Clear();

            Data.DotController.InitNet(Window.Current.Bounds.Width, Window.Current.Bounds.Height);
            Data.LineController.ReloadLines();
            Data.GateController.ReloadGates();

            foreach (Ellipse ellipse in Data.DotController.dots)
            {
                ellipse.Tapped += Dot_Tapped;
                WorkSpace.Children.Add(ellipse);
            }

            foreach (Line line in Data.LineController.lines)
            {
                WorkSpace.Children.Add(line);
                line.Tapped += Line_Tapped;
            }
                

            //gates
            foreach (Data.Gate g in Data.GateController.gates)
            {
                WorkSpace.Children.Add(g.body);
                WorkSpace.Children.Add(g.title);
                g.title.Tapped += GateName_Tapped;

                if (g.id == (int)GateId.IN || g.id == (int)GateId.OUT)
                    continue;

                //gate out
                WorkSpace.Children.Add(g.outputEllipse);
                g.outputEllipse.Tapped += GateInOut_Tapped;

                //gate in
                foreach (Ellipse e in g.inputEllipse)
                {
                    WorkSpace.Children.Add(e);
                    e.Tapped += GateInOut_Tapped;
                }
            }
        }

        private void Tracing()
        {
            Data.Tracing.ShowTracing();

            foreach (TextBlock tb in Data.Tracing.textBlocks)
                WorkSpace.Children.Add(tb);
        }
    }
}