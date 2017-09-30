using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class Detect : MonoBehaviour
{

    public enum Tur {BalonPatlatma, ElmaIslemleri, KupItme};

    public Tur oyunTuru;
    [HideInInspector]//Inspectorda gözükenler
    public GameObject patlamaParticle, balonPrefab,elmaPrefab, islemObjesi,sepetObjesi, kuvvetSprite, kuvvetSlider;
    //Sadece kodda kullanılanlar
    private GameObject elmaKlon, kuvvetKlon, sanalKup;
    private List<GameObject> klonlananlar = new List<GameObject>();
    [HideInInspector]//Inspectorda gözükenler
    public int baslangicSayisi, balonSayisi,maxElmaSayisi;
    //Sadece kodda kullanılanlar
    private int elmaSayisi, sepettekiElmalar;
    [HideInInspector]//Inspectorda gözükenler
    public Vector3 spawnMin, spawnMax;
    [HideInInspector]//Inspectorda gözükenler
    public float elmalarArasi, kuvvetCarpani;

    private void Start()
    {
        if (oyunTuru == Tur.BalonPatlatma)
        {
            for (int i = baslangicSayisi; i <= balonSayisi; i++)
            {
                BalonSpawnla(i);
            }
        }
        else if(oyunTuru == Tur.ElmaIslemleri)
        {
            ElmaSpawnla();
            sepettekiElmalar = 0;
            sepetObjesi.GetComponentInChildren<Text>().text = sepettekiElmalar.ToString();
        }
    }

    void ElmaSpawnla()
    {
        int islem = Random.Range(1, 11); //1-5 toplama, 6-10 çıkarma
        int spL = Random.Range(1, maxElmaSayisi + 1);
        int spR = Random.Range(1, maxElmaSayisi + 1);
        if (islem < 5)
        {
            islemObjesi.SetActive(true);
            elmaSayisi = spR + spL;
            //Soldaki elmaların klonlanması
            for (int i = 0; i < spL; i++)
            {
                if (i < 5)
                {
                    Instantiate(elmaPrefab, new Vector3(spawnMin.x + (i * elmalarArasi), spawnMin.y, spawnMin.z), Quaternion.identity);
                }
                else
                {
                    Instantiate(elmaPrefab, new Vector3(spawnMin.x + ((i - 5) * elmalarArasi), spawnMin.y, spawnMin.z - elmalarArasi), Quaternion.identity);
                }
            }
            //Sağdaki elmaların klonlanması
            for (int i = 0; i < spR; i++)
            {
                if (i < 5)
                {
                    Instantiate(elmaPrefab, new Vector3(spawnMax.x + (i * elmalarArasi), spawnMax.y, spawnMax.z), Quaternion.identity);
                }
                else
                {
                    Instantiate(elmaPrefab, new Vector3(spawnMax.x + ((i - 5) * elmalarArasi), spawnMax.y, spawnMax.z - elmalarArasi), Quaternion.identity);
                }
            }
        }
        else
        {
            islemObjesi.SetActive(false);
            elmaSayisi = Mathf.Abs(spR - spL);
            //Çocuğun karşısına eksi değer çıkmaması için
            if (spR > spL)
            {
                //Soldaki elmaların klonlanması
                for (int i = 0; i < spR; i++)
                {
                    if (i < 5)
                    {
                        Instantiate(elmaPrefab, new Vector3(spawnMin.x + (i * elmalarArasi), spawnMin.y, spawnMin.z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(elmaPrefab, new Vector3(spawnMin.x + ((i - 5) * elmalarArasi), spawnMin.y, spawnMin.z - elmalarArasi), Quaternion.identity);
                    }
                }
                //Sağdaki elmaların klonlanması
                for (int i = 0; i < spL; i++)
                {
                    if (i < 5)
                    {
                        Instantiate(elmaPrefab, new Vector3(spawnMax.x + (i * elmalarArasi), spawnMax.y, spawnMax.z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(elmaPrefab, new Vector3(spawnMax.x + ((i - 5) * elmalarArasi), spawnMax.y, spawnMax.z - elmalarArasi), Quaternion.identity);
                    }
                }
            }
            else
            {
                //Soldaki elmaların klonlanması
                for (int i = 0; i < spL; i++)
                {
                    if (i < 5)
                    {
                        Instantiate(elmaPrefab, new Vector3(spawnMin.x + (i * elmalarArasi), spawnMin.y, spawnMin.z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(elmaPrefab, new Vector3(spawnMin.x + ((i - 5) * elmalarArasi), spawnMin.y, spawnMin.z - elmalarArasi), Quaternion.identity);
                    }
                }
                //Sağdaki elmaların klonlanması
                for (int i = 0; i < spR; i++)
                {
                    if (i < 5)
                    {
                        Instantiate(elmaPrefab, new Vector3(spawnMax.x + (i * elmalarArasi), spawnMax.y, spawnMax.z), Quaternion.identity);
                    }
                    else
                    {
                        Instantiate(elmaPrefab, new Vector3(spawnMax.x + ((i - 5) * elmalarArasi), spawnMax.y, spawnMax.z - elmalarArasi), Quaternion.identity);
                    }
                }
            }
        }
    }

    void BalonSpawnla(int sayi)
    {
        Camera camera = Camera.main;
        RaycastHit hit1,hit2;

        Vector3 kameraT = camera.transform.position;
        Vector3 spawnYer = RastgeleYer(spawnMin, spawnMax);
        Vector3 dir = (kameraT - spawnYer).normalized;
        Physics.Raycast(kameraT, new Vector3(-dir.x,0,-dir.z), out hit2, 30);
        Physics.SphereCast(kameraT, 2/balonSayisi, new Vector3(-dir.x, 0, -dir.z), out hit1, 30);
        if (hit1.transform != null || hit2.transform != null)
        {
            BalonSpawnla(sayi);
        }
        else
        {
            GameObject balon = Instantiate(balonPrefab, spawnYer, Quaternion.Euler(90,0,0));
            balon.GetComponent<MeshRenderer>().materials[0].color = RandomRenk();
            balon.GetComponentInChildren<Text>().text = "" + sayi;
            balon.name = "balon-" + sayi;
            balon.AddComponent<BalonHareket>().BalonHareketBaslat();
        }
    }

    Color RandomRenk()
    {
        float r = Random.Range(0f, 0.9f);
        float b = Random.Range(0f, 0.9f);
        float g = Random.Range(0f, 0.9f);

        return new Color(r, g, b);
    }

    Vector3 RastgeleYer(Vector3 spawnMinimum, Vector3 spawnMaximum)
    {
        float spawnX = Random.Range(spawnMinimum.x, spawnMaximum.x);
        float spawnY = Random.Range(spawnMinimum.y, spawnMaximum.y);
        float spawnZ = Random.Range(spawnMinimum.z, spawnMaximum.z);

        return new Vector3(spawnX, spawnY, spawnZ);
    }

    void Update()
    {
        Camera camera = Camera.main;
        RaycastHit hit;

        if (camera == null)
            return;

#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        // PC için kod (mouse)
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit,150))
            {
                if(oyunTuru == Tur.BalonPatlatma)
                {
                    string[] sayiAl = hit.transform.name.Split("-"[0]);
                    if (hit.transform.gameObject.tag == "Interactable" && int.Parse(sayiAl[1]) == baslangicSayisi)
                    {
                        baslangicSayisi = int.Parse(sayiAl[1]) + 1;
                        hit.transform.gameObject.SetActive(false);
                        GameObject patlama = Instantiate(patlamaParticle, hit.transform.GetChild(0).transform.position, Quaternion.identity);
                        patlama.GetComponent<ParticleSystem>().Play();
                        Destroy(hit.transform.gameObject, 0.2f);
                        Destroy(patlama, 0.2f);
                        if (baslangicSayisi > balonSayisi)
                        {
                            Debug.Log("Tebrikler, bitirdin");
                        }
                    }
                    else if (hit.transform.GetComponent<MeshRenderer>().materials[0].color != Color.red && hit.transform.gameObject.tag == "Interactable")
                    {
                        StartCoroutine(RengiEskiyeDondurme(hit.transform.gameObject, hit.transform.GetComponent<MeshRenderer>().materials[0].color, 0.5f));
                        hit.transform.GetComponent<MeshRenderer>().materials[0].color = Color.red;
                    }
                }
                else if(oyunTuru == Tur.ElmaIslemleri)
                {
                    if(hit.transform.gameObject.tag == "Respawn")
                    {
                        elmaKlon = Instantiate(elmaPrefab, hit.point, Quaternion.identity);
                    }
                    if(hit.transform.gameObject.tag == "Interactable" && sepettekiElmalar > 0)
                    {
                        sepettekiElmalar--;
                        sepetObjesi.GetComponentInChildren<Text>().text = sepettekiElmalar.ToString();
                        elmaKlon = Instantiate(elmaPrefab, hit.point, Quaternion.identity);
                    }
                    //if(hit.transform.tag == "Finish")
                    //{
                    //    if(sepettekiElmalar == elmaSayisi)
                    //    {
                    //        Debug.Log("Dogru bildin");
                    //        SceneManager.LoadScene("ToplamaCikarma");
                    //    }
                    //    else
                    //    {
                    //        Debug.Log("Yanlış!");
                    //        sepettekiElmalar = 0;
                    //        sepetObjesi.GetComponentInChildren<Text>().text = sepettekiElmalar.ToString();
                    //    }
                    //}
                }
                else if(oyunTuru == Tur.KupItme)
                {
                    if(hit.transform.tag == "Interactable")
                    {
                        Destroy(kuvvetKlon);
                        kuvvetKlon = Instantiate(kuvvetSprite, hit.point, Quaternion.FromToRotation(Vector3.back, hit.normal),hit.transform);
                        sanalKup = hit.transform.gameObject;
                    }
                }
            }
        }
        if (Input.GetMouseButton(0))
        {
            Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 150);
            if (oyunTuru == Tur.ElmaIslemleri)
            {
                if(elmaKlon != null)
                {
                    elmaKlon.transform.position = hit.point;
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit, 150);
            if(oyunTuru == Tur.ElmaIslemleri)
            {
                if(hit.transform != null)
                {
                    if (elmaKlon != null && hit.transform.tag == "Interactable")
                    {
                        sepettekiElmalar++;
                        sepetObjesi.GetComponentInChildren<Text>().text = sepettekiElmalar.ToString();
                        Destroy(elmaKlon);
                        elmaKlon = null;
                    }
                    else if (elmaKlon != null)
                    {
                        Destroy(elmaKlon);
                        elmaKlon = null;
                    }
                }
                else
                {
                    Destroy(elmaKlon);
                    elmaKlon = null;
                }
            }
        }
#else
        // Mobil cihazlar için kod (multi-touch)
     
        // Ekranda basılı olan tüm parmaklara bak
        for( int i = 0; i < Input.touchCount; i++ )
        {
            Touch parmak = Input.GetTouch( i );
            if( parmak.phase == TouchPhase.Began )
            {
                if (oyunTuru == Tur.BalonPatlatma)
                {
                    // Eğer bir parmak ekrana daha yeni basılmışsa o noktada bir Raycasting yaparak herhangi bir objeye tıklanıp tıklanmadığına bak
                    if (Physics.Raycast(camera.ScreenPointToRay(Input.mousePosition), out hit,150))
                    {
                        string[] sayiAl = hit.transform.name.Split("-"[0]);
                        if (hit.transform.gameObject.tag == "Interactable" && int.Parse(sayiAl[1]) == baslangicSayisi)
                        {
                            baslangicSayisi = int.Parse(sayiAl[1]) + 1;
                            hit.transform.gameObject.SetActive(false);
                            GameObject patlama = Instantiate(patlamaParticle, hit.transform.GetChild(0).transform.position, Quaternion.identity);
                            patlama.GetComponent<ParticleSystem>().Play();
                            Destroy(hit.transform.gameObject, 0.2f);
                            Destroy(patlama, 0.2f);
                            if (baslangicSayisi > balonSayisi)
                            {
                                Debug.Log("Tebrikler, bitirdin");
                            }
                        }
                        else if(hit.transform.GetComponent<MeshRenderer>().materials[0].color != Color.red && hit.transform.gameObject.tag == "Interactable")
                        {
                            StartCoroutine(RengiEskiyeDondurme(hit.transform.gameObject, hit.transform.GetComponent<MeshRenderer>().materials[0].color, 0.5f));
                            hit.transform.GetComponent<MeshRenderer>().materials[0].color = Color.red;
                        }
                    }
                }
            }
        }
#endif
    }

    public IEnumerator RengiEskiyeDondurme(GameObject obje, Color matColor, float sure)
    {
        yield return new WaitForSeconds(sure);
        obje.GetComponent<MeshRenderer>().materials[0].color = matColor;
    }

    public void ElmalariSay()
    {
        if (sepettekiElmalar == elmaSayisi)
        {
            Debug.Log("Dogru bildin");
            SceneManager.LoadScene("ToplamaCikarma");
        }
        else
        {
            Debug.Log("Yanlış!");
            sepettekiElmalar = 0;
            sepetObjesi.GetComponentInChildren<Text>().text = sepettekiElmalar.ToString();
        }
    }

    public void KuvvetUygula()
    {
        if(sanalKup != null)
        {
            sanalKup.GetComponent<Rigidbody>().AddForceAtPosition(kuvvetSlider.GetComponentInChildren<D3Slider>().value * kuvvetCarpani * kuvvetKlon.transform.forward, kuvvetKlon.transform.position);
        }
    }

    public void OyuncakKlonla(GameObject oyuncak)
    {
        GameObject Oynck = Instantiate(oyuncak, new Vector3(0, 1.2f, 0), Random.rotation);
        Oynck.GetComponent<Rigidbody>().useGravity = true;
        Oynck.tag = "Interactable";
        Destroy(Oynck.GetComponent<D3Button>());
        klonlananlar.Add(Oynck);
    }

    public void OyuncaklariTemizle()
    {
        for(int i = 0; i < klonlananlar.Count; i++)
        {
            Destroy(klonlananlar[i].gameObject);
        }
        klonlananlar.Clear();
    }

}

public class BalonHareket : MonoBehaviour
{
    int yon = 1;

    public void BalonHareketBaslat()
    {
        //Invoke("Hareket1", Random.Range(0, 1f));
        StartCoroutine(Hareket());
    }

    void Hareket1()
    {
        StartCoroutine(Hareket());
    }

    private void LateUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, transform.position.y + yon, transform.position.z), Time.deltaTime * 0.5f);
    }

    IEnumerator Hareket()
    {
        yield return new WaitForSeconds(1f);
        if (yon == 1) yon = -1;
        else yon = 1;
        StartCoroutine(Hareket());
    }

}