using UnityEngine;

public class PowerUp : MonoBehaviour
{
    ////////////////////////////////
    /// PowerUp
    /// 
    /// PowerUp is defined by enum.
    /// Triple, Shield, Speed
    /// LaserEnergy, Repair, Ultimate (Magnet?)
    /// 
    /// Upon collision/trigger with player
    /// PLAYER script requests 
    /// the PowerUp type via 
    /// public PowerUpType PowerType()
    /// 
    /// 
    public enum PowerUpType
    {
        TripleShot,
        Shield,
        Speed,
        EnergyCell,
        Repair,
        Ultimate
    }

    public PowerUpType powerUpType;

    [SerializeField] float _speed = 3.0f;
    [SerializeField] float _destoryYAxisThreshold = -4.50f;
    [SerializeField] Color _powerUpCountDownBarColor;

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if (transform.position.z != 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }

        if (transform.position.y < _destoryYAxisThreshold)
            Destroy(this.gameObject);
    }

    public PowerUpType PowerType()
    {
        return (powerUpType);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // Enable player power up
            // Power Up enabled on Player script
            Destroy(this.gameObject);

            // PLAYER handles Triggers/Collisions
            // OnTriggerEnter2D and PowerUps
            // PowerUp script initiate 
            // call into player to activate powerUp
            /*
            switch (powerUpType)
            {
                case PowerUpType.TripleShot:
                    break;
                case PowerUpType.Speed:
                    break;
                case PowerUpType.Shield:
                    break;
            }
            */
        }
    }
}