using System;
using System.Data.Entity;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace WPFDomoshkaEF
{
    public partial class MainWindow : Window
    {
        private readonly AppDBContextWPF contextWPF;

        public MainWindow()
        {
            InitializeComponent();
            contextWPF = new AppDBContextWPF();
            LoadRoles();
            LoadUsers();
        }       

        private void LoadRoles()
        {            
            ChoseRole.Items.Clear();
            ChoseRole.ItemsSource = contextWPF.Roles.ToList();
            ChoseRole.SelectedIndex = -1;
        }

        private void LoadUsers()
        {
            
            datagridUser.ItemsSource = null;

           
            contextWPF.Users.Include(u => u.Role).Load();
            datagridUser.ItemsSource = contextWPF.Users.Local;
        }

        private void AddNewChuvaks_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EnterName.Text) ||
                string.IsNullOrWhiteSpace(EnterAge.Text) ||
                string.IsNullOrWhiteSpace(EnterEmail.Text) ||
                string.IsNullOrWhiteSpace(EnterCity.Text))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }

            if (!int.TryParse(EnterAge.Text, out int age) || age <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректный возраст");
                return;
            }

            if (ChoseRole.SelectedItem == null)
            {
                MessageBox.Show("Пожалуйста, выберите роль");
                return;
            }

            var newUser = new User
            {
                Name = EnterName.Text,
                Age = age,
                Email = EnterEmail.Text,
                City = EnterCity.Text,
                RoleId = ((Role)ChoseRole.SelectedItem).Id
            };

            try
            {
                contextWPF.Users.Add(newUser);
                contextWPF.SaveChanges();

                
                EnterName.Text = "";
                EnterAge.Text = "";
                EnterEmail.Text = "";
                EnterCity.Text = "";
                ChoseRole.SelectedIndex = -1;

                MessageBox.Show("Пользователь успешно добавлен!");
                LoadUsers(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}");
            }
        }
        
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            if (button == null) return;

            int userId = (int)button.Tag;

            var userToDelete = contextWPF.Users.FirstOrDefault(u => u.Id == userId);
            if (userToDelete == null) return;

            var result = MessageBox.Show($"Вы уверены, что хотите удалить пользователя {userToDelete.Name}?",
                                       "Подтверждение удаления",
                                       MessageBoxButton.YesNo,
                                       MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    contextWPF.Users.Remove(userToDelete);
                    contextWPF.SaveChanges();
                    LoadUsers(); 
                    MessageBox.Show("Пользователь успешно удален!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
        }
    }
}