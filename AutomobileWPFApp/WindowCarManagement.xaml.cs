using AutomobileLibrary.DataAccess;
using AutomobileLibrary.Repository;
using Microsoft.IdentityModel.Tokens;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;

namespace AutomobileWPFApp
{
    /// <summary>
    /// Interaction logic for WindowCarManagement.xaml
    /// </summary>
    public partial class WindowCarManagement : Window
    {
        ICarRepository carRepository;
        public WindowCarManagement(ICarRepository repository)
        {
            
            InitializeComponent();
            carRepository = repository;
        }
        private Car GetCarObject(bool isModify = true)
        {
            Car car = null;
            try
            {
                car = new Car
                {
                    CarId = int.Parse(txtCarId.Text),
                    CarName = txtCarName.Text,
                    Manufacturer = txtManufacturer.Text,
                    Price = decimal.Parse(txtPrice.Text),
                    ReleasedYear = int.Parse(txtReleasedYear.Text),
                };
                if(isModify)
                {
                    if (car.ReleasedYear < 2000 || car.ReleasedYear > 2100) throw new("ReleasedYear must in 2000-2100");
                    if (car.Price < 0) throw new("Price should over positive value");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Get car");
            }
            return car;
        }//end GetCarObject
        public void LoadCarList()
        {
            lvCars.ItemsSource = carRepository.GetCars();
        }
        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadCarList();
            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Load car lsit");
            }
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Car car = GetCarObject();
                carRepository.InsertCar(car);
                LoadCarList();
                System.Windows.MessageBox.Show($"{car.CarName} inserted successfully", "Insert car");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Insert car");
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Car car = GetCarObject();
                carRepository.UpdateCar(car);
                LoadCarList();
                System.Windows.MessageBox.Show($"{car.CarName} updated successfully", "Update car");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Update car");
            }
        }
        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Car car = GetCarObject();
                carRepository.DeleteCar(car);
                LoadCarList();
                System.Windows.MessageBox.Show($"{car.CarName} deleted successfully", "Delete car");
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "Delete car");
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)=> Close();

        private void txtPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (double.TryParse(txtPrice.Text, out double value))
            {
                txtPrice.Text = string.Format("{0:N3}", value).TrimEnd('0').TrimEnd('.');
                txtPrice.CaretIndex = txtPrice.Text.Length;
            }
        }

        private void lvCars_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            Car car = GetCarObject(false);
            if (car != null && car.CarId > 0)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show($"Do you want to delete '{car.CarName}'?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    carRepository.DeleteCar(car);
                    LoadCarList();
                    System.Windows.MessageBox.Show($"{car.CarName} deleted successfully", "Delete car");
                }
            }
        }
    }
}
