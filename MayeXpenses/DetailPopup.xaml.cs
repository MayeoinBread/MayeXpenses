using MayeXpenses.Converters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace MayeXpenses
{
    public sealed partial class DetailPopup : UserControl
    {
        public event RoutedEventHandler ClosePopup;
        public event RoutedEventHandler EditCost;
        public event RoutedEventHandler DeleteCost;

        private Cost thisCost;

        public DetailPopup()
        {
            this.InitializeComponent();
        }

        public DetailPopup(Cost c, Size s)
        {
            this.InitializeComponent();

            //ProgRing.IsActive = true;

            thisCost = c;

            TxtDate.Text = CostConversions.FormatDate(c.Date, true);
            TxtCost.Text = DefaultVars.currency[(int)c.Currency] + CostConversions.FormatCost(c.Value);
            SetTbColour();
            TxtDesc.Text = c.Description;
            pupBorder.Width = s.Width - 64;
            pupBorder.Height = s.Height - 64;

            if (c.ReceiptName != null && !c.ReceiptName.Equals("NONE"))
            {
                LoadPhoto(c.ReceiptName);
            }
            else
                ProgRing.IsActive = false;

            //ProgRing.IsActive = false;
        }

        private void SetTbColour()
        {
            if (thisCost.Value > 0)
                TxtCost.Foreground = new SolidColorBrush(Colors.Red);
            else if (thisCost.Value < 0)
                TxtCost.Foreground = new SolidColorBrush(Colors.Green);
        }

        private async void LoadPhoto(string fileName)
        {
            SoftwareBitmapSource mSource = await PhotoFunctions.LoadReceiptBitmapSource(await PhotoFunctions.LoadReceipt(fileName));
            if (mSource != null)
                RecImg.Source = mSource;

            ProgRing.IsActive = false;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            ClosePopup?.Invoke(sender, e);
        }

        private void Rectangle_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ClosePopup?.Invoke(sender, e);
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            DeleteCost?.Invoke(thisCost, e);
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            EditCost?.Invoke(thisCost, e);
        }
    }
}
