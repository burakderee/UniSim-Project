using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GeriDon : MonoBehaviour
{
    public Button geriDonButon;

    void Start()
    {
        geriDonButon.onClick.AddListener(() => SceneManager.LoadScene("AnaMenu"));
    }
}