using System.Text.RegularExpressions;
using WpfCustomControlLibrary;

namespace WorkerCard.Model
{
    /// <summary>
    /// Проверяет введённые данные
    /// </summary>
    internal class DataValidator
    {
        // Методы ниже должны работать с бд, вместо этого сейчас происходит работа с активным пользователем

        /// <summary>
        /// Сравнивает пароль в БД с введённым паролем
        /// </summary>
        /// <param name="password">Пароль пользователя</param>
        /// <param name="employee">Пользователь</param>
        /// <returns></returns>
        public static bool IsPasswordValid(string password, Employee employee)
        {
            if (employee == null) return false;
            if (employee.Password != password) return false;
            return true;
        }

        /// <summary>
        /// Ищет логин в БД
        /// </summary>
        /// <param name="login">Логин пользователя</param>
        /// <returns></returns>
        public static bool IsLoginActuallyUsing(string login)
        {
            return false;
        }

        /// <summary>
        /// Проверяет логин на соответствие требованиям
        /// </summary>
        /// <param name="password">Пароль пользователя</param>
        /// <returns></returns>
        public static bool IsPasswordMeetsRequirements(string password)
        {
            if (password.Length < 8)
                return false;
            string pattern = @"^[a-zA-Z0-9_]+$";
            return Regex.IsMatch(password, pattern);
        }
    }
}
