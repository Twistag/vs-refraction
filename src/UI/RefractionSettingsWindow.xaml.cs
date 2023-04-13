using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Refraction
{

    public partial class RefractionSettingsWindow : UserControl
    {

        public RefractionSettingsWindow()
        {
            InitializeComponent();
            var userCredentials = PropertyService.GetUserCredentials();
            userIdValueField.Text = userCredentials.UserId;
            teamIdValueField.Text = userCredentials.TeamId;
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            var userCredentials = new UserCredentials(userIdValueField.Text, teamIdValueField.Text);
            PropertyService.saveUserCredentials(userCredentials);
            Window.GetWindow(this).Close();
        }

    }
}
