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

            NewSchemeBt.Click += NewSchemeBt_Click;
            NewElementBt.Click += NewElementBt_Click;
            NewLineBt.Click += NewLineBt_Click;
            ChangeValueBt.Click += ChangeValueBt_Click;
            WorkBt.Click += WorkBt_Click;
            TraceBt.Click += TraceBt_Click;
            QuitBt.Click += QuitBt_Click;

            if (!SchemeCreated)
                return;

            switch (NewElementName)
            {
                case "IN":
                case "OUT":
                    Data.GateController.gateInfo.Add(new Data.GateInfo(userPoints[0], NewElementName, 0));
                    Data.GateController.CreateInOut(userPoints[0], NewElementName);
                    Data.GateController.gates[Data.GateController.gates.Count - 1].gateName.Tapped += GateName_Tapped;
                    break;

                default:
                    {
                        Data.GateController.gateInfo.Add(new Data.GateInfo(userPoints[0], NewElementName, NewGateInputs));
                        Data.GateController.CreateGate(userPoints[0], NewElementName, NewGateInputs);

                        foreach (var ellipse in from Data.Gate gate in Data.GateController.gates
                                                from Ellipse ellipse in Data.GateController.gates[Data.GateController.gates.Count - 1].inputEllipse
                                                select ellipse)
                        { ellipse.Tapped += GateInOut_Tapped; }

                        break;
                    }
            }

            UpdatePage();

            AddGateMode = false;
        }

        //----------------------------------------------------------------------------------------------------------//
        // event handlers for buttons

        private void ChangeValueBt_Click(object sender, RoutedEventArgs e) => ChangeValueMode = true;
        private void NewLineBt_Click(object sender, RoutedEventArgs e) => AddLineStartMode = true;
        private void NewElementBt_Click(object sender, RoutedEventArgs e) => AddGateMode = true;
        private void TraceBt_Click(object sender, RoutedEventArgs e) => Tracing();
        private void QuitBt_Click(object sender, RoutedEventArgs e) => Application.Current.Exit();
        private void WorkBt_Click(object sender, RoutedEventArgs e)
        {
            // Sends signals from outputs to inputs
            foreach (Line l in Data.LineController.lines)
                SendValue(Data.Scheme.GetValue(l), l); 
        }
        
        private void NewSchemeBt_Click(object sender, RoutedEventArgs e)
        {
            // Provides initialization of grid's dots
            if (!SchemeCreated)
            {
                Data.DotController.InitNet(ActualWidth, ActualHeight);

                foreach (Ellipse ellipse in Data.DotController.dots)
                {
                    ellipse.Tapped += Dot_Tapped;
                    WorkSpace.Children.Add(ellipse);
                }

                SchemeCreated = true;
            }
        }

        //----------------------------------------------------------------------------------------------------------//
        // dynamic event handlers

        private void GateName_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AddLineStartMode)
            {
                if ((e.OriginalSource as TextBlock).Text != "IN")
                    return;

                //saving line start X and Y
                userPoints[1] = new Point((e.OriginalSource as TextBlock).Margin.Left + lineStartOffset * 2,
                    (e.OriginalSource as TextBlock).Margin.Top + lineStartOffset * 2);

                AddLineStartMode = false;
                AddLineEndMode = true;
            }
            else if (AddLineEndMode)
            {
                if ((e.OriginalSource as TextBlock).Text != "OUT")
                    return;

                //saving line end X and Y
                userPoints[2] = new Point((e.OriginalSource as TextBlock).Margin.Left + lineStartOffset * 2,
                    (e.OriginalSource as TextBlock).Margin.Top + lineStartOffset * 2);

                if (userPoints[1] == userPoints[2])
                    return;

                foreach (var gate in from Data.Gate gate in Data.GateController.gates
                                     where gate.gateName == e.OriginalSource as TextBlock
                                     where !gate.isReserved
                                     select gate)
                {
                    Data.LineController.lineInfo.Add(new Data.LineInfo(userPoints[1], userPoints[2]));
                    WorkSpace.Children.Add(Data.LineController.CreateLine(userPoints[1], userPoints[2]));

                    gate.isReserved = true;
                    AddLineEndMode = false;
                }
            }
            else if (ChangeValueMode)
            {
                foreach (var g in from Data.Gate g in Data.GateController.gates
                                  where g.gateName == e.OriginalSource as TextBlock
                                  select g)
                {
                    g.outputValue = !g.outputValue;
                    g.ColorByValue();
                    break;
                }

                ChangeValueMode = false;
            }
        }

        private void Dot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (!AddGateMode)
                return;

            //saving element X and Y
            userPoints[0] = new Point((e.OriginalSource as Ellipse).Margin.Left + lineStartOffset,
                (e.OriginalSource as Ellipse).Margin.Top + lineStartOffset);

            WorkSpace.Children.Clear();
            Frame.Navigate(typeof(UI.Dialogs.AddElement));
        }

        private void GateInOut_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //function for inputs and outputs of the element
            //has 2 modes
            if (AddLineStartMode)
            {
                //saving 1st element's X and Y
                userPoints[1] = new Point((e.OriginalSource as Ellipse).Margin.Left + lineStartOffset,
                    (e.OriginalSource as Ellipse).Margin.Top + lineStartOffset);

                //switching to next mode
                AddLineStartMode = false;
                AddLineEndMode = true;
            }
            else if (AddLineEndMode)
            {
                //saving 2nd element's X and Y
                userPoints[2] = new Point((e.OriginalSource as Ellipse).Margin.Left + lineStartOffset,
                    (e.OriginalSource as Ellipse).Margin.Top + lineStartOffset);

                if (userPoints[1] == userPoints[2])
                    return;

                //if choosed element is not reserved
                foreach (Data.Gate g in Data.GateController.gates)
                {
                    //for IN/OUT elements 
                    if (g.inputEllipse == null)
                        continue;

                    for (int i = 0; i < g.inputEllipse.Length; i++)
                    {
                        if ((e.OriginalSource as Ellipse) != g.inputEllipse[i])
                            continue;

                        if (g.inputReserved[i])
                            continue;

                        //creating line and saving it to the current grid
                        Data.LineController.lineInfo.Add(new Data.LineInfo(userPoints[1], userPoints[2]));
                        WorkSpace.Children.Add(Data.LineController.CreateLine(userPoints[1], userPoints[2]));

                        UpdateLayout();

                        //reserving the input
                        g.inputReserved[i] = true;

                        //switching off the mode
                        AddLineEndMode = false;
                    }
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------//
        // local functions

        private void UpdatePage()
        {
            Data.DotController.dots.Clear();
            Data.LineController.lines.Clear();
            Data.GateController.gates.Clear();

            Data.DotController.InitNet(ActualHeight, ActualWidth);
            Data.LineController.ReloadLines();
            Data.GateController.ReloadGates();

            foreach (Ellipse ellipse in Data.DotController.dots)
            {
                ellipse.Tapped += Dot_Tapped;
                WorkSpace.Children.Add(ellipse);
            }

            foreach (Line line in Data.LineController.lines)
                WorkSpace.Children.Add(line);

            //gates
            foreach (Data.Gate g in Data.GateController.gates)
            {
                WorkSpace.Children.Add(g.gateRect);
                WorkSpace.Children.Add(g.gateName);
                g.gateName.Tapped += GateName_Tapped;

                if (g.gateName.Text == "IN" || g.gateName.Text == "OUT")
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
            Data.Tracing.ShowTracing(); //Testing

            foreach (TextBlock tb in Data.Tracing.textBlocks)
                WorkSpace.Children.Add(tb);
        }
    }
}