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
    bool contrôleActivé = false;
    private void Start()
    {
        view = GetComponent<PhotonView>();        
        if (view.IsMine)
        {
            GameObject.Find("Main Camera").GetComponent<CaméraDéplacement>().AffecterCaméraAuVaisseau(gameObject);
            GameObject décompte = GameObject.Find("Décompte");
            if (décompte != null)
                décompte.GetComponent<Décompte>().AssocierVaisseau(gameObject);
        }
    }
    public void GérerContrôle(bool activation) => contrôleActivé = activation;
    
    void Update()
    {
        if (view.IsMine && contrôleActivé)
        {
            GérerActionPropulseurs(Input.GetKey, true);
            GérerActionPropulseurs(Input.GetKeyUp, false);
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Destroy(GameObject.Find("SpawnPlayers"));
                PhotonNetwork.Disconnect();
                SceneManager.LoadScene("Loading");
            }
        }
   }

   private void GérerActionPropulseurs(ActionPropulseur actionPropulseur, bool commutateurPropulseur)
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
