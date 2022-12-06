using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Roulette_Simulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Initialise the bets class and the roulette wheel
        Bets opBet = new Bets();
        Bets statBet = new Bets();

        //Boolean to check which mode is currently open
        bool simmode = false;

        //Boolean to check whether Profit Mode is active
        bool simWinmode = false;

        //Initialise the main Window
        public MainWindow()
        {
            InitializeComponent();
            rb_winMode.IsChecked = true;
            Sim_check.IsChecked = false;
        }

        //refresh the betting Listbox
        public void listBox_refresh()
        {
            Dictionary<string, double> allBets = opBet.InnerDict;
            lb_BetsList.Items.Clear();
            foreach (KeyValuePair<string, double> bet in allBets)
            {
                lb_BetsList.Items.Add(bet.Key + " - " + bet.Value + "€");
            }
        }

        //onClick function for the "SIM start" button -> starts the Wheelspin & calculates the profit
        void Simulate(object sender, RoutedEventArgs e)
        {
            // if single mode is active
            if (!simWinmode)
            {
                //Spin The Wheel
                opBet.Spin();

                //Check all Properties for the current Number
                opBet.CheckProperties();

                //show the output values of the simulation in the SimDialog Window
                var winDialog = new SimDialog(opBet.Number, opBet.Color, opBet.Row, opBet.Third, opBet.Iseven, opBet.Isupperhalf);
                winDialog.ShowDialog();

                //Calculate the Profit & display it
                double win = opBet.CalcGewinn();
                winLabel.Content = win + "€";
            }
            // if statistic winmode is active
            else
            {
                if (int.TryParse(sim_input.Text, out int sim_resolution))
                {    
                    double win = 0;
                    for (int i = 0; i < sim_resolution; i++)
                    {
                        //Spin The Wheel
                        opBet.Spin();

                        //Check all Properties for the current Number
                        opBet.CheckProperties();

                        //Calculate the Profit & display it
                        win += opBet.CalcGewinn();
                        winLabel.Content = win + "€";
                    }
                }
                else
                {
                    MessageBox.Show("Please enter a valid number");
                }
            }
        }

        //onClick function for every field on the table
        void Bet_Click(object sender, RoutedEventArgs e)
        {
            string field = (string)((Button)sender).Content;

            //checks if the selected field is a '2 to 1' row and changes the name to the correct one
            if (field == "2 to 1")
            {
                field = (string)((Button)sender).Name;
            }

            //Initialise and check PopUp window for betting Input
            var Input = new BetInput(field);
            bool? dialogResult = Input.ShowDialog();

            //If Input != cancelled and Input >= 0
            if (dialogResult == true && Input.ValueToBet >= 0)
            {
                // Put Bet into Class
                opBet.Bet(field, Input.ValueToBet);

                // Refresh the Listbox
                listBox_refresh();
            }
        }

        //onClick function to change the program mode   
        void changeMode(object sender, RoutedEventArgs e)
        {
            if (rb_winMode.IsChecked == true)
            {
                simmode = false;

                Fields.Visibility = Visibility.Visible;
                Start_btn.Visibility = Visibility.Visible;
                Bets_Panel.Visibility = Visibility.Visible;
                Win_Panel.Visibility = Visibility.Visible;

                Statistic_Panel.Visibility = Visibility.Hidden;
                Stat_btn.Visibility = Visibility.Hidden;
                Descript_Panel.Visibility = Visibility.Hidden;
            }
            else
            {
                simmode = true;

                Fields.Visibility = Visibility.Hidden;
                Start_btn.Visibility = Visibility.Hidden;
                Bets_Panel.Visibility = Visibility.Hidden;
                Win_Panel.Visibility = Visibility.Hidden;

                Statistic_Panel.Visibility = Visibility.Visible;
                Stat_btn.Visibility = Visibility.Visible;
                Descript_Panel.Visibility = Visibility.Visible;
            }
        }

        //build and render the chart
        void renderChart(Dictionary<int, double> spread)
        {
            var scaling = new ColumnDefinition();
            scaling.Width = new GridLength(20);
            graphic.ColumnDefinitions.Add(scaling);

            var scale = new Label()
            {
                Content = "2,7",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontSize = 8,
                Margin = new Thickness(0, 0,0, 128)
            };
            var scaleZero = new Label()
            {
                Content = "0",
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom,
                FontSize = 8,
                Margin = new Thickness(0, 0, 0, 0)
            };

            Grid.SetColumn(scale, 0);   Grid.SetColumn(scaleZero, 0);
            Grid.SetRow(scale, 0);      Grid.SetRow(scaleZero, 1);
            Grid.SetRowSpan(scale, 2);

            graphic.Children.Add(scale);
            graphic.Children.Add(scaleZero);


            for (int i = 0; i < 37; i++)
            {
                // Add a new Column
                var ColumnDef = new ColumnDefinition();
                ColumnDef.Width = new GridLength(1, GridUnitType.Star);
                graphic.ColumnDefinitions.Add(ColumnDef);

                // Get Columns Fill
                var fill = opBet.checkColor(i);

                // create new Rectangle and Label
                var Rect = new Rectangle()
                {
                    Fill = fill,
                    Height = spread[i] * 5000,
                    Margin = new Thickness(2, 2, 2, 2),
                    VerticalAlignment = VerticalAlignment.Bottom,
                    ToolTip = spread[i] * 100 + "%"
                };
                var lb = new Label()
                {
                    Content = i,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 8
                };

                //select the right column and row to put in
                Grid.SetColumn(Rect, i+1);
                Grid.SetRow(Rect, 0);
                Grid.SetRowSpan(Rect, 2);

                Grid.SetColumn(lb, i+1);
                Grid.SetRow(lb, 2);

                graphic.Children.Add(Rect);

                // Only add the Label ever second Column
                if (i % 2 == 0) { graphic.Children.Add(lb); }
            }
            //var fill = new SolidColorBrush();
            //fill = Brushes.Black;

            var Line = new Rectangle()
            {
                Fill = Brushes.Black,
                Height = 1,
                Margin = new Thickness(2, 2, 2, 2),
                VerticalAlignment = VerticalAlignment.Bottom,
                ToolTip = "2.7% - Perfect Distribution"
            };

            Grid.SetColumn(Line, 1);
            Grid.SetRow(Line, 0);
            Grid.SetColumnSpan(Line, 37);
            graphic.Children.Add(Line);
        }

        // Start the simulation
        void Sim_Statistic(object sender, RoutedEventArgs e)
        {
            //Calculate the spread
            var Numberspread = new Dictionary<int, double>();
            Numberspread = opBet.CalculateNumberspread(resolution_slider.Value);
            if (simWinmode)
            {

            }
            //clear the old Graphic
            graphic.Children.Clear();
            graphic.ColumnDefinitions.Clear();

            //build and render the new graphic
            renderChart(Numberspread);
        }

        // Update the resolutionSliders LabelValue on Value changed
        void updateSlider(object sender, RoutedEventArgs e)
        {
            if (simmode)
                Slider_label.Content = "x = " + resolution_slider.Value.ToString();
        }
        
        //change Statistic mode to Profit
        void Profit_Stat_Sim(object sender, RoutedEventArgs e)
        {
            if (!simWinmode)
            {
                windescr.Content = "Statistic Profit";
                sim_input.Text = "1000";
                sim_input.IsReadOnly = false;
                sim_input.BorderThickness = new Thickness(1);
                sim_input.BorderBrush = Brushes.Gray;
                simWinmode = true;
            }
            else
            {
                windescr.Content = "Profit";
                sim_input.Text = "Statistic Sim";
                sim_input.IsReadOnly = true;
                sim_input.BorderBrush = Brushes.Transparent;
                simWinmode = false;
            }
        }
    }
}
