using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class Test : MonoBehaviour
{
    public class TestClass
    {
        public int id;
        public int score;
    }

    [SerializeField]
    private Button _btnPeekRoulette;

    [SerializeField]
    private Button _btnPrintRoulette;

    private RandomEx.Roulette<int> _roulett;

    private TestClass _cachedTest = new TestClass();
    private CachedValue2<TestClass> _cachedValue = new CachedValue2<TestClass>();

    private void Awake()
    {
        _cachedTest.id = 1;
        _cachedTest.score = 100;

        _cachedValue.Set(_cachedTest);

        Debug.LogWarning("[Test::Awake] 1 // _cachedValue.score = " + _cachedValue.Get<int>("score"));
        Debug.LogWarning("[Test::Awake] 1 // _cachedValue.id = " + _cachedValue.Get<int>("id"));

        _cachedTest.id = 3;
        _cachedTest.score = 0;

        Debug.LogWarning("[Test::Awake] 2 // _cachedValue.score = " + _cachedValue.Get<int>("score"));
        Debug.LogWarning("[Test::Awake] 2 // _cachedValue.id = " + _cachedValue.Get<int>("id"));

        _cachedValue.Set(_cachedTest);

        Debug.LogWarning("[Test::Awake] 3 // _cachedValue.score = " + _cachedValue.Get<int>("score"));
        Debug.LogWarning("[Test::Awake] 3 // _cachedValue.id = " + _cachedValue.Get<int>("id"));

        _btnPeekRoulette.onClick.AddListener(PeekRoulette);
        _btnPrintRoulette.onClick.AddListener(PrintRoulette);

        CreateRoulett();
    }

    private void CreateRoulett()
    {
        Dictionary<int, int> table = new Dictionary<int, int>();
        table.Add(0, 10);
        table.Add(1, 20);
        table.Add(2, 30);
        table.Add(3, 10);
        table.Add(4, 5);
        table.Add(5, 25);

        _roulett = RandomEx.RandomEx.CreateRoulette(0, table);

        Debug.LogWarning("CreateRoulette");

        PrintRoulette();

        PeekRoulette();
        PeekRoulette();
        PeekRoulette();
        PeekRoulette();
        PeekRoulette();
    }
    
    public void PeekRoulette()
    {
        print(_roulett.Peek());
    }

    public void PrintRoulette()
    {
        _roulett.PrintSeedTable();
    }
}

public class EventData
{
    
}

public interface TestInterface
{
    Observer observer { get; }
}

public interface Observer
{
    void OnEvent(EventData data);

    IEnumerator OnEventAsync(EventData data);
}

public static class EventManager
{
    private class ObserverTemplete : Observer
    {
        public void OnEvent(EventData data)
        { 
        }

        public IEnumerator OnEventAsync(EventData data)
        {
            yield return new WaitForEndOfFrame();
        }
    }

    public static Dictionary<int, List<Observer>> _observer;

    public static void DispatchEvent(int type, EventData data)
    {
        _observer[type].ForEach((Observer a) => { a.OnEvent(data); });
    }

    public static Observer GetObserver(System.Action<EventData> inAction)
    {
        ObserverTemplete _observer = new ObserverTemplete();

        return _observer;
    }
}

public class TestClass : TestInterface
{
    public Observer observer
    {
        get
        {
            return null;
        }
    }
}
