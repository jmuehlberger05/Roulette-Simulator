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
using System.Windows.Shapes;

namespace Roulette_Simulator
{
    /// <summary>
    /// Interaction logic for BetWindow.xaml
    /// </summary>
    public partial class BetWindow : Window
    {
        public BetWindow()
        {
            InitializeComponent();
        }

        Bets simulatedBets = new Bets();

        void Bet_Click (object sender, RoutedEventArgs e)
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
                simulatedBets.Bet(field, Input.ValueToBet);
            }
        }

        private void submit(object sender, RoutedEventArgs e)
        {
            //opBet.bet(currentField, Convert.ToDouble(txtBet.Text));
            this.DialogResult = true;
            try
            {
                //ValueToBet = Convert.ToDouble(txtBet.Text);
            }
            catch (Exception)
            {
                MessageBox.Show("Please enter a valid number");
            }

            this.Close();
        }

        //Onclick Cancel Button
        private void cancel(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
