using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace Roulette_Simulator
{
    class Bets
    {
        /* Constructor */
        
        // Fill the Colors Dctionary on start
        public Bets() 
        {
            fillColorDict();
        }


        /* Properties */

        //Dictionaries to Store the Bets
        public Dictionary<string, double> InnerDict { get { return bets; } }
        private Dictionary<string, double> bets = new Dictionary<string, double>();

        //Dictionary to store the colors
        public Dictionary<int, string> colors = new Dictionary<int, string>();

        // all private & public properties that can occur
        public int Number { get { return number; } }
        private int number;

        public string Color { get { return color; } }
        private string color = "";

        public string Row { get { return row; } }
        private string row = "";

        public string Third { get { return third; } }
        private string third = "";

        public bool Iseven { get { return iseven; } }
        private bool iseven = false;

        public bool Isupperhalf { get { return isupperhalf; } }
        private bool isupperhalf = false;

        
        /* Methods */

        //Method to add Bets to the Dictionary
        public void Bet(string field, double amount)
        {
            if (bets.ContainsKey(field))
            {
                //Remove Bets if amount = 0
                if (amount == 0)
                {
                    bets.Remove(field);
                }
                else
                {
                    bets[field] = amount;
                }
            }
            else
            {
                bets.Add(field, amount);
            }
        }

        // Method to spin the wheel and get the properties
        public void Spin()
        {
            // spin the wheel
            Random rnd = new Random();
            number = rnd.Next(0, 37);
            //number = 0;
        }

        // Check properties if number != 0
        public void CheckProperties()
        {
            if (number > 0)
            {
                checkColor();
                checkRow();
                checkThird();
                isEven();
                isUpperHalf();
            }
        }

        // Method to calculate the Profit
        public double CalcGewinn()
        {
            // calculate the total amount of money bet
            double wholeBet = 0;
            foreach (KeyValuePair<string, double> bet in bets) { wholeBet += bet.Value; }

            double winNumber = calcNumber();

            return winNumber;
        }

        //Method to calculate the Spread for the Numbers
        public Dictionary<int, double> CalculateNumberspread(double resolution)
        {
            Dictionary<int, double> amount = new Dictionary<int, double>();
            Dictionary<int, double> spread = new Dictionary<int, double>();
            amount.Clear();
            
            for (int i = 0; i < 37; i++)
            {
                amount.Add(i, 0);
            }
            for (int j = 0; j < (int)resolution; j++)
            {
                Spin();
                amount[number]++;
            }
            for (int i = 0; i < 37; i++)
            {
                spread.Add(i, amount[i] / (int)resolution);
            }
            return spread;
        }

        //Method to get the colorBrush of the number
        public SolidColorBrush checkColor(int Numbercheck) {
            SolidColorBrush output = Brushes.Transparent;
            if (colors[Numbercheck] == "Red")
            {
                output = Brushes.Red;
            }
            else if (colors[Numbercheck] == "Black")
            {
                output = Brushes.Black;
            }
            else if (colors[Numbercheck] == "Green")
            {
                output = Brushes.LimeGreen;
            }
            return output;
        }
        
        
        /* Internal operations to get the Bets' properties */

        private void checkColor()
        {
            color = colors[number];
        }

        private void checkRow()
        {

            if (number % 3 == 0)
            {
                row = "Line1";
            }
            else if (number % 3 == 1)
            {
                row = "Line3";
            }
            else if (number % 3 == 2)
            {
                row = "Line2";
            }
        }

        private void checkThird()
        {
            if (number > 0 && number < 13)
            {
                third = "First 12";
            }
            else if (number > 12 && number < 25)
            {
                third = "Second 12";
            }
            else if (number > 24 && number < 37)
            {
                third = "Third 12";
            }
        }

        private void isEven()
        {
            iseven = false;

            if (number % 2 == 0)
            {
                iseven = true;
            }
        }

        private void isUpperHalf()
        {
            isupperhalf = false;

            if (number > 18)
            {
                isupperhalf = true;
            }
        }

        //calculates the profit for every item in the dictionary
        private double calcNumber()
        {
            double output = 0;

            foreach (KeyValuePair<string, double> bet in bets)
            {
                // Check fr thirds
                if (bet.Key == "First 12" || bet.Key == "Second 12" || bet.Key == "Third 12")
                {
                    if (bet.Key == third)
                    {
                        output += bet.Value * 2;
                    }
                    else { output += bet.Value * -1; }
                }

                //check for rows
                else if (bet.Key == "Line1" || bet.Key == "Line2" || bet.Key == "Line3")
                {
                    if (bet.Key == row)
                    {
                        output += bet.Value * 2;
                    }
                    else { output += bet.Value * -1; }
                }

                // check for color
                else if (bet.Key == "Red" || bet.Key == "Black")
                {
                    if (bet.Key == color)
                    {
                        output += bet.Value * 2;
                    }
                    else { output += bet.Value * -1; }
                }

                //check for even / odd
                else if (bet.Key == "Even" || bet.Key == "Odd")
                {
                    if ((bet.Key == "Even" && (iseven)) || (bet.Key == "Odd" && (!iseven)))
                    {
                        output += bet.Value * 2;
                    }
                    else { output += bet.Value * -1; }
                }

                //check isupperhalf
                else if (bet.Key == "1 to 18" || bet.Key == "19 to 36")
                {
                    if ((bet.Key == "1 to 18" && (!isupperhalf)) || (bet.Key == "19 to 36" && (isupperhalf)))
                    {
                        output += bet.Value * 2;
                    }
                    else { output += bet.Value * -1; }
                }

                //check if number
                else if (Double.TryParse(bet.Key, out double number))
                {
                    if (number == this.number)
                    {
                        output += bet.Value * 35;
                    }
                    else { output += bet.Value * -1; }
                }
            }
            return output;
        }

        // internal void to fill the colors dictionary with the roulette colors
        private void fillColorDict()
        {
            int[] red_numbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
            int[] black_numbers = { 2, 4, 6, 8, 10, 11, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };
            for (int i = 0; i < 37; i++)
            {
                if (i == 0)
                {
                    colors.Add(i, "Green");
                }
                else
                {
                    foreach (int redNum in red_numbers)
                    {
                        if (i == redNum)
                        {
                            colors.Add(i, "Red");
                        }
                    }
                    foreach (int blackNum in black_numbers)
                    {
                        if (i == blackNum)
                        {
                            colors.Add(i, "Black");
                        }
                    }
                }
            }
        }
    }
}
