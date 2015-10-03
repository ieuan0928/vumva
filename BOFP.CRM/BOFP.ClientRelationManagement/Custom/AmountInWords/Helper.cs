using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BOFP.ClientRelationManagement.Custom.AmountInWords
{
    public class Helper
    {
        private enum LargeNumber : long
        {
            Hundred = 100,
            Thousand = 1000,
            Million = 1000000,
            Billion = 1000000000,
            Trillion = 1000000000000,
            Quadrillion = 1000000000000000
        }

        private static string[] smallNumberCollection = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
        private static string[] tensLiteralCollection = { "Zero", "Ten", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

        private static void LargeNumberToString(LargeNumber largeNumber, ref string result, ref long value)
        {
            long largeNumberValue = (long)largeNumber;
            if (value / largeNumberValue > 0)
            {
                result = string.Format("{0}{1} {2} ", result, NumberToWord(value / largeNumberValue), largeNumber);
                value %= largeNumberValue;

                if (value > 0 && largeNumberValue > (long)LargeNumber.Hundred) result = string.Format("{0}, ", result.Trim());
            }
        }

        private static string NumberToWord(long amount)
        {
            string result = string.Empty;
            long value = amount;

            LargeNumberToString(LargeNumber.Quadrillion, ref result, ref value);
            LargeNumberToString(LargeNumber.Trillion, ref result, ref value);
            LargeNumberToString(LargeNumber.Billion, ref result, ref value);
            LargeNumberToString(LargeNumber.Million, ref result, ref value);
            LargeNumberToString(LargeNumber.Thousand, ref result, ref value);
            LargeNumberToString(LargeNumber.Hundred, ref result, ref value);

            bool flag19 = false;
            if (value > 19)
            {
                flag19 = true;
                byte tensIndex = Convert.ToByte(value.ToString().Substring(0, 1));

                byte tensValue = (byte)(tensIndex * 10);
                result = string.Format("{0}{1}", result, tensLiteralCollection[tensIndex]);
                value -= tensValue;

                if (value > 0) result += "-";
            }

            if (value > 0)
            {
                string smallNumberString = smallNumberCollection[value];

                if (flag19) smallNumberString = smallNumberString.ToLower();

                result = string.Format("{0}{1}", result, smallNumberString);
            }

            return result.TrimStart();
        }


        public static string Generate(decimal amount)
        {
            string result = string.Empty;
            string fractionalString = string.Empty;
            string wholeNumberString = string.Empty;

            long wholeNumber = (long)amount;

            long fractional = Convert.ToInt64(amount.ToString("#0.00##############").Split('.')[1]);

            if (fractional > 0 && fractional < 100)
                fractionalString = string.Format("{0}/100", fractional.ToString());

            if (wholeNumber > 0)
                wholeNumberString = NumberToWord(wholeNumber).Trim();

            if (wholeNumberString.Length > 0 && fractionalString.Length > 0) result = string.Format("{0} & {1}", wholeNumberString, fractionalString);
            else if (wholeNumberString.Length > 0 && fractionalString.Length == 0) result = wholeNumberString;
            else if (wholeNumberString.Length == 0 && fractionalString.Length > 0) result = fractionalString;
            else if (wholeNumberString.Length == 0 && fractionalString.Length == 0) result = "Zero";

            return result;
        }
    }
}
