using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace BOMBS.UI.Foundation.Controls
{
    public class PasswordBoxExtension
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(PasswordBoxExtension), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));
        public static string GetPassword(DependencyObject dp) { return (string)dp.GetValue(PasswordProperty); }
        public static void SetPassword(DependencyObject dp, string value) { dp.SetValue(PasswordProperty, value); }

        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(PasswordBoxExtension), new PropertyMetadata(false, AttachCallBack));
        public static bool GetAttach(DependencyObject dp) { return (bool)dp.GetValue(AttachProperty); }
        public static void SetAttach(DependencyObject dp, bool value) { dp.SetValue(AttachProperty, value); }

        public static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxExtension));
        public static bool GetIsUpdating(DependencyObject dp) { return (bool)dp.GetValue(IsUpdatingProperty); }
        public static void SetIsUpdating(DependencyObject dp, bool value) { dp.SetValue(IsUpdatingProperty, value); }

        private static void AttachCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = d as PasswordBox;

            if (passwordBox == null) return;

            if ((bool)e.NewValue) passwordBox.PasswordChanged += passwordBox_PasswordChanged;
            else passwordBox.PasswordChanged -= passwordBox_PasswordChanged;
        }

        private static void OnPasswordPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = d as PasswordBox;

            passwordBox.PasswordChanged -= passwordBox_PasswordChanged;

            if (!GetIsUpdating(passwordBox)) { passwordBox.Password = (string)e.NewValue; }

            passwordBox.PasswordChanged += passwordBox_PasswordChanged;
        }

        private static void passwordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            SetIsUpdating(passwordBox, true);

            SetPassword(passwordBox, passwordBox.Password);

            SetIsUpdating(passwordBox, false);
        }
    }
}
