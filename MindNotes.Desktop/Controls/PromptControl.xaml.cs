using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Input;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MindNotes.Desktop.Controls
{
    public sealed partial class PromptControl : UserControl
    {
        public PromptControl()
        {
            this.InitializeComponent();
        }

        private void button_PointerEntered(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Hand);
        }

        private void button_PointerExited(object sender, Microsoft.UI.Xaml.Input.PointerRoutedEventArgs e) {
            this.ProtectedCursor = InputSystemCursor.Create(InputSystemCursorShape.Arrow);
        }

    }
}
