using System.Collections;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    public static void ChangeRoom(string toRoom)
    {
        Debug.Log("Changing scene to " + toRoom);
        PlayerController.Instance.StartCoroutine(ChangeRoomRoutine());

        IEnumerator ChangeRoomRoutine()
        {
            yield return UIManager.Instance.ShowUI("Fader");
            UnityEngine.SceneManagement.SceneManager.LoadScene(toRoom);
            yield return UIManager.Instance.HideUI("Fader");
        }
    }
}
