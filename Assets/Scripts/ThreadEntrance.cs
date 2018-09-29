using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

/// <summary>
/// 子线程发送数据给主线程并对主线程创建的可见对象进行操作
/// 主要通过把子线程创建的数据缓存到队列中，然后在主线程的update中把队列中的数据分发出去
/// </summary>
public class ThreadEntrance : MonoBehaviour
{
	public GameObject m_curGm;
	private void Awake()
	{
		MsgDispatch.MInstance.RegMsg(100, MoveTarget);
	}

	private void MoveTarget(object obj)
	{
		m_curGm.transform.Rotate(0, (float)obj, 0);
	}

	// Use this for initialization
	void Start ()
	{
		Thread thread = new Thread(new ThreadStart(MoveTarget));
		thread.IsBackground = true;
		thread.Start();
	}

	private void MoveTarget()
	{
		while (true)
		{
			MsgDispatch.MInstance.Push(new MsgData() { MMsgId = 100, MSpeed = 10});
			Thread.Sleep(1);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		MsgDispatch.MInstance.Update();
	}
}
