using MedLab.Database.DBHelper;
using MedLab.Views.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace MedLab.Views.Pages
{
    /// <summary>
    /// Interaction logic for LaborantPage.xaml
    /// </summary>
    public partial class LaborantPage : Page
    {
        public User User { get; set; }
        public LaborantPage(User user)
        {
            InitializeComponent();
            this.User = user;
            this.DataContext = User;

            var statuses = DBHelper.GetContext().Status.ToList();
            
            statuses.Insert(0, new Status { Name = "Все статусы" });
            StatusCombo.ItemsSource = statuses;

            NoteList.ItemsSource = new ObservableCollection<Note>(User.Note.ToList());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            var notes = User.Note.ToList();

            if (NameSortCheck.IsChecked == true)
                notes = notes.OrderBy(n => n.Patient.FullName).ToList();
            if (TimeSortCheck.IsChecked == true)
                notes = notes.OrderBy(n => n.CreateDate).ToList();
            if (StatusCombo.SelectedItem != null && StatusCombo.SelectedIndex != 0)
                notes = notes.Where(n => n.StatusId == ((Status)StatusCombo.SelectedItem).Id).ToList();

            NoteList.ItemsSource = new ObservableCollection<Note>(notes); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoneButton_Click(object sender, RoutedEventArgs e)
        {
            var note = (Note)NoteList.SelectedItem;

            if (note.StatusId == 4)
            {
                MessageBox.Show("Эти анализы и так готовы.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (MessageBox.Show("Анализы точно готовы?\nЭто действие нельзя будет обратить.", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                note.StatusId = 4;
                note.Status = DBHelper.GetContext().Status.Where(n => n.Id == 4).SingleOrDefault();
            }
            NoteList.Items.Refresh();
            DBHelper.GetContext().SaveChanges();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            NoteInfoForm noteInfoForm = new NoteInfoForm((Note)NoteList.SelectedItem);
            noteInfoForm.ShowDialog();
        }
    }
}
