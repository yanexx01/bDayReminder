# 🎉 BirthdayManager

Простое консольное приложение на C# для хранения, отображения и управления днями рождения с использованием SQLite и/или JSON.

---

## 📦 Возможности

- Добавление, удаление и редактирование записей о днях рождения
- Отображение ближайших дней рождения с указанием, сколько дней осталось
- Сортировка по имени, фамилии, дате рождения и полу
- Переключение между кратким и расширенным режимом отображения
- Сохранение и загрузка данных в формате `.json`
- Хранение данных в базе данных SQLite

---

## 🛠️ Технологии

- C# / .NET
- Entity Framework Core
- SQLite
- JSON (System.Text.Json)

---

## 🚀 Как запустить

1. Установи .NET SDK (https://dotnet.microsoft.com/)
2. Клонируй репозиторий:

   ```bash
   git clone https://github.com/your-username/BirthdayManager.git
   cd BirthdayManager
   ```
3. Построй и запусти проект:

    ```dotnet build
    dotnet run
    ```

---

## 💾 Персистентность
- Все данные сохраняются в SQLite-файл birthdays.db (создаётся автоматически).
- Также можно вручную сохранить/загрузить данные в .json через соответствующее меню.