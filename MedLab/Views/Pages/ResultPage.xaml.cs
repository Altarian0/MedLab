using MedLab.Classes.Models;
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
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MedLab.Views.Pages
{
    /// <summary>
    /// Interaction logic for ResultPage.xaml
    /// </summary>
    public partial class ResultPage : Page
    {
        private User user { get; }
        private ResultModel resultModel;

        public ResultPage(User user)
        {
            InitializeComponent();
            this.user = user;

            ChartTypeCombo.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
            ResultGroupBox.Header = $"Результаты на {DateTime.Now.ToString("dd.MM.yyyy")}";

            ResultChart.ChartAreas.Add(new ChartArea("MainArea"));
            ResultChart.Series.Add(new Series("MainSerie"));

            List<Note> notes = DBHelper.GetContext().Note.ToList();

            resultModel = new ResultModel()
            {
                WaitingCount = notes.Where(n => n.StatusId == 1).Count(),
                BioMaterialSendCount = notes.Where(n => n.StatusId == 2).Count(),
                AssistantPinnedCount = notes.Where(n => n.StatusId == 3).Count(),
                DoneCount = notes.Where(n => n.StatusId == 4).Count(),
            };
            ResultGroupBox.DataContext = resultModel;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenDiagramButton_Click(object sender, RoutedEventArgs e)
        {
            ResultChart.Series[0].Points.Clear();

            if (ChartTypeCombo.SelectedItem == null)
            {
                MessageBox.Show("Выберите тип диаграммы!");
                return;
            }

            ResultChart.Series[0].ChartType = (SeriesChartType)ChartTypeCombo.SelectedItem;

            foreach (var status in DBHelper.GetContext().Status.ToList())
            {
                ResultChart.Series[0].Points.AddXY(status.Name, DBHelper.GetContext().Note.Where(n=>n.StatusId == status.Id).Count());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PrintButton_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            printDialog.PrintVisual(PrintGrid,"ResultGroup");
            printDialog.ShowDialog();
        }
    }
}
