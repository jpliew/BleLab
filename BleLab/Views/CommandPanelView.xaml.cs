﻿using System;
using System.Collections;
using System.Linq;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace BleLab.Views
{
    public sealed partial class CommandPanelView : UserControl
    {
        private DateTimeOffset _prevCommandTime;

        public CommandPanelView()
        {
            this.InitializeComponent();
            Hide();
        }

        public void ResetTimeDiff()
        {
            _prevCommandTime = default(DateTimeOffset);
        }

        public void AddString(string s)
        {
            AddInline(new Run() { Text = s });
        }

        public void AddInline(Inline inline)
        {
            var paragraph = new Paragraph();
            paragraph.Inlines.Add(inline);
            AddParagraph(paragraph);
        }

        public void AddParagraph(Paragraph paragraph)
        {
            PrepandTime(paragraph);

            Viewer.Blocks.Add(paragraph);

            Scroller.UpdateLayout();
            Scroller.ChangeView(0.0f, double.MaxValue, 1.0f);
        }

        public void Clear()
        {
            Viewer.Blocks.Clear();
        }

        private void PrepandTime(Paragraph paragraph)
        {
            var now = DateTimeOffset.Now;
            var timeRun = new Run
            {
                Text = now.ToString("T").Replace(" ", ""),
                FontWeight = FontWeights.ExtraLight,
                Foreground = new SolidColorBrush(Colors.Gray)
            };

            if (_prevCommandTime == default(DateTimeOffset))
                _prevCommandTime = now;

            var diffSeconds = (now - _prevCommandTime).TotalSeconds;
            var diffText = FormatDiff(diffSeconds);

            timeRun.Text += diffText;

            _prevCommandTime = now;

            paragraph.Inlines.Insert(0, timeRun);
        }

        private string FormatDiff(double diffSeconds)
        {
            var format = "0.000";
            var diffNumText = diffSeconds.ToString(format).Substring(0, format.Length); // keeping unified width
            var diffText = $"+{diffNumText}s ";

            if (diffSeconds < 0.001)
                diffText = new string(' ', diffText.Length);

            return diffText;
        }
        
        private void Expand_OnClick(object sender, RoutedEventArgs e)
        {
            Expand();
        }

        private void Hide_OnClick(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Expand()
        {
            ExpandButton.Visibility = Visibility.Collapsed;
            HideButton.Visibility = Visibility.Visible;
            Scroller.Visibility = Visibility.Visible;
            ButtonsPanel.Visibility = Visibility.Visible;
        }

        private void Hide()
        {
            ExpandButton.Visibility = Visibility.Visible;
            HideButton.Visibility = Visibility.Collapsed;
            Scroller.Visibility = Visibility.Collapsed;
            ButtonsPanel.Visibility = Visibility.Collapsed;
        }
    }
}