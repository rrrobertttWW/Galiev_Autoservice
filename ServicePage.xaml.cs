using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Galiev_Autoservice
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        public ServicePage()
        {
            InitializeComponent();

            var currentServices = ГалиевАвтосервисEntities.GetContext().Service.ToList();
            ServiceListView.ItemsSource = currentServices;
            ComboType.SelectedIndex = 0;
            UpdateServices();
        }
        private void UpdateServices()
        {
            var currentServices = ГалиевАвтосервисEntities.GetContext().Service.ToList();

            if (ComboType.SelectedIndex == 0)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountIt) >= 0 && Convert.ToInt32(p.DiscountIt) <= 100)).ToList();
            }

            if (ComboType.SelectedIndex == 1)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountIt) >= 0 && Convert.ToInt32(p.DiscountIt) < 5)).ToList();
            }

            if (ComboType.SelectedIndex == 2)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountIt) >= 5 && Convert.ToInt32(p.DiscountIt) < 15)).ToList();
            }


            if (ComboType.SelectedIndex == 3)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountIt) >= 15 && Convert.ToInt32(p.DiscountIt) < 30)).ToList();
            }


            if (ComboType.SelectedIndex == 4)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountIt) >= 30 && Convert.ToInt32(p.DiscountIt) < 70 )).ToList();
            }


            if (ComboType.SelectedIndex == 5)
            {
                currentServices = currentServices.Where(p => (Convert.ToInt32(p.DiscountIt) >= 70 && Convert.ToInt32(p.DiscountIt) < 100)).ToList();
            }

            currentServices = currentServices.Where(p => p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();


            ServiceListView.ItemsSource = currentServices.ToList();

            if (RButtonDown.IsChecked.Value)
            {

                ServiceListView.ItemsSource = currentServices.OrderByDescending(p => p.Cost).ToList();
            }

            if (RButtonUp.IsChecked.Value)
            {

                ServiceListView.ItemsSource = currentServices.OrderBy(p => p.Cost).ToList();
            }
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateServices();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateServices();
        }



        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }



        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {
            UpdateServices();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
        var currentService = (sender as Button).DataContext as Service;
            var currentClientServices = ГалиевАвтосервисEntities.GetContext().ClientService.ToList();
            currentClientServices = currentClientServices.Where(p => p.ServiceID == currentService.ID).ToList();
            if (currentClientServices.Count != 0)
                MessageBox.Show("Невозможно выполнить удаление, так как сущестувует записи на эту услугу");
            else
            {
                if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        ГалиевАвтосервисEntities.GetContext().Service.Remove(currentService);
                        ГалиевАвтосервисEntities.GetContext().SaveChanges();
                        ServiceListView.ItemsSource = ГалиевАвтосервисEntities.GetContext().Service.ToList();
                        UpdateServices();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }
                }
            }
        }
    }
}
