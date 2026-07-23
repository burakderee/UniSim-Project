using UnityEngine;
using UnityEngine.SceneManagement;

public class EscMenu : MonoBehaviour
{
    public GameObject escPanel;
    public GameObject havaPanel;
    public GameObject modelPanel;
    bool acik = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            acik = !acik;
            escPanel.SetActive(acik);

            // Diðer panelleri kapat
            if (havaPanel != null) havaPanel.SetActive(false);
            if (modelPanel != null) modelPanel.SetActive(false);

            Time.timeScale = acik ? 0f : 1f;
        }
    }

    public void HavaDurumuAc()
    {
        escPanel.SetActive(false);
        havaPanel.SetActive(true);
        acik = false;
        Time.timeScale = 1f;
    }

    public void ModelYukleAc()
    {
        escPanel.SetActive(false);
        modelPanel.SetActive(true);
        acik = false;
        Time.timeScale = 1f;
    }

    public void AnaMenuyeDon()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("AnaMenu");
    }

    public void SimulasyonuSurdur()
    {
        acik = false;
        escPanel.SetActive(false);
        Time.timeScale = 1f;
    }
}