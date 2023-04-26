using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Razer : MonoBehaviour
{
    [SerializeField]
    private Transform razerControllerTrm;
    [SerializeField]
    private Transform arrowTrm;
    [SerializeField]
    private int damage;

    public void OnRazer(Vector3 vec)
    {
        vec = vec - razerControllerTrm.transform.position;
        razerControllerTrm.localScale = new Vector3(50,1,1);

        Vector3 referenceVec = new Vector3(1, 0, 0);
        Vector3 referenceVecRight = Vector3.Cross(Vector3.up, referenceVec);

        float angle = Vector3.Angle(vec, referenceVec);
        float sign =    Mathf.Sign(Vector3.Dot(vec,referenceVecRight));
        float finalAngle = sign * angle;
        Debug.Log($"sign : {sign} * angle : {angle} = finalangle : {finalAngle}");

        if (vec.y < 0)
            finalAngle *= -1;

        razerControllerTrm.rotation = Quaternion.Euler(new Vector3(0, 0, finalAngle));
        arrowTrm.rotation = Quaternion.Euler(new Vector3(0, 0, finalAngle));
    }

    public void OffRazer()
    {
        razerControllerTrm.localScale = new Vector3(0, 1, 1);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.GetComponent<HealthSystem>().Damage(damage);
        }
    }
}
