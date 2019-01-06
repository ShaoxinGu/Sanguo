using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class Menu : MonoBehaviour {
    Dictionary<string, List<object>> dataDict = new Dictionary<string, List<object>>();
    public Button startButton;
    public Button continueButton;
    public Button customButton;
    public Button exitButton;
    public GameObject startPanel;
    public GameObject continuePanel;
    public GameObject customPanel;

    private void Awake()
    {
        startButton.onClick.AddListener(delegate (){startPanel.SetActive(true); });
        continueButton.onClick.AddListener(delegate (){continuePanel.SetActive(true);});
        customButton.onClick.AddListener(delegate (){customPanel.SetActive(true);});
        exitButton.onClick.AddListener(delegate (){Application.Quit();});
        StartCoroutine(ReadGeneralPropertyConfigFile());
        StartCoroutine(ReadCityPropertyConfigFile());
    }

    public IEnumerator ReadGeneralPropertyConfigFile()
    {
        string filepath = Application.streamingAssetsPath + "/Config/GeneralProperty.cfg";
        WWW www = new WWW(filepath);
        yield return www;
        while (www.isDone == false) yield return null;
        if (www.error == null)
        {
            byte[] data = www.bytes;
            List<object> datalist = DeserializeObj(data) as List<object>;
            dataDict.Add("GeneralProperty", datalist);
        }
        else
        {
            Debug.Log("wwwError<<" + www.error + "<<" + filepath);
        }
    }

    IEnumerator ReadCityPropertyConfigFile()
    {
        string filepath = Application.streamingAssetsPath + "/Config/CityProperty.cfg";
        WWW www = new WWW(filepath);
        yield return www;
        while (www.isDone == false) yield return null;
        if (www.error == null)
        {
            byte[] data = www.bytes;
            List<object> datalist = DeserializeObj(data) as List<object>;
            dataDict.Add("CityProperty", datalist);
        }
        else
        {
            Debug.Log("wwwError<<" + www.error + "<<" + filepath);
        }
    }

    public static object DeserializeObj(byte[] bytes)
    {
        object dic = null;
        if (bytes == null)
        {
            Debug.Log("当前字节流为空");
            return dic;
        }
        MemoryStream ms = new MemoryStream(bytes); //利用传来的byte[]创建一个内存流
        BinaryFormatter formatter = new BinaryFormatter();
        dic = formatter.Deserialize(ms);//把流中转换为Dictionary
        return dic;
    }
}
