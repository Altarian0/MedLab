using MedLab.Database.DBHelper;
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
using System.Windows.Shapes;

namespace MedLab.Views.Forms
{
    /// <summary>
    /// Interaction logic for PatientForm.xaml
    /// </summary>
    public partial class PatientForm : Window
    {
        private Patient patient = new Patient();
        public PatientForm(Patient patient)
        {
            InitializeComponent();
            this.patient = patient;
            this.DataContext = this.patient;
        }
    }
}
