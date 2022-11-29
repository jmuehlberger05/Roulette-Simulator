using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
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
        void listBox_refresh()
        {
            Dictionary<string, double> allBets = opBet.InnerDict;
            lb_BetsList.Items.Clear();
            foreach (KeyValuePair<string, double> bet in allBets)
            {
                lb_BetsList.Items.Add(bet.Key + " - " + bet.Value + "€");
            }
        }

        //onClick function for the "SIM start" button -> starts the Wheelspin
        void Simulate (object sender, RoutedEventArgs e)
        {
            opBet.Spin();
            string output = opBet.Number + "  " + opBet.Color + "  " + opBet.Row + "  " + opBet.Third + "  " + opBet.Iseven + "  " + opBet.Isupperhalf;
            MessageBox.Show(output);
            double win = opBet.CalcGewinn();

            winLabel.Content = win + "€";
        }

        //onClick function for every field on the table
        void Bet_Click(object sender, RoutedEventArgs e)
        {
            string field = (string)((Button)sender).Content;
            
            //checks if the selected field is a row and changes the name to the correct one
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
                //((Button)sender).Background = Brushes.Purple;
                opBet.Bet(field, Input.ValueToBet);
                listBox_refresh();
            }
        }
    }
}
