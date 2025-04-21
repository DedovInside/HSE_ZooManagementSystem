# HSE_Zoo_ManagementSystem
Конструирование Программного Обеспечения. Домашняя работа №1. Закрепление концепции Domain-Driven Design и принципов проектирования Clean Architecture. 

Напомню условие: вам предстоит разработать веб-приложение для автоматизации следующих бизнес-процессов зоопарка: управление животными; вольерами; расписанием кормлений. 

Решение было разделено на 4 проекта (в соответствии с Clean Architecture):

1. `Domain` - нижний слой (сущности, доменные события, Value Objects)
2.  `Application` - слой, содержащий
3.  `Infrastructure` - слой
4.  `Presentation` - слой, но сам проект имеет название ZooWebApi



###Реализованный функционал
##Use Cases
1. Добавление/удаление животных
   - Реализовано в `AnimalController` (`createAnimal`, `deleteAnimal`)
   - Бизнес-логика в `AnimalService`

2. Добавление/удаление вольеров
   - Реализовано в `EnclosureController` (`createEnclosure`, `deleteEnclosure`)
   - Бизнес-логика в `EnclosureService`

3. Перемещение животных между вольерами
   - Реализовано в `AnimalTransferController` (`transferAnimal`)
   - Бизнес-логика в `AnimalTransferService`
   - Генерируется доменное событие `AnimalMovedEvent`
   - Создается запись о перемещении в `TransferRecord`

4. Просмотр расписания кормления
   - Реализовано в `FeedingScheduleController` (`getFeedingSchedules`)
   - Бизнес-логика в `FeedingOrganizationService`

5. Добавление кормления в расписание
   - Реализовано в `FeedingScheduleController` (`addFeedingSchedule`)
   - Поддержка повторяющихся кормлений через `IsRecurring`
   - Генерируется доменное событие `FeedingTimeEvent` при наступлении времени кормления

6. Просмотр статистики зоопарка
   - Реализовано в `StatisticsController`
   - Получение общей статистики, статистики по кормлениям и использованию вольеров
