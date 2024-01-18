using System.Linq;
using UnityEngine;
using Photon.Pun;

public class MouvementVaisseau : MonoBehaviour
{
    [SerializeField] float forceAvancer = 1000;
    [SerializeField] float forceVerticale = 30;
    [SerializeField] float brakeForce = 20;
    [SerializeField] float forceDeRotation = 5;
    Rigidbody CorpsVaisseau;

    private void Awake()
    {
        CorpsVaisseau = gameObject.GetComponent<Rigidbody>();
    }
    public void DoublerVitesse(bool estActif)
    {
        if (estActif)
            forceAvancer *= 2;
        else
            forceAvancer /= 2;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CorpsVaisseau.position);
            stream.SendNext(CorpsVaisseau.rotation);
            stream.SendNext(CorpsVaisseau.velocity);
        }
        else
        {
            CorpsVaisseau.position = (Vector3)stream.ReceiveNext();
            CorpsVaisseau.rotation = (Quaternion)stream.ReceiveNext();
            CorpsVaisseau.velocity = (Vector3)stream.ReceiveNext();
            float lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            CorpsVaisseau.position += CorpsVaisseau.velocity * lag;
        }
    }

    public void PivoterVersLaDroite(bool commutateur)
    {
        if (!commutateur)
            CorpsVaisseau.angularVelocity = Vector3.zero;
        CorpsVaisseau.AddTorque(Vector3.up * forceDeRotation * Time.deltaTime, ForceMode.Impulse);
        Vector3 rotationEuler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotationEuler.y, 0);
    }

    public void PivoterVersLaGauche(bool commutateur)
    {
        if (!commutateur)
            CorpsVaisseau.angularVelocity = Vector3.zero;
        CorpsVaisseau.AddTorque(Vector3.down * forceDeRotation * Time.deltaTime, ForceMode.Impulse);
        Vector3 rotationEuler = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(0, rotationEuler.y, 0);

    }

    public void Monter(bool commutateur)
    {
        if (!commutateur)
        {
            CorpsVaisseau.velocity = new Vector3(CorpsVaisseau.velocity.x, CorpsVaisseau.velocity.y / 1000, CorpsVaisseau.velocity.z);
        }
        if (gameObject.transform.position.y < 40)
            CorpsVaisseau.AddForce(Vector3.up * forceVerticale * Time.deltaTime, ForceMode.Impulse);
        if (gameObject.transform.position.y > 40)
            CorpsVaisseau.velocity = new Vector3(CorpsVaisseau.velocity.x, 0, CorpsVaisseau.velocity.z);
    }
    public void Descendre(bool commutateur)
    {
        if (!commutateur)
            CorpsVaisseau.velocity = new Vector3(CorpsVaisseau.velocity.x, CorpsVaisseau.velocity.y / 1000, CorpsVaisseau.velocity.z);
        CorpsVaisseau.AddForce(Vector3.down * forceVerticale * Time.deltaTime, ForceMode.Impulse);
    }

    public void Avancer(bool commutateur)
    {
        CorpsVaisseau.AddRelativeForce(Vector3.forward * forceAvancer * Time.deltaTime);
    }

    public void Reculer(bool commutateur)
    {
        CorpsVaisseau.AddRelativeForce(Vector3.back * forceAvancer * Time.deltaTime);
    }

    public void Freiner(bool commutateur)
    {
        CorpsVaisseau.AddForce(-CorpsVaisseau.velocity / brakeForce);
        CorpsVaisseau.AddTorque(-CorpsVaisseau.angularVelocity / 2);
    }
}
