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
    /// Interaction logic for NoteEditForm.xaml
    /// </summary>
    public partial class NoteEditForm : Window
    {
        public Note Note { get; set; }
        public NoteEditForm(Note note)
        {
            InitializeComponent();

            this.Note = note;
            this.DataContext = Note;
            AssistantCombo.ItemsSource = DBHelper.GetContext().User.ToList();
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
        /// Сохранение при успешной валидации данных
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            foreach (var service in Note.Service)
            {
                if (service.SendDate == null)
                {
                    MessageBox.Show("Нельзя прикрепить лаборанта к записи без всех сданных биоматериалов!","Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }

            if (AssistantCombo.SelectedItem != null)
            {
                Note.StatusId = 3;
                Note.Status = DBHelper.GetContext().Status.Where(n => n.Id == 3).SingleOrDefault();
            }

            DBHelper.GetContext().SaveChanges();
            this.Close();
        }

        /// <summary>
        /// Сдать биоматериалы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var service = (Service)ServiceList.SelectedItem;
            service.SendDate = DateTime.Now;
            Note.StatusId = 2;
            Note.Status = DBHelper.GetContext().Status.Where(n => n.Id == 2).SingleOrDefault();
            ServiceList.Items.Refresh();
        }
    }
}
