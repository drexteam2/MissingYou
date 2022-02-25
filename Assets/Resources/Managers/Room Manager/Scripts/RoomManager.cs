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
            UIManager.Instance.ShowUI("Fader");
            yield return new WaitForSeconds(UIManager.Instance.GetUI("Fader").GetComponent<Animator>()
                .GetCurrentAnimatorStateInfo(0).length);
            UnityEngine.SceneManagement.SceneManager.LoadScene(toRoom);
            UIManager.Instance.HideUI("Fader");
            yield return new WaitForSeconds(UIManager.Instance.GetUI("Fader").GetComponent<Animator>()
                .GetCurrentAnimatorStateInfo(0).length);
        }
    }
}
