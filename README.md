# Little Service
Разработать маленький сервис, который будет принимать и отправлять события на сервер аналитики.
Примеры событий: старт уровня, получение награды, трата монеток.
Платформы проекта: Android, WebGL.

Ориентировочное время на выполнение задания 2-3 часа (время не фиксированное, вполне можно потратить и больше, если есть желание).

Событие - это объект, включающий в себя поля:

type - тип события, строка
data - данные события, строка
Сервер принимает несколько событий в одном POST запросе в формате JSON.
Сам сервер делать не надо. Предполагается что его делает другой человек в команде, и он ещё не готов.
Формат запроса на сервер описан в представленном ниже примере:

'''C#
{
    "events": [
        {
            "type": "levelStart",
            "data": "level:3"
        },
        ...
    ]
}
'''

URL для отправки задаём внешним параметром сервиса serverUrl.

Сервис принимает события через метод TrackEvent с аргументами string type и string data. Полученное событие отправляется не сразу, а через определённое время (задаём параметром сервиса cooldownBeforeSend). Этот кулдаун нужен, чтобы накопить и отправить за раз несколько событий.

Например, если вызвать метод TrackEvent несколько раз в течение короткого промежутка времени (меньше чем cooldownBeforeSend), то сервис отправит все эти события за один раз в одном сообщении.

Также, важно обеспечить гарантированную доставку событий до сервера. События считаются доставленными, только если в ответ на сообщение, сервер вернул 200 OK.

Сервер аналитики не всегда может быть доступен (например, отсутствие сети на телефоне), поэтому успешная отправка может произойти через неопределённое время. Если приложение завершилось (или скрешилось по ошибке), то недоставленные события должны быть отправлены при следующем запуске приложения (считаем, что сервис стартует вместе с приложением). Таким образом, события не должны теряться.

Сам сервис достаточно оформить в класс наследник MonoBehaviour, например:

'''C#
public class EventService : MonoBehaviour {

    public void TrackEvent(string type, string data) {
    }
}
'''

Для проекта используем Unity 2021 LTS.
Можно использовать дополнительные библиотеки.
