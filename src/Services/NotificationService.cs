using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Refracion
{
    internal class NotificationService
    {
        public static void ShowErrorMessage(string message)
        {
            System.Windows.Forms.MessageBox.Show(message, "Refraction", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
