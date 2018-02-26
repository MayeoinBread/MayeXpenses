using System;
using Windows.Storage;

namespace MayeXpenses
{
    public class SettingsHelper
    {
        static ApplicationDataContainer dc;

        public static readonly string KEY_CURRENCY = "k_curr";
        public static readonly string KEY_DATE = "k_date";
        public static readonly string KEY_EURPND = "k_eurPnd";
        public static readonly string KEY_EURDOL = "k_eurDol";
        public static readonly string KEY_EURYEN = "k_eurYen";
        public static readonly string KEY_PNDDOL = "k_pndDol";
        public static readonly string KEY_PNDYEN = "k_pndYen";
        public static readonly string KEY_DOLYEN = "k_dolYen";

        public static Currencies GetCurrency()
        {
            dc = ApplicationData.Current.LocalSettings;
            object o = dc.Values[KEY_CURRENCY];
            if (o == null || !Enum.IsDefined(typeof(Currencies), (string)o))
                return Currencies.Euro;

            return (Currencies) Enum.Parse(typeof(Currencies), (string)o);
        }

        public static DateFormats GetDateFormat(DateFormats def)
        {
            dc = ApplicationData.Current.LocalSettings;
            object o = dc.Values[KEY_DATE];
            if (o == null || !Enum.IsDefined(typeof(DateFormats), (string)o))
                return def;

            return (DateFormats)Enum.Parse(typeof(DateFormats), (string)o);
        }

        public static void SaveCurrency(Currencies currencyEnum)
        {
            dc = ApplicationData.Current.LocalSettings;
            dc.Values[KEY_CURRENCY] = currencyEnum.ToString();
        }

        public static void SaveDateFormat(DateFormats dateFormat)
        {
            dc = ApplicationData.Current.LocalSettings;
            dc.Values[KEY_DATE] = dateFormat.ToString();
        }

        public static string GetExchange(Exchanges exc)
        {
            dc = ApplicationData.Current.LocalSettings;
            object o;
            switch (exc)
            {
                case Exchanges.EurPnd:
                    o = dc.Values[KEY_EURPND];
                    break;
                case Exchanges.EurDol:
                    o = dc.Values[KEY_EURDOL];
                    break;
                case Exchanges.EurYen:
                    o = dc.Values[KEY_EURYEN];
                    break;
                case Exchanges.PndDol:
                    o = dc.Values[KEY_PNDDOL];
                    break;
                case Exchanges.DolYen:
                    o = dc.Values[KEY_DOLYEN];
                    break;
                case Exchanges.PndYen:
                    o = dc.Values[KEY_PNDYEN];
                    break;
                default:
                    o = null;
                    break;
            }

            if (o == null)
                return "1.00";

            return (string)o;
        }

        public static void SaveExchange(Exchanges exc, string value)
        {
            dc = ApplicationData.Current.LocalSettings;
            switch (exc)
            {
                case Exchanges.EurPnd:
                    dc.Values[KEY_EURPND] = value;
                    break;
                case Exchanges.EurDol:
                    dc.Values[KEY_EURDOL] = value;
                    break;
                case Exchanges.EurYen:
                    dc.Values[KEY_EURYEN] = value;
                    break;
                case Exchanges.PndDol:
                    dc.Values[KEY_PNDDOL] = value;
                    break;
                case Exchanges.DolYen:
                    dc.Values[KEY_DOLYEN] = value;
                    break;
                case Exchanges.PndYen:
                    dc.Values[KEY_PNDYEN] = value;
                    break;
            }
        }
    }
}