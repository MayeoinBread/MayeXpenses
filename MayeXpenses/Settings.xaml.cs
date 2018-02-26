using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace MayeXpenses
{
    public sealed partial class Settings : Page
    {
        public Settings()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            Currencies defaultCurrency = SettingsHelper.GetCurrency();
            CbbCurr.SelectedIndex = (int)defaultCurrency;

            DateFormats defaultFormat = SettingsHelper.GetDateFormat(DateFormats.ddmmyyyy);
            CbbDate.SelectedIndex = (int)defaultFormat;

            TbEurPnd.Text = SettingsHelper.GetExchange(Exchanges.EurPnd);
            TbEurDol.Text = SettingsHelper.GetExchange(Exchanges.EurDol);
            TbEurYen.Text = SettingsHelper.GetExchange(Exchanges.EurYen);
            TbPndDol.Text = SettingsHelper.GetExchange(Exchanges.PndDol);
            TbPndYen.Text = SettingsHelper.GetExchange(Exchanges.PndYen);
            TbDolYen.Text = SettingsHelper.GetExchange(Exchanges.DolYen);
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if(Enum.IsDefined(typeof(Currencies), CbbCurr.SelectedIndex)){
                SettingsHelper.SaveCurrency((Currencies)CbbCurr.SelectedIndex);
            }
            else
            {
                SettingsHelper.SaveCurrency(Currencies.Euro);
            }

            if(Enum.IsDefined(typeof(DateFormats), CbbDate.SelectedIndex))
            {
                SettingsHelper.SaveDateFormat((DateFormats)CbbDate.SelectedIndex);
            }
            else
            {
                SettingsHelper.SaveDateFormat(DateFormats.ddmmyyyy);
            }

            SettingsHelper.SaveExchange(Exchanges.EurPnd, TbEurPnd.Text.ToString());
            SettingsHelper.SaveExchange(Exchanges.EurDol, TbEurDol.Text.ToString());
            SettingsHelper.SaveExchange(Exchanges.EurYen, TbEurYen.Text.ToString());
            SettingsHelper.SaveExchange(Exchanges.PndDol, TbPndDol.Text.ToString());
            SettingsHelper.SaveExchange(Exchanges.PndYen, TbPndYen.Text.ToString());
            SettingsHelper.SaveExchange(Exchanges.DolYen, TbDolYen.Text.ToString());

            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }
    }
}
