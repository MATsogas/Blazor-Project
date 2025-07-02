namespace BlazorApp.Data
{
    public class Employee
    {
        public string Name { get; set; }
    }

    public class Manager
    {
        public string Name { get; set; }
    }

    public class PersonelNamePrinter
    {
        public void Print(Employee employee)
        {
            if (employee != null)
            {
                return;
            }

            Console.WriteLine(employee.Name);
        }

        public void Print(Manager manager)
        {
            if(manager != null)
            {
                return;
            }

            Console.Write(manager.Name);
        }
    }
}
