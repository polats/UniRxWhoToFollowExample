using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using SimpleJSON;

public class GameManager : MonoBehaviour {

    // set from editor
    public GameObject refreshButton;
    public GameObject suggestion1Text;

    const string HTML_URL = "html_url";
    const string LOGIN = "login";
    const string AVATAR_URL = "avatar_url";

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
                return ObservableWWW.Get(requestUrl).Select(
                    jsonString =>
                    {
                        // convert jsonString to list of users as Dictionaries
                        JSONNode root = SimpleJSON.JSON.Parse(jsonString);
                        List<Dictionary<string, string>> listUsers = new List<Dictionary<string, string>>();

                        for (int i = 0; i < root.Count; i++)
                        {
                            Dictionary<string, string> user = new Dictionary<string, string>();
                            user[HTML_URL] = root[i][HTML_URL];
                            user[LOGIN] = root[i][LOGIN];
                            user[AVATAR_URL] = root[i][AVATAR_URL];
                            listUsers.Add(user);
                        }
                            
                        return listUsers;
                    });
            });

        responseStream.Subscribe(
            response => // onSuccess
            {
                // response is a list of users
                foreach (Dictionary<string,string> user in response)
                {
                    Debug.Log("user: " + user[LOGIN]); 
                }
            },
            e => // onError
            {
                Debug.LogException(e);
            });
                

        var suggestion1Stream = responseStream.Select(
            listUsers =>
            {
                return listUsers[Random.Range(0, listUsers.Count)];
            });

        suggestion1Stream.Subscribe(
            suggestion =>
            {
                suggestion1Text.GetComponentInChildren<Text>().text = suggestion[LOGIN];
            });
	}
    
	
}
