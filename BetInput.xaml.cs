using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace Roulette_Simulator
{
    /// <summary>
    /// Interaction logic for BetInput.xaml
    /// </summary>
    public partial class BetInput : Window
    {
        public double ValueToBet { get; set; }

        //Initialisation of the popup window
        public BetInput(string field)
        {
            InitializeComponent();
            lblQuestion.Content = "How much do you want to bet on " + field + "?";
            txtBet.Focus();
        }

        //Copied from Stackoverflow https://stackoverflow.com/questions/35640001/regex-for-integer-or-double-values
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        //Onclick Submit Button
        private void submit(object sender, RoutedEventArgs e)
        {
            //opBet.bet(currentField, Convert.ToDouble(txtBet.Text));
            this.DialogResult = true;
            try
            {
                ValueToBet = Convert.ToDouble(txtBet.Text);
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
