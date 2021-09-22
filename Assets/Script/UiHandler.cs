using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UiHandler : MonoBehaviour
{

    [SerializeField] private MapCreator _mapCreator;
    public Button StartGameBtn, LaboratoryBtn, SettingsBtn, MaxFuelBtns, StarterFuelBtn, NitroBtn;
    public GameObject TopBtns, MidleBtns, BottomBtns, CloseBarPanel;
    public bool IsBarShowed = false;
    private int nitroLvl, maxFuellvl, StarterFuelLvl;
    
    void Start()
    {
        MidleBtns.GetComponent<Animator>().SetTrigger("BackToMenu");
        BottomBtns.GetComponent<Animator>().SetTrigger("ShowBottomBtns");

        #region SetData

        if (!PlayerPrefs.HasKey("nitro"))
            PlayerPrefs.SetInt("nitro", 1);
        
        nitroLvl = PlayerPrefs.GetInt("nitro"); 
        NitroBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = nitroLvl.ToString();
        NitroBtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = (nitroLvl * 20).ToString();
        

        if (!PlayerPrefs.HasKey("maxFuel"))
            PlayerPrefs.SetInt("maxFuel", 1);
        
        maxFuellvl = PlayerPrefs.GetInt("maxFuel");
        MaxFuelBtns.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = maxFuellvl.ToString();
        MaxFuelBtns.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = (maxFuellvl * 20).ToString();
        

        if (!PlayerPrefs.HasKey("starterFuel"))
            PlayerPrefs.SetInt("starterFuel", 1);
        
        StarterFuelLvl = PlayerPrefs.GetInt("starterFuel");
        StarterFuelBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = StarterFuelLvl.ToString();
        StarterFuelBtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = (StarterFuelLvl * 20).ToString();
        
        if(!PlayerPrefs.HasKey("coin"))
            PlayerPrefs.SetInt("coin",10);
        BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text =
            PlayerPrefs.GetInt("Coin").ToString();
        
        if(!PlayerPrefs.HasKey("Record"))
            PlayerPrefs.SetInt("Record",0);
        BottomBtns.transform.Find("Record").GetChild(0).GetComponent<TMP_Text>().text =
            PlayerPrefs.GetInt("Record").ToString();

        #endregion
        
        StartGameBtn.onClick.AddListener(() =>
        {
            if (!IsBarShowed)
            {
                MidleBtns.GetComponent<Animator>().SetTrigger("StartGame");
                BottomBtns.GetComponent<Animator>().SetTrigger("HideBottomBtns");
                TopBtns.GetComponent<Animator>().SetTrigger("ShowTopBtns");
                _mapCreator.StartGame();
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
            }
        });
        
        MaxFuelBtns.onClick.AddListener(() =>
        {
            int coin = int.Parse(BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text);
            if (coin >= maxFuellvl * 20)
            {
                int RemindCoin = coin - maxFuellvl * 20;
                BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text =
                    RemindCoin.ToString();
                PlayerPrefs.SetInt("Coin",RemindCoin);
                PlayerPrefs.SetInt("maxFuel", ++maxFuellvl);
                MaxFuelBtns.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = maxFuellvl.ToString();
                MaxFuelBtns.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text =
                    (maxFuellvl * 20).ToString();
            }
        }); 
        
        StarterFuelBtn.onClick.AddListener(() =>
        {
            int coin = int.Parse(BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text);
            if (coin >= StarterFuelLvl * 20)
            {
                int RemindCoin = coin - StarterFuelLvl * 20;
                BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text =
                    RemindCoin.ToString();
                PlayerPrefs.SetInt("Coin",RemindCoin);
                PlayerPrefs.SetInt("starterFuel", ++StarterFuelLvl);
                StarterFuelBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text =
                    StarterFuelLvl.ToString();
                StarterFuelBtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text =
                    (StarterFuelLvl * 20).ToString();
            }
        }); 
        
        NitroBtn.onClick.AddListener(() =>
        {
            int coin = int.Parse(BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text);
            if (coin >= nitroLvl * 20)
            {
                int RemindCoin = coin - nitroLvl * 20;
                BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text =
                    RemindCoin.ToString();
                PlayerPrefs.SetInt("Coin",RemindCoin);
                PlayerPrefs.SetInt("nitro", ++nitroLvl);
                NitroBtn.gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = nitroLvl.ToString();
                NitroBtn.gameObject.transform.GetChild(1).GetComponent<TMP_Text>().text = (nitroLvl * 20).ToString();
            }
        }); 
    }

    public void CloseBar()
    {
        MidleBtns.GetComponent<Animator>().SetTrigger("CloseMenuBar");
        IsBarShowed = false;
        CloseBarPanel.SetActive(false);
        MidleBtns.transform.Find("Bar").Find("Laboratory").gameObject.SetActive(false);
        MidleBtns.transform.Find("Bar").Find("Settings").gameObject.SetActive(false);
    }
}
