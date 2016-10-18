using Lumia.Imaging;
using Lumia.Imaging.Adjustments;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LumiaW
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private WriteableBitmap bitmap;
        private GrayscaleEffect grayscaleEffect;
        SwapChainPanelRenderer renderer;
        public MainPage()
        {
            this.InitializeComponent();
            bitmap = new WriteableBitmap(10, 10);
            grayscaleEffect = new GrayscaleEffect();
            //renderer = new SwapChainPanelRenderer(grayscaleEffect, SwapChainPanelTarget);
            if (SwapChainPanelTarget.ActualHeight > 0 && SwapChainPanelTarget.ActualWidth > 0)
            {
                if (renderer == null)
                {
                    renderer = new SwapChainPanelRenderer(grayscaleEffect, SwapChainPanelTarget);
                }
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".jpeg");
            openPicker.FileTypeFilter.Add(".png");

            StorageFile file = await openPicker.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream stream = await file.OpenReadAsync())
                {
                    BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);
                    bitmap.SetSource(stream);
                    img.Source = bitmap;
                    stream.Seek(0);
                  grayscaleEffect.Source = new Lumia.Imaging.RandomAccessStreamImageSource(stream);
                        await renderer.RenderAsync();
                }
            }
        }

        private  void SwapChainPanelTarget_Loaded(object sender, RoutedEventArgs e)
        {
            if (SwapChainPanelTarget.ActualHeight > 0 && SwapChainPanelTarget.ActualWidth > 0)
            {
                if (renderer == null)
                {
                    renderer = new SwapChainPanelRenderer(grayscaleEffect, SwapChainPanelTarget);
                }
            }
        }
    }
}
