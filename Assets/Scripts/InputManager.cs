using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;
using UnityEngine.SceneManagement;
public class InputManager : MonoBehaviour
{
    [SerializeField]
    private KeyCode[] Touches;

   [SerializeField]
   private UnityEvent<bool>[] CommandesMouvement;

   delegate bool ActionPropulseur(KeyCode touche);
   delegate bool ActionVaisseau(KeyCode touche);
    PhotonView view;
    bool contr�leActiv� = false;
    private void Start()
    {
        view = GetComponent<PhotonView>();        
        if (view.IsMine)
        {
            GameObject.Find("Main Camera").GetComponent<Cam�raD�placement>().AffecterCam�raAuVaisseau(gameObject);
            GameObject d�compte = GameObject.Find("D�compte");
            if (d�compte != null)
                d�compte.GetComponent<D�compte>().AssocierVaisseau(gameObject);
        }
    }
    public void G�rerContr�le(bool activation) => contr�leActiv� = activation;
    
    void Update()
    {
        if (view.IsMine && contr�leActiv�)
        {
            G�rerActionPropulseurs(Input.GetKey, true);
            G�rerActionPropulseurs(Input.GetKeyUp, false);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(GameObject.Find("SpawnPlayers"));
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("Loading");
            }
        }
   }

   private void G�rerActionPropulseurs(ActionPropulseur actionPropulseur, bool commutateurPropulseur)
   {
      for (int i = 0; i < CommandesMouvement.Length; ++i)
      {
         if (actionPropulseur(Touches[i]))
         {
            CommandesMouvement[i].Invoke(commutateurPropulseur);
         }
      }
   }
}
