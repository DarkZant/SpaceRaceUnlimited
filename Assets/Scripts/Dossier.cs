using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using Photon.Pun;

public class Dossier : MonoBehaviour
{
    //Déplacement du vaisseau avec la physique
    public class MouvementVaisseau : MonoBehaviour
    {
        [SerializeField] float forceAvancer = 1000;
        [SerializeField] float forceVerticale = 30;
        [SerializeField] float brakeForce = 20;
        [SerializeField] float forceDeRotation = 5;
        Rigidbody CorpsVaisseau;
        public void PivoterVersLaDroite(bool commutateur)
        {
            if (!commutateur)
                CorpsVaisseau.angularVelocity = Vector3.zero;
            CorpsVaisseau.AddTorque(Vector3.up * forceDeRotation * Time.deltaTime, ForceMode.Impulse);
            Vector3 rotationEuler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(0, rotationEuler.y, 0);
        }
        public void Monter(bool commutateur)
        {
            if (!commutateur)
                CorpsVaisseau.velocity = new Vector3(CorpsVaisseau.velocity.x, CorpsVaisseau.velocity.y / 1000, CorpsVaisseau.velocity.z);
            if (gameObject.transform.position.y < 40)
                CorpsVaisseau.AddForce(Vector3.up * forceVerticale * Time.deltaTime, ForceMode.Impulse);
            if (gameObject.transform.position.y > 40)
                CorpsVaisseau.velocity = new Vector3(CorpsVaisseau.velocity.x, 0, CorpsVaisseau.velocity.z);
        }
        public void Avancer(bool commutateur)
        {
            CorpsVaisseau.AddRelativeForce(Vector3.forward * forceAvancer * Time.deltaTime);
        }
        public void Freiner(bool commutateur)
        {
            CorpsVaisseau.AddForce(-CorpsVaisseau.velocity / brakeForce);
            CorpsVaisseau.AddTorque(-CorpsVaisseau.angularVelocity / 2);
        }
        public void DoublerVitesse(bool estActif)
        {
            if (estActif)
                forceAvancer *= 2;
            else
                forceAvancer /= 2;
        }
    }
    //Gestion de la vie du vaisseau avec le système de dégât qui utilise la physique
    public class VieVaisseau : MonoBehaviour
    {
        [SerializeField]
        int ptVieMax = 100;
        int ptvie;
        [SerializeField]
        GameObject compteur;
        [SerializeField]
        GameObject ModèleExplosion;
        public Vector3 PointDeRéapparition = new Vector3(-8.67f, 13.31f, -178.42f);
        public Quaternion Rotation = Quaternion.Euler(0, 52, 0);
        [SerializeField]
        GameObject barreDeVie;
        BarreDeVie scriptBarreDeVie;
        Rigidbody CorpsVaisseau;
        float temps = 0;
        int tempsDégats = 1;
        void Awake()
        {
            CorpsVaisseau = GetComponent<Rigidbody>();
        }
        public void AssocierCompteur()
        {
            compteur = GameObject.Find("Compteur");
            compteur.GetComponent<Compteur>().AssocierVaisseau(gameObject);
        }
        private void Update()
        {
            temps += Time.deltaTime;
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (barreDeVie == null)
            {
                barreDeVie = GameObject.Find("HealthBar");
                scriptBarreDeVie = barreDeVie.GetComponent<BarreDeVie>();
                scriptBarreDeVie.ChangerVieMax(ptVieMax);
                ptvie = ptVieMax;
            }
            if (temps > tempsDégats)
            {
                temps = 0;
                if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Projectile")
                {
                    ptvie -= (int)(Mathf.Abs(CorpsVaisseau.velocity.x) + Mathf.Abs(CorpsVaisseau.velocity.z));
                    Rigidbody rbobject = collision.gameObject.GetComponent<Rigidbody>();
                    ptvie -= (int)(Mathf.Abs(rbobject.velocity.x) + Mathf.Abs(rbobject.velocity.z));
                    scriptBarreDeVie.ChangerVie(ptvie);
                }
                else
                {
                    CorpsVaisseau.angularVelocity = CorpsVaisseau.angularVelocity / 4;
                    ptvie -= (int)(Mathf.Abs(CorpsVaisseau.velocity.x) + Mathf.Abs(CorpsVaisseau.velocity.z));
                    scriptBarreDeVie.ChangerVie(ptvie);
                }
            }
            if (ptvie <= 0)
            {
                gameObject.SetActive(false);
                GameObject explosion = Instantiate(ModèleExplosion, gameObject.transform.position, Quaternion.identity);
                Destroy(explosion, 0.5f);
                ptvie = ptVieMax;
                CorpsVaisseau.angularVelocity = Vector3.zero;
                CorpsVaisseau.velocity = Vector3.zero;
                Invoke("Respawn", 2.5f);
            }
        }
        private void Respawn()
        {
            scriptBarreDeVie.ChangerVie(ptVieMax);
            gameObject.transform.position = PointDeRéapparition;
            gameObject.transform.rotation = Rotation;
            gameObject.SetActive(true);
        }
    }
    //Chronomètre + Sauvegarde des records
    public class Chrono : MonoBehaviour
    {
        Text texte;
        float temps = 0;
        [SerializeField]
        bool chronoEnMarche = false;
        void Start()
        {
            texte = GetComponent<Text>();
        }
        void Update()
        {
            if (chronoEnMarche)
            {
                temps += Time.deltaTime;
                float secondes = (float)Mathf.Round(temps % 60);
                int minute = Mathf.FloorToInt(temps / 60);
                if (secondes < 10)
                    texte.text = $"{minute}" + ":0" + $"{secondes}";
                else
                    texte.text = $"{minute}" + ":" + $"{secondes}";
            }
        }
        public void GérerChrono(bool activé)
        {
            chronoEnMarche = activé;
            if (!activé)
            {
                float secondes = temps % 60;
                string secs = secondes.ToString("0.00");
                int minute = Mathf.FloorToInt(temps / 60);
                if (secondes < 10)
                    texte.text = $"{minute}" + ":0" + secs;
                else
                    texte.text = $"{minute}" + ":" + secs;
            }
        }


