using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
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

        //Initialise the main Window
        public MainWindow()
        {
            InitializeComponent();
            rb_winMode.IsChecked = true;
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
            //Spin The Wheel
            opBet.Spin();

            //Check all Properties for the current Number
            opBet.CheckProperties();

            string output = "Number:\t" + opBet.Number + "\nColor:\t" + opBet.Color + "\n2 to 1:\t" + opBet.Row + "\nThrid:\t" + opBet.Third + "\nEven?\t" + opBet.Iseven + "\n19 to 36?\t" + opBet.Isupperhalf;
            MessageBox.Show(output);

            //Calculate the Profit & display it
            double win = opBet.CalcGewinn();
            winLabel.Content = win + "€";
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

        //Boolean to check which mode is currently open
        bool simmode = false;

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
            for (int i = 0; i < 37; i++)
            {
                // Add a new Row
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
                    VerticalAlignment = VerticalAlignment.Bottom
                };
                var lb = new Label()
                {
                    Content = i,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 8
                };

                //select the right column and row to put in
                Grid.SetColumn(Rect, i);
                Grid.SetRow(Rect, 0);

                Grid.SetColumn(lb, i);
                Grid.SetRow(lb, 1);

                graphic.Children.Add(Rect);

                // Only add the Label ever second Column
                if (i % 2 == 0) { graphic.Children.Add(lb); }
            }
        }

        // Start the simulation
        void Sim_Statistic(object sender, RoutedEventArgs e)
        {
            //Calculate the spread
            var Numberspread = new Dictionary<int, double>();
            Numberspread = opBet.CalculateNumberspread(resolution_slider.Value);

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
    }
}
