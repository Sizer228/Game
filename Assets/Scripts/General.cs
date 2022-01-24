using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class General : MonoBehaviour
{
    public static General instance;
    private void Awake() { instance = this; }

    public int partCount = 7;
    public List<GameObject> PrefabList = new List<GameObject>();
    public float speed = 0;
    public float maxSpeed = 12;

    private bool died = false;

    private List<GameObject> parts = new List<GameObject>();
    private int selector;
    private float distans = 23; 

    // Start is called before the first frame update
    void Start()
    {
        ResetLevel();
        StartLevel();
    }

    // Update is called once per frame
    void Update()
    {
        if (speed < 1 || died) { return; }

        foreach (GameObject part in parts)
        {
            part.transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
        }

        if (parts[0].transform.position.z < -distans)
        {
            Destroy(parts[0]);
            parts.RemoveAt(0);
            CreatePart();
        }

    }

    public void StartLevel()
    {
        speed = maxSpeed;
		SwipeController.instance.enabled = true;
    }
    public void PauseLevel()
    {
        speed = 0;
		SwipeController.instance.enabled = false;
    }

    public void ResetLevel()
    {
        speed = 0;
        while (parts.Count > 0)
        {
            Destroy(parts[0]);
            parts.RemoveAt(0);
        }
        for (int i = 0; i < partCount; i++)
        {
            CreatePart();
        }
		SwipeController.instance.enabled = false;
    }

    private void CreatePart()
    {

        Vector3 pos = Vector3.zero;

        if (parts.Count > 0)
        {
            pos = parts[parts.Count - 1].transform.position + new Vector3(0, 0, distans);
        }
        selector = (selector + 1) % PrefabList.Count;
        GameObject go = Instantiate(PrefabList[selector], pos, Quaternion.identity);
        go.transform.SetParent(transform);

        parts.Add(go);
    }

}
