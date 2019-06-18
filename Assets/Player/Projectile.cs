using UnityEngine;

public class Projectile : MonoBehaviour
{

    #region New Projectile Stuff

    [SerializeField] Transform target = null;
    [SerializeField] public float projectileSpeed = 1;
    public float damage;
    [SerializeField] float targetYShift;

    void Update()
    {
        if (target == null)
        {
            return;
        }

        transform.LookAt(GetAimPosition());
        transform.Translate(Vector3.forward * projectileSpeed * Time.deltaTime);
    }
    //발사체
    private Vector3 GetAimPosition()
    {
        CapsuleCollider targetCapsule = target.GetComponent<CapsuleCollider>();
        if (targetCapsule == null)
        {
            return target.position;
        }
        return target.position + Vector3.up * ((targetCapsule.height / 2) - targetYShift);

    }

    public void SetTarget(Transform target, float damage)
    {
        this.target = target;
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (target.GetComponent<CapsuleCollider>() == other.GetComponent<CapsuleCollider>())
        {
            print("They're the same thing!");
            if (other.GetComponent<Enemy>())
            {
                other.GetComponent<Enemy>().TakeDamage(damage);
                Destroy(gameObject);
            }
            else if (other.GetComponent<Player>())
            {
                other.GetComponent<Player>().TakeDamage(damage);
                Destroy(gameObject);
            }
        }

    }

    #endregion
}
