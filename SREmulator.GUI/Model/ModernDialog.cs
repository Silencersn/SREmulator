using iNKORE.UI.WPF.Modern.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SREmulator.GUI.Model
{
    class ModernDialog
    {
        /// <summary>
        /// 显示一个带 Yes/No 按钮的对话框，但消息内容支持 Markdown 格式
        /// </summary>
        public static async Task<bool?> ShowMarkdownAsync(string message, string title)
        {
            var markdownViewer = new Markdig.Wpf.MarkdownViewer
            {
                Markdown = message
            };
            if (markdownViewer.Document is not null)
                markdownViewer.Document.FontFamily = new FontFamily("Microsoft YaHei UI");
            var dialog = new ContentDialog
            {
                Title = title,
                Content = markdownViewer,
                PrimaryButtonText = "是",
                CloseButtonText = "否"
            };

            var result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        /// <summary>
        /// 显示一个带错误信息的对话框，包含"复制信息到剪切板"和"确认"按钮
        /// </summary>
        public static async Task ShowErrorAsync(string message, string title = "错误")
        {
            // StackPanel
            var stackPanel = new StackPanel
            {
                Orientation = Orientation.Horizontal
            };

            // FontIcon
            var icon = new FontIcon
            {
                Glyph = "\uEA39", // ErrorBadge图标
                FontFamily = new FontFamily("Segoe MDL2 Assets"),
                FontSize = 20,
                Margin = new Thickness(0, 0, 10, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            // TextBlock
            var textBlock = new TextBlock
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 400 // 限制最大宽度以确保可读性
            };
            // Add
            stackPanel.Children.Add(icon);
            stackPanel.Children.Add(textBlock);

            // ContentDialog
            var dialog = new ContentDialog
            {
                Title = title,
                Content = stackPanel,
                PrimaryButtonText = "复制信息到剪切板",
                CloseButtonText = "确认",
                // 自适应大小
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                // 复制信息到剪切板
                Clipboard.SetText(message);
            }
        }
    }
}
