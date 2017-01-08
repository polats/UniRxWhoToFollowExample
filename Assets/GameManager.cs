﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using SimpleJSON;

public class GameManager : MonoBehaviour {

    // set from editor
    public GameObject refreshButton;
    public GameObject suggestion1Text;
    public GameObject close1Button;
    public GameObject suggestion2Text;
    public GameObject close2Button;
    public GameObject suggestion3Text;
    public GameObject close3Button;

    const string HTML_URL = "html_url";
    const string LOGIN = "login";
    const string AVATAR_URL = "avatar_url";

	void Start () {
        
        var refreshClickStream = refreshButton.GetComponent<Button>().onClick.AsObservable();
        var close1ClickStream = close1Button.GetComponent<Button>().onClick.AsObservable();
        var close2ClickStream = close2Button.GetComponent<Button>().onClick.AsObservable();
        var close3ClickStream = close3Button.GetComponent<Button>().onClick.AsObservable();

        var requestStream = refreshClickStream.StartWith(new Unit()).
            Select(
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
                Debug.Log("users found: " + response.Count); 
            },
            e => // onError
            {
                Debug.LogException(e);
            });
                

        var suggestion1Stream = close1ClickStream.
            StartWith(new Unit()).
            CombineLatest(
            responseStream, 
            (t, listUsers) =>
            {
                return listUsers[Random.Range(0, listUsers.Count)];
            })
            .Merge(
                refreshClickStream.Select(
                    t =>
                    {
                        Dictionary<string,string> emptyDictionary = null;
                        return emptyDictionary;
                    })
            ).StartWith(new Dictionary<string,string>());  


        suggestion1Stream.Subscribe(
            suggestion =>
            {
                if (suggestion == null)
                {
                    suggestion1Text.SetActive(false);
                }
                else
                {
                    suggestion1Text.SetActive(true);
                    suggestion1Text.GetComponentInChildren<Text>().text = suggestion[LOGIN];
                }
            });

        var suggestion2Stream = close2ClickStream.
            StartWith(new Unit()).
            CombineLatest(
                responseStream, 
                (t, listUsers) =>
                {
                    return listUsers[Random.Range(0, listUsers.Count)];
                })
            .Merge(
                refreshClickStream.Select(
                    t =>
                    {
                        Dictionary<string,string> emptyDictionary = null;
                        return emptyDictionary;
                    })
            ).StartWith(new Dictionary<string,string>());  


        suggestion2Stream.Subscribe(
            suggestion =>
            {
                if (suggestion == null)
                {
                    suggestion2Text.SetActive(false);
                }
                else
                {
                    suggestion2Text.SetActive(true);
                    suggestion2Text.GetComponentInChildren<Text>().text = suggestion[LOGIN];
                }
            });

        var suggestion3Stream = close3ClickStream.
            StartWith(new Unit()).
            CombineLatest(
                responseStream, 
                (t, listUsers) =>
                {
                    return listUsers[Random.Range(0, listUsers.Count)];
                })
            .Merge(
                refreshClickStream.Select(
                    t =>
                    {
                        Dictionary<string,string> emptyDictionary = null;
                        return emptyDictionary;
                    })
            ).StartWith(new Dictionary<string,string>());  


        suggestion3Stream.Subscribe(
            suggestion =>
            {
                if (suggestion == null)
                {
                    suggestion3Text.SetActive(false);
                }
                else
                {
                    suggestion3Text.SetActive(true);
                    suggestion3Text.GetComponentInChildren<Text>().text = suggestion[LOGIN];
                }
            });
	}
    
	
}
