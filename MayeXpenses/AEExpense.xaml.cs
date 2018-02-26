using MayeXpenses.DatabaseAccess;
using System;
using System.Threading.Tasks;
using Windows.Media.Capture;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace MayeXpenses
{
    public sealed partial class AEExpense : Page
    {
        bool isEdit = false;
        Cost thisCost;
        string deletePhoto = null;

        StorageFile capturedPhoto;

        public AEExpense()
        {
            this.InitializeComponent();
            SystemNavigationManager.GetForCurrentView().BackRequested += AEExpense_BackRequested;
        }

        private async void AEExpense_BackRequested(object sender, BackRequestedEventArgs e)
        {
            if (capturedPhoto != null)
                await capturedPhoto.DeleteAsync();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            thisCost = e.Parameter as Cost;

            isEdit = thisCost.Id != -1;

            if (isEdit)
                SetEdit();
        }

        private async void SetEdit()
        {
            DatePick.Date = thisCost.Date.Date;
            TimePick.Time = thisCost.Date.TimeOfDay;
            TxtDesc.Text = thisCost.Description;

            if(thisCost.ReceiptName != null && !thisCost.ReceiptName.Equals("NONE"))
            {
                capturedPhoto = await PhotoFunctions.LoadReceipt(thisCost.ReceiptName);
                UpdatePhotoElements();
            }

            if(thisCost.Value < 0)
            {
                RbExp.IsChecked = false;
                RbRei.IsChecked = true;
            }
            TxtCost.Text = Math.Abs(thisCost.Value).ToString("0.00");

            CbxCurr.SelectedIndex = (int)thisCost.Currency;
        }

        private async void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            thisCost.Description = TxtDesc.Text.ToString();
            thisCost.Currency = (Currencies)CbxCurr.SelectedIndex;
            thisCost.Date = new DateTime(DatePick.Date.Year, DatePick.Date.Month, DatePick.Date.Day, TimePick.Time.Hours, TimePick.Time.Minutes, TimePick.Time.Seconds);

            if (capturedPhoto != null)
            {
                thisCost.ReceiptName = await PhotoFunctions.CopyFile(capturedPhoto, thisCost.GetDateString());

                if (deletePhoto != null)
                {
                    PhotoFunctions.DeleteFile(deletePhoto);
                }

                // TODO might not need this as we go back after saving
                capturedPhoto = await PhotoFunctions.LoadReceipt(thisCost.ReceiptName);
            }
            else
                thisCost.ReceiptName = "NONE";

            var temp = TxtCost.Text.ToString();
            if (!temp.Contains("."))
                temp += ".00";
            else
            {
                if(temp.IndexOf(".") > temp.Length - 3) 
                {
                    if (temp.IndexOf(".") == temp.Length - 1)
                        temp += "00";
                    else
                        temp += "0";
                }
            }
            thisCost.Value = float.Parse(temp);
            if (RbRei.IsChecked == true)
                thisCost.Value = -thisCost.Value;

            DBFunctions.SaveCost(thisCost);
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
                Frame.GoBack();
        }

        private async void BtnPhoto_Click(object sender, RoutedEventArgs e)
        {
            if (capturedPhoto != null && thisCost.ReceiptName != null && thisCost.ReceiptName.Length > 0)
                deletePhoto = thisCost.ReceiptName;

            CameraCaptureUI captureUI = new CameraCaptureUI();
            captureUI.PhotoSettings.Format = CameraCaptureUIPhotoFormat.Jpeg;

            capturedPhoto = await captureUI.CaptureFileAsync(CameraCaptureUIMode.Photo);

            if (capturedPhoto == null)
                return;

            UpdatePhotoElements();
        }

        private async void UpdatePhotoElements()
        {
            BtnPhoto.Content = "Replace Photo";
            BtnDeletePhoto.Visibility = Visibility.Visible;
            RowPhoto.Height = new GridLength(1, GridUnitType.Star);
            ProgRing.IsActive = true;

            SoftwareBitmapSource mSource = await PhotoFunctions.LoadReceiptBitmapSource(capturedPhoto);
            if (mSource != null)
            {
                ImgReceipt.Source = mSource;
            }
            else
                RowPhoto.Height = new GridLength(0);

            ProgRing.IsActive = false;
        }

        private void BtnDeletePhoto_Click(object sender, RoutedEventArgs e)
        {
            if(capturedPhoto != null && deletePhoto != null)
            {
                PhotoFunctions.DeleteFile(deletePhoto);
                deletePhoto = null;
                thisCost.ReceiptName = "NONE";
            }
            else if(capturedPhoto != null)
            {
                PhotoFunctions.DeleteFile(capturedPhoto.Name);
                capturedPhoto = null;
                thisCost.ReceiptName = "NONE";
            }

            if (capturedPhoto == null)
            {
                BtnPhoto.Content = "Attach Photo";
                BtnDeletePhoto.Visibility = Visibility.Collapsed;
            }
        }
    }
}
