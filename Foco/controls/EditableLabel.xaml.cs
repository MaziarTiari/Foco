using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Foco.controls
{
    /// <summary>
    /// Interaktionslogik für EditableLabel.xaml
    /// </summary>
    public partial class EditableLabel : UserControl
    {

        public delegate void EditedCallback(string text);

        private bool isEditing;
        private bool allowEditing;
        private EditedCallback editedCallback;

        public EditedCallback EditedCallbackFunction { get => editedCallback; set => editedCallback = value; }
        public bool AllowEditing { get => allowEditing; set => allowEditing = value; }
        public int MaxLength { get => EditTextBox.MaxLength; set => EditTextBox.MaxLength = value; }
        public bool AllowNewLine { get => EditTextBox.AcceptsReturn; set => EditTextBox.AcceptsReturn = value; }
        public string Text { get => EditLabel.Text; set { EditLabel.Text = value; EditTextBox.Text = value; } }

        public EditableLabel()
        {
            InitializeComponent();
            isEditing = false;
            allowEditing = true;
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BeginEditing();
        }

        public void BeginEditing()
        {
            if (!isEditing && allowEditing)
            {
                EditRow.Height = new GridLength(1, GridUnitType.Star);
                LabelRow.Height = new GridLength(0);
                EditTextBox.Text = EditLabel.Text;
                EditTextBox.Focus();
                EditTextBox.SelectAll();
                isEditing = true;

                // Workaround fuer den Fokus der Textbox (von stackoverflow.com):
                // https://stackoverflow.com/questions/13955340/keyboard-focus-does-not-work-on-text-box-in-wpf
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() => EditTextBox.Focus()));

            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && isEditing && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
            {
                EditRow.Height = new GridLength(0);
                LabelRow.Height = new GridLength(1, GridUnitType.Star);
                EditLabel.Text = EditTextBox.Text;
                isEditing = false;
                if (editedCallback != null)
                    editedCallback(EditLabel.Text);
            }
        }

    }
}
