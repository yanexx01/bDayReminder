using System.Text.RegularExpressions;
using System.Globalization;
using System.Text.Json;

namespace Test
{
    class BirthdayManager
    {
        private BirthdayContext context = new BirthdayContext();
        private List<Person> people = new List<Person>();
        private string filePath = "bDays.json";
        private bool ext = true;

        public BirthdayManager()
        {
            context.Database.EnsureCreated();
        }

        public void LoadFromJSON()
        {
            Console.WriteLine("Введите путь до JSON файла:");
            string input = Console.ReadLine().Trim();

            string path = string.IsNullOrWhiteSpace(input) ? "bDays.json"
                        : Path.GetExtension(input).ToLower() == ".json" ? input
                        : input + ".json";

            if (!File.Exists(path))
            {
                Console.WriteLine($"Файл {path} не найден.");
                Console.ReadKey();
                return;
            }

            try
            {
                string json = File.ReadAllText(path);
                var fromJson = JsonSerializer.Deserialize<List<Person>>(json);

                if (fromJson != null)
                {
                    // Очищаем старые данные и добавляем новые
                    context.People.RemoveRange(context.People);
                    context.People.AddRange(fromJson);
                    context.SaveChanges();

                    Console.WriteLine("Данные успешно загружены из JSON.");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при загрузке: {e.Message}");
            }

            Console.ReadKey();
        }

        public void SaveToJSON()
        {
            Console.WriteLine("Введите путь до JSON файла:");
            string input = Console.ReadLine().Trim();

            string path = string.IsNullOrWhiteSpace(input) ? "bDays.json"
                        : Path.GetExtension(input).ToLower() == ".json" ? input
                        : input + ".json";

            try
            {
                var people = context.People.ToList();
                string json = JsonSerializer.Serialize(people, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(path, json);

                Console.WriteLine("Данные успешно сохранены в JSON.");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Ошибка при сохранении: {e.Message}");
            }

            Console.ReadKey();
        }

        public void ToggleExtendedView() {ext = !ext;}

        public static Person CreatePerson()           //TODO:  норм проверки
        {
            Person chel = new Person();
            chel.Name = InputString("Введите имя: ");

            chel.Second_name = InputString("Введите фамилию: ");

            Console.WriteLine("Выберите пол:");
            chel.Gender = ChooseEnum<Gender>();

            Console.WriteLine("Введите дату рождения:");       //? сделать проверку на дату
            chel.Date_of_birth = InputDate();


            return chel;
        }

        public void ShowAllPerson()
        {
            var people = context.People.ToList();

            if (people.Count == 0)
            {
                Console.WriteLine("Список пуст");
                return;
            }

            for (int i = 0; i < people.Count; i++)
            {
                Console.WriteLine(people[i].Output(i, ext));
            }
        }

        public void AddBirthday(Person person)
        {
            context.People.Add(person);
            context.SaveChanges();
        }

        public void RemoveBirthday(Person person)
        {
            context.Remove(person);
            context.SaveChanges();
        }

        public static TEnum ChooseEnum<TEnum>() where TEnum : struct, Enum
        {
            var values = Enum.GetValues<TEnum>();
            int index = 0;

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите значение:");

                for (int i = 0; i < values.Length; i++)
                {
                    if (i == index)
                        Console.WriteLine($"> {values[i]}");
                    else
                        Console.WriteLine($"  {values[i]}");
                }

                var key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        index = (index - 1 + values.Length) % values.Length;
                        break;

                    case ConsoleKey.DownArrow:
                        index = (index + 1) % values.Length;
                        break;

                    case ConsoleKey.Enter:
                        return values[index];

                    case ConsoleKey.Escape:
                        return default;
                }
            }
        }

        public static string InputString(string str)
        {
            string input;
            bool isValid;

            do
            {
                Console.Write($"{str}");
                input = Console.ReadLine();

                isValid = !string.IsNullOrWhiteSpace(input) && input.Length > 1 && Regex.IsMatch(input, @"^[a-zA-Zа-яА-ЯёЁ\-]+$");

                if (!isValid)
                {
                    Console.WriteLine("Неверный ввод. Строка может содержать только буквы и тире и быть длинее 1 символа.\n Нажмите любую клавишу для продолжения\n");
                    Console.ReadKey();
                    Console.Clear();

                }

            } while (!isValid);

            return input;
        }

        public static DateOnly InputDate()
        {
            DateOnly input;
            Console.Write("Дата рождения: ");

            if (DateOnly.TryParse(Console.ReadLine(), CultureInfo.CurrentCulture, DateTimeStyles.None, out input))
            {
                Console.WriteLine($"Распознанная дата: {input:dd.MM.yyyy}");
            }
            else
            {
                Console.WriteLine("Не удалось распознать дату.");
            }

            return input;
        }

