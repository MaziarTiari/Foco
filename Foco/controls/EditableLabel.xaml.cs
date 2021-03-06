﻿using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Foco.controls
{
    // Interaktionslogik für EditableLabel.xaml
    public partial class EditableLabel : UserControl
    {

        public delegate void EditedCallback(string text);

        private bool isEditing;
        private bool allowEditing;
        private EditedCallback editedCallback;

        public EditableLabel()
        {
            InitializeComponent();
            isEditing = false;
            allowEditing = true;
        }

        public EditedCallback EditedCallbackFunction
        {
            get => editedCallback; set => editedCallback = value;
        }
        public bool AllowEditing
        {
            get => allowEditing; set => allowEditing = value;
        }
        public int MaxLength
        {
            get => EditTextBox.MaxLength; set => EditTextBox.MaxLength = value;
        }
        public bool AllowNewLine
        {
            get => EditTextBox.AcceptsReturn; set => EditTextBox.AcceptsReturn = value;
        }
        public string Text
        {
            get => EditLabel.Text;
            set { EditLabel.Text = value; EditTextBox.Text = value; }
        }

        public void BeginEditing()
        {
            if (!isEditing && allowEditing)
            {
                EditRow.Height = new GridLength(1, GridUnitType.Star);
                LabelRow.Height = new GridLength(0);
                EditTextBox.Text = EditLabel.Text;

                // Workaround for focus the textbox (from stackoverflow.com):
                // https://stackoverflow.com/questions/13955340/keyboard-focus-does-not-work-on-text-box-in-wpf
                Dispatcher.BeginInvoke(
                    DispatcherPriority.Input,
                    new ThreadStart( () => _BeginEditingThread() )
                    );

                void _BeginEditingThread()
                {
                    EditTextBox.Focus();
                    EditTextBox.SelectAll();
                    isEditing = true;
                }

            }
        }

        public void EndEditing()
        {
            if (isEditing)
            {
                EditRow.Height = new GridLength(0);
                LabelRow.Height = new GridLength(1, GridUnitType.Star);
                EditLabel.Text = EditTextBox.Text;
                isEditing = false;
                editedCallback?.Invoke(EditLabel.Text);
            }
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BeginEditing();
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            EndEditing();
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && isEditing 
                    && !Keyboard.IsKeyDown(Key.LeftShift) 
                    && !Keyboard.IsKeyDown(Key.RightShift))
            {
                EndEditing();
            }
        }

    }
}
