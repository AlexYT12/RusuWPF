using System.ComponentModel;
using System.Windows;

namespace Rusu.Views
{
    /// <summary>
    /// Логика взаимодействия для TemplateEditorWindow.xaml
    /// </summary>
    public partial class TemplateEditorWindow : Window
    {
        public TemplateEditorWindow()
        {
            InitializeComponent();
        }

        internal bool CantClose = true;
        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = CantClose;
            Hide();
        }
    }
}
