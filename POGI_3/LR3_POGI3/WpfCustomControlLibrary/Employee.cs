
namespace WpfCustomControlLibrary
{
    public class Employee
    {
        public int ID { get; }
        public string Nickname { get; set; }
        public string Password { get; private set; }
        public string RealName { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }
        public string ImagePath { get; set; }

        public Employee(int id)
        {
            this.ID = id;
        }

        public Employee(int id, string nickname, string password) : this(id)
        {
            Nickname = nickname;
            Password = password;
        }

        public Employee(int id, string nickname, string password, string realName, string department, string position, string imagePath) : this(id, nickname, password)
        {
            RealName = realName;
            Department = department;
            Position = position;
            ImagePath = imagePath;
        }

        public Employee(Employee employee)
        {
            this.ID = employee.ID;
            this.Nickname = employee.Nickname;
            this.Password = employee.Password;
            this.RealName = employee.RealName;
            this.Department = employee.Department;
            this.Position = employee.Position;
            this.ImagePath = employee.ImagePath;
        }

    }
}
