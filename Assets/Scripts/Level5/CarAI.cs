using UnityEngine;

public class CarAI : MonoBehaviour
{
    [SerializeField] float detectDistance = 2f;
    [SerializeField] bool Horizontal = true;
    public float speed = 5;
    public float rotateTime = 3f;
    bool isWait = false;
    bool isBlock = false;
    Obstacle currentObstacle;
    Transform rotateTarget;
    // temp variant
    float startTime, pastTime;
    float sx, sz;
    float deltax;
    float deltaz;
    float alpha;
    float k;
    [SerializeField]LayerMask layer = 256;
    Transform detectorPos;
    private void Start()
    {
        k = 2 / rotateTime / rotateTime;
        detectorPos = transform.GetChild(0);
    }
    private void Update()
    {
        if (isWait) return;
        // rotation
       
        Vector3 starPos = detectorPos.position;
        Ray ray = new Ray(starPos, transform.forward * detectDistance);
        RaycastHit hit;
        isBlock = false;
        if (Physics.Raycast(starPos, ray.direction, out hit, detectDistance, layer))
        {
            if (hit.collider.tag == "car")
            {
                isBlock = true;
                if (!rotateTarget)
                {
                    float temp;
                    temp = speed;
                    speed = hit.collider.GetComponent<CarAI>().speed;
                    hit.collider.GetComponent<CarAI>().speed = temp;
                }
            }
        }
        if (!isBlock) {
            if (rotateTarget)
            {
                pastTime = Mathf.Min(Time.time - startTime, rotateTime);
                deltax = k * sx * ((Horizontal) ? (rotateTime - pastTime) : pastTime) * Time.deltaTime;
                deltaz = k * sz * ((Horizontal) ? pastTime : (rotateTime - pastTime)) * Time.deltaTime;
                if (deltaz == 0)
                {
                    alpha = (sx > 0) ? 90 : -90;
                }
                else
                {
                    alpha = Mathf.Atan(deltax / deltaz) * 180 / Mathf.PI;
                    if (sz < 0)
                    {
                        alpha = 180 + alpha;
                    }
                }
                transform.eulerAngles = new Vector3(0, alpha, 0);
                transform.position += new Vector3(deltax, 0, deltaz);
                if (pastTime == rotateTime)
                {
                    rotateTarget = null;
                    Horizontal = !Horizontal;
                }
            }
            else
            {
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }
        else
        {
            startTime += Time.deltaTime;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Obstacle")
        {
            
            currentObstacle = other.GetComponent<Obstacle>();
            if (currentObstacle.target)
            {
                rotateTarget = currentObstacle.target;
                sx = rotateTarget.position.x - transform.position.x;
                sz = rotateTarget.position.z - transform.position.z;
            }
            currentObstacle.currentCar = this;
            isWait = currentObstacle.IsBlock || currentObstacle.isLimit;
            startTime = Time.time;
        }
        else if(other.tag == "car")
        {
            Destroy(gameObject);
        }
    }
    public void WaitRelease()
    {
        if(isWait) startTime = Time.time;
        isWait = false;
        if (currentObstacle)
        {
            currentObstacle.currentCar = null;
            currentObstacle = null;
        }
    }
}