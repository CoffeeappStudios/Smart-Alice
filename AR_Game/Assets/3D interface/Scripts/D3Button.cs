using UnityEngine;
using UnityEngine.Events;

public class D3Button : MonoBehaviour {

    [System.Serializable]
    public class Eventim : UnityEvent { }

    public Eventim onClick;

    private void OnMouseDown()
    {
        onClick.Invoke();
    }
}