        public void RemoveAll()
        {
            Console.WriteLine("Вы уверены что хотите очистить весь список?(y/n)");
            if (Console.ReadKey().Key == ConsoleKey.Y)
            {
                context.People.RemoveRange(context.People);
                context.SaveChanges();
                Console.WriteLine("\nСписок полностью очищен");
                Console.ReadKey();
            }
        }

        public void EditBirthday(int num)
        {

            var people = context.People.ToList();
            var person = people[num];
            int index = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Выберите редактируемое поле: \n");

                Console.WriteLine(index == 0 ? $"> Фамилия: {people[num].Second_name}" : $"  Фамилия: {people[num].Second_name}");
                Console.WriteLine(index == 1 ? $"> Имя: {people[num].Name}" : $"  Имя: {people[num].Name}");
                Console.WriteLine(index == 2 ? $"> Пол: {people[num].Gender}" : $"  Пол: {people[num].Gender}");
                Console.WriteLine(index == 3 ? $"> Дата рождения: {people[num].Date_of_birth}" : $"  Дата рождения: {people[num].Date_of_birth}");
                Console.WriteLine(index == 4 ? $"> Удалить запись" : $"  Удалить запись");

                ConsoleKey key = Console.ReadKey().Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow: index = (index + 4) % 5; break;
                    case ConsoleKey.DownArrow: index = (index + 1) % 5; break;
                    case ConsoleKey.Enter:
                        switch (index)      //ToDO: Дописать функцию для проверки фамилии(без символов, цифр, пробелов)
                        {
                            case 0:
                                person.Second_name = InputString("- Введите новую фамилию: ");
                                break;

                            case 1:
                                person.Name = InputString("- Введите новое имя: ");
                                break;

                            case 2:
                                Console.Write("Выберите новый пол: ");
                                person.Gender = ChooseEnum<Gender>();
                                break;

                            case 3:
                                Console.Write("- Введите новую дату рождения (дд.мм.гггг): ");
                                string dateInput = Console.ReadLine();
                                if (DateOnly.TryParse(dateInput, out DateOnly newDate))
                                {
                                    person.Date_of_birth = newDate;
                                }
                                else
                                {
                                    Console.WriteLine("Неверный формат даты.");
                                    Console.ReadKey();
                                }
                                break;

                            case 4:
                                Console.WriteLine("Вы уверены, что хотите удалить эту запись? (y/n)");
                                if (Console.ReadKey(true).Key == ConsoleKey.Y || Console.ReadKey().Key == ConsoleKey.Enter)
                                {
                                    context.People.Remove(person);
                                    context.SaveChanges();
                                    Console.WriteLine("Запись удалена.");
                                    Console.ReadKey();
                                    return;
                                }
                                break;
                        }
                        context.SaveChanges();
                        break;
                    case ConsoleKey.Escape:
                        return;
                }
            }
        }

        public void SelectBirthday()
        {
            var people = context.People.ToList();
            Console.WriteLine("Выберите номер записи: ");
            string input = Console.ReadLine();

            if (int.TryParse(input, out int num) && num > 0 && num <= people.Count)
            {
                EditBirthday(num - 1);
            }
            else
            {
                Console.WriteLine("Ошибка ввода");
                Console.ReadKey();
            }
        }

        public void SortBySecondName()
        {
            people.Sort((p1, p2) => p1.Second_name.CompareTo(p2.Second_name));

            Console.WriteLine("Выберите способ сортировки:");
            if (ChooseEnum<SortBy>() == 0)
            {
                return;
            }
            else people.Reverse();
        }

        public void SortByName()
        {
            people.Sort((p1, p2) => p1.Name.CompareTo(p2.Name));

            if (ChooseEnum<SortBy>() == 0)
            {
                return;
            }
            else people.Reverse();
        }

        public void SortByDate()
        {
            people.Sort((p1, p2) => p1.Date_of_birth.CompareTo(p2.Date_of_birth));

            if (ChooseEnum<SortBy>() == 0)
            {
                return;
            }
            else people.Reverse();
        }

        public void SortByGender()
        {
            people.Sort((p1, p2) => p1.Gender.CompareTo(p2.Gender));

            if (ChooseEnum<SortBy>() == 0)
            {
                return;
            }
            else people.Reverse();
        }

        public List<Person> GetUpcomingBirthdays(int count = 3)
        {
            var today = DateTime.Today;

            return context.People
                .AsEnumerable()
                .Select(p =>
                {
                    var nextBirthday = new DateTime(today.Year, p.Date_of_birth.Month, p.Date_of_birth.Day);
                    if (nextBirthday < today) nextBirthday = nextBirthday.AddYears(1);
                    return new { Person = p, NextBirthday = nextBirthday };
                })
                .OrderBy(x => x.NextBirthday)
                .Take(count)
                .Select(x => x.Person)
                .ToList();
        }


        enum SortBy
        {
            прямая,
            обратная
        }


    }
}
