using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class SpaceShipHandler : MonoBehaviour
{
    [SerializeField] private MapCreator _mapCreator;
    [SerializeField] private UiHandler _uiHandler;
    public GameObject Spaceship, Stars, Result;
    public TMP_Text top_StarterFuelTxt, top_NitroTxt, top_CoinTxt;
    public Image LeftMaxFuel, RightMaxFuel;
    private float SpaceShipSpeed, SpaceShipRotateSpeed;
    private int MaxFuel, StarterFuel ,Coin;
    private float Nitro;
    private bool IsNetroOn;
    private Coroutine _startSpaceShipContril, _spaceshipInfoHandler;
    private bool IsFirstTouch = true;
    private Vector2 FirstTouchPosition;

    public void SpaceShipControl()
    {
        _startSpaceShipContril = StartCoroutine(StartSpaceShipControl());
    }
    
    IEnumerator StartSpaceShipControl()
    {
        SpaceShipSpeed = 3;
        SpaceShipRotateSpeed = 5;
        IsNetroOn = false;
        Nitro = 0;
        StarterFuel = PlayerPrefs.GetInt("starterFuel") * 2 + 15;
        MaxFuel = PlayerPrefs.GetInt("maxFuel") + StarterFuel;
        LeftMaxFuel.fillAmount = (float) Math.Round((100f * StarterFuel / MaxFuel) / 100f,1);
        RightMaxFuel.fillAmount = (float) Math.Round((100f * StarterFuel / MaxFuel) / 100f,1);
        top_StarterFuelTxt.text = StarterFuel.ToString();
        top_NitroTxt.text = "0";
        Coin = 0;
        _spaceshipInfoHandler = StartCoroutine(SpaceshipInfoHandler());
        while (true)
        {
            //harakate safine wa stars ru be jolo
            Spaceship.transform.Translate(Vector3.forward*SpaceShipSpeed*Time.deltaTime);
            Stars.transform.Translate(Vector3.back * SpaceShipSpeed * Time.deltaTime);
            
            //charkhesh safine
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                if (Input.GetKey(KeyCode.W))
                {
                    Spaceship.transform.Rotate(Vector3.forward * SpaceShipRotateSpeed);
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    Spaceship.transform.Rotate(Vector3.back * SpaceShipRotateSpeed);
                }

                if (Input.GetKey(KeyCode.Space))
                {
                    activeNitro();
                }
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 1 && IsFirstTouch)
                {
                    FirstTouchPosition = Input.GetTouch(0).position;
                    IsFirstTouch = false;
                    if(Input.GetTouch(0).position.y > FirstTouchPosition.y)
                        Spaceship.transform.Rotate(Vector3.forward * SpaceShipRotateSpeed);
                    else if(Input.GetTouch(0).position.y < FirstTouchPosition.y)
                        Spaceship.transform.Rotate(Vector3.back * SpaceShipRotateSpeed);
                }
                else if (Input.touchCount == 1 && !IsFirstTouch)
                {
                    if(Input.GetTouch(0).position.y > FirstTouchPosition.y)
                        Spaceship.transform.Rotate(Vector3.forward * SpaceShipRotateSpeed);
                    else if(Input.GetTouch(0).position.y < FirstTouchPosition.y)
                        Spaceship.transform.Rotate(Vector3.back * SpaceShipRotateSpeed);
                }
                else if (Input.touchCount == 0 && !IsFirstTouch)
                {
                    IsFirstTouch = true;
                }
                
                /*if(Input.GetTouch(0).tapCount==2)
                    activeNitro();*/
            }
            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator SpaceshipInfoHandler()
    {
        yield return new WaitForSeconds(.5f);
        while (true)
        {
            yield return new WaitForSeconds(1);
            SpaceShipSpeed += .01f;
            if (StarterFuel - 1 >= 0)
            {
                StarterFuel--;
                top_StarterFuelTxt.text = StarterFuel.ToString();
                LeftMaxFuel.fillAmount = (float) Math.Round((100f * StarterFuel / MaxFuel) / 100f,1);
                RightMaxFuel.fillAmount = (float) Math.Round((100f * StarterFuel / MaxFuel) / 100f,1);
            }
            else if (StarterFuel - 2 > 0 && IsNetroOn)
            {
                StarterFuel -= 2;
                top_StarterFuelTxt.text = StarterFuel.ToString();
                LeftMaxFuel.fillAmount = (float) Math.Round((100f * StarterFuel / MaxFuel) / 100f,1);
                RightMaxFuel.fillAmount = (float) Math.Round((100f * StarterFuel / MaxFuel) / 100f,1);
            }
            else
            {
                GameFinished();
            }

            if (!IsNetroOn)
            {
                Nitro += PlayerPrefs.GetInt("nitro") / 500f;
                Nitro = (float) Math.Round(Nitro, 2);
                top_NitroTxt.text = Nitro.ToString();
            }
        }
    }
    
    public void activeNitro()
    {
        if (!IsNetroOn)
            StartCoroutine(ActiveNitro());
    }
    
    IEnumerator ActiveNitro()
    {
        Spaceship.transform.Find("Stars").gameObject.SetActive(true);
        SpaceShipSpeed += 4;
        IsNetroOn = true;
        while (Nitro>0)
        {
            Nitro -= PlayerPrefs.GetInt("nitro") / 100f;
            Nitro = (float) Math.Round(Nitro, 2);
            top_NitroTxt.text = Nitro.ToString();
            yield return new WaitForSeconds(.1f);
        }

        Nitro = 0;
        top_NitroTxt.text = "0";
        
        Spaceship.transform.Find("Stars").gameObject.SetActive(false);
        
        SpaceShipSpeed -= 4;
        IsNetroOn = false;
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider.tag == "Agent" && !IsNetroOn){
            GameFinished();
        }
        if(collision.collider.tag == "Fuel"){
            if (StarterFuel + 5 <= MaxFuel)
            {
                StarterFuel += 5;
                top_StarterFuelTxt.text = StarterFuel.ToString();
            }
            else
            {
                StarterFuel = MaxFuel;
                top_StarterFuelTxt.text = StarterFuel.ToString();
            }
            collision.gameObject.SetActive(false);
            GameObject effect = Spaceship.transform.Find("Charge_Fuel").gameObject;
            StartCoroutine(OnOffEffect(effect));
        }
        if(collision.collider.tag == "Coin")
        {
            Coin += 5;
            top_CoinTxt.text = Coin.ToString();
            collision.gameObject.SetActive(false);
            GameObject effect = Spaceship.transform.Find("Charge_Coin").gameObject;
            StartCoroutine(OnOffEffect(effect));
        }
    }

    void GameFinished()
    {
        //animation enfejar
        Spaceship.transform.GetChild(0).gameObject.SetActive(false);
        GameObject effect = Spaceship.transform.Find("Exposion").gameObject;
        StartCoroutine(OnOffEffect(effect));
        StopCoroutine(_spaceshipInfoHandler);
        StopCoroutine(_startSpaceShipContril);
        StopCoroutine(_uiHandler.startRecord);
        StartCoroutine(gameFinished());
    }

    IEnumerator OnOffEffect(GameObject effect)
    {
        switch (effect.name)
        {
            case "Charge_Fuel":
                if (!effect.activeInHierarchy)
                {
                    effect.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    effect.SetActive(false);
                }
                break;
            case "Charge_Coin":
                if (!effect.activeInHierarchy)
                {
                    effect.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    effect.SetActive(false);
                }
                break;
            case "Exposion":
                if (!effect.activeInHierarchy)
                {
                    effect.SetActive(true);
                    yield return new WaitForSeconds(1.5f);
                    effect.SetActive(false);
                }
                break;
        }
    }
    
    IEnumerator gameFinished()
    {
        //larzes dorbin
        Spaceship.transform.Find("MainCamera").GetComponent<Animator>().SetTrigger("CameraGameOver");
        
        yield return new WaitForSeconds(1);
        _uiHandler.TopBtns.GetComponent<Animator>().SetTrigger("HideTopBtns");
        
        //namayesh natije
        float record = float.Parse(_uiHandler.TopBtns.transform.Find("RecordTxt").GetComponent<TMP_Text>().text);
        if (record > PlayerPrefs.GetFloat("Record"))
        {
            PlayerPrefs.SetFloat("Record",record);
            _uiHandler.BottomBtns.transform.Find("Record").GetChild(0).GetComponent<TMP_Text>().text =
                record.ToString();
            Result.SetActive(true);
            Result.transform.Find("Record").GetChild(0).GetComponent<TMP_Text>().text = record.ToString();
            yield return new WaitForSeconds(2);
            Result.SetActive(false);
        }
        else yield return new WaitForSeconds(1);
        
        Spaceship.transform.Find("MainCamera").GetComponent<Animator>().SetTrigger("MenuCamera");
        _uiHandler.MidleBtns.GetComponent<Animator>().SetTrigger("BackToMenu");
        _uiHandler.BottomBtns.GetComponent<Animator>().SetTrigger("ShowBottomBtns");
        Spaceship.transform.GetChild(0).gameObject.SetActive(true);
        Spaceship.transform.position = new Vector3(0, 0, 0);
        for(int i=0;i<_mapCreator.MapsPart.Count;i++)
            Destroy(_mapCreator.MapsPart[i]);
        _mapCreator.MapsPart.Clear();

        

        int coin = PlayerPrefs.GetInt("Coin") + Coin;
        PlayerPrefs.SetInt("Coin",coin);
        _uiHandler.BottomBtns.transform.Find("Coin").GetChild(0).GetComponent<TMP_Text>().text = coin.ToString();
        
        _uiHandler.TopBtns.transform.Find("RecordTxt").GetComponent<TMP_Text>().text = "0";
        _uiHandler.TopBtns.transform.Find("CoinTxt").GetComponent<TMP_Text>().text = "0";
    }
}
