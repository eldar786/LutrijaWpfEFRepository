using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace CurrencyTextBoxControl
{
    [TemplatePart(Name = TextBoxName, Type = typeof(TextBox))]
    public class CurrencyTextBox : Control
    {
        public delegate string ValidationDelegate(decimal number);

        #region Constants
        private const string TextBoxName = "PART_TextBox";
        #endregion

        #region Fields
        private TextBox _textBox;
        #endregion

        #region Dependency Properties
        public static readonly DependencyProperty NumberProperty = DependencyProperty.Register(
            "Number",
            typeof(decimal),
            typeof(CurrencyTextBox),
            new FrameworkPropertyMetadata(0M, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        public decimal Number
        {
            get
            {
                var number = (decimal)GetValue(NumberProperty);
                return number;
            }
            set
            {
                SetValue(NumberProperty, value);
            }
        }
        #endregion

        #region Constructor
        static CurrencyTextBox()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(CurrencyTextBox),
                new FrameworkPropertyMetadata(typeof(CurrencyTextBox)));
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _textBox = this.GetTemplateChild(TextBoxName) as TextBox;
            if (_textBox == null)
            {
                throw new Exception(string.Format(
                    "TextBox not found in template. Make sure the TextBox's name is \"{0}\".",
                    TextBoxName));
            }

            // Disable copy/paste
            DataObject.AddCopyingHandler(_textBox, PastingEventHandler);
            DataObject.AddPastingHandler(_textBox, PastingEventHandler);

            _textBox.CaretIndex = _textBox.Text.Length;

            _textBox.PreviewKeyDown += TextBox_PreviewKeyDown;
            _textBox.PreviewMouseDown += TextBox_PreviewMouseDown;
            _textBox.PreviewMouseUp += TextBox_PreviewMouseUp;
            _textBox.TextChanged += TextBox_TextChanged;
            _textBox.ContextMenu = null;
        }
        #endregion

        #region Events
        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Keep the caret at the end
            var tb = sender as TextBox;
            tb.CaretIndex = tb.Text.Length;
        }

        private void TextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Prevent changing the caret index
            e.Handled = true;
            (sender as TextBox).Focus();
        }

        void TextBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            // Prevent changing the caret index
            e.Handled = true;
            (sender as TextBox).Focus();
        }

        private void TextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            e.Handled = true;

            if (Keyboard.Modifiers != ModifierKeys.None)
            {
                return;
            }
            else if (IsNumericKey(e.Key))
            {
                // Push the new number from the right
                Number = (Number * 10M) + (GetDigitFromKey(e.Key) / 100M);
            }
            else if (e.Key == Key.Back)
            {
                // Remove the right-most digit
                Number = (Number - (Number % 0.1M)) / 10M;
            }
            else if (e.Key == Key.Delete)
            {
                Number = 0M;
            }
            else if (e.Key == Key.Subtract || e.Key == Key.OemMinus)
            {
                Number *= -1;
            }
            else if(e.Key == Key.Tab)
            {
                ((UIElement)e.OriginalSource).MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
            }
        }

        private void PastingEventHandler(object sender, DataObjectEventArgs e)
        {
            // Prevent copy/paste
            e.CancelCommand();
        }
        #endregion

        #region Private Methods
        private decimal GetDigitFromKey(Key key)
        {
            switch (key)
            {
                case Key.D0:
                case Key.NumPad0: return 0M;
                case Key.D1:
                case Key.NumPad1: return 1M;
                case Key.D2:
                case Key.NumPad2: return 2M;
                case Key.D3:
                case Key.NumPad3: return 3M;
                case Key.D4:
                case Key.NumPad4: return 4M;
                case Key.D5:
                case Key.NumPad5: return 5M;
                case Key.D6:
                case Key.NumPad6: return 6M;
                case Key.D7:
                case Key.NumPad7: return 7M;
                case Key.D8:
                case Key.NumPad8: return 8M;
                case Key.D9:
                case Key.NumPad9: return 9M;
                default: throw new ArgumentOutOfRangeException("Invalid key: " + key.ToString());
            }
        }

        private bool IsNumericKey(Key key)
        {
            return key == Key.D0 ||
                key == Key.D1 ||
                key == Key.D2 ||
                key == Key.D3 ||
                key == Key.D4 ||
                key == Key.D5 ||
                key == Key.D6 ||
                key == Key.D7 ||
                key == Key.D8 ||
                key == Key.D9 ||
                key == Key.NumPad0 ||
                key == Key.NumPad1 ||
                key == Key.NumPad2 ||
                key == Key.NumPad3 ||
                key == Key.NumPad4 ||
                key == Key.NumPad5 ||
                key == Key.NumPad6 ||
                key == Key.NumPad7 ||
                key == Key.NumPad8 ||
                key == Key.NumPad9;
        }

        private bool IsBackspaceKey(Key key)
        {
            return key == Key.Back;
        }
        #endregion
    }
}
