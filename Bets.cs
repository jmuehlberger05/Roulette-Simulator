﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Roulette_Simulator
{
    class Bets
    {
        private Dictionary<string, double> bets = new Dictionary<string, double>();

        public Dictionary<string, double> InnerDict { get { return bets; } }

        // No special things happening in the constructor
        public Bets() { }

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

            // Check properties if numer != 0
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

            //double winNumber = calcNumber();

            return wholeBet;
        }

        // operations to get the Bets' properties
        private int[] red_numbers = { 1, 3, 5, 7, 9, 12, 14, 16, 18, 19, 21, 23, 25, 27, 30, 32, 34, 36 };
        private int[] black_numbers = { 2, 4, 6, 8, 10, 13, 15, 17, 20, 22, 24, 26, 28, 29, 31, 33, 35 };

        private void checkColor() {
            foreach (int redNum in red_numbers) {
                if (number == redNum)
                {
                    color = "red";
                }
            }
            foreach (int blackNum in black_numbers)
            {
                if (number == blackNum)
                {
                    color = "black";
                }
            }
            if (number == 0)
            {
                color = "green";
            }
        }

        private void checkRow()
        { 

            if (number % 3 == 0)
            {
                row = "line1";
            }else if (number % 3 == 1)
            {
                row = "line3";
            }else if (number % 3 == 2)
            {
                row = "line2";
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
            else if (number >24 && number < 37)
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

        //private double calcNumber()
        //{
        //    double output = 69;
        //    if (bets.ContainsKey(number.ToString()))
        //    {
        //        output = bets[number.ToString()] * 35;
        //    }
        //    else
        //    {
        //        output = bets[number.ToString()] * -1;
        //    }
        //    return output;
        //}
    }
}
