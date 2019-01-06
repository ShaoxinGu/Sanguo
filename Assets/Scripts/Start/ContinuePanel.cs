using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ContinuePanel : MonoBehaviour
{
    public Button backButton;
    public GameObject recordsContent;
    private void Awake()
    {
        for (int i = 1; i <= 5; i++)
        {
            GameObject go = Instantiate(Resources.Load("Prefabs/Record")) as GameObject; ;
            go.GetComponentInChildren<Text>().text += i.ToString();
            go.transform.SetParent(recordsContent.transform);
        }
        backButton.onClick.AddListener(delegate (){gameObject.SetActive(false);});
    }

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
}
