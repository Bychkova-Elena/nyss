using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NotebookApp
{
    internal class Notebook
    {
        public static List<Note> notes = new List<Note>();

        private static void Main(string[] args)
        {
            Console.WriteLine("Привет! Я - твоя телефонная записная книжка!");
            ShowInstruction();

            static void ShowInstruction()
            {       // Работа с пользователем
                Console.WriteLine();
                Console.WriteLine("____________________________________________________________");
                Console.WriteLine("Используй команды для работы со мной:" +
                            "\n'create' - создать новую запись;" +
                            "\n'edit' - редактировать запись; " +
                            "\n'delete' - удалить запись; " +
                            "\n'read' - просмотреть все записи; " +
                            "\n'short' - просмотреть записи в коротком формате;" +
                            "\n'exit' - выйти. При выходе все данные будут удалены! ");
                Console.WriteLine("____________________________________________________________");
                Console.WriteLine();
                switch (Console.ReadLine())
                {
                    case "create":
                        CreateNote();
                        ShowInstruction();
                        break;
                    case "edit":
                        EditNote();
                        ShowInstruction();
                        break;
                    case "delete":
                        DeleteNote();
                        ShowInstruction();
                        break;
                    case "read":
                        ReadNote();
                        ShowInstruction();
                        break;
                    case "short":
                        ReadShotNote();
                        ShowInstruction();
                        break;
                    case "exit":
                        Console.WriteLine("Приятно было поработать! До встречи!");
                        break;
                    default:
                        Console.WriteLine("Я вас не понял! Давай попробуем ещё раз!");
                        ShowInstruction();
                        break;
                }
            }


        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static bool IsValid(string memberName, string value)      //Проверка валидации
        {
            Note valid = new Note();
            var result = new List<ValidationResult>(); // для хранения ошибок
            ValidationContext context = new ValidationContext(valid)
            {
                MemberName = memberName
            };
            if (!Validator.TryValidateProperty(value, context, result))
            {
                Console.WriteLine(result[0].ErrorMessage, Console.ForegroundColor = ConsoleColor.DarkRed);
                Console.ResetColor();
                return false;

            }
            return true;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void CreateNote()  //Создание записи
        {
            int id;
            string surname;
            string name;
            string patronymic;
            string telephone;
            string country;
            string dateOfBirth;
            string organization;
            string position;
            string comments;

            if (notes.Count > 0)
            {
                id = notes[^1].Id + 1;   //Id
            }
            else
            {
                id = 1;
            }

            do                           // Считывание и проверка введенной фамилии
            {
                Console.WriteLine("Введите фамилию:");
                surname = Console.ReadLine();

            } while (!IsValid("Surname", surname));


            do                           // Считывание и проверка введенного имени
            {
                Console.WriteLine("Введите имя:");
                name = Console.ReadLine();

            } while (!IsValid("Name", name));

            Console.WriteLine("Введите отчество:");    // Считывание отчества
            patronymic = Console.ReadLine();

            do                           // Считывание и проверка введенного телефона
            {
                Console.WriteLine("Введите телефон:");
                telephone = Console.ReadLine();

            } while (!IsValid("Telephone", telephone));

            Console.WriteLine("Введите страну:");     // Считывание страны
            country = Console.ReadLine();

            do                            // Считывание и проверка дня рождения
            {
                Console.WriteLine("Введите дату рождения:");
                dateOfBirth = Console.ReadLine();
                if (dateOfBirth != "" && !DateTime.TryParse(dateOfBirth, out _))
                {
                    Console.WriteLine("Неверный формат! Попробуй еще!", Console.ForegroundColor = ConsoleColor.DarkRed);
                    Console.ResetColor();
                }
                else
                {
                    break;
                }
            } while (true);



            Console.WriteLine("Введите организацию:");
            organization = Console.ReadLine();         // Считывание организации
            Console.WriteLine("Введите должность:");
            position = Console.ReadLine();            // Считывание должности
            Console.WriteLine("Введите прочие заметки:");
            comments = Console.ReadLine();            // Считывание прочих заметок
            Note note = new Note(id, surname, name, patronymic, telephone, country, dateOfBirth, organization, position, comments);
            notes.Add(note);
            Console.WriteLine("Ваша запись успешно добавлена", Console.ForegroundColor = ConsoleColor.DarkGreen);
            Console.ResetColor();
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static int GetValidId()   // Проверка введенного id и возвращение корректного id
        {
            bool check;
            int id;
            do
            {
                QuestionForUser();
                void QuestionForUser()
                {
                    Console.WriteLine("Хотите просмотреть список своих записей? (y/n)");
                    switch (Console.ReadLine())
                    {
                        case "y":
                            ReadShotNote();
                            break;
                        case "n":
                            break;
                        default:
                            Console.WriteLine("Я вас не понял(");
                            QuestionForUser();
                            break;
                    }
                }

                Console.WriteLine("Введите номер записи:");
                check = Int32.TryParse(Console.ReadLine(), out id);
                if (!check)
                {
                    Console.WriteLine("Неверный формат номера!", Console.ForegroundColor = ConsoleColor.DarkRed);
                    Console.ResetColor();
                }
                else if (GetNoteById(id) == null)
                {
                    Console.WriteLine("Такой записи не существует! Выберите другую запись!", Console.ForegroundColor = ConsoleColor.DarkRed);
                    Console.ResetColor();
                    check = false;
                }
            } while (!check);
            return id;

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        public static void EditNote() // Редактирование записи
        {
            if (notes.Count > 0)
            {

                int id = GetValidId();

                Console.WriteLine("Редактируйте!");
                Note note = GetNoteById(id);

                Console.WriteLine("_______________________________________________");
                Console.WriteLine("№" + id);
                Console.WriteLine("_______________________________________________");

                Console.WriteLine("Текущая фамилия:");  // Редактирование фамилии
                Console.WriteLine(note.Surname);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    do
                    {
                        Console.WriteLine("\bНовая фамилия:");
                        note.Surname = Console.ReadLine();
                    } while (!IsValid("Surname", note.Surname));
                }


                Console.WriteLine("Текущее имя:");    //  Редактирование имени
                Console.WriteLine(note.Name);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    do
                    {
                        Console.WriteLine("\bНовое имя:");
                        note.Name = Console.ReadLine();
                    } while (!IsValid("Name", note.Name));
                }


                Console.WriteLine("Текущее отчество:");  //  Редактирование отчества
                Console.WriteLine(note.Patronymic);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.WriteLine("\bНовое отчество:");
                    note.Patronymic = Console.ReadLine();
                    if (note.Patronymic == "")
                    {
                        note.Patronymic = "НЕ УКАЗАНО";
                    }
                }


                Console.WriteLine("Текущий телефон:");   //  Редактирование телефона
                Console.WriteLine(note.Telephone);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    do
                    {
                        Console.WriteLine("\bНовый телефон:");
                        note.Telephone = Console.ReadLine();
                    } while (!IsValid("Telephone", note.Telephone));
                }


                Console.WriteLine("Текущая страна:");   //  Редактирование страны
                Console.WriteLine(note.Country);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.WriteLine("\bНовая страна:");
                    note.Country = Console.ReadLine();
                    if (note.Country == "")
                    {
                        note.Country = "НЕ УКАЗАНО";
                    }
                }

                Console.WriteLine("Текущая дата рождения:");   //  Редактирование дня рождения
                Console.WriteLine(note.DateOfBirth);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    do
                    {
                        Console.WriteLine("\bНовая дата рождения:");
                        note.DateOfBirth = Console.ReadLine();
                        if (note.DateOfBirth == "")
                        {
                            note.DateOfBirth = "НЕ УКАЗАНО";
                        }
                        else if (note.DateOfBirth != "" && !DateTime.TryParse(note.DateOfBirth, out _))
                        {
                            Console.WriteLine("Неверный формат! Попробуй еще!", Console.ForegroundColor = ConsoleColor.DarkRed);
                            Console.ResetColor();
                        }
                        else
                        {
                            note.DateOfBirth = DateTime.Parse(note.DateOfBirth).ToString("dd.MM.yyyy");
                            break;
                        }
                    } while (true);
                }


                Console.WriteLine("Текущая организация:");        //  Редактирование организации
                Console.WriteLine(note.Organization);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.WriteLine("\bНовая организацию:");
                    note.Organization = Console.ReadLine();
                    if (note.Organization == "")
                    {
                        note.Organization = "НЕ УКАЗАНО";
                    }
                }

                Console.WriteLine("Текущая должность:");           //  Редактирование должности
                Console.WriteLine(note.Position);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.WriteLine("\bНовая должность:");
                    note.Position = Console.ReadLine();
                    if (note.Position == "")
                    {
                        note.Position = "НЕ УКАЗАНО";
                    }
                }

                Console.WriteLine("Текущие заметки:");     //  Редактирование прочих заметок
                Console.WriteLine(note.Comments);
                if (Console.ReadKey().Key != ConsoleKey.Enter)
                {
                    Console.WriteLine("\bНовые заметки:");
                    note.Comments = Console.ReadLine();
                    if (note.Comments == "")
                    {
                        note.Comments = "НЕ УКАЗАНО";
                    }
                }


                Console.WriteLine($"Запись №{id} отредактирована", Console.ForegroundColor = ConsoleColor.DarkGreen);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Записная книжка пуста!", Console.ForegroundColor = ConsoleColor.DarkYellow);
                Console.ResetColor();
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void DeleteNote()  //Удаление записи
        {
            if (notes.Count > 0)
            {
                int id = GetValidId();

                notes.Remove(GetNoteById(id));
                Console.WriteLine($"Запись №{id} успешно удалена", Console.ForegroundColor = ConsoleColor.DarkGreen);
                Console.ResetColor();
            }
            else
            {
                Console.WriteLine("Записная книжка пуста!", Console.ForegroundColor = ConsoleColor.DarkYellow);
                Console.ResetColor();
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static Note GetNoteById(int id)  // Возвращает запись по id
        {
            Note note = null;
            foreach (Note n in notes)
            {
                if (n.Id == id)
                {
                    note = n;
                }
            }
            return note;
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void ReadNote()   // Вывод всех записей
        {
            if (notes.Count > 0)
            {
                foreach (Note note in notes)
                {
                    Console.WriteLine("_______________________________________________");
                    Console.WriteLine("№" + note.Id);
                    Console.WriteLine("_______________________________________________");
                    Console.WriteLine("Фамилия: " + note.Surname);
                    Console.WriteLine("Имя: " + note.Name);
                    Console.WriteLine("Отчество: " + note.Patronymic);
                    Console.WriteLine("Телефон: " + note.Telephone);
                    Console.WriteLine("Страна: " + note.Country);
                    Console.WriteLine("Дата рождения: " + note.DateOfBirth);
                    Console.WriteLine("Организация: " + note.Organization);
                    Console.WriteLine("Должность: " + note.Position);
                    Console.WriteLine("Прочие заметки: " + note.Comments);

                }
            }
            else
            {
                Console.WriteLine("Записная книжка пуста!", Console.ForegroundColor = ConsoleColor.DarkYellow);
                Console.ResetColor();
            }

        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        public static void ReadShotNote()  // Вывод краткой информации записи
        {
            if (notes.Count > 0)
            {
                Console.WriteLine("{0, -3}\t{1, -10}\t{2, -10}\t{3}", "№", "Фамилия", "Имя", "Телефон");
                foreach (Note note in notes)
                {
                    Console.WriteLine("{0, -3}\t{1, -10}\t{2, -10}\t{3}", note.Id, note.Surname, note.Name, note.Telephone);

                }

            }
            else
            {
                Console.WriteLine("Записная книжка пуста!", Console.ForegroundColor = ConsoleColor.DarkYellow);
                Console.ResetColor();
            }

        }
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    public class Note  //Класс записи
    {
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Не указана фамилия")]
        [DisplayName("Surname")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Не указано имя")]
        [DisplayName("Name")]
        public string Name { get; set; }

        public string Patronymic { get; set; } = "НЕ УКАЗАНО";

        [Required(ErrorMessage = "Не указан телефон")]
        [Phone(ErrorMessage = "Неверный формат телефона")]
        [DisplayName("Telephone")]
        public string Telephone { get; set; }

        public string Country { get; set; } = "НЕ УКАЗАНО";
        public string DateOfBirth { get; set; } = "НЕ УКАЗАНО";
        public string Organization { get; set; } = "НЕ УКАЗАНО";
        public string Position { get; set; } = "НЕ УКАЗАНО";
        public string Comments { get; set; } = "НЕ УКАЗАНО";

        public Note() { }

        public Note(int id, string surname, string name, string patronymic, string telephone, string country, string dateOfBirth, string organization, string position, string comments)
        {
            Id = id;
            Surname = surname;
            Name = name;
            if (patronymic != "")
            {
                Patronymic = patronymic;
            }

            Telephone = telephone;
            if (country != "")
            {
                Country = country;
            }

            if (dateOfBirth != "")
            {
                DateOfBirth = (DateTime.Parse(dateOfBirth)).ToString("dd.MM.yyyy");
            }

            if (organization != "")
            {
                Organization = organization;
            }

            if (position != "")
            {
                Position = position;
            }

            if (comments != "")
            {
                Comments = comments;
            }
        }

    }
}
