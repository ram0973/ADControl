using System;
using System.Windows.Forms;


namespace ADcontrol
{
    class AppMenus
    {
        public static ContextMenuStrip gridContextMenu;

        public static void InitAppMenus()
        {
            gridContextMenu = new ContextMenuStrip();

            gridContextMenu.Items.Add("Connect via Shadow RDP");
            gridContextMenu.Items[0].Click += new System.EventHandler(menuItem1_Click);
            
        }

        private static void menuItem1_Click(object sender, EventArgs e)
        {
            AppActions.ConnectViaShadowRDP("");

        }
    }
}
