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

        // Beginnt das Editieren
        public void BeginEditing()
        {
            if (!isEditing && allowEditing)
            {
                EditRow.Height = new GridLength(1, GridUnitType.Star);
                LabelRow.Height = new GridLength(0);
                EditTextBox.Text = EditLabel.Text;

                // Workaround fuer den Fokus der Textbox (von stackoverflow.com):
                // https://stackoverflow.com/questions/13955340/keyboard-focus-does-not-work-on-text-box-in-wpf
                Dispatcher.BeginInvoke(DispatcherPriority.Input, new ThreadStart(() => _BeginEditingThread())); ;

                void _BeginEditingThread()
                {
                    EditTextBox.Focus();
                    EditTextBox.SelectAll();
                    isEditing = true;
                }

            }
        }

        // beendet das Editieren und ruft das Callback auf
        public void EndEditing()
        {
            if (isEditing)
            {
                EditRow.Height = new GridLength(0);
                LabelRow.Height = new GridLength(1, GridUnitType.Star);
                EditLabel.Text = EditTextBox.Text;
                isEditing = false;
                if (editedCallback != null)
                    editedCallback(EditLabel.Text);
            }
        }

        // Doppelklick beginnt das Editieren
        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BeginEditing();
        }

        // Fokusverlust beendet das Editieren
        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            EndEditing();
        }

        // Enter (ohne Shift) beendet das Editieren
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && isEditing && !Keyboard.IsKeyDown(Key.LeftShift) && !Keyboard.IsKeyDown(Key.RightShift))
                EndEditing();
        }

    }
}
