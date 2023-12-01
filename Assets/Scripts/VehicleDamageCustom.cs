using UnityEngine;
using System.Collections;
using BNG;

public class VehicleDamageCustom : MonoBehaviour
{



    public float maxMoveDelta = 1.0f; // maximum distance one vertice moves per explosion (in meters)
    public float maxCollisionStrength = 50.0f;
    public float YforceDamp = 0.1f; // 0.0 - 1.0
    public float demolutionRange = 0.5f;
    public float impactDirManipulator = 0.0f;
    public MeshFilter[] optionalMeshList;
    public AudioSource crashSound;

    public GameObject DestroyEffect;
    public CarExit carExitScript;
    public Damageable damageable;


    private MeshFilter[] meshfilters;
    private float sqrDemRange;


    public void Start()
    {
        if(damageable == null)
        {
            damageable = gameObject.GetComponent<Damageable>();
        }

        if (optionalMeshList.Length > 0)
            meshfilters = optionalMeshList;
        else
            meshfilters = GetComponentsInChildren<MeshFilter>();

        sqrDemRange = demolutionRange * demolutionRange;

    }

    void Update()
    {
        //carExitScript.ExitCarIfInTheCar();
        //Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        //Destroy(gameObject);
    }


    private Vector3 colPointToMe;
    private float colStrength;
    private float nextColDamageTime = 0f;

    public float colDamageDelay = 0.1f;
    public float colDamageThreshold = 5.0f;

    public void DestroyVehicle()
    {
        if(carExitScript)
        {
            carExitScript.ExitCarIfInTheCar();
        }
        Instantiate(DestroyEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void OnCollisionEnter(Collision collision)
    {

        //  if (collision.gameObject.CompareTag("car")) return;

        Vector3 colRelVel = collision.relativeVelocity;
        colRelVel.y *= YforceDamp;


        if (collision.contacts.Length > 0 && collision.gameObject.layer != LayerMask.NameToLayer("Weapon"))
        {

            colPointToMe = transform.position - collision.contacts[0].point; //충돌지점에서 차량의 중앙까지의 벡터
            colStrength = colRelVel.magnitude * Vector3.Dot(collision.contacts[0].normal, colPointToMe.normalized); //힘의 방향 추가

            
            if(Time.time > nextColDamageTime && colRelVel.magnitude > colDamageThreshold)
            {
                Debug.Log("상대 충돌 속도(Unit per Sec): " + colRelVel.magnitude);
                //OnDamage(colRelVel.magnitude);
                damageable.DealDamage(colRelVel.magnitude);
                nextColDamageTime = Time.time + colDamageDelay;
            }

            //Debug.Log("crash sound isPlaying?: " + crashSound.isPlaying);
            if (colPointToMe.magnitude > 1.0f && !crashSound.isPlaying)
            {
                crashSound.Play();
                crashSound.volume = colStrength / 200;

                float originalForce = colStrength / maxCollisionStrength;

                OnMeshForce(collision.contacts[0].point, Mathf.Clamp01(originalForce));

            }
        }

    }

    // if called by SendMessage(), we only have 1 param
    public void OnMeshForce(Vector4 originPosAndForce)
    {
        OnMeshForce((Vector3)originPosAndForce, originPosAndForce.w);

    }

    public void OnMeshForce(Vector3 originPos, float force)
    {
        // force should be between 0.0 and 1.0
        force = Mathf.Clamp01(force);






        for (int j = 0; j < meshfilters.Length; ++j)
        {
            Vector3[] verts = meshfilters[j].mesh.vertices;

            for (int i = 0; i < verts.Length; ++i)
            {
                Vector3 scaledVert = Vector3.Scale(verts[i], transform.localScale);
                Vector3 vertWorldPos = meshfilters[j].transform.position + (meshfilters[j].transform.rotation * scaledVert);
                Vector3 originToMeDir = vertWorldPos - originPos;
                Vector3 flatVertToCenterDir = transform.position - vertWorldPos;
                flatVertToCenterDir.y = 0.0f;


                // 0.5 - 1 => 45?to 0? / current vertice is nearer to exploPos than center of bounds
                if (originToMeDir.sqrMagnitude < sqrDemRange) //dot > 0.8f )
                {
                    float dist = Mathf.Clamp01(originToMeDir.sqrMagnitude / sqrDemRange);
                    float moveDelta = force * (1.0f - dist) * maxMoveDelta;

                    Vector3 moveDir = Vector3.Slerp(originToMeDir, flatVertToCenterDir, impactDirManipulator).normalized * moveDelta;

                    verts[i] += Quaternion.Inverse(transform.rotation) * moveDir;


                }

            }

            meshfilters[j].mesh.vertices = verts;
            meshfilters[j].mesh.RecalculateBounds();
        }




    }
}
