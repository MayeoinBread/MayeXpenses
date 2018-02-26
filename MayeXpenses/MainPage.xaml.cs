using MayeXpenses.Converters;
using MayeXpenses.DatabaseAccess;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MayeXpenses
{
    public sealed partial class MainPage : Page
    {
        DetailPopup pup;

        Size s;

        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += MainPage_SizeChanged;
            SystemNavigationManager.GetForCurrentView().BackRequested += MainPage_BackRequested;
        }

        private void MainPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if(pup != null)
            {
                e.Handled = true;
                RemovePopup();
            }
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var bounds = ApplicationView.GetForCurrentView().VisibleBounds;
            //var scaleFactor = DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
            s = new Size(bounds.Width, bounds.Height);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            UpdateCosts();
        }

        private void UpdateCosts()
        {
            Currencies currency = SettingsHelper.GetCurrency();
            AllCosts.ItemsSource = DBFunctions.GetAllCosts();
            double tCost = CostConversions.TotalCost(currency);
            ColourTV(tCost, CstTotal);
            if (tCost < 0)
                tCost = -tCost;
            CstTotal.Text = DefaultVars.currency[(int)SettingsHelper.GetCurrency()] + tCost.ToString(DefaultVars.costFormat);
        }

        private void ColourTV(double cost, TextBlock tv)
        {
            if (cost < 0)
                tv.Foreground = new SolidColorBrush(Colors.DarkGreen);
            else if (cost > 0)
                tv.Foreground = new SolidColorBrush(Colors.Red);
            else
                tv.Foreground = new SolidColorBrush(Colors.White);
        }

        private void AllCosts_ItemClick(object sender, ItemClickEventArgs e)
        {
            Cost c = e.ClickedItem as Cost;
            pup = new DetailPopup(c, s);
            pup.ClosePopup += Pup_ClosePopup;
            pup.EditCost += Pup_EditCost;
            pup.DeleteCost += Pup_DeleteCost;

            Grid.SetRowSpan(pup, 3);
            Grid.SetColumnSpan(pup, 2);
            GridCurCosts.Children.Add(pup);
        }

        private void Pup_DeleteCost(object sender, RoutedEventArgs e)
        {
            DBFunctions.DeleteCost(sender as Cost);
            RemovePopup();
            UpdateCosts();
        }

        private void Pup_EditCost(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AEExpense), sender as Cost);
        }

        private void Pup_ClosePopup(object sender, RoutedEventArgs e)
        {
            RemovePopup();
        }

        private void RemovePopup()
        {
            if (pup != null)
            {
                GridCurCosts.Children.Remove(pup);
                pup = null;
            }
        }

        private void AddCost_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AEExpense), new Cost(-1));
        }

        private void BtnSettings_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Settings));
        }
    }
}
