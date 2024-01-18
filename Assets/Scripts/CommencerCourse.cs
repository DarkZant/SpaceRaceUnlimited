using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Linq;

public class CommencerCourse : MonoBehaviour
{
    [SerializeField]
    GameObject playerReady;
    GameObject spawnPlayers;
    SpawnPlayers scriptSpawnPlayers;
    ModifierCanvas scriptCanvas;
    Button button;
    ExitGames.Client.Photon.Hashtable _playerCustomProperties = new ExitGames.Client.Photon.Hashtable();

    void Awake()
    {
        button = GetComponent<Button>();
        scriptCanvas = GetComponentInParent<ModifierCanvas>();
        spawnPlayers = GameObject.Find("SpawnPlayers");
        scriptSpawnPlayers = spawnPlayers.GetComponent<SpawnPlayers>();
    }

    public void GoCourse()
    {
        button.interactable = false;
        PhotonNetwork.Instantiate(playerReady.name, Vector2.zero, Quaternion.identity);
    }
    private void Update()
    {
        int joueursPrêt = GameObject.FindGameObjectsWithTag("PlayerReady").Length;
        if (joueursPrêt == PhotonNetwork.CurrentRoom.PlayerCount)
        {
            scriptCanvas.PréparerScène();
            scriptSpawnPlayers.SpawnVaisseaux();
        }
    }
}
