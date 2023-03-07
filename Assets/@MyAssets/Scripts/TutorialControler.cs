using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TutorialControler : MonoBehaviour
{
    [SerializeField] private Transform counterPoint;
    public Transform helmetPoint;
    public Transform baseballPoint;

    public static TutorialControler Instance;
    public Transform targetPoint;
    public UnityEvent tutorialEvent = new UnityEvent();

    [SerializeField] LineRenderer lineRenderer;

    PlayerController _playerController;
    //public List<Unlockable> allUnlockables;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _playerController = PlayerController.instance;
        lineRenderer.material.DOOffset(new Vector2(-1, 0), 1f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Incremental);
        if (PlayerPrefs.GetInt(PlayerPrefsKey.TutorialCount, 0).Equals(0))
        {
            targetPoint = counterPoint;
        }
    }

    private void FixedUpdate()
    {
        if (targetPoint != null)
        {
            lineRenderer.gameObject.SetActive(true);
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, _playerController.transform.position.With(y: 0.08f));
            lineRenderer.SetPosition(1, targetPoint.position.With(y: 0.08f));
        }
        else
        {
            lineRenderer.gameObject.SetActive(false);
        }

        /*if (PlayerPrefs.GetInt(PlayerPrefsKey.TutorialCount, 0).Equals(1))
        {
            for (var i = 0; i < allUnlockables.Count; i++)
            {
                var money = PlayerPrefs.GetInt(PlayerPrefsKey.Money, 0);
                if (allUnlockables[i].price > 0)
                {
                    if (money >= allUnlockables[i].price)
                    {
                        lineRenderer.gameObject.SetActive(true);
                        lineRenderer.positionCount = 2;
                        lineRenderer.SetPosition(0, _playerController.transform.position.With(y: 0.2f));
                        lineRenderer.SetPosition(1, allUnlockables[i].transform.position.With(y: 0.2f));
                    }
                    else
                    {
                        lineRenderer.gameObject.SetActive(false);
                    }

                    break;
                }
            }
        }*/
    }
}