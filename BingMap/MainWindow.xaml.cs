using BingMap.Services.Models;

using Microsoft.Maps.MapControl.WPF;
using Microsoft.Maps.MapControl.WPF.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
namespace BingMap;

public partial class MainWindow : Window, INotifyPropertyChanged
{
    private bool isokay = true;
    private BakuBus? bakuBus;
    public BakuBus? BakuBus
    {
        get { return bakuBus; }
        set
        {
            bakuBus = value;
            NotifyPropertyChanged();
        }
    }

    public string MyProperty2 { get; set; }
    public string CurrentStop { get; set; }
    public string PrevStop { get; set; }
    DispatcherTimer timer = new DispatcherTimer();
    public MainWindow()
    {
        DataContext = this;
        InitializeComponent();
        timer.Interval = new TimeSpan(0, 0, 0, 1);
        timer.Tick += Timer_Tick;
        timer.Start();
    }


    public SolidColorBrush ColorBus { get; set; }
    public string ROUTENAME { get; set; }
    private async void Window_Loaded(object sender, RoutedEventArgs e)
    {

            using HttpClient client = new HttpClient();
            var jsonString = await client.GetStringAsync("https://www.bakubus.az/az/ajax/apiNew1");
            BakuBus = JsonSerializer.Deserialize<BakuBus>(jsonString);
        

    }
    private SolidColorBrush Col()
    {
        Random r = new();
        return new SolidColorBrush(Color.FromRgb((byte)r.Next(0, 255), (byte)r.Next(0, 255), (byte)r.Next(0, 255)));
    }
    private static List<Pushpin> baku1 = new();
    static Pushpin pshp = new Pushpin();
    [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
    static extern int MessageBoxTimeout(IntPtr hwnd, String text, String title,uint type, Int16 wLanguageId, Int32 milliseconds);
    private void BusList()
    {
        if (isokay)
        {

            isokay = false;
            for (int i = 0; i < bakuBus.BUS.Count; i++)
            {

                pshp = new();
                string av = bakuBus.BUS[i].Attributes.PLATE;
                pshp.Name =$"A{av}";
                double num1 = Convert.ToDouble(bakuBus.BUS[i].Attributes.LONGITUDE);
                double num2 = Convert.ToDouble(bakuBus.BUS[i].Attributes.LATITUDE);
                Style ax = (Style)(Resources["Men"]);
                MyProperty2 = bakuBus.BUS[i].Attributes.DISPLAY_ROUTE_CODE;
                ROUTENAME = bakuBus.BUS[i].Attributes.ROUTE_NAME;
                CurrentStop = $"Cari: {bakuBus.BUS[i].Attributes.CURRENT_STOP}";
                PrevStop = $"Novbeti: {bakuBus.BUS[i].Attributes.PREV_STOP}";
                MessageBoxTimeout((System.IntPtr)0,"", "", 0, 0,1);
                ColorBus = Col();
                pshp.MouseEnter += Pshp_MouseEnter;
                pshp.MouseLeave += Pshp_MouseLeave;
                pshp.Tag = i.ToString();
                pshp.Style = ax;
                pshp.Location = new Location(num2, num1);
                baku1.Add(pshp);
                m.Children.Add(baku1[baku1.Count-1]);
                
            }
        }
        else
        {
            try
            {
                    for (int i = 0; i < BakuBus.BUS.Count; i++)
                    {
                        if ($"A{bakuBus.BUS[i].Attributes.PLATE}" == baku1[i].Name)
                        {
                            double num1 = Convert.ToDouble(bakuBus.BUS[i].Attributes.LONGITUDE);
                            double num2 = Convert.ToDouble(bakuBus.BUS[i].Attributes.LATITUDE);
                            baku1[i].Location = new Location(num2, num1);
                        }
                    };
            }
            catch { MessageBox.Show("Error"); }
        }
    }

    private void Pshp_MouseLeave(object sender, MouseEventArgs e)
    {
        Values.Background = new SolidColorBrush(Colors.Transparent);
        Values.IsHitTestVisible = false;
        Busimage.Visibility = Visibility.Hidden;
        Text1.Content = "";
        Sep.Visibility = Visibility.Hidden;
        Text2.Content = "";
        Text3.Content = "";
    }
    private void Pshp_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
    {
        if (sender is Pushpin psp)
        {
                    Values.IsHitTestVisible = true;
                    Values.Background = new SolidColorBrush(Colors.White);
                    Busimage.Visibility = Visibility.Visible;
                    Text1.Content = bakuBus.BUS[Convert.ToInt32(psp.Tag)].Attributes.ROUTE_NAME;
                    Sep.Visibility = Visibility.Visible;
                    Text2.Content = bakuBus.BUS[Convert.ToInt32(psp.Tag)].Attributes.CURRENT_STOP;
                    Text3.Content = bakuBus.BUS[Convert.ToInt32(psp.Tag)].Attributes.PREV_STOP;
        }
    }

    private async void Timer_Tick(object? sender, EventArgs e)
    {
        using HttpClient client = new HttpClient();
        var jsonString = await client.GetStringAsync("https://www.bakubus.az/az/ajax/apiNew1");
        BakuBus = JsonSerializer.Deserialize<BakuBus>(jsonString);
        BusList();
    }
    public event PropertyChangedEventHandler? PropertyChanged;

    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}