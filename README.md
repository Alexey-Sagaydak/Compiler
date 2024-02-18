# Компилятор

Разработка текстового редактора с функциями языкового процессора.

## Оглавление

- [Лабораторная работа №1: Разработка пользовательского интерфейса (GUI) для языкового процессора](#лабораторная-работа-1-разработка-пользовательского-интерфейса-gui-для-языкового-процессора)
- [Лабораторная работа №2: Разработка лексического анализатора (сканера)](#лабораторная-работа-2-разработка-лексического-анализатора-сканера)
- [Лабораторная работа №3: Разработка синтаксического анализатора (парсера)](#лабораторная-работа-3-разработка-синтаксического-анализатора-парсера)
- [Лабораторная работа №4: Нейтрализация ошибок (метод Айронса)](#лабораторная-работа-4-нейтрализация-ошибок-метод-айронса)
- [Лабораторная работа №5: Включение семантики в анализатор. Создание внутренней формы представления программы](#лабораторная-работа-5-включение-семантики-в-анализатор-создание-внутренней-формы-представления-программы)
- [Лабораторная работа №6: Реализация алгоритма поиска подстрок с помощью регулярных выражений](#лабораторная-работа-6-реализация-алгоритма-поиска-подстрок-с-помощью-регулярных-выражений)
- [Лабораторная работа №7: Реализация метода рекурсивного спуска для синтаксического анализа](#лабораторная-работа-7-реализация-метода-рекурсивного-спуска-для-синтаксического-анализа)

## Лабораторная работа №1: Разработка пользовательского интерфейса (GUI) для языкового процессора
**Тема:** разработка текстового редактора с возможностью дальнейшего расширения функционала до языкового процессора.

**Цель работы:** разработать приложение с графическим интерфейсом пользователя, способное редактировать текстовые данные. Это приложение будет базой для будущего расширения функционала в виде языкового процессора.

**Язык реализации:** C#, WPF.

### Интерфейс текстового редактора

![Главное окно программы](/README_images/main_window.png)

#### Получившийся текстовый редактор имеет следующие элементы:
1. Заголовок окна.

   Содержит информацию о названии открытого файла, полного пути к нему, а также о том, сохранен ли он на текущий момент (наличие символа звездочки справа от названия означает наличие несохраненных изменений).
3. Меню
   | Пункт меню | Подпункты |
   | ------ | ------ |
   | Файл | ![Главное окно программы](/README_images/menu_file.png) |
   | Правка | ![Правка](/README_images/menu_edit.png) |
   | Текст | ![Текст](/README_images/menu_text.png) |
   | Пуск | — |
   | Справка | ![Справка](/README_images/menu_help.png) |
4. Панель инструментов
   
   ![Панель инструментов](/README_images/toolbar.png)

   - Создать
   - Открыть
   - Сохранить
   - Изменить размер текста
   - Отменить
   - Повторить
   - Копировать
   - Вырезать
   - Вставить
   - Пуск
   - Вызов справки
   - О программе
5. Область редактирования
   
   Поддерживаются следующие функции:
   - Изменение размера текста
   - Открытие файла при перетаскивании его в окно программы
   - Базовая подсветка синтаксиса
   - Нумерация строк
7. Область отображения результатов

   В область отображения результатов выводятся сообщения и результаты работы языкового процессора.

   Поддерживаются следующие функции:
   - Изменение размера текста
   - Отображение ошибок в виде таблицы
8. Строка состояния

   В дополнении к информации, выводимой в заголовке окна, показываются текушие номера строки и столбца, где находится курсор.

### Справочная система

Разделы справочной системы открываются как HTML-документы в браузере.

| Раздел | Изображение |
| ------ | ------ |
| Вызов справки | <img src="/README_images/help_click.png" alt="Вызов справки" width="500"> |
| О программе | <img src="/README_images/about_click.png" alt="О программе" width="500"> |

### Вывод сообщений

| Сообщение | Описание | Изображение |
| ------ | ------| ------ |
| Закрытие окна программы | Появляется при закрытии программы нажатием крестика или комбинацией клавиш при наличии несохраненных изменений | ![Закрытие окна программы](/README_images/exit_message.png) |
| Сохранение изменений | Появляется при попытке открыть существующий файл или создать новый при наличии несохраненных изменений | ![Сохранение изменений](/README_images/unsaved_changes_message.png) |

## Лабораторная работа №2: Разработка лексического анализатора (сканера)

**Тема:** разработка лексического анализатора (сканера).

**Цель работы:** изучить назначение лексического анализатора. Спроектировать алгоритм и выполнить программную реализацию сканера.

| № | Тема | Пример верной строки | Справка |
| ------ | ------ | ------ | ------ |
| 42 | Объявление и инициализация целочисленной константы в СУБД PostgreSQL | DECLARE product_price CONSTANT INTEGER := 150; | [ссылка](https://www.commandprompt.com/education/constants-in-postgresql-explained-with-examples/) |

**В соответствии с вариантом задания необходимо:**

1. Спроектировать диаграмму состояний сканера.
2. Разработать лексический анализатор, позволяющий выделить в тексте лексемы, иные символы считать недопустимыми (выводить ошибку).
3. Встроить сканер в ранее разработанный интерфейс текстового редактора. Учесть, что текст для разбора может состоять из множества строк.

**Входные данные:** строка (текст программного кода).

**Выходные данные:** последовательность условных кодов, описывающих структуру разбираемого текста с указанием места положения и типа.

### Примеры допустимых строк

```sql
DECLARE
product_price CONSTANT INTEGER = 150;
```

```sql
DECLARE total_amount CONSTANT INTEGER := -150;
```

```sql
DECLARE productPrice CONSTANT INTEGER := +150;
```

```sql
DECLARE expense_1_amount CONSTANT INTEGER := -50;
```

```sql
DECLARE product_price CONSTANT INTEGER := -150; DECLARE total_2 CONSTANT INTEGER := 50;
```

```sql
DECLARE productPrice3 CONSTANT INTEGER := 150; DECLARE expense_amount_4 CONSTANT INTEGER := -50;
```

### Диаграмма состояний сканера

![Диаграмма состояний сканера](/README_images/scanner_diagram.jpg)

### Тестовые примеры

1. **Тест №1.** Пример, показывающий все возможные лексемы, которые могут быть найдены лексическим анализатором.
   
   ![Тест 1](/README_images/scanner_test_3.png)
3. **Тест №2.** Сложный пример.

   > При нажатии на лексему в таблице, соответствующий фрагмент текста подсвечивается в поле редактирования.
  
   ![Тест 2](/README_images/scanner_test_1.png)
6. **Тест №3.** Сложный пример.

   > При нажатии на лексему в таблице, соответствующий фрагмент текста подсвечивается в поле редактирования.
  
   ![Тест 3](/README_images/scanner_test_2.png)

## Лабораторная работа №3: Разработка синтаксического анализатора (парсера)

**Тема:** разработка синтаксического анализатора (парсера).

**Цель работы:** изучить назначение синтаксического анализатора, спроектировать алгоритм и выполнить программную реализацию парсера.

**В соответствии с вариантом задания на курсовую работу необходимо:**
1. Разработать автоматную грамматику.
2. Спроектировать граф конечного автомата (перейти от автоматной грамматики к конечному автомату).
3. Выполнить программную реализацию алгоритма работы конечного автомата.
4. Встроить разработанную программу в интерфейс текстового редактора, созданного на первой лабораторной работе.

## Лабораторная работа №4: Нейтрализация ошибок (метод Айронса)

## Лабораторная работа №5: Включение семантики в анализатор. Создание внутренней формы представления программы

## Лабораторная работа №6: Реализация алгоритма поиска подстрок с помощью регулярных выражений

## Лабораторная работа №7: Реализация метода рекурсивного спуска для синтаксического анализа
