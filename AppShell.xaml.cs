namespace Clockin
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
        }

        public class TabState
        {
            public int LastActiveTabID { get; set; }
            public int NumberOfTabs { get; set; }

        }
    }
}
