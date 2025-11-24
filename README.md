# Arkanoid

Небольшой проект на Unity, повторяющий механику классического Arkanoid с управлением платформой, запуском мяча и разрушением блока за блоком. Логика построена поверх Zenject и реактивных потоков R3, что упрощает связывание UI, ввода и игровой сцены.

## Ключевые особенности
- **Генерация поля**: `GameFieldService` создаёт сетку блоков по настройкам `GameFieldSettings` (размер сетки, префаб, отступы) и отправляет событие, когда все блоки уничтожены. 【F:Assets/Scripts/Field/GameFieldService.cs†L12-L76】
- **Физика мяча**: `BallSystem` отвечает за прицеливание по курсору/тачу, расчёт стартовой скорости и отражения от коллизий с контролем углов, заданных в `BallSettings`. 【F:Assets/Scripts/Systems/BallSystem.cs†L10-L140】【F:Assets/Scripts/Field/Data/BallSettings.cs†L5-L11】
- **Движение платформы**: `PlatformSystem` считывает ввод из `PlayerInputReader` и перемещает платформу в пределах сцены с учётом коллайдеров. 【F:Assets/Scripts/Systems/PlatformSystem.cs†L10-L50】【F:Assets/Scripts/Input/PlayerInputReader.cs†L9-L60】
- **Состояние игры**: `GameStateSystem` отслеживает уничтожение всех блоков и попадание мяча в зону проигрыша, останавливая игру и показывая итоговый экран через `ArkanoidModule`. 【F:Assets/Scripts/Systems/GameStateSystem.cs†L13-L53】
- **UI-слой**: `ArkanoidView` и `ArkanoidPresenter` выводят итоговые сообщения, управляют видимостью канваса и перезапускают сцену по кнопке «Restart». 【F:Assets/Scripts/Ui/ArkanoidModule.cs†L7-L43】【F:Assets/Scripts/Ui/ArkanoidView.cs†L7-L45】【F:Assets/Scripts/Ui/ArkanoidPresenter.cs†L8-L36】
- **Внедрение зависимостей**: `GameContext` связывает все сервисы, настройки и prefab-объекты через Zenject. 【F:Assets/Scripts/Di/GameContext.cs†L13-L77】

## Требования
- Unity **6000.0.50f1** (см. `ProjectSettings/ProjectVersion.txt`). 【F:ProjectSettings/ProjectVersion.txt†L1-L2】
- DOTween, Zenject, R3 (реактивные стримы) и Unity Input System уже добавлены в проект.

## Запуск
1. Откройте проект в Unity версии 6000.0.50f1.
2. Загрузите сцену `Assets/Scenes/ArcanoidGame.unity`.
3. Убедитесь, что в инсталлере `GameContext` назначены ссылки на `ArkanoidView`, `BallView`, `PlatformView`, `GameFieldService` и ScriptableObject-настройки.
4. Нажмите **Play**. Управляйте платформой клавишами/тачем (Input System), а мяч запускайте кликом/тапом по направлению прицельной линии.

## Структура папок
- `Assets/Scripts` — код проекта (системы, UI, DI, пул объектов, модели).
- `Assets/Scenes` — основная сцена `ArcanoidGame.unity`.
- `Assets/Settings` — ScriptableObject с параметрами игры (скорость мяча, размеры поля, здоровье блоков и т. д.).
- `Assets/Plugins` и `Packages` — внешние библиотеки (Zenject, DOTween, Input System, TextMeshPro).

## Настройка параметров
- **Мяч:** значения скорости и допустимых углов отражения редактируются в `BallSettings` ScriptableObject. 【F:Assets/Scripts/Field/Data/BallSettings.cs†L5-L11】
- **Поле:** размеры сетки, размер и отступ блоков, префаб блока задаются в `GameFieldSettings`. 【F:Assets/Scripts/Field/Data/GameFieldSettings.cs†L5-L12】
- **Блоки:** прочность и анимации разрушения вынесены в `BlockHealthSettings` и `BlockAnimationSettings` ScriptableObject (см. папку `Assets/Settings`).

## Управление
- **Движение платформы:** действия из карты ввода `PlayerInput` (клавиши/джойстик/тач-свайпы).
- **Запуск мяча:** левый клик мыши или тап по экрану в нужную сторону; линия прицеливания обновляется до запуска. 【F:Assets/Scripts/Systems/BallSystem.cs†L28-L76】

## Известные нюансы
- В момент победы/поражения `Time.timeScale` ставится в 0; кнопка «Restart» перезагружает текущую сцену через `ArkanoidModule`. 【F:Assets/Scripts/Ui/ArkanoidModule.cs†L18-L36】【F:Assets/Scripts/Systems/GameStateSystem.cs†L37-L53】
- Для корректной работы DOTween убедитесь, что в проекте инициализирован DOTweenSetup (стандартный ассет библиотеки).