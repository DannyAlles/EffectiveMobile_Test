Задачу решила как REST API, для удобства тестирования присутствует документация Swagger.
Старалась подвести задачу к более реальной, чтобы показать навыки. но могу переделать и на обычную консоль, если такой формат не подходит.
Готова внести исправления, если потребуется.

Используется СУБД Postgres с дефолтным подключением.
_Host=127.0.0.1;
Port=5432;
User ID=postgres;
Password=postgres;
database=delivery_

Миграции, в которых присутствует заполнение БД данными, применяются автоматически после запуска.

Логирование происходит как операций в БД (Logbook), так и с использованием в SeriLog.

В appsettings.json присутствует строки: 
- **MinutesAfterFirst** - овечает за ближайшее время после первого заказа.
- **DecimalPlacesNum** - овечает за кол-во знаков после запятой для веса.

В таблицу Logbooks записываются операции:
- Создание новых заказов

По формату времени в БД обычно я использую UTC, но в ТЗ был указан формат  yyyy-MM-dd HH:mm:ss, из-за чего поставила без привязки к зоне.

Поскольку я разрабатывала API, формулировка "_В результирующий файл либо БД необходимо вывести результат фильтрации заказов для доставки в конкретный район города в ближайшие полчаса после времени первого заказа_." показалась мне слегка странной, из-за вывода результата фильтрации в БД, в условиях реальной задачи я бы уточнила этот вопрос, но в текущей ситуации решила выводить информацию в формате ответа от сервера, но в контроллере присутствует метод для скачивания .txt файла с выводом

Также имеются вопросы к формулировке "_которое фильтрует заказы в зависимости от количества обращений в конкретном районе города и времени обращения с и по._", возможно имелась ввиду не фильтрация, а сортировка по кол-ву обращений и фильтрация по времени?
