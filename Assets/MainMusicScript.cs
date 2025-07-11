using UnityEngine;

public class MainMusicScript : MonoBehaviour
{
    public static bool started = false;
    public static MainMusicScript Instance;
    private void Awake()
    {
        if (!started)
        {
            started = true;
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }
}
