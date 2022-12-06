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
    /// Interaction logic for SimDialog.xaml
    /// </summary>
    public partial class SimDialog : Window
    {
        public SimDialog(int Number, string color, string row, string third, bool iseven, bool isupperhalf)
        {
            InitializeComponent();
           
            //Converter Line suggested by CoPilot
            RectColor.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            
            //Assign the label Values
            lblNumber.Content = "Number " + Number;
            lblRow.Content = row;
            lblThird.Content = third;

            //See whether iseven is true and then set the label to even or odd
            lblEven.Content = iseven ? "Even" : "Odd";

            //See whether isupperhalf is true and then set the label to upper or lower
            lblHalf.Content = isupperhalf ? "1 - 18" : "19 - 36";
        }

        // onclick function to close the dialog
        private void btnClose(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
