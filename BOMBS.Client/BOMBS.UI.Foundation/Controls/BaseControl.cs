using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;

using BOMBS.Core.Common;

namespace BOMBS.UI.Foundation.Controls
{
    public class EnabledChangedEventArgs : EventArgs
    {
        public bool Value { get; set; }
    }


    public class BaseControl : UserControl, IControl
    {
        public delegate void EnabledChangedHandler(object sender, EnabledChangedEventArgs e);
        public event EnabledChangedHandler NextEnabledChanged;
        public event EnabledChangedHandler PreviousEnabledChanged;
        public event EnabledChangedHandler CreateEnabledChanged;


        public IModelBase DataModel
        {
            set
            {
                DataContext = (IModelBase)value;
            }
        }

        private string _title = string.Empty;
        public string Title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        public void LoadState()
        {

        }

        public void ExitState()
        {

        }

        private bool _isPreviousEnabled = true;
        public bool IsPreviousEnabled
        {

            get
            {
                return _isPreviousEnabled;
            }
            set
            {

                if (value != _isPreviousEnabled)
                {
                    _isPreviousEnabled = value;
                    if (PreviousEnabledChanged != null) PreviousEnabledChanged(this, new EnabledChangedEventArgs() { Value = value });
                }
            }
        }

        private bool _isNextEnabled = true;
        public bool IsNextEnabled
        {
            get
            {
                return _isNextEnabled;
            }
            set
            {

                if (value != _isNextEnabled)
                {
                    _isNextEnabled = value;
                    if (NextEnabledChanged != null)
                        NextEnabledChanged(this, new EnabledChangedEventArgs() { Value = value });
                }
            }
        }

        private bool _isCreateEnabled = true;
        public bool IsCreateEnable
        {
            get
            {
                return _isCreateEnabled;
            }
            set
            {
                if (value != _isCreateEnabled)
                {
                    _isCreateEnabled = value;
                    if (CreateEnabledChanged != null)
                        CreateEnabledChanged(this, new EnabledChangedEventArgs() { Value = value });
                }
            }

        }

        public bool HasError(DependencyObject parent)
        {
            bool result = false;
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                if (Validation.GetHasError(child))
                {
                    result = true;
                    break;

                }
                if (HasError(child))
                {
                    result = true;
                    break;
                }
            }


            return result;
        }
    }
}