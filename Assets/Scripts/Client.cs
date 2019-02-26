using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{

    private static SubscriberSocket client;
    private static void StartClient()
    {
        try
        {
            client = new SubscriberSocket();
            string connect_to = "tcp://127.0.0.1:5556";
            client.Connect(connect_to);
            client.Subscribe("");
        }
        catch (Exception e)
        {
            //Console.WriteLine(e.ToString());
            Debug.Log(e.ToString());
        }
    }

    private static string ReadData()
    {
        string reply = client.ReceiveFrameString();
        Debug.Log("> " + reply);
        //Console.WriteLine("> " + reply);
        return reply;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        ReadData();
    }
}
