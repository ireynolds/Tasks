using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace Tasks.Common
{
    public partial class WatermarkedTextBox : UserControl
    {
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(WatermarkedTextBox), new PropertyMetadata(string.Empty));

        public string Watermark
        {
            get { return (string)GetValue(WatermarkProperty); }
            set { SetValue(WatermarkProperty, value); }
        }

        public static readonly DependencyProperty WatermarkProperty =
            DependencyProperty.Register("Watermark", typeof(string), typeof(WatermarkedTextBox), new PropertyMetadata(string.Empty));

        public WatermarkedTextBox()
        {
            this.Loaded += WatermarkedTextBox_Loaded;
            InitializeComponent();
            this.LayoutRoot.DataContext = this;

            ContentBox.GotFocus += ContentBox_GotFocus;
            ContentBox.LostFocus += ContentBox_LostFocus;

            // HACK:: The TwoWay binding between ContentBox.Text and Value isn't working for some
            // reasons.
            ContentBox.TextChanged += (s, e) => Value = ContentBox.Text;

            OriginalForegroundBrush = ContentBox.Foreground;
        }

        private void WatermarkedTextBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.Loaded -= WatermarkedTextBox_Loaded;
            ContentBox_LostFocus(null, null);
        }

        private bool IsWatermarkVisible;
        private Brush OriginalForegroundBrush;
        private Brush WatermarkForegroundBrush = new SolidColorBrush(new Color() { R = 105, G = 105, B = 105, A = 255 }); // DimGray

        private void ContentBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (Value.Equals(string.Empty))
            {
                IsWatermarkVisible = true;

                Value = Watermark;
                ContentBox.Foreground = WatermarkForegroundBrush;
            }
        }

        private void ContentBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (IsWatermarkVisible)
            {
                IsWatermarkVisible = false;

                Value = string.Empty;
                ContentBox.Foreground = OriginalForegroundBrush;
            }
        }
    }
}
