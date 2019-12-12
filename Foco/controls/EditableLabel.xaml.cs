﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

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
        public string Text { get => EditLabel.Text; set { EditLabel.Text = value; EditTextBox.Text = value; } }

        public EditableLabel()
        {
            InitializeComponent();
            isEditing = false;
            allowEditing = true;
        }

        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (!isEditing && allowEditing)
            {
                EditRow.Height = new GridLength(1, GridUnitType.Star);
                LabelRow.Height = new GridLength(0);
                EditTextBox.Text = EditLabel.Text;
                EditTextBox.Focus();
                EditTextBox.CaretIndex = EditTextBox.Text.Length;
                isEditing = true;
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && isEditing)
            {
                EditRow.Height = new GridLength(0);
                LabelRow.Height = new GridLength(1, GridUnitType.Star);
                EditLabel.Text = EditTextBox.Text;
                isEditing = false;
                if(editedCallback != null)
                    editedCallback(EditLabel.Text);
            }
        }

    }
}
