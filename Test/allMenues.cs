using System;

namespace Test
{
    class MainMenu : Menu
    {
        public MainMenu(BirthdayManager birthdayManager) : base(birthdayManager)
        {
            Title = "Главное меню";
            AddItem(new MenuItem("Добавление дня рождения", bm => bm.AddBirthday(BirthdayManager.CreatePerson())));
            AddItem(new MenuItem("Просмотр и редактирование", _ => new SelectMenu(birthdayManager).Run(false)));
            AddItem(new MenuItem("Очистка всего списка", bm => bm.RemoveAll()));
            AddItem(new MenuItem("Работа с файлом", bm => new FileMenu(birthdayManager).Run(true)));

            // AddItem(new ExitItem(birthdayManager));
        }

        public override void UpperOutput()
        {
            Console.WriteLine("Ближайшие дни рождения:\n");

            var upcoming = birthdayManager.GetUpcomingBirthdays();

            if (upcoming.Count == 0)
            {
                Console.WriteLine("Нет данных");
            }
            else
            {
                var today = DateTime.Today;

                foreach (var person in upcoming)
                {
                    var next = new DateTime(today.Year, person.Date_of_birth.Month, person.Date_of_birth.Day);
                    if (next < today)
                        next = next.AddYears(1);

                    var daysLeft = (next - today).Days;
                    string when = daysLeft == 0 ? "сегодня" : $"через {daysLeft} {GetDayWord(daysLeft)}";

                    Console.WriteLine($"{person.Second_name} {person.Name} — {when}");
                }
            }

            Console.WriteLine();
        }
    }

    class SelectMenu : Menu
    {
        public SelectMenu(BirthdayManager birthdayManager) : base(birthdayManager)
        {
            Title = "Select Menu";
            AddItem(new MenuItem("Выбор записи", bm => bm.SelectBirthday()));
            AddItem(new MenuItem("Переключить вид отображения", bm => bm.ToggleExtendedView()));
            AddItem(new MenuItem("Сортировка списка", bm => new SortMenu(birthdayManager).Run(true)));
            // AddItem(new ExitItem(birthdayManager));
        }

        public override void UpperOutput()
        {
            birthdayManager.ShowAllPerson();
        }
    }

    class FileMenu : Menu
    {
        public FileMenu(BirthdayManager birthdayManager) : base(birthdayManager)
        {
            Title = "Работа с файлом\nЗапись в базу данных происходит при каждом изменени/добавлении/удалении записи.\nЗапись и чтение из файла осуществляется вручную в данном меню.";
            AddItem(new MenuItem("Записать в json", bm => bm.SaveToJSON()));
            AddItem(new MenuItem("Загрузить из json", bm => bm.LoadFromJSON()));
        }
    }

    class SortMenu : Menu
    {
        public SortMenu(BirthdayManager birthdayManager) : base(birthdayManager)
        {
            Title = "Sort Menu";
            AddItem(new MenuItem("фамилии", bm => bm.SortBySecondName()));
            AddItem(new MenuItem("имени", bm => bm.SortByName()));
            AddItem(new MenuItem("дате рождения", bm => bm.SortByDate()));
            AddItem(new MenuItem("полу", bm => bm.SortByGender()));
        }

        public override void UpperOutput()
        {
            Console.Clear();
            Console.WriteLine("Сортировать по:");
        }
    }

}
