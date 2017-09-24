using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Detect : MonoBehaviour
{

    public enum Tur {BalonPatlatma};

    public Tur oyunTuru;
    [HideInInspector]
    public GameObject patlamaParticle, balonPrefab;
    [HideInInspector]
    public int baslangicSayisi, balonSayisi;
    [HideInInspector]
    public Vector3 spawnMin, spawnMax;

    private void Start()
    {
        if (oyunTuru == Tur.BalonPatlatma)
        {
            for (int i = baslangicSayisi; i <= balonSayisi; i++)
            {
                BalonSpawnla(i);
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
                if (oyunTuru == Tur.BalonPatlatma)
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