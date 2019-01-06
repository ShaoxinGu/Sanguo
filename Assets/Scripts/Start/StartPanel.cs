using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartPanel : MonoBehaviour
{
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public Button backButton;
    private void Awake()
    {
        easyButton.onClick.AddListener(delegate (){});
        normalButton.onClick.AddListener(delegate (){});
        hardButton.onClick.AddListener(delegate (){});
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
