using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CustomPanel : MonoBehaviour
{
    public Button backButton;
    public Image avatarImage;
    public Toggle[] genderToggle;

    private bool isWoman = false;

    private void Awake()
    {
        backButton.onClick.AddListener(delegate (){gameObject.SetActive(false);});
        genderToggle[0].onValueChanged.AddListener(delegate(bool selected){
            if (selected && isWoman)
            {
                Sprite sp = Resources.Load("Images/man", typeof(Sprite)) as Sprite;
                avatarImage.sprite = sp;
                isWoman = false;
            }
            else if(!selected && !isWoman)
            {
                Sprite sp = Resources.Load("Images/woman", typeof(Sprite)) as Sprite;
                avatarImage.sprite = sp;
                isWoman = true;
            }
        });
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
