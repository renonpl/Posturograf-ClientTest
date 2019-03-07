using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{

    private static SubscriberSocket client;
    private static Vector2 data;
    public Vector2 Data { get => data; }


    /*
     * Server has to work when game is running!
     * The server must work when the game is running,
     * otherwise game will freeze!
     */
    private static void StartClient(string server_address = "127.0.0.1", int port = 5556)
    {
        try
        {
            client = new SubscriberSocket();
            string connect_to = "tcp://"+ server_address +":" + port.ToString();
            client.Connect(connect_to);
            client.Subscribe("");
        }
        catch (Exception e)
        {
            //Console.WriteLine(e.ToString());
            Debug.Log(e.ToString());
        }
        Debug.Log("Server connected!");
    }

    //download data as long as possible
    private static void ReadData()
    {
        string reply;
        while(client.TryReceiveFrameString(out reply))
        {
            string[] s = reply.Split(' ');
            int i = 0;
            while (s[i].Length < 1) i++;
            data.x = Convert.ToInt32(s[i]); i++;
            while (s[i].Length < 1) i++;
            data.y = Convert.ToInt32(s[i]);
        }
    }
    /*
    //download data once
    private static void ReadData()
    {
        string reply = client.ReceiveFrameString();
        string[] s = reply.Split(' ');
        int i = 0;
        while (s[i].Length < 1) i++;
        data.x = Convert.ToInt32(s[i]); i++;
        while (s[i].Length < 1) i++;
        data.y = Convert.ToInt32(s[i]);
    }
    */

    void Start()
    {
        StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        ReadData();
        //Debug.Log(data.x.ToString() + " " + data.y.ToString());
    }

    private void OnDestroy()
    {
        client.Dispose();
        NetMQConfig.Cleanup();
    }
}
