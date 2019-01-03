using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Shapes;
using static SchemeCreator.Data.Scheme;
using System;

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
        public MainPage()
        {
            InitializeComponent();

            SizeChanged += MainPage_SizeChanged;
            NewSchemeBt.Click += NewSchemeBt_Click;
            NewElementBt.Click += NewElementBt_Click;
            NewLineBt.Click += NewLineBt_Click;
            ChangeValueBt.Click += ChangeValueBt_Click;
            WorkBt.Click += WorkBt_Click;
            TraceBt.Click += TraceBt_Click;

            //WorkSpace.DataContext = Data.GateController.gates;
            WorkSpace.DataContext = Data.DotController.dots;
        }
        //----------------------------------------------------------------------------------------------------------//
        // event handlers for buttons

        private void ChangeValueBt_Click(object sender, RoutedEventArgs e) => ChangeValueMode = true;
        private void NewLineBt_Click(object sender, RoutedEventArgs e) => AddLineStartMode = true;
        private void NewElementBt_Click(object sender, RoutedEventArgs e) => AddGateMode = true;
        private void TraceBt_Click(object sender, RoutedEventArgs e) => throw new NotImplementedException();

        /// <summary>
        /// Sends signals from outputs to inputs
        /// </summary>
        private void WorkBt_Click(object sender, RoutedEventArgs e)
        {
            foreach (Line l in Data.LineController.lines)
                SendValue(Data.Scheme.GetValue(l), l); 
        }
        
        /// <summary>
        /// Provides initialization of grid's dots
        /// </summary>
        private void NewSchemeBt_Click(object sender, RoutedEventArgs e)
        {
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
        // event handlers for objects

        /// <summary>
        /// Event handler for page update
        /// </summary>
        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (SchemeCreated)
            {
                if (NewElementName == "IN" || NewElementName == "OUT")
                {
                    Data.GateController.CreateInOut(userPoints[0], NewElementName);
                    Data.GateController.gates[Data.GateController.gates.Count - 1].gateName.Tapped += GateName_Tapped;
                }
                else
                {
                    Data.GateController.CreateGate(userPoints[0], NewElementName, NewGateInputs);

                    foreach (Data.Gate gate in Data.GateController.gates)
                        foreach (Ellipse ellipse in Data.GateController.gates[Data.GateController.gates.Count - 1].inputEllipse)
                            ellipse.Tapped += GateInOut_Tapped;
                }

                AddGateMode = false;

                UpdatePage();

                SubscribeDot(true);
                SubscribeGateInOut(true);
                SubscribeInOut(false);
            }
        }
        private void GateName_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AddLineStartMode)
            {
                if ((e.OriginalSource as TextBlock).Text == "IN")
                {
                    userPoints[1].X = (e.OriginalSource as TextBlock).Margin.Left + dotSize;
                    userPoints[1].Y = (e.OriginalSource as TextBlock).Margin.Top + dotSize;

                    AddLineStartMode = false;
                    AddLineEndMode = true;
                }
            }
            else if (AddLineEndMode)
            {
                if ((e.OriginalSource as TextBlock).Text == "OUT")
                {
                    userPoints[2].X = (e.OriginalSource as TextBlock).Margin.Left + dotSize;
                    userPoints[2].Y = (e.OriginalSource as TextBlock).Margin.Top + dotSize;

                    if (userPoints[1] != userPoints[2])
                    {
                        Line l = Data.LineController.CreateLine(userPoints[1], userPoints[2]);
                        WorkSpace.Children.Add(l);

                        AddLineEndMode = false;
                    }
                }
            }
            else if (ChangeValueMode)
            {
                foreach (Data.Gate g in Data.GateController.gates)
                    if (g.gateName == e.OriginalSource as TextBlock)
                    {
                        g.outputValue = !g.outputValue;
                        g.ColorByValue();
                        break;
                    }
                ChangeValueMode = false;
            }
        }

        /// <summary>
        /// dot's event handler 
        /// </summary>
        private void Dot_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AddGateMode)
            {
                userPoints[0].X = (e.OriginalSource as Ellipse).Margin.Left + lineStartOffset;
                userPoints[0].Y = (e.OriginalSource as Ellipse).Margin.Top + lineStartOffset;

                SubscribeDot(false);
                SubscribeInOut(false);
                SubscribeGateInOut(false);

                //WorkSpace.Children.Clear();

                Frame.Navigate(typeof(UI.Dialogs.AddElement));
            }
        }

        ///<summary>
        /// function for inputs and outputs of element
        /// </summary>
        private void GateInOut_Tapped(object sender, TappedRoutedEventArgs e)
        {
            //has 2 modes
            if (AddLineStartMode)
            {
                //saving 1st element's X and Y
                userPoints[1].X = (e.OriginalSource as Ellipse).Margin.Left + lineStartOffset;
                userPoints[1].Y = (e.OriginalSource as Ellipse).Margin.Top + lineStartOffset;

                //switching to next mode
                AddLineStartMode = false;
                AddLineEndMode = true;
            }
            else if (AddLineEndMode)
            {
                //saving 2nd element's X and Y
                userPoints[2].X = (e.OriginalSource as Ellipse).Margin.Left + lineStartOffset;
                userPoints[2].Y = (e.OriginalSource as Ellipse).Margin.Top + lineStartOffset;

                if (userPoints[1] != userPoints[2])
                {
                    //if choosed element is not reserved
                    foreach (Data.Gate g in Data.GateController.gates)
                    {
                        //for IN OUT elements 
                        if (g.inputEllipse == null)
                            continue;

                        for (int i = 0; i < g.inputEllipse.Length; i++)
                            if ((e.OriginalSource as Ellipse) == g.inputEllipse[i])
                                if (!g.inputReserved[i])
                                {
                                    //creating line and saving it to the current grid
                                    Line l = Data.LineController.CreateLine(userPoints[1], userPoints[2]);
                                    WorkSpace.Children.Add(l);

                                    UpdateLayout();

                                    //reserving the input
                                    g.inputReserved[i] = true;
                                    //switching off the mode
                                    AddLineEndMode = false;
                                }
                    }
                }
            }
        }

        //----------------------------------------------------------------------------------------------------------//
        // local functions
        private void UpdatePage()
        {
            return;
            //clean up children
            WorkSpace.Children.Clear();
            //lines
            foreach (Line l in Data.LineController.lines)
                WorkSpace.Children.Add(l);
            //dots
            //SubscribeDot(false);
            foreach (Ellipse e in Data.DotController.dots)
                WorkSpace.Children.Add(e);
            //SubscribeDot(true);
            //gates
            foreach(Data.Gate g in Data.GateController.gates)
            {
                WorkSpace.Children.Add(g.gateRect);
                //SubscribeInOut(false);
                WorkSpace.Children.Add(g.gateName);
                //SubscribeInOut(true);
                if (g.gateName.Text != "IN" && g.gateName.Text != "OUT")
                {
                    //gate out
                    //SubscribeGateInOut(false);
                    WorkSpace.Children.Add(g.outputEllipse);
                    //gate in
                    foreach (Ellipse e in g.inputEllipse)
                        WorkSpace.Children.Add(e);
                    //SubscribeGateInOut(true);
                }
            }
        }
        //----------------------------------------------------------------------------------------------------------//
        //subscribe functions
        private void SubscribeDot(bool s)
        {
            if (s)
                foreach (Ellipse e in Data.DotController.dots)
                    e.Tapped += Dot_Tapped;
            else
                foreach (Ellipse e in Data.DotController.dots)
                    e.Tapped -= Dot_Tapped;
        }
        private void SubscribeGateInOut(bool s)
        {
            if (s)
            {
                foreach (Data.Gate g in Data.GateController.gates)
                    if (g.gateName.Text != "IN" && g.gateName.Text != "OUT")
                    {
                        foreach (Ellipse e in g.inputEllipse)
                            e.Tapped += GateInOut_Tapped;
                        g.outputEllipse.Tapped += GateInOut_Tapped;
                    }
            }
            else
            {
                foreach (Data.Gate g in Data.GateController.gates)
                    if (g.gateName.Text != "IN" && g.gateName.Text != "OUT")
                    {
                        foreach (Ellipse e in g.inputEllipse)
                            e.Tapped -= GateInOut_Tapped;
                        g.outputEllipse.Tapped -= GateInOut_Tapped;
                    }
            }
        }
        private void SubscribeInOut(bool s)
        {
            if(s)
                foreach(Data.Gate g in Data.GateController.gates)
                    if(g.gateName.Text=="IN"|| g.gateName.Text == "OUT")
                        g.gateName.Tapped += GateName_Tapped;
            else
                foreach (Data.Gate g2 in Data.GateController.gates)
                    if (g2.gateName.Text == "IN" || g2.gateName.Text == "OUT")
                        g2.gateName.Tapped -= GateName_Tapped;
        }
    }
}