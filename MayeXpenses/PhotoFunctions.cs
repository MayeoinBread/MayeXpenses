using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace MayeXpenses
{
    public class PhotoFunctions
    {
        public static async Task<StorageFile> LoadReceipt(string name)
        {
            if (!name.Equals("NONE"))
            {
                StorageFolder mFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Receipts");
                if (mFolder != null)
                {
                    return await mFolder.GetFileAsync(name);
                }
            }
            return null;
        }

        public static async void DeleteFile(string name)
        {
            StorageFolder mFolder = await ApplicationData.Current.LocalFolder.GetFolderAsync("Receipts");
            if(mFolder != null)
            {
                StorageFile mFile = await mFolder.GetFileAsync(name);

                if (mFile != null)
                    await mFile.DeleteAsync();
            }
        }

        public static async void DeleteTemporaryFiles()
        {
            StorageFolder tempFolder = ApplicationData.Current.TemporaryFolder;
            IReadOnlyList<IStorageItem> tempItems = await tempFolder.GetItemsAsync();
            if(tempItems.Count > 0)
            {
                foreach (var item in tempItems)
                {
                    await item.DeleteAsync();
                }
            }
        }

        public static async Task<string> CopyFile(StorageFile storageFile, string dateString)
        {
            StorageFolder destinationFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync("Receipts", CreationCollisionOption.OpenIfExists);
            StorageFile newFile = await storageFile.CopyAsync(destinationFolder, dateString + ".jpg", NameCollisionOption.GenerateUniqueName);
            await storageFile.DeleteAsync();

            return newFile.Name;
        }

        public static async Task<SoftwareBitmapSource> LoadReceiptBitmapSource(StorageFile mFile)
        {
            if(mFile != null)
            {
                using (var stream = await mFile.OpenAsync(FileAccessMode.Read))
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    SoftwareBitmap sBitmap = await decoder.GetSoftwareBitmapAsync();
                    SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(sBitmap, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
                    SoftwareBitmapSource bitmapSource = new SoftwareBitmapSource();
                    await bitmapSource.SetBitmapAsync(softwareBitmapBGR8);

                    return bitmapSource;
                }
            }
            return null;
        }
    }
}
