using UnityEngine;

public class HeartMeter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject HeartPrefab;
    [SerializeField] Heart[] Hearts;
    private int _MaxHearts = 0;

    public void UpdateValue(int healthAmount, int MaxHealthAmount)
    {
        if (_MaxHearts != MaxHealthAmount)
        {
            foreach (Heart oldHeart in Hearts)
            {
                Destroy(oldHeart.gameObject);
            }
            Hearts = new Heart[MaxHealthAmount];
            for (int i = 0; i < MaxHealthAmount; i++)
            {
                var heartInstance = Instantiate(HeartPrefab, this.transform);
                Hearts[i] = heartInstance.GetComponent<Heart>();
            }
        }
        
        ResetHeart();
        for (int i = 0; i < healthAmount; i++)
        {
            Hearts[i].SetHeart(Heart.Status.Enabled);
        }
    }

    private void ResetHeart()
    {
        foreach (Heart heart in Hearts)
        {
            heart.SetHeart(Heart.Status.Disabled);
        }
    }
}
