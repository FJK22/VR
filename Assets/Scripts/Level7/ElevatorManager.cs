using UnityEngine;
using UnityEngine.Events;

public class ElevatorManager : MonoBehaviour {

	private int elevatorsCount;
	public bool RandomStartFloor = true;
	public int InitialFloor = 1;
	public UnityAction WasStarted;
	[HideInInspector]
	public int _floor;
    public static ElevatorManager Instance;
	// Use this for initialization
	void Awake () {
        if (Instance)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
		if (RandomStartFloor) {
			elevatorsCount = transform.childCount;
			InitialFloor = Random.Range (1, elevatorsCount+1);
		}
	}
}
