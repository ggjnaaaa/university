using WpfCustomControlLibrary;

namespace WorkerCard.Model
{
    /// <summary>
    /// Работа с данными пользвателей (обновление и создание новых пользователей)
    /// </summary>
    internal class UsersData
    {
        private int actualID = 1;

        // Методы ниже должны работать с бд, вместо этого сейчас происходит работа с активным пользователем

        /// <summary>
        /// Создаёт нового пользователя с новым ID
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <param name="password">Пароль пользователя</param>
        /// <returns></returns>
        public Employee CreateEmployee(string login, string password)
        {
            return new Employee(actualID++, login, password);
        }

        public static Employee UpdateEmployeeLogin(Employee employee, string login)
        {
            employee.Nickname = login;
            return employee;
        }
        public static Employee UpdateEmployeeRealName(Employee employee, string realName)
        {
            employee.RealName = realName;
            return employee;
        }
        public static Employee UpdateEmployeeDepartment(Employee employee, string department)
        {
            employee.Department = department;
            return employee;
        }
        public static Employee UpdateEmployeePosition(Employee employee, string position)
        {
            employee.Position = position;
            return employee;
        }
        public static Employee UpdateEmployeeImagePath(Employee employee, string imagePath)
        {
            employee.ImagePath = imagePath;
            return employee;
        }
    }
}
