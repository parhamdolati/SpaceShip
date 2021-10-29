using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour
{
    [SerializeField] private SpaceShipHandler _spaceShipHandler;
    [SerializeField] private AudioHandler _audioHandler;
    [SerializeField] private MapCreator _mapCreator;
    public Button StartGameBtn, LaboratoryBtn, SettingsBtn, MaxFuelBtns, StarterFuelBtn, NitroBtn;
    public GameObject TopBtns, MidleBtns, BottomBtns, CloseBarPanel;
    public GameObject Warning,Spaceship;
    public bool IsBarShowed = false, IsGameStarted = false;
    public Coroutine startRecord;
    private int nitroLvl, maxFuellvl, StarterFuelLvl;
    private Vector3 StartPosition;
    public Texture[] SpaceshipColors;
    public GameObject[] SpaceshipPrefabs;
    public GameObject Spaceship_PrefabsScrollerConten, Spaceship_ColorsScrollerConten;
    public Button Music, Efx, LefthandTogle;
    public Slider RotateSpeed;

    void Start()
    {
        MidleBtns.GetComponent<Animator>().SetTrigger("BackToMenu");
        BottomBtns.GetComponent<Animator>().SetTrigger("ShowBottomBtns");

        #region SetData

        if (!PlayerPrefs.HasKey("nitro"))
            PlayerPrefs.SetInt("nitro", 1);

        nitroLvl = PlayerPrefs.GetInt("nitro");
        NitroBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = nitroLvl.ToString();
        NitroBtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = (nitroLvl * 50).ToString();


        if (!PlayerPrefs.HasKey("maxFuel"))
            PlayerPrefs.SetInt("maxFuel", 1);

        maxFuellvl = PlayerPrefs.GetInt("maxFuel");
        MaxFuelBtns.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = maxFuellvl.ToString();
        MaxFuelBtns.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = (maxFuellvl * 20).ToString();


        if (!PlayerPrefs.HasKey("starterFuel"))
            PlayerPrefs.SetInt("starterFuel", 1);

        StarterFuelLvl = PlayerPrefs.GetInt("starterFuel");
        StarterFuelBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = StarterFuelLvl.ToString();
        StarterFuelBtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text =
            (StarterFuelLvl * 20).ToString();

        if (!PlayerPrefs.HasKey("Coin"))
            PlayerPrefs.SetInt("Coin", 10);
        BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text =
            PlayerPrefs.GetInt("Coin").ToString();

        if (!PlayerPrefs.HasKey("Record"))
            PlayerPrefs.SetFloat("Record", 0);
        BottomBtns.transform.Find("Record").GetChild(0).GetComponent<TMP_Text>().text =
            PlayerPrefs.GetFloat("Record").ToString();
        
        if(!PlayerPrefs.HasKey("SpaceshipPrefabIndex"))
            PlayerPrefs.SetInt("SpaceshipPrefabIndex",12);
        SpaceshipPrefab(PlayerPrefs.GetInt("SpaceshipPrefabIndex"));
        if(!PlayerPrefs.HasKey("SpaceshipColorIndex"))
            PlayerPrefs.SetInt("SpaceshipColorIndex",12);
        SpaceshipColer(PlayerPrefs.GetInt("SpaceshipColorIndex"));

        if(!PlayerPrefs.HasKey("Music"))
            PlayerPrefs.SetInt("Music",1);
        else
        {
            if (PlayerPrefs.GetInt("Music") == 0)
            {
                Music.GetComponent<Image>().color = new Color32(255, 0, 8, 255);
                Music.transform.Find("OnOff").GetComponent<Text>().text = "Off";
            }
            else
            {
                Music.GetComponent<Image>().color = new Color32(12, 255, 0, 255);
                Music.transform.Find("OnOff").GetComponent<Text>().text = "On";
            }
        }
        
        if(!PlayerPrefs.HasKey("Efx")) 
            PlayerPrefs.SetInt("Efx",1);
        else
        {
            if (PlayerPrefs.GetInt("Efx") == 0)
            {
                Efx.GetComponent<Image>().color = new Color32(255, 0, 8, 255);
                Efx.transform.Find("OnOff").GetComponent<Text>().text = "Off";
            }
            else
            {
                Efx.GetComponent<Image>().color = new Color32(12, 255, 0, 255);
                Efx.transform.Find("OnOff").GetComponent<Text>().text = "On";
            }
        }
        
        if(!PlayerPrefs.HasKey("IsRightHand"))
            PlayerPrefs.SetInt("IsRightHand",1);//1 yani rast dast
        else
        {
            if (PlayerPrefs.GetInt("IsRightHand") == 0)
            {
                LefthandTogle.transform.Find("Image").localPosition = new Vector3(-37, 0, 0);
                LefthandTogle.transform.Find("Image").GetComponent<Image>().color = new Color32(12, 255, 0, 255);
            }
            else
            {
                LefthandTogle.transform.Find("Image").localPosition = new Vector3(37, 0, 0);
                LefthandTogle.transform.Find("Image").GetComponent<Image>().color = new Color32(255, 8, 0, 255);
            }
        }
        
        if(!PlayerPrefs.HasKey("RotateSpeed"))
            PlayerPrefs.SetFloat("RotateSpeed",5);
        else
        {
            RotateSpeed.value = PlayerPrefs.GetFloat("RotateSpeed")/10;
        }
        #endregion
        
        StartGameBtn.onClick.AddListener(() =>
        {
            if (!IsBarShowed && !IsGameStarted)
            {
                IsGameStarted = true;
                MidleBtns.GetComponent<Animator>().SetTrigger("StartGame");
                BottomBtns.GetComponent<Animator>().SetTrigger("HideBottomBtns");
                TopBtns.GetComponent<Animator>().SetTrigger("ShowTopBtns");
                _mapCreator.SpaceShip.transform.Find("MainCamera").GetComponent<Animator>().SetTrigger("GameCamera");
                _mapCreator.StartGame();
                
                //taEn kardan Position Start bazi
                Vector3 tmpVector = _mapCreator.SpaceShip.transform.position;
                StartPosition = new Vector3(tmpVector.x, tmpVector.y, tmpVector.z + 12.34f);
                startRecord = StartCoroutine(StartRecord());
            }
        });

        LaboratoryBtn.onClick.AddListener(() =>
        {
            if (!IsBarShowed)
            {
                CloseBarPanel.SetActive(true);
                MidleBtns.GetComponent<Animator>().SetTrigger("ShowMenuBar");
                IsBarShowed = true;
                MidleBtns.transform.Find("Bar").Find("Laboratory").gameObject.SetActive(true);
                
                var content = MidleBtns.transform.Find("Bar").Find("Laboratory").Find("SpaceshipModels").Find("Scroll")
                    .Find("Conten");
                content.localPosition = new Vector3(599, content.position.z, content.position.z);
                content = MidleBtns.transform.Find("Bar").Find("Laboratory").Find("SpaceshipColer").Find("Scroll")
                    .Find("Conten");
                content.localPosition = new Vector3(599, content.position.z, content.position.z);
                
                _mapCreator.SpaceShip.transform.Find("MainCamera").GetComponent<Animator>().SetTrigger("CameraOnBar");

                Spaceship_PrefabsScrollerConten.transform.GetChild(PlayerPrefs.GetInt("SpaceshipPrefabIndex"))
                    .GetComponent<Image>().color = new Color(0, 255, 255, 255);
                Spaceship_ColorsScrollerConten.transform.GetChild(PlayerPrefs.GetInt("SpaceshipColorIndex"))
                    .GetComponent<Image>().color = new Color(0, 255, 255, 255);
                
            }
        });

        SettingsBtn.onClick.AddListener(() =>
        {
            if (!IsBarShowed)
            {
                CloseBarPanel.SetActive(true);
                MidleBtns.GetComponent<Animator>().SetTrigger("ShowMenuBar");
                IsBarShowed = true;
                MidleBtns.transform.Find("Bar").Find("Settings").gameObject.SetActive(true);
                _mapCreator.SpaceShip.transform.Find("MainCamera").GetComponent<Animator>().SetTrigger("CameraOnBar");
            }
        });

        StarterFuelBtn.onClick.AddListener(() =>
        {
            int coin = int.Parse(BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text);
            if (coin >= StarterFuelLvl * 20)
            {
                _audioHandler.Efx("Upgrade");
                StartCoroutine(SpaceshipUpgradeEffect());
                int RemindCoin = coin - StarterFuelLvl * 20;
                BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text =
                    RemindCoin.ToString();
                PlayerPrefs.SetInt("Coin", RemindCoin);
                PlayerPrefs.SetInt("starterFuel", ++StarterFuelLvl);
                StarterFuelBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text =
                    StarterFuelLvl.ToString();
                StarterFuelBtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text =
                    (StarterFuelLvl * 20).ToString();
            }
            else ShowWarning("Coin");
        });

        MaxFuelBtns.onClick.AddListener(() =>
        {
            int coin = int.Parse(BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text);
            if (coin >= maxFuellvl * 20)
            {
                _audioHandler.Efx("Upgrade");
                StartCoroutine(SpaceshipUpgradeEffect());
                int RemindCoin = coin - maxFuellvl * 20;
                BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text =
                    RemindCoin.ToString();
                PlayerPrefs.SetInt("Coin", RemindCoin);
                PlayerPrefs.SetInt("maxFuel", ++maxFuellvl);
                MaxFuelBtns.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = maxFuellvl.ToString();
                MaxFuelBtns.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text =
                    (maxFuellvl * 20).ToString();
            }
            else ShowWarning("Coin");
        });

        NitroBtn.onClick.AddListener(() =>
        {
            int coin = int.Parse(BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text);
            if (coin >= nitroLvl * 50)
            {
                _audioHandler.Efx("Upgrade");
                StartCoroutine(SpaceshipUpgradeEffect());
                int RemindCoin = coin - nitroLvl * 50;
                BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text =
                    RemindCoin.ToString();
                PlayerPrefs.SetInt("Coin", RemindCoin);
                PlayerPrefs.SetInt("nitro", ++nitroLvl);
                NitroBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = nitroLvl.ToString();
                NitroBtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = (nitroLvl * 50).ToString();
            }
            else ShowWarning("Coin");
        });
        
        Music.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetInt("Music") == 1)
            {
                PlayerPrefs.SetInt("Music", 0);
                Music.GetComponent<Image>().color = new Color32(255, 0, 8, 255);
                Music.transform.Find("OnOff").GetComponent<Text>().text = "Off";
            }
            else
            {
                PlayerPrefs.SetInt("Music", 1);
                Music.GetComponent<Image>().color = new Color32(12, 255, 0, 255);
                Music.transform.Find("OnOff").GetComponent<Text>().text = "On";
            }
        });
        
        Efx.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetInt("Efx") == 1)
            {
                PlayerPrefs.SetInt("Efx", 0);
                Efx.GetComponent<Image>().color = new Color32(255, 0, 8, 255);
                Efx.transform.Find("OnOff").GetComponent<Text>().text = "Off";
            }
            else
            {
                PlayerPrefs.SetInt("Efx", 1);
                Efx.GetComponent<Image>().color = new Color32(12, 255, 0, 255);
                Efx.transform.Find("OnOff").GetComponent<Text>().text = "On";
            }
        });
        
        LefthandTogle.onClick.AddListener(() =>
        {
            if (PlayerPrefs.GetInt("IsRightHand") == 1)
            {
                PlayerPrefs.SetInt("IsRightHand",0);
                LefthandTogle.transform.Find("Image").localPosition = new Vector3(-37, 0, 0);
                LefthandTogle.transform.Find("Image").GetComponent<Image>().color = new Color32(12, 255, 0, 255);
            }
            else
            {
                PlayerPrefs.SetInt("IsRightHand",1);
                LefthandTogle.transform.Find("Image").localPosition = new Vector3(37, 0, 0);
                LefthandTogle.transform.Find("Image").GetComponent<Image>().color = new Color32(255, 8, 0, 255);
            }
        });
    }
    
    IEnumerator SpaceshipUpgradeEffect()
    {
        Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(3, 3, 3, 1);
        yield return new WaitForSeconds(.1f);
        Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
        yield return new WaitForSeconds(.1f);
        Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(3, 3, 3, 1);
        yield return new WaitForSeconds(.1f);
        Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.color = new Color(1, 1, 1, 1);
    }

    public void CloseBar()
    {
        MidleBtns.GetComponent<Animator>().SetTrigger("CloseMenuBar");
        IsBarShowed = false;
        CloseBarPanel.SetActive(false);
        MidleBtns.transform.Find("Bar").Find("Laboratory").gameObject.SetActive(false);
        MidleBtns.transform.Find("Bar").Find("Settings").gameObject.SetActive(false);
        _mapCreator.SpaceShip.transform.Find("MainCamera").GetComponent<Animator>().SetTrigger("CameraOffBar");
    }

    void ShowWarning(string name)
    {
        Animator anim = Warning.GetComponent<Animator>();
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Wait"))
        {
            switch (name)
            {
                case "Coin":
                    anim.SetTrigger("Coin");
                    break;
            }
        }
    }
    
    IEnumerator StartRecord()
    {
        float record;
        while (Vector3.Distance(StartPosition,_mapCreator.SpaceShip.transform.position)>.1f)
        {
            yield return new WaitForEndOfFrame();
        }
        IsGameStarted = false;
        
        if (PlayerPrefs.GetInt("IsRightHand") == 1)
            _spaceShipHandler.NitroAbilityBtn.transform.localPosition = new Vector2(800,150);
        else _spaceShipHandler.NitroAbilityBtn.transform.localPosition = new Vector2(-800,0);
        _spaceShipHandler.NitroAbilityBtn.SetActive(true);
        
        while (true)
        {
            record = (_mapCreator.SpaceShip.transform.position.z - 12);
            record = (float)Math.Round(record, 2);
            TopBtns.transform.Find("RecordTxt").GetComponent<TMP_Text>().text =
                record.ToString();
            yield return new WaitForSeconds(.1f);
        }
    }

    public void ChangePrefab(GameObject btn)
    {
        _audioHandler.Efx("ChangeSkin");
        Spaceship_PrefabsScrollerConten.transform.GetChild(PlayerPrefs.GetInt("SpaceshipPrefabIndex"))
            .GetComponent<Image>().color = new Color(207, 207, 207, 255);
        btn.GetComponent<Image>().color = new Color(0, 255, 255, 255);
        SpaceshipPrefab(int.Parse(btn.name));
    }
    
    public void ChangeColer(GameObject btn)
    {
        _audioHandler.Efx("ChangeSkin");
        Spaceship_ColorsScrollerConten.transform.GetChild(PlayerPrefs.GetInt("SpaceshipColorIndex"))
            .GetComponent<Image>().color = new Color(207, 207, 207, 255);
        btn.GetComponent<Image>().color = new Color(0, 255, 255, 255);
        SpaceshipColer(int.Parse(btn.name));
    }
    
    void SpaceshipColer(int index)
    {
        PlayerPrefs.SetInt("SpaceshipColorIndex",index);
        switch (index)
        {
            case 0:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[0];
                break;
            case 1:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[1];
                break;
            case 2:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[2];
                break;
            case 3:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[3];
                break;
            case 4:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[4];
                break;
            case 5:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[5];
                break;
            case 6:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[6];
                break;
            case 7:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[7];
                break;
            case 8:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[8];
                break;
            case 9:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[9];
                break;
            case 10:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[10];
                break;
            case 11:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[11];
                break;
            case 12:
                Spaceship.transform.GetChild(0).GetComponent<Renderer>().material.mainTexture = SpaceshipColors[12];
                break;
        }
    }

    void SpaceshipPrefab(int index)
    {
        PlayerPrefs.SetInt("SpaceshipPrefabIndex", index);
        GameObject tmp = Instantiate(SpaceshipPrefabs[index], new Vector3(0, .5f, 0), Quaternion.identity);
        tmp.transform.SetParent(Spaceship.transform.GetChild(0).parent);
        tmp.transform.SetSiblingIndex(0);
        SpaceshipColer(PlayerPrefs.GetInt("SpaceshipColorIndex"));
        Destroy(Spaceship.transform.GetChild(1).gameObject);
    }

    public void SpaceshipRotateSpeed(Slider slider)
    {
        float value;
        if (slider.value >= .1f)
            value = slider.value * 10;
        else
            value = 1;
        _spaceShipHandler.SpaceShipRotateSpeed = value;
        PlayerPrefs.SetFloat("RotateSpeed", value);
    }
}
