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
    /// Interaction logic for NoteInfoForm.xaml
    /// </summary>
    public partial class NoteInfoForm : Window
    {
        private Note note = new Note();
        public NoteInfoForm(Note note)
        {
            InitializeComponent();

            this.note = note;
            this.DataContext = this.note;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PatientButton_Click(object sender, RoutedEventArgs e)
        {
            PatientForm patientForm = new PatientForm(note.Patient);
            patientForm.Show();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            note.StatusId = 4;
            note.Status = DBHelper.GetContext().Status.Where(n => n.Id == 4).Single();
        }
    }
}
