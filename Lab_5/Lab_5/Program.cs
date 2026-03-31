using static System.Console;
namespace Lab_5
{
    internal class Program
    {
        class Document
        {
            public string Number { get; set; }
            public DateTime Date { get; set; }

            public Document() { Number = "000"; Date = DateTime.Now; WriteLine("Document: Конструктор за замовчуванням"); }
            public Document(string n) { Number = n; Date = DateTime.Now; WriteLine("Document: Параметризований (1 аргумент)"); }
            public Document(string n, DateTime d) { Number = n; Date = d; WriteLine("Document: Параметризований (2 аргументи)"); }

            ~Document() => WriteLine($"Document {Number}: Деструктор");

            public virtual void Show() => WriteLine($"Документ №{Number} від {Date.ToShortDateString()}");
        }

        class Receipt : Document
        {
            public decimal Amount { get; set; }
            public Receipt() : base() => WriteLine("Receipt: Конструктор 1");
            public Receipt(string n, decimal a) : base(n) { Amount = a; WriteLine("Receipt: Конструктор 2"); }
            public Receipt(string n, DateTime d, decimal a) : base(n, d) { Amount = a; WriteLine("Receipt: Конструктор 3"); }
            ~Receipt() => WriteLine("Receipt: Деструктор");
            public override void Show() { base.Show(); WriteLine($"Тип: Квитанція, Сума: {Amount}"); }
        }

        // --- ЗАВДАННЯ 3: ПРОГРАМНЕ ЗАБЕЗПЕЧЕННЯ ---
        abstract class Software
        {
            public string Name { get; set; }
            public string Manufacturer { get; set; }
            protected Software(string n, string m) { Name = n; Manufacturer = m; }
            public abstract void ShowInfo();
            public abstract bool IsUsable(DateTime date);
        }

        class FreeSoftware : Software
        {
            public FreeSoftware(string n, string m) : base(n, m) { }
            public override void ShowInfo() => WriteLine($"[Вільне] {Name}, Виробник: {Manufacturer}");
            public override bool IsUsable(DateTime date) => true;
        }

        class SharewareSoftware : Software
        {
            public DateTime InstallDate { get; set; }
            public int TrialDays { get; set; }
            public SharewareSoftware(string n, string m, DateTime inst, int days) : base(n, m)
            { InstallDate = inst; TrialDays = days; }
            public override void ShowInfo() => WriteLine($"[Trial] {Name}, Тріал: {TrialDays} днів");
            public override bool IsUsable(DateTime date) => (date - InstallDate).TotalDays <= TrialDays;
        }

        class CommercialSoftware : Software
        {
            public decimal Price { get; set; }
            public DateTime InstallDate { get; set; }
            public int UsePeriod { get; set; }
            public CommercialSoftware(string n, string m, decimal p, DateTime inst, int days) : base(n, m)
            { Price = p; InstallDate = inst; UsePeriod = days; }
            public override void ShowInfo() => WriteLine($"[Комерційне] {Name}, Ціна: {Price}, Термін: {UsePeriod} дн.");
            public override bool IsUsable(DateTime date) => (date - InstallDate).TotalDays <= UsePeriod;
        }

        // --- ЗАВДАННЯ 4: СТРУКТУРИ, КОРТЕЖІ, ЗАПИСИ ---
        struct InfoStruct
        {
            public string Media; public double Volume; public string Title; public string Author;
            public InfoStruct(string m, double v, string t, string a) { Media = m; Volume = v; Title = t; Author = a; }
        }

        record InfoRecord(string Media, double Volume, string Title, string Author);
        static void Main(string[] args)
        {
            WriteLine("--- Завдання 1-2: Документи ---");
            {
                Receipt r = new Receipt("R-101", DateTime.Now, 550.50m);
                r.Show();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();

            // Демонстрація Завдання 3
            WriteLine("\n--- Завдання 3: ПЗ ---");
            Software[] softBase = {
                new FreeSoftware("VS Code", "Microsoft"),
                new SharewareSoftware("WinRAR", "RARLab", DateTime.Now.AddDays(-30), 40),
                new CommercialSoftware("Office 365", "MS", 200, DateTime.Now.AddDays(-100), 365)
            };
            foreach (var s in softBase) s.ShowInfo();

            WriteLine("\nДоступні на сьогодні:");
            foreach (var s in softBase.Where(x => x.IsUsable(DateTime.Now)))
                WriteLine($"- {s.Name}");

            WriteLine("\n--- Завдання 4: Інформація (Records) ---");
            var list = new List<InfoRecord> {
                new("HDD", 1024, "System", "Admin"),
                new("USB", 32, "Data", "User"),
                new("SSD", 512, "Games", "Gamer")
            };

            var itemToRemove = list.FirstOrDefault(x => x.Volume == 32);
            if (itemToRemove != null) list.Remove(itemToRemove);

            list.Insert(0, new InfoRecord("Cloud", 2048, "Backup", "System"));

            foreach (var item in list) WriteLine(item);

            WriteLine("\nНатисніть будь-яку клавішу...");
            ReadKey();
        }
    }
}
