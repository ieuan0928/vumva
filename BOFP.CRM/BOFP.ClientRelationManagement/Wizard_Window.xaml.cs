﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BOFP.ClientRelationManagement.Controls;

namespace BOFP.ClientRelationManagement
{
    /// <summary>
    /// Interaction logic for Wizard_Window.xaml
    /// </summary>
    public partial class Wizard_Window : Window
    {
        public Wizard_Window()
        {
            InitializeComponent();
        }

        private void BaseControl_NextEnabledChanged(object sender, EnabledChangedEventArgs e)
        {
            btnNext.IsEnabled = e.Value;
        }

        public Dictionary<string, IControl> controlList = new Dictionary<string, IControl>();

        public void AddControl(string _ItemCaption, IControl _Control)
        {
            listBox1.Items.Add(new ListBoxItem() { Content = _ItemCaption, Padding = new Thickness(20, 14, 20, 14) });
            ((UserControl)_Control).Visibility = System.Windows.Visibility.Collapsed;
            RightPanel.Children.Add((UserControl)_Control);
            //_Control.DataModel = this.DataContext as UserInformation;
            ((BaseControl)_Control).NextEnabledChanged += BaseControl_NextEnabledChanged;

            controlList.Add(_ItemCaption, _Control);
        }

        public void HideControls(string excemptedKey)
        {
            foreach (KeyValuePair<string, IControl> key in controlList)
                if (key.Key != excemptedKey) ((UserControl)key.Value).Visibility = System.Windows.Visibility.Collapsed;
        }

        private void listBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string key = (listBox1.SelectedItem as ListBoxItem).Content.ToString();
            HideControls(key);
            IControl ctrl = controlList[key];
            wizardTitle.Content = ctrl.Title;
            ((UserControl)ctrl).Visibility = System.Windows.Visibility.Visible;
            btnNext.IsEnabled = ((BaseControl)ctrl).IsNextEnabled;
            btnPrev.IsEnabled = ((BaseControl)ctrl).IsPreviousEnabled;
            btnCreate.IsEnabled = ((BaseControl)ctrl).IsCreateEnable;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (listBox1.Items.Count > 0)
                listBox1.SelectedIndex = 0;
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex < listBox1.Items.Count) listBox1.SelectedIndex++;
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex < listBox1.Items.Count) listBox1.SelectedIndex--;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            MessageBoxResult ask = MessageBox.Show("Are you want to cancel Wizard?","Cancel Wizard", MessageBoxButton.YesNo);
            if (ask == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }



    }


}
