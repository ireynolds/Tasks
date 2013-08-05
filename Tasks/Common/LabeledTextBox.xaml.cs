using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;

namespace Tasks.Common
{
    public partial class LabeledTextBox : UserControl
    {
        public string Value
        {
            get { return (string)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register("Value", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(string.Empty));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register("Label", typeof(string), typeof(LabeledTextBox), new PropertyMetadata(string.Empty));

        public bool AcceptsReturn
        {
            get { return (bool)GetValue(AcceptsReturnProperty); }
            set { SetValue(AcceptsReturnProperty, value); }
        }

        public static readonly DependencyProperty AcceptsReturnProperty =
            DependencyProperty.Register("AcceptsReturn", typeof(bool), typeof(LabeledTextBox), new PropertyMetadata(false));

        public LabeledTextBox()
        {
            InitializeComponent();
            this.LayoutRoot.DataContext = this;
            
            // HACK:: The TwoWay binding between ContentBox.Text and Value isn't working for some
            // reasons.
            ContentBox.TextChanged += (s, e) => Value = ContentBox.Text;
        }
    }
}
