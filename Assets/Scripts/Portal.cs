using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {

    [SerializeField] PortalDestination destination;
    [SerializeField] int sceneIndexToLoad = -1;
    [SerializeField] Transform spawnPoint;

    public void OnPlayerTriggered(Player player) {
        StartCoroutine(switchScene(player));
    }

    private IEnumerator switchScene(Player player) {
        LevelLoader levelLoader = FindObjectOfType<LevelLoader>();

        DontDestroyOnLoad(gameObject);

        yield return levelLoader.transitionOutOfScene();
        yield return SceneManager.LoadSceneAsync(sceneIndexToLoad);

        Portal destinationPortal = FindObjectsOfType<Portal>().First(portal =>
            portal != this &&
            portal.destination == this.destination
        );
        player.handleSceneLoaded(destinationPortal.spawnPoint.position);

        yield return levelLoader.transitionIntoScene();

        Destroy(gameObject);
    }
}

public enum PortalDestination {
    NOT_SPECIFIED,
    CORNELIA
}