        public void SauvegarderTemps()
        {
            for (int i = 0; i < 6; ++i)
            {
                if (!PlayerPrefs.HasKey($"record{i}"))
                {
                    PlayerPrefs.SetFloat($"record{i}", temps);
                    break;
                }
            }
            PlayerPrefs.Save();
        }
    }
    //Gestion des records
    public class GérerRecords : MonoBehaviour
    {
        string texteRecords;
        Text texte;
        void Awake()
        {
            texte = GetComponent<Text>();
            AfficherRecords();
        }
        public void RéinitialiserRecords()
        {
            PlayerPrefs.DeleteAll();
            texteRecords = "";
            AfficherRecords();
        }

        void AfficherRecords()
        {
            List<float> records = new List<float>();
            for (int i = 0; i < 6; ++i)
            {
                if (PlayerPrefs.HasKey($"record{i}"))
                    records.Add(PlayerPrefs.GetFloat($"record{i}"));
                else
                    records.Add(300);
            }
            records.Sort();
            for (int i = 0; i < 6; ++i)
            {
                if (PlayerPrefs.GetFloat($"record{i}") == records[records.Count - 1])
                    PlayerPrefs.DeleteKey($"record{i}");
            }
            records.RemoveAt(records.Count - 1);
            foreach (float temps in records)
            {
                string tps;
                float secondes = temps % 60;
                string secs = secondes.ToString("0.00");
                int minute = Mathf.FloorToInt(temps / 60);
                if (secondes < 10)
                    tps = $"{minute}" + ":0" + secs;
                else
                    tps = $"{minute}" + ":" + secs;
                texteRecords += tps + "\n";
            }
            texte.text = texteRecords;
        }
    }
    //Gestion des pouvoirs du joueur
    public class PouvoirJoueur : MonoBehaviour
    {
        float ForceImpulsionProjectile = 60;
        Transform canon;
        int pouvoir = -1;
        MouvementVaisseau scriptMouv;
        ImagePouvoir imagepouvoir;
        void Start()
        {
            Transform[] mesTransforms = GetComponentsInChildren<Transform>();
            canon = mesTransforms.First(X => X.gameObject.name == "Canon");
            scriptMouv = GetComponent<MouvementVaisseau>();
        }
        public void AssocierImagePouvoir()
        {
            imagepouvoir = GameObject.Find("ImagePouvoir").GetComponent<ImagePouvoir>();
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (pouvoir == 0)
                {
                    scriptMouv.DoublerVitesse(true);
                    pouvoir = -1;
                    imagepouvoir.ChangerPouvoir(pouvoir);
                    Invoke("RemettreVitesse", 5);
                }
                else if (pouvoir == 1)
                {
                    pouvoir = -1;
                    imagepouvoir.ChangerPouvoir(pouvoir);
                    TirerProjectile();
                }
            }
        }
        public void AssignerPouvoir(int pouv)
        {
            if (pouvoir == -1)
            {
                pouvoir = pouv;
                imagepouvoir.ChangerPouvoir(pouvoir);
            }
        }
        void RemettreVitesse() => scriptMouv.DoublerVitesse(false);
        void TirerProjectile()
        {
            GameObject nouvProjectile = PhotonNetwork.Instantiate("Projectile", canon.position, canon.rotation);
            nouvProjectile.GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * ForceImpulsionProjectile, ForceMode.Impulse);
        }
    }
    //Comportement des objets de pouvoir
    public class Pouvoir : MonoBehaviour
    {
        [SerializeField]
        GameObject joueur;
        [SerializeField]
        PouvoirJoueur scriptJoueur;
        [SerializeField]
        int valeurPouvoir;
        void Update()
        {
            transform.Rotate(new Vector3(0, 30, -30) * Time.deltaTime);
        }
        void OnTriggerEnter(Collider collision)
        {
            System.Random générateur = new System.Random();
            valeurPouvoir = générateur.Next(2);
            joueur = collision.gameObject.transform.parent.parent.gameObject;
            PhotonView view = joueur.GetComponent<PhotonView>();
            if (view.IsMine)
            {
                scriptJoueur = joueur.GetComponent<PouvoirJoueur>();
                scriptJoueur.AssignerPouvoir(valeurPouvoir);
            }
            Invoke("Réactiver", 10);
            gameObject.SetActive(false);
        }
        void Réactiver() => gameObject.SetActive(true);
    }
    //Gestion de la fin de la course
    public class FinirCourse : MonoBehaviour
    {
        const int nbToursMax = 3;
        int nbDeTours;
        [SerializeField]
        GameObject canvas;
        ModifierCanvas scriptCanvas;
        public GameObject joueur;
        [SerializeField]
        GameObject cam;
        bool prêtÀQuitter = false;
        int tempsDattente = 45;
        float temps = 0;
        [SerializeField]
        GameObject tours;
        GérerTours scriptTours;
        void Start()
        {
            scriptCanvas = canvas.GetComponent<ModifierCanvas>();
            scriptTours = tours.GetComponent<GérerTours>();
        }
        private void Update()
        {
            temps += Time.deltaTime;
            if (prêtÀQuitter)
            {
                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Destroy(GameObject.Find("SpawnPlayers"));
                    PhotonNetwork.Disconnect();
                    SceneManager.LoadScene("Loading");
                }
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (temps >= tempsDattente)
            {
                temps = 0;
                ++nbDeTours;
                scriptTours.AugmenterTours(nbDeTours, nbToursMax);
                if (nbDeTours >= nbToursMax)
                    TerminerCourse();
            }
        }
        void TerminerCourse()
        {
            scriptCanvas.PréparerFin();
            joueur.SetActive(false);
            cam.GetComponent<CaméraDéplacement>().regarde = false;
            cam.transform.position = new Vector3(0, 50, 0);
            cam.transform.rotation = Quaternion.Euler(270, 0, 0);
            prêtÀQuitter = true;
        }
    }
    //Gestion du début de la course incluant de la réseautique (Salle d'attente)
    public class CommencerCourse : MonoBehaviour
    {
        [SerializeField]
        GameObject playerReady;
        GameObject spawnPlayers;
        SpawnPlayers scriptSpawnPlayers;
        ModifierCanvas scriptCanvas;
        Button bouton;
        void Awake()
        {
            bouton = GetComponent<Button>();
            scriptCanvas = GetComponentInParent<ModifierCanvas>();
            spawnPlayers = GameObject.Find("SpawnPlayers");
            scriptSpawnPlayers = spawnPlayers.GetComponent<SpawnPlayers>();
        }
        public void Prêt()
        {
            bouton.interactable = false;
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
    //Décompte
    public class Décompte : MonoBehaviour
    {
        const int tempsdécompte = 3;
        const int tempsAttente = 2;
        Text texte;
        float temps;
        [SerializeField]
        GameObject vaisseau;
        GameObject chrono;
        void Start()
        {
            texte = GetComponent<Text>();
            chrono = GameObject.Find("Chrono");
        }
        public void AssocierVaisseau(GameObject joueur) => vaisseau = joueur;
        void Update()
        {
            temps += Time.deltaTime;
            if (temps >= tempsAttente)
            {
                if (temps >= tempsAttente + tempsdécompte + 1)
                    gameObject.SetActive(false);
                else if (temps >= tempsAttente + tempsdécompte)
                {
                    texte.text = "GO!";
                    vaisseau.GetComponent<InputManager>().GérerContrôle(true);
                    chrono.GetComponent<Chrono>().GérerChrono(true);
                }
                else
                    texte.text = $"{Mathf.FloorToInt(-temps + tempsAttente + tempsdécompte + 1)}";
            }
        }
    }
    //Compteur pour afficher la vitesse
    public class Compteur : MonoBehaviour
    {
        const float vitesseMax = 260f;
        Rigidbody CorpsVaisseau;
        float vitesse;
        [SerializeField]
        float vitesseflechemax;
        [SerializeField]
        float vitesseflechemin;
        [SerializeField]
        Text texteVitesse;
        [SerializeField]
        RectTransform fleche;
        public void AssocierVaisseau(GameObject vaisseau)
        {
            CorpsVaisseau = vaisseau.GetComponent<Rigidbody>();
        }
        void Update()
        {
            Vector3 vélocité = new Vector3(Mathf.Abs(CorpsVaisseau.velocity.x), 0, Mathf.Abs(CorpsVaisseau.velocity.z));
            vitesse = vélocité.magnitude * 3.6f * 2;
            texteVitesse.text = ((int)vitesse) + " km/h";
            fleche.localEulerAngles = new Vector3(0, 0, Mathf.Lerp(vitesseflechemin, vitesseflechemax, vitesse / vitesseMax));
        }
    }
    //Gestion des commandes
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
    //Déplacement de la caméra
    public class CaméraDéplacement : MonoBehaviour
    {

        [SerializeField]
        Transform transformVaisseau;
        [SerializeField]
        Vector3 décalage = new Vector3(0, 6f, -16);
        [SerializeField]
        Space décalagePositionEspace = Space.Self;
        [SerializeField]
        public bool regarde = true;
        Camera cam;
        Rigidbody rbVaisseau;

        private void Awake()
        {
            cam = GetComponent<Camera>();
        }
        public void AffecterCaméraAuVaisseau(GameObject Vaisseau)
        {
            transformVaisseau = Vaisseau.transform;
            rbVaisseau = Vaisseau.GetComponent<Rigidbody>();
        }
        private void Update()
        {
            if (regarde)
            {
                UpdatePosition();
                if (rbVaisseau != null)
                    cam.fieldOfView = 60 + (Mathf.Abs(rbVaisseau.velocity.x) + Mathf.Abs(rbVaisseau.velocity.z)) / 2;
            }

        }
        public void UpdatePosition()
        {
            if (transformVaisseau == null)
                return;
            if (décalagePositionEspace == Space.Self)
                transform.position = transformVaisseau.TransformPoint(décalage);
            else
                transform.position = transformVaisseau.position + décalage;
            if (regarde)
                transform.LookAt(transformVaisseau);
            else
                transform.rotation = transformVaisseau.rotation;
        }
    }
    //Modification du UI en jeu
    public class ModifierCanvas : MonoBehaviour
    {
        [SerializeField]
        GameObject[] enfants;
        public void PréparerScène()
        {
            foreach (GameObject enfant in enfants)
            {
                if (enfant.name != "FinText" && enfant.name != "Quitter")
                    enfant.SetActive(!enfant.activeSelf);
            }
        }
        public void PréparerFin()
        {
            foreach (GameObject enfant in enfants)
            {
                if (enfant.name != "Chrono" && enfant.name != "FinText" && enfant.name != "Quitter")
                    enfant.SetActive(false);
                else if (enfant.name == "FinText" || enfant.name == "Quitter")
                    enfant.SetActive(true);
                else if (enfant.name == "Chrono")
                {
                    enfant.GetComponent<RectTransform>().anchorMin = Vector2.one * 0.5f;
                    enfant.GetComponent<RectTransform>().anchorMax = Vector2.one * 0.5f;
                    enfant.GetComponent<RectTransform>().anchoredPosition = Vector3.zero;
                    enfant.GetComponent<Text>().fontSize = 50;
                    enfant.GetComponent<Chrono>().GérerChrono(false);
                    enfant.GetComponent<Chrono>().SauvegarderTemps();

                }
            }
        }
    }
    //Sélectionneur de vaisseau
    public class Sélectionneur : MonoBehaviour
    {
        Transform[] enfants;
        public int index = 0;
        void Start()
        {
            enfants = GetComponentsInChildren<Transform>();
        }
        void Update()
        {
            for (int i = 1; i < enfants.Length; ++i)
                enfants[i].Rotate(new Vector3(0, -15, 0) * Time.deltaTime);
        }
        public void Prochain()
        {
            if (transform.position.x == -245)
            {
                transform.position = new Vector3(0, 0, 0);
                index = 0;
            }
            else
            {
                transform.position -= new Vector3(35, 0, 0);
                ++index;
            }
        }
        public void Précédent()
        {
            if (transform.position.x == 0)
            {
                transform.position = new Vector3(-245, 0, 0);
                index = 7;
            }
            else
            {
                transform.position += new Vector3(35, 0, 0);
                --index;
            }
        }
    }
}
