using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class EventService : MonoBehaviour
{
    [SerializeField] private string _serverUrl;

    [SerializeField] private float _cooldownBeforeSend = 5f;

    private Queue<EventData> _eventQueue = new Queue<EventData>();

    private Coroutine _sendCoroutine;

    private string _jsonData;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        string savedData = PlayerPrefs.GetString("EventServiceData");
        if (!string.IsNullOrEmpty(savedData))
        {
            List<EventData> eventDataList = JsonUtility.FromJson<EventDataList>(savedData).Events;
            foreach (EventData eventData in eventDataList)
            {
                _eventQueue.Enqueue(eventData);
            }
            SendEvents();
        }
    }

    private void OnDestroy()
    {
        if (_eventQueue.Count > 0)
        {
            EventDataList eventDataList = new EventDataList();
            eventDataList.Events = new List<EventData>(_eventQueue);
            _jsonData = JsonUtility.ToJson(eventDataList);
            PlayerPrefs.SetString("EventServiceData", _jsonData);
            PlayerPrefs.Save();
        }
    }

    public void TrackEvent(string type, string data)
    {
        _eventQueue.Enqueue(new EventData(type, data));

        if (_sendCoroutine == null)
        {
            _sendCoroutine = StartCoroutine(SendEventsCoroutine());
        }
    }

    private IEnumerator SendEventsCoroutine()
    {
        yield return new WaitForSeconds(_cooldownBeforeSend);

        SendEvents();
        _sendCoroutine = null;
    }

    private void SendEvents()
    {
        if (_eventQueue.Count == 0)
        {
            return;
        }

        EventDataList eventDataList = new EventDataList();
        eventDataList.Events = new List<EventData>(_eventQueue);
        _jsonData = JsonUtility.ToJson(eventDataList);

        UnityWebRequest request = UnityWebRequest.Post(_serverUrl, _jsonData);
        request.SetRequestHeader("Content-Type", "application/json");

        StartCoroutine(SendRequestCoroutine(request));
    }

    private IEnumerator SendRequestCoroutine(UnityWebRequest request)
    {
        yield return request.SendWebRequest();

        if(request.isHttpError || request.isNetworkError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log("200 Œ ");
        }
    }
}
