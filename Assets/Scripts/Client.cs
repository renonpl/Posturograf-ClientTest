#if UNITY_EDITOR_WIN
using NetMQ;
using NetMQ.Sockets;
#endif
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    private static Vector2 data;
    public Vector2 Data { get => data; }
    public Canvas can;
    public GameObject panel;

#if UNITY_EDITOR_OSX
    private RectTransform rtrans;
    private Vector3 offset;

    void Start()
    {
        panel.gameObject.SetActive(true);
        rtrans = panel.GetComponent<RectTransform>();
        offset = panel.transform.position;
        data = new Vector2(0, 0);
    }

    void Update()
    {
        if (Input.GetMouseButton(0)) ReadInput();
    }

    void ReadInput()
    {
        Vector2 mousePos = Input.mousePosition - offset;
        float currentX = mousePos.x * 200 / rtrans.rect.width; //200, because server operates with ints in range [-100,100]
        float currentY = mousePos.y * 200 / rtrans.rect.height;
        if (currentX >= -100 && currentX <= 100 && currentY >= -100 && currentY <= 100)
        {
            data = new Vector2((int)currentX, (int)currentY);
        }
        //Debug.Log(data.ToString());
    }
#endif
#if UNITY_EDITOR_WIN
    private static SubscriberSocket client;


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
            string connect_to = "tcp://" + server_address + ":" + port.ToString();
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

    /*download data as long as possible*/
    private static void ReadData()
    {
        string reply;
        while (client.TryReceiveFrameString(out reply))
        {
            string[] s = reply.Split(' ');
            int i = 0;
            while (s[i].Length < 1) i++;
            data.x = Convert.ToInt32(s[i]); i++;
            while (s[i].Length < 1) i++;
            data.y = Convert.ToInt32(s[i]);
        }
    }

    void Start()
    {
        can.gameObject.SetActive(false);
        StartClient();
    }

    // Update is called once per frame
    void Update()
    {
        ReadData();
      //  Debug.Log(data.x.ToString() + " " + data.y.ToString());
    }

    private void OnDestroy()
    {
        client.Dispose();
        NetMQConfig.Cleanup();
    }
#endif
}
