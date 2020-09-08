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
    /// Interaction logic for AdminPage.xaml
    /// </summary>
    public partial class AdminPage : Page
    {
        private ObservableCollection<Note> notes = new ObservableCollection<Note>(DBHelper.GetContext().Note.ToList());
        private User user = new User();
        public AdminPage(User user)
        {
            InitializeComponent();

            NoteList.ItemsSource = notes;
            this.user = user;

            var assistants = DBHelper.GetContext().User.ToList();
            assistants.Insert(0, new User { Name = "Все лаборанты" });
            LaborantCombo.ItemsSource = assistants;

            var statuses = DBHelper.GetContext().Status.ToList();
            statuses.Insert(0, new Status { Name = "Все статусы" });
            StatusCombo.ItemsSource = statuses;
        }

        /// <summary>
        /// Сортировка списка записей
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            var notes = DBHelper.GetContext().Note.ToList();

            if (NameSortCheck.IsChecked == true)
                notes = notes.OrderBy(n => n.Patient.FullName).ToList();
            if (TimeSortCheck.IsChecked == true)
                notes = notes.OrderBy(n => n.CreateDate).ToList();
            if (LaborantCombo.SelectedItem != null && LaborantCombo.SelectedIndex != 0)
                notes = notes.Where(n => n.LaborantId == ((User)LaborantCombo.SelectedItem).Id).ToList();
            if (StatusCombo.SelectedItem != null && StatusCombo.SelectedIndex != 0)
                notes = notes.Where(n => n.StatusId == ((Status)StatusCombo.SelectedItem).Id).ToList();

            NoteList.ItemsSource = new ObservableCollection<Note>(notes); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EditItem_Click(object sender, RoutedEventArgs e)
        {
            var note = (Note)NoteList.SelectedItem;
            if (note.StatusId == 4)
            {
                MessageBox.Show("Нельзя редактировать записи с готовыми результатами анализов.", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            NoteEditForm noteEditForm = new NoteEditForm(note);
            noteEditForm.ShowDialog();
            ReloadPage();
            NoteList.Items.Refresh();
        }

        private void ReloadPage()
        {
            DBHelper.GetContext().ChangeTracker.Entries().ToList().ForEach(n => n.Reload());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteItem_Click(object sender, RoutedEventArgs e)
        {
            var note = (Note)NoteList.SelectedItem;

            if (MessageBox.Show("Вы действительно хотите удалить эту запись?", "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DBHelper.GetContext().Service.RemoveRange( note.Service.ToList());

                DBHelper.GetContext().Note.Remove(note);
                DBHelper.GetContext().SaveChanges();

                notes.Remove(note);
                NoteList.Items.Refresh();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AssistButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LaborantPage(user));
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

        private void ResultButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new ResultPage(user));
        }
    }
}
