using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetspeedMainWebService
{
    public partial class CallMe_Form : Form
    {
        public CallMe_Form()
        {
            InitializeComponent();
        }

        private void CallMe_Form_Load(object sender, EventArgs e)
        {
            
        }

        private void SendCallMe_Button_Click(object sender, EventArgs e)
        {
            var FullName = CallMeFullName_TextBox.Text;
            var PhoneNumber = CallMePhoneNumber_TextBox.Text;
        }
    }
}
