using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelHandler : MonoBehaviour {

    [SerializeField] TransitionManager transitionManager;
    [SerializeField] Player player;
    [SerializeField] List<TravelDestinationToSceneIndexItem> destinationToSceneIndexItems;

    private Dictionary<TravelDestination, int> destinationToSceneIndexDictionary;

    public static TravelHandler Instance { get; private set; }

    void Awake() {
        Instance = this;

        destinationToSceneIndexDictionary = new Dictionary<TravelDestination, int>();
        destinationToSceneIndexItems.ForEach(item => {
            destinationToSceneIndexDictionary.Add(item.TravelDestination, item.SceneIndex);
        });
    }

    public void travelTo(TravelDestination destination) {
        StartCoroutine(switchScene(destination));
    }

    private IEnumerator switchScene(TravelDestination destination) {
        int sceneIndexToLoad = destinationToSceneIndexDictionary[destination];

        yield return transitionManager.startTransition();
        yield return SceneManager.LoadSceneAsync(sceneIndexToLoad);

        TravelPoint travelPoint = FindObjectsOfType<TravelPoint>()
            .First(travelPoint => travelPoint.Destination == destination);
        player.handleSceneLoaded(travelPoint.Position, travelPoint.IsInRoom, travelPoint.FacingDirection);

        yield return transitionManager.endTransition();
    }
}

[System.Serializable]
public class TravelDestinationToSceneIndexItem {
    [SerializeField] TravelDestination travelDestination;
    [SerializeField] int sceneIndex;

    public TravelDestination TravelDestination => travelDestination;
    public int SceneIndex => sceneIndex;
}
