using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameManager : MonoBehaviour {

    // set from editor
    public GameObject refreshButton;

	void Start () {
        
        var refreshClickStream = refreshButton.GetComponent<Button>().onClick.AsObservable();

        var requestStream = refreshClickStream.Select(
            t =>
            {
                var randomOffset = Random.Range(1,500);
                return "https://api.github.com/users?since=" + randomOffset;
            })
            .StartWith("https://api.github.com/users");
        
        var responseStream = requestStream.SelectMany(
            requestUrl =>
            {
                return ObservableWWW.Get(requestUrl);
            });

        responseStream.Subscribe(
            response => // onSuccess
            {
                Debug.Log("response: " + response); 
            },
            e => // onError
            {
                Debug.LogException(e);
            });
                

        var suggestion1Stream = responseStream.Select(
            listUsers =>
            {
                return listUsers[Random.Range(0, listUsers.Length)];
            });
	}
	
}
