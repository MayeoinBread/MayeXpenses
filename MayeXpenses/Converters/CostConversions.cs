using MayeXpenses.DatabaseAccess;
using System;
using System.Collections.ObjectModel;

namespace MayeXpenses.Converters
{
    public class CostConversions
    {
        static double eurDol;
        static double eurPnd;
        static double eurYen;
        static double pndDol;
        static double pndYen;
        static double dolYen;

        public static string FormatDate(DateTime dateTime, bool fullDate = false)
        {
            var temp = SettingsHelper.GetDateFormat(DateFormats.ddmmyyyy);
            if(fullDate)
                return dateTime.ToString(DefaultVars.dateFormats[(int)temp] + " HH:mm");
            else
                return dateTime.ToString(DefaultVars.dateFormats[(int)temp]);
        }

        public static string FormatCost(float cost)
        {
            return Math.Abs(cost).ToString(DefaultVars.costFormat);
        }

        public static void LoadExchangeRates()
        {
            Windows.Storage.ApplicationDataContainer ls = Windows.Storage.ApplicationData.Current.LocalSettings;
            eurPnd = double.Parse(SettingsHelper.GetExchange(Exchanges.EurPnd));
            eurDol = double.Parse(SettingsHelper.GetExchange(Exchanges.EurDol));
            eurYen = double.Parse(SettingsHelper.GetExchange(Exchanges.EurYen));
            pndDol = double.Parse(SettingsHelper.GetExchange(Exchanges.PndDol));
            pndYen = double.Parse(SettingsHelper.GetExchange(Exchanges.PndDol));
            dolYen = double.Parse(SettingsHelper.GetExchange(Exchanges.DolYen));
        }

        public static double TotalCost(Currencies selectedCurrency)
        {
            LoadExchangeRates();
            ObservableCollection<Cost> allCosts = DBFunctions.GetAllCosts();
            double ttl = 0;

            foreach (var c in allCosts)
            {
                switch (selectedCurrency)
                {
                    case Currencies.Euro:
                        ttl += ToEuro(c.Currency, c.Value);
                        break;
                    case Currencies.Pound:
                        ttl += ToPound(c.Currency, c.Value);
                        break;
                    case Currencies.Dollar:
                        ttl += ToDollar(c.Currency, c.Value);
                        break;
                    case Currencies.Yen:
                        ttl += ToYen(c.Currency, c.Value);
                        break;
                }
            }

            return ttl;
        }

        public static double ToEuro(Currencies curr, float cost)
        {
            switch (curr)
            {
                default:
                    return cost;
                case Currencies.Pound:
                    return cost * (1.0 / eurPnd);
                case Currencies.Dollar:
                    return cost * (1.0 / eurDol);
                case Currencies.Yen:
                    return cost * (1.0 / eurYen);
            }
        }

        public static double ToPound(Currencies curr, float cost)
        {
            switch (curr)
            {
                case Currencies.Euro:
                    return cost * eurPnd;
                default:
                    return cost;
                case Currencies.Dollar:
                    return cost * (1.0 / pndDol);
                case Currencies.Yen:
                    return pndDol * (1.0 / pndYen);
            }
        }

        public static double ToDollar(Currencies curr, float cost)
        {
            switch (curr)
            {
                case Currencies.Euro:
                    return cost * eurDol;
                case Currencies.Pound:
                    return cost * pndDol;
                default:
                    return cost;
                case Currencies.Yen:
                    return cost * (1.0 / dolYen);
            }
        }

        public static double ToYen(Currencies curr, float cost)
        {
            switch (curr)
            {
                case Currencies.Euro:
                    return cost * eurYen;
                case Currencies.Pound:
                    return cost * pndYen;
                case Currencies.Dollar:
                    return cost * dolYen;
                default:
                    return cost;
            }
        }
    }
}